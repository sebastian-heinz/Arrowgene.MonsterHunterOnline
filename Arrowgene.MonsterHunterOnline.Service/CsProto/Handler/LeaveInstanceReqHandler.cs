using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Constant;
using Arrowgene.MonsterHunterOnline.Protocol.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Protocol.Old.Structures;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.IO;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class LeaveInstanceReqHandler : CsProtoStructureHandler<LeaveInstanceReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(LeaveInstanceReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_LEAVE_INSTANCE_REQ;

    public LeaveInstanceReqHandler()
    {
    }

    public override void Handle(Client client, LeaveInstanceReq req)
    {
        client.State.StopBattleMonsterLoop();
        client.State.PendingMonsterSpawnPos = null;
        client.State.PendingMonsterNetId = null;

        CsCsProtoStructurePacket<LeaveInstanceRsp> rsp = CsProtoResponse.LeaveInstanceRsp;
        client.SendCsProtoStructurePacket(rsp);

        CsCsProtoStructurePacket<MainInstanceClose> mainInstanceClose = CsProtoResponse.MainInstanceClose;
        mainInstanceClose.Structure.LevelId = 1;
        mainInstanceClose.Structure.RoomId = 1;
        mainInstanceClose.Structure.Reason = 0;
        mainInstanceClose.Structure.TriggerNetId = 1;
        mainInstanceClose.Structure.RoleName = client.Character.Name;
        client.SendCsProtoStructurePacket(mainInstanceClose);

        client.State.SelectRoleTrigger = false;

        CsCsProtoStructurePacket<TownInstanceVerifyRsp> townServerInitNtf = CsProtoResponse.TownServerInitNtf;
        TownInstanceVerifyRsp verifyRsp = townServerInitNtf.Structure;
        verifyRsp.ErrNo = 0;
        verifyRsp.LineId = 0;
        verifyRsp.LevelEnterType = 0;

        InstanceInitInfo instanceInitInfo = verifyRsp.InstanceInitInfo;
        instanceInitInfo.BattleGroundId = 0;
        instanceInitInfo.LevelId = 150301;

        // TODO hack
        instanceInitInfo.LevelId = client.State.InitLevelId;

        instanceInitInfo.CreateMaxPlayerCount = 4;
        instanceInitInfo.GameMode = GameMode.Town;
        instanceInitInfo.TimeType = TimeType.Noon;
        instanceInitInfo.WeatherType = WeatherType.Sunny;
        instanceInitInfo.Time = 1;
        instanceInitInfo.LevelRandSeed = 1;
        instanceInitInfo.WarningFlag = 0;
        instanceInitInfo.CreatePlayerMaxLv = 99;

        string staticFolder = Path.Combine(Util.ExecutingDirectory(), "Files", "Static");
        string csvSpawnPointsPath = Path.Combine(staticFolder, "SpawnPoints.csv");
        int level = instanceInitInfo.LevelId;
        using (TextFieldParser parser = new TextFieldParser(csvSpawnPointsPath))
        {
            string level_comp = level.ToString();
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // Skip the header line
            parser.ReadLine();
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                string levelId = fields[0];
                bool isMatch = (level_comp.Contains(levelId) || levelId.Contains(level_comp));
                if (isMatch)
                {
                    string filename = fields[1];
                    string areaName = fields[2];
                    string pos = fields[3];
                    string rotate = fields[4];

                    string[] posValues = pos.Split(',');
                    string[] rotateValues = rotate.Split(',');

                    float posX = float.Parse(posValues[0], CultureInfo.InvariantCulture);
                    float posY = float.Parse(posValues[1], CultureInfo.InvariantCulture);
                    float posZ = float.Parse(posValues[2], CultureInfo.InvariantCulture);

                    float rotateX = float.Parse(rotateValues[0], CultureInfo.InvariantCulture);
                    float rotateY = float.Parse(rotateValues[1], CultureInfo.InvariantCulture);
                    float rotateZ = float.Parse(rotateValues[2], CultureInfo.InvariantCulture);
                    float rotateW = float.Parse(rotateValues[3], CultureInfo.InvariantCulture);

                    CSQuatT targetPos = new CSQuatT()
                    {
                        q = new CSQuat()
                        {
                            v = new CSVec3() { x = rotateX, y = rotateY, z = rotateZ },
                            w = rotateW
                        },
                        t = new CSVec3() { x = (float)posX, y = (float)posY, z = (float)posZ }
                    };

                    Logger.Debug($"Warp point found at {posX} {posY} {posZ} for level {level}");

                    CsCsProtoStructurePacket<PlayerTeleport> PlayerTeleport = CsProtoResponse.PlayerTeleport;
                    PlayerTeleport.Structure.SyncTime = 1;
                    PlayerTeleport.Structure.NetObjId = client.Character.Id;
                    PlayerTeleport.Structure.Region = client.State.MainInstanceLevelId;
                    PlayerTeleport.Structure.TargetPos = targetPos;
                    PlayerTeleport.Structure.ParentGuid = 1;
                    PlayerTeleport.Structure.InitState = 1;
                    client.SendCsProtoStructurePacket(PlayerTeleport);

                    break;
                }
            }
        }

        client.SendCsProtoStructurePacket(townServerInitNtf);
        client.State.prevLevelId = client.State.levelId;
        client.State.levelId = instanceInitInfo.LevelId;
    }
}
