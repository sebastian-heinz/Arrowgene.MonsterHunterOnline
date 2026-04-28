using System;
using System.Threading;
using System.Threading.Tasks;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Old.Structures;
using Arrowgene.MonsterHunterOnline.Protocol.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto;

namespace Arrowgene.MonsterHunterOnline.Service;

/// <summary>
///  this is a temporary holder for central management of packet and information,
/// it can be considered the "playground" for now.
///
/// On*- function are lifecycle hooks
/// Send*- functions are to send specific data that has been consistently populated
/// </summary>
public class PlayerState
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(PlayerState));

    private const float BattleMonsterOrbitRadius = 60.0f;
    private const float BattleMonsterAngleStep = 0.14f;
    // Tick rate for the orbit/attack loop. With smooth steering (SetPos=0) the
    // client interpolates between waypoints, so we only need to send a packet
    // when the target waypoint changes — 5 Hz was masking lack of interpolation
    // by snapping every 200 ms. 1 Hz is plenty for visible smooth motion.
    private const int BattleMonsterTickMs = 1000;
    private const float BattleMonsterSkillTriggerRange = 20.0f;
    private const uint BattleMonsterSkillId = 5;
    // CryAnimation graph state names. Empty AnimSeqName disables walk animation
    // and falls back to Idle on the client. "Run" is a common default state in
    // monster anim graphs; tune per-monster if the client never picks it up.
    private const string BattleMonsterMoveSequence = "Run";
    private const string BattleMonsterAttackSequence = "Attack_HeavyTail";
    private static readonly TimeSpan BattleMonsterSkillCooldown = TimeSpan.FromSeconds(4);
    private static readonly TimeSpan BattleMonsterAttackPause = TimeSpan.FromSeconds(1500.0 / 1000.0);

    private readonly object _battleMonsterLock = new();
    private readonly Client _client;
    private CancellationTokenSource _battleMonsterLoopCts;
    private Task _battleMonsterLoopTask;

    public static Server Server;
    public int levelId { get; set; }
    public int prevLevelId { get; set; }
    public CSVec3 Position { get; set; }
    public CSVec3 PendingMonsterSpawnPos { get; set; }
    public uint? PendingMonsterNetId { get; set; }
    public uint? ActiveMonsterNetId { get; private set; }
    public CSVec3 ActiveMonsterCenterPos { get; private set; }
    public CSVec3 ActiveMonsterPos { get; private set; }
    public float ActiveMonsterAngle { get; private set; }
    public float ActiveMonsterSequenceTime { get; private set; }
    public int MainInstanceLevelId { get; set; }
    public bool SelectRoleTrigger { get; set; }

    public CSQuatT InitSpawnPose = new CSQuatT()
    {
        q = new CSQuat()
        {
            v = new CSVec3() { x = 10, y = 10, z = 10 },
            w = 10
        },
        t = new CSVec3() { x = 404.91379f, y = 396.74976f, z = 85.0f }
    };

    public CSVec3 InitSpawnPos = new CSVec3()
    {
        x = 404.91379f,
        y = 396.74976f,
        z = 85.0f
    };

    public int InitLevelId = 150101;

    public PlayerState(Client client)
    {
        _client = client;
        if (Position == null)
        {
            Position = InitSpawnPos;
        }
    }

    public void StartBattleMonsterLoop(uint netId, CSVec3 spawnPos)
    {
        StopBattleMonsterLoop();

        CancellationTokenSource loopCts = new();
        CSVec3 orbitCenter = new(spawnPos.x - BattleMonsterOrbitRadius, spawnPos.y, spawnPos.z);

        lock (_battleMonsterLock)
        {
            ActiveMonsterNetId = netId;
            ActiveMonsterCenterPos = CloneVec(orbitCenter);
            ActiveMonsterPos = CloneVec(spawnPos);
            ActiveMonsterAngle = 0.0f;
            ActiveMonsterSequenceTime = 0.0f;
            _battleMonsterLoopCts = loopCts;
            _battleMonsterLoopTask = Task.Run(() => RunBattleMonsterLoop(netId, loopCts.Token), loopCts.Token);
        }

        Logger.Info(_client,
            $"Start battle monster runtime loop NetId=0x{netId:X8} Spawn={FormatVec(spawnPos)} Center={FormatVec(orbitCenter)} Radius={BattleMonsterOrbitRadius:F1} SkillId={BattleMonsterSkillId} RuntimePackets=CS_CMD_BATTLE_MONSTER_LOCOMOTION,CS_CMD_BATTLE_MONSTER_MOVESTATE,CS_CMD_BATTLE_MONSTER_SEQUENCESTATE,CS_CMD_MONSTER_AISKILL");
    }

    public void StopBattleMonsterLoop()
    {
        CancellationTokenSource loopCts;
        Task loopTask;
        uint? netId;

        lock (_battleMonsterLock)
        {
            loopCts = _battleMonsterLoopCts;
            loopTask = _battleMonsterLoopTask;
            netId = ActiveMonsterNetId;

            _battleMonsterLoopCts = null;
            _battleMonsterLoopTask = null;
            ActiveMonsterNetId = null;
            ActiveMonsterCenterPos = null;
            ActiveMonsterPos = null;
            ActiveMonsterAngle = 0.0f;
            ActiveMonsterSequenceTime = 0.0f;
        }

        if (loopCts == null)
        {
            return;
        }

        try
        {
            loopCts.Cancel();
        }
        catch
        {
            // ignore cancellation races during shutdown
        }

        if (loopTask != null)
        {
            _ = loopTask.ContinueWith(_ => loopCts.Dispose());
        }
        else
        {
            loopCts.Dispose();
        }

        if (netId != null)
        {
            Logger.Info(_client, $"Stop battle monster runtime loop NetId=0x{netId.Value:X8}");
        }
    }

    private async Task RunBattleMonsterLoop(uint netId, CancellationToken cancellationToken)
    {
        float tickSeconds = BattleMonsterTickMs / 1000.0f;
        DateTime nextSkillUtc = DateTime.UtcNow + TimeSpan.FromSeconds(2);
        DateTime attackPauseUntilUtc = DateTime.MinValue;

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                CSVec3 currentPos;
                CSVec3 orbitCenter;
                float angle;

                lock (_battleMonsterLock)
                {
                    if (ActiveMonsterNetId != netId || ActiveMonsterCenterPos == null || ActiveMonsterPos == null)
                    {
                        return;
                    }

                    currentPos = CloneVec(ActiveMonsterPos);
                    orbitCenter = CloneVec(ActiveMonsterCenterPos);
                    angle = ActiveMonsterAngle;
                }

                DateTime nowUtc = DateTime.UtcNow;
                CSVec3 playerPos = SnapshotPlayerPosition();
                float playerDistance = Distance2D(currentPos, playerPos);

                if (nowUtc >= attackPauseUntilUtc && playerDistance <= BattleMonsterSkillTriggerRange && nowUtc >= nextSkillUtc)
                {
                    CSQuat attackRotation = LookAtQuat(currentPos, playerPos);
                    CSVec3 zeroSpeed = new(0.0f, 0.0f, 0.0f);

                    SendBattleMonsterLocomotion(
                        netId,
                        currentPos,
                        attackRotation,
                        playerPos,
                        zeroSpeed,
                        BattleMonsterAttackSequence,
                        BattleMonsterSkillId,
                        restartAnim: true,
                        needTargetAttackPos: true,
                        // Attacks need exact pose: snap pos & rot.
                        setPos: true,
                        setRotate: true);
                    SendBattleMonsterMovestate(netId, currentPos, attackRotation, zeroSpeed);
                    SendBattleMonsterSequenceState(netId, BattleMonsterAttackSequence, 0.0f, currentPos, attackRotation);
                    SendMonsterSkill(netId);

                    Logger.Info(_client,
                        $"Battle monster attack trigger NetId=0x{netId:X8} SkillId={BattleMonsterSkillId} MonsterPos={FormatVec(currentPos)} PlayerPos={FormatVec(playerPos)} Dist={playerDistance:F2}");

                    attackPauseUntilUtc = nowUtc + BattleMonsterAttackPause;
                    nextSkillUtc = nowUtc + BattleMonsterSkillCooldown;

                    await Task.Delay(BattleMonsterTickMs, cancellationToken);
                    continue;
                }

                if (nowUtc < attackPauseUntilUtc)
                {
                    await Task.Delay(BattleMonsterTickMs, cancellationToken);
                    continue;
                }

                angle += BattleMonsterAngleStep;
                CSVec3 nextPos = new(
                    orbitCenter.x + BattleMonsterOrbitRadius * MathF.Cos(angle),
                    orbitCenter.y + BattleMonsterOrbitRadius * MathF.Sin(angle),
                    orbitCenter.z);
                float heading = angle + (MathF.PI / 2.0f);
                CSQuat moveRotation = YawQuat(heading);
                CSVec3 moveSpeed = ComputeVelocity(currentPos, nextPos, tickSeconds);

                SendBattleMonsterLocomotion(
                    netId,
                    // MonsterPos = NEXT waypoint (steering goal). The client
                    // walks the entity from its current visible position toward
                    // this point at MoveSpeed using the AnimSeqName state.
                    nextPos,
                    moveRotation,
                    nextPos,
                    moveSpeed,
                    BattleMonsterMoveSequence,
                    0,
                    restartAnim: false,
                    needTargetAttackPos: false,
                    // Smooth walk: don't snap. Client interpolates current → MonsterPos.
                    setPos: false,
                    setRotate: false);
                SendBattleMonsterMovestate(netId, nextPos, moveRotation, moveSpeed);

                lock (_battleMonsterLock)
                {
                    if (ActiveMonsterNetId == netId)
                    {
                        ActiveMonsterPos = CloneVec(nextPos);
                        ActiveMonsterAngle = angle;
                        ActiveMonsterSequenceTime = AdvanceBattleMonsterSequenceTime(netId, tickSeconds);
                    }
                }

                await Task.Delay(BattleMonsterTickMs, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            // expected on stop
        }
        catch (Exception ex)
        {
            Logger.Exception(_client, ex);
        }
    }

    private void SendMonsterSkill(uint netId)
    {
        CSAISkillSync skill = new()
        {
            EntityId = netId,
            SkillID = BattleMonsterSkillId,
        };

        _client.SendCsPacket(NewCsPacket.AISkillSync(skill));
    }

    // setPos / setRotate: when 1 the client SNAPS the entity to MonsterPos /
    // MonsterRot (teleport). When 0 the client uses MonsterPos as a steering
    // target and walks toward it using MoveSpeed + AnimSeqName. Use 1 for
    // attacks (which need an exact pivot pose) and 0 for normal walking.
    private void SendBattleMonsterLocomotion(uint netId, CSVec3 position, CSQuat rotation, CSVec3 targetPos, CSVec3 moveSpeed, string animSequence, uint skillId, bool restartAnim, bool needTargetAttackPos, bool setPos, bool setRotate)
    {
        CSMonsterLocomotion locomotion = new()
        {
            SteeringEnabled = 1,
            SyncTime = CurrentSyncTimeMs(),
            MonsterID = netId,
            AnimSeqName = animSequence ?? string.Empty,
            SkillID = (int)skillId,
            MoveSpeed = CloneVec(moveSpeed),
            MonsterPos = CloneVec(position),
            MonsterRot = rotation,
            TargetDis = new CSVec3(targetPos.x - position.x, targetPos.y - position.y, targetPos.z - position.z),
            TargetAttackPos = CloneVec(targetPos),
            NeedTargetAttackPos = needTargetAttackPos ? (byte)1 : (byte)0,
            SkillSpeed = 1.0f,
            RestartAnim = restartAnim ? (byte)1 : (byte)0,
            SetRotate = setRotate ? (byte)1 : (byte)0,
            SetPos = setPos ? (byte)1 : (byte)0,
        };

        _client.SendCsPacket(NewCsPacket.MonsterLCM(locomotion));
    }

    private void SendBattleMonsterMovestate(uint netId, CSVec3 position, CSQuat rotation, CSVec3 speed)
    {
        CSMonsterMovestate movestate = new()
        {
            SyncTime = CurrentSyncTimeMs(),
            MonsterID = netId,
            Location = CloneVec(position),
            Rotation = rotation,
            Speed = CloneVec(speed),
        };

     //   _client.SendCsPacket(NewCsPacket.MonsterMovestate(movestate));
    }

    private void SendBattleMonsterSequenceState(uint netId, string animSequence, float currentTime, CSVec3 position, CSQuat rotation)
    {
        CSMonsterSequenceState sequenceState = new()
        {
            MonsterID = netId,
            AnimSeqName = animSequence ?? string.Empty,
            CurTime = currentTime,
            Location = CloneVec(position),
            Rotation = rotation,
        };

        _client.SendCsPacket(NewCsPacket.MonsterSequenceState(sequenceState));
    }

    private CSVec3 SnapshotPlayerPosition()
    {
        CSVec3 pos = Position ?? InitSpawnPos;
        return CloneVec(pos);
    }

    private static float Distance2D(CSVec3 from, CSVec3 to)
    {
        float deltaX = to.x - from.x;
        float deltaY = to.y - from.y;
        return MathF.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
    }

    private float AdvanceBattleMonsterSequenceTime(uint netId, float deltaSeconds)
    {
        lock (_battleMonsterLock)
        {
            if (ActiveMonsterNetId != netId)
            {
                return 0.0f;
            }

            ActiveMonsterSequenceTime += deltaSeconds;
            return ActiveMonsterSequenceTime;
        }
    }

    private static long CurrentSyncTimeMs()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    private static CSQuat LookAtQuat(CSVec3 from, CSVec3 to)
    {
        float deltaX = to.x - from.x;
        float deltaY = to.y - from.y;

        if (MathF.Abs(deltaX) < 0.001f && MathF.Abs(deltaY) < 0.001f)
        {
            return new CSQuat(1.0f, 0, 0, 0);
        }

        return YawQuat(MathF.Atan2(deltaY, deltaX));
    }

    private static CSQuat YawQuat(float yaw)
    {
        float halfYaw = yaw * 0.5f;
        return new CSQuat(MathF.Cos(halfYaw), 0, 0, MathF.Sin(halfYaw));
    }

    private static CSVec3 ComputeVelocity(CSVec3 from, CSVec3 to, float deltaSeconds)
    {
        if (deltaSeconds <= 0.0f)
        {
            return new CSVec3();
        }

        return new CSVec3(
            (to.x - from.x) / deltaSeconds,
            (to.y - from.y) / deltaSeconds,
            (to.z - from.z) / deltaSeconds);
    }

    private static CSVec3 CloneVec(CSVec3 vec)
    {
        return new CSVec3(vec.x, vec.y, vec.z);
    }

    private static string FormatVec(CSVec3 vec)
    {
        return $"({vec.x:F3}, {vec.y:F3}, {vec.z:F3})";
    }
}
