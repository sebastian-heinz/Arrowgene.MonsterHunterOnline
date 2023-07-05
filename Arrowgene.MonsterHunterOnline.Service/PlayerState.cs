using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

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

    private Client _client;

    public CSRoleBaseInfo _roleBaseInfo;

    //  public CSRoleBaseInfo _roleBaseInfo2;
    public CSPlayerInitInfo _playerInitInfo;
    public CSInstanceInitInfo _instanceInitInfo;
    public CSPlayerLevelInitInfo _playerLevelInitInfo;
    public CSSpawnPlayer _spawnPlayer;
    public CSTownInstanceVerifyRsp _townInstanceVerifyRsp;
    public CSEnterInstanceRsp _enterInstanceRsp;
    public CSInstanceVerifyRsp _verifyRsp;
    public CSReselectRoleRsp _reselectRoleRsp;
    public CSPlayerAppearNtf _playerAppearNtf;


    public PlayerState(Client client)
    {
        _client = client;
        _roleBaseInfo = new CSRoleBaseInfo();
        _roleBaseInfo.RoleID = 1;
        _roleBaseInfo.RoleIndex = 0;
        _roleBaseInfo.Name = "Star";
        _roleBaseInfo.Gender = 1;
        _roleBaseInfo.AvatarSetID = 1;
        _roleBaseInfo.Level = 1;
        _roleBaseInfo.FaceId = 1;
        _roleBaseInfo.HairId = 1;
        _roleBaseInfo.UnderclothesId = 1;
        _roleBaseInfo.SkinColor = 1;
        _roleBaseInfo.HairColor = 1;
        _roleBaseInfo.InnerColor = 1;
        _roleBaseInfo.EyeBall = 1;
        _roleBaseInfo.EyeColor = 1;

        // _roleBaseInfo2 = new CSRoleBaseInfo();
        // _roleBaseInfo2.RoleID = 1;
        // _roleBaseInfo2.RoleIndex = 1;
        // _roleBaseInfo2.Name = "Moon";
        // _roleBaseInfo2.Gender = 1;
        // _roleBaseInfo2.Level = 1;
        // _roleBaseInfo2.FaceId = 1;
        // _roleBaseInfo2.HairId = 1;
        // _roleBaseInfo2.UnderclothesId = 1;
        // _roleBaseInfo2.SkinColor = 1;
        // _roleBaseInfo2.HairColor = 1;
        // _roleBaseInfo2.InnerColor = 1;
        // _roleBaseInfo2.EyeBall = 1;
        // _roleBaseInfo2.EyeColor = 1;

        _playerInitInfo = new CSPlayerInitInfo();
        _playerInitInfo.AccountID = 1;
        _playerInitInfo.NetID = 1;
        _playerInitInfo.DBId = 1;
        _playerInitInfo.SessionID = 1;
        _playerInitInfo.WorldID = 1;
        _playerInitInfo.ServerID = 1;
        _playerInitInfo.WorldSvrID = 1;
        _playerInitInfo.Name = _roleBaseInfo.Name;
        _playerInitInfo.Gender = _roleBaseInfo.Gender;
        _playerInitInfo.IsGM = 0;
        _playerInitInfo.AvatarSetID = _roleBaseInfo.AvatarSetID;
        _playerInitInfo.ParentEntityGUID = 0;
        _playerInitInfo.RandSeed = 0;
        _playerInitInfo.CatCuisineID = 0;
        _playerInitInfo.FirstEnterLevel = 1;
        _playerInitInfo.FirstEnterMap = 1;
        _playerInitInfo.PvpPrepareStageState = 0;
        _playerInitInfo.Pose.t.x = 409.91379f;
        _playerInitInfo.Pose.t.y = 358.74976f;
        _playerInitInfo.Pose.t.z = 100.0f; // height
        _playerInitInfo.Pose.q.v.x = 0;
        _playerInitInfo.Pose.q.v.y = 0;
        _playerInitInfo.Pose.q.v.z = 0;
        _playerInitInfo.Pose.q.w = 0;

        _instanceInitInfo = new CSInstanceInitInfo();
        _instanceInitInfo.BattleGroundID = 0;
        _instanceInitInfo.LevelID = 150301;
        _instanceInitInfo.CreateMaxPlayerCount = 4;
        _instanceInitInfo.GameMode = 99;
        _instanceInitInfo.TimeType = 1;
        _instanceInitInfo.WeatherType = 1;
        _instanceInitInfo.time = 1;
        _instanceInitInfo.LevelRandSeed = 1;
        _instanceInitInfo.WarningFlag = 0;
        _instanceInitInfo.CreatePlayerMaxLv = 99;

        _playerLevelInitInfo = new CSPlayerLevelInitInfo();

        _spawnPlayer = new CSSpawnPlayer();
        _spawnPlayer.PlayerId = 1;
        _spawnPlayer.NetObjId = 1;
        _spawnPlayer.Name = _roleBaseInfo.Name;
        _spawnPlayer.Gender = _roleBaseInfo.Gender;
        _spawnPlayer.Scale = 1.0f;
        _spawnPlayer.NewConnect = 0;
        _spawnPlayer.SendSrvId = 0;
        _spawnPlayer.AvatarSetID = _roleBaseInfo.AvatarSetID;

        _townInstanceVerifyRsp = new CSTownInstanceVerifyRsp();
        _townInstanceVerifyRsp.IntanceInitInfo = _instanceInitInfo;
        _townInstanceVerifyRsp.LineID = 0;
        _townInstanceVerifyRsp.LevelEnterType = 0;

        _enterInstanceRsp = new CSEnterInstanceRsp();
        _enterInstanceRsp.InstanceID = 0;
        _enterInstanceRsp.Key = "test";
        _enterInstanceRsp.BattleSvr = "127.0.0.1:8143";
        _enterInstanceRsp.RoleId = 0;
        _enterInstanceRsp.ServiceID = 0;
        _enterInstanceRsp.InstanceInfo = _instanceInitInfo;

        _verifyRsp = new CSInstanceVerifyRsp();
        _verifyRsp.ErrNo = 0;

        _reselectRoleRsp = new CSReselectRoleRsp();


        _playerAppearNtf = new CSPlayerAppearNtf();
        _playerAppearNtf.NetID = 1;
        _playerAppearNtf.SessionID = 1;
        _playerAppearNtf.Name = _roleBaseInfo.Name;
        _playerAppearNtf.Gender = _roleBaseInfo.Gender;
        _playerAppearNtf.Pose = _playerInitInfo.Pose;
        _playerAppearNtf.AvatarSetID = _roleBaseInfo.AvatarSetID;
    }

    /// <summary>
    /// Since FileCheck could occur multiple times during connection,
    /// i chose multi_net_ip info, hoping that its only once on game startup called.
    /// ideally want to only send this packet once when the game loaded.
    /// the problem is via the CsProto over TDPU connection, is that we do not get a tcp connection / disconnect event.
    /// but for future battle server connectivity weg get those, but we still need to be able to manage both the same way.
    /// </summary>
    public void OnReady()
    {
        SendListRoleRsp();
    }

    /// <summary>
    /// client selected a role to play, unsure as of yet what to reply.
    /// Based on logs we are hitting the right spots, but there is still more missing.
    /// </summary>
    public void OnRoleSelected()
    {
        _client.SendCsPacket(NewCsPacket.SelecteRoleRsp(new CSSelectRoleRsp()));
        SendTownSessionStart();
        SendPlayerInitNtf();
    }

    /// <summary>
    /// client used menu from level -> char selection
    /// </summary>
    public void OnRoleReSelected()
    {
        SendListRoleRsp();
    }

    /// <summary>
    /// Need to send after client has processed player init ntf, currently choose a random packet, that occurs after.
    /// Hoping it will not be asked again in the future.
    /// </summary>
    public void OnPlayerInitFinished()
    {
        SendTownServerInitNtf();
    }

    /// <summary>
    /// we have entered level, now need a way to assing a NetID for movement sync.
    /// </summary>
    public void OnEnterLevel()
    {
        SendPlayerLevelInitNtf();
      // _client.SendCsPacket(NewCsPacket.AssignId(new CSAssignPlayerId()
      // {
      //     PlayerId = 1
      // }));
      // //SendPlayerSpawn();

      // //_client.SendCsPacket(NewCsPacket.PlayerAppearNtf(_playerAppearNtf));
      // _client.SendCsPacket(NewCsPacket.NewPlayer(new CSSpawnNewPlayer()
      // {
      //     Name = _roleBaseInfo.Name
      // }));
    }

    /// <summary>
    /// in game press M-key -> select another town
    /// </summary>
    public void OnChangeTownInstance(CSChangeTownInstanceReq req)
    {
        _client.SendCsPacket(NewCsPacket.ChangeTownInstanceRsp(new CSChangeTownInstanceRsp()
        {
            ErrCode = 0,
            LevelID = req.LevelId
        }));
        _instanceInitInfo.LevelID = req.LevelId;
        SendTownServerInitNtf();
    }
    public void OnBattleSvr()
    {
        //SendPlayerLevelInitNtf();
        //SendTownServerInitNtf();
        //SendPlayerSpawn();
        //SendLoadLevelNtf();
        //SendInstanceInitNtf();
        //SendPlayerInitNtf();
    }

    public void OnCreateRole(CSRoleCreateInfo info)
    {
        CSListRoleRsp roleRsp = new CSListRoleRsp();
        _roleBaseInfo.Name = info.Name;
        _roleBaseInfo.Gender = info.Gender;
        _roleBaseInfo.FaceId = info.FaceId;
        _roleBaseInfo.HairId = info.HairId;
        _roleBaseInfo.UnderclothesId = info.UnderclothesId;
        _roleBaseInfo.SkinColor = info.SkinColor;
        _roleBaseInfo.HairColor = info.HairColor;
        _roleBaseInfo.InnerColor = info.InnerColor;
        _roleBaseInfo.EyeBall = info.EyeBall;
        _roleBaseInfo.EyeColor = info.EyeColor;
        _roleBaseInfo.FaceTattooIndex = info.FaceTattooIndex;
        _roleBaseInfo.FaceTattooColor = info.FaceTattooColor;
        _roleBaseInfo.FacialInfo = info.FacialInfo;
        roleRsp.RoleList.Role.Add(_roleBaseInfo);
        _client.SendCsPacket(NewCsPacket.CreateRoleRsp(roleRsp));
    }

    public void SendBruteForceT()
    {
        Thread bruteForce = new Thread(SendBruteForce);
        bruteForce.Start();
    }

    public void SendBruteForce()
    {
        Thread.Sleep(3000);

        List<string> exclude = new List<string>()
        {
            "CatTreatureErr",
            "CatTreatureOpen",
            "XHunterResultNtf",
            "CommanderChgNtf",
            "BattlePunishNtf",
            "ExcellentPointNtf",
            "SensitiveVerify",
            "SensitiveVerifyResult",
            "CatCuisineUnlockNtf",
            "C2STalkExec",
            "C2STalkEnd",
            "S2CTalkErr",
            "S2CTalkExec",
            "EquipPlanUnlockNtf",
            "EquipPlanEditNtf",
            "LineUpStateNtf",
            "GuildMatchSignUpReadyNtf",
            "GuildMatchSignUpListNtf",
            "GuildMatchSignUpAdd",
            "GuildMatchSignUpDel",
            "ScheduleError",
            "ObtainTargetListRes",
            "GuildMatchQualifierFirstNtf",
            "GuildMatchStateNtf",
            "GuildMatchPairListNtf",
            "PkgEncryptData",
            "Ping",
            "HeartBeat",
            "GameManagerCmd",
            "GlobalErrNtf",
            "PkgTimerRecord",
            "PkgTransAntiData",
            "PingReply",
            "PkgChatEncryptData",
            "ZipPkg",
            "DelBuff",
            "BuffParamChange",
            "NotifyInfo",
            "DropClientNotifyInfo",
            "PropSync",
            "TimeOfDayNtf",
            "AttrSync",
            "AttrInfo",
            "FarmSetEquipAvatarNtf",
            "FarmWoodIndexIDMappingNtf",
            "CatTreatureInfo",
            "C2SSpeakWord",
            "C2SSpeakSetSelfDef",
            "C2SSpeakSetAuto",
            "S2CSculptureLibSnapshot",
            "S2CGetSculptureLibs",
            "S2CSculptureErr",
            "S2CSpeakErr",
            "SurrenderVoteNtf",
            "C2SGetSculptureLibs",
            "S2CGetSculpture",
            "S2CSculptureAvatarSnapshot",
            "S2CScriptAddSculpture",
            "S2CSpoorFetchPrize",
            "S2CSpoorErr",
            "S2CSpoorAddPoints",
            "ItemMgrMoveSwapItemsNtf",
        };

        Type type = typeof(NewCsPacket);
        List<MethodInfo> collectedMethods = new List<MethodInfo>();
        foreach (MethodInfo mi in type.GetMethods())
        {
            if (exclude.Contains(mi.Name))
            {
                continue;
            }

            if (mi.Name == "Equals"
                || mi.Name == "GetType"
                || mi.Name == "ToString"
                || mi.Name == "GetHashCode")
            {
                continue;
            }

            if (mi.Name.EndsWith("Req"))
            {
                continue;
            }

            if (mi.Name.StartsWith("C2S"))
            {
                continue;
            }

            if (mi.Name.EndsWith("Rsp"))
            {
                continue;
            }

            collectedMethods.Add(mi);
        }


        //LevelIntegrateNtf
//2023-06-25 05:47:10 - Error - PlayerState: Sending S2CScriptActivityTimeUpdateNtf (28/206)

        int start = 80;
        int current = 1;
        int total = collectedMethods.Count;
        foreach (MethodInfo mi in collectedMethods)
        {
            if (current < start)
            {
                current++;
                continue;
            }

            ParameterInfo[] parameters = mi.GetParameters();
            if (parameters.Length != 1)
            {
                Logger.Error($"parameters.Length != 1 ({mi})");
                continue;
            }

            object parameterInstance = CreateInstance(parameters[0].ParameterType);
            object ret = mi.Invoke(null, new object[] { parameterInstance });
            if (ret is CsPacket csPacket)
            {
                Logger.Error($"Sending {mi.Name} ({current}/{total})");
                try
                {
                    _client.SendCsPacket(csPacket);
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                }

                Thread.Sleep(1000);
                current++;
            }
            else
            {
                Logger.Error($"ret is NOT CsPacket csPacket({mi})");
            }
        }
    }

    private object CreateInstance(Type type)
    {
        if (type.IsInterface)
        {
            Type concrete = null;
            foreach (Type t in Assembly.GetExecutingAssembly().GetExportedTypes())
            {
                if (!t.IsInterface && !t.IsAbstract && type.IsAssignableFrom(t))
                {
                    concrete = t;
                    break;
                }
            }

            if (concrete == null)
            {
                Logger.Error($"Interface ({type}) can not be created, no implementation found");
                return null;
            }

            Logger.Info($"Interface ({type}) can not be created, using implementation {concrete}");
            type = concrete;
        }

        ConstructorInfo[] cis = type.GetConstructors();

        if (cis.Length <= 0)
        {
            return Activator.CreateInstance(type);
        }

        ParameterInfo[] pis = cis[0].GetParameters();
        if (pis.Length <= 0)
        {
            return Activator.CreateInstance(type);
        }

        object[] paramInstances = new object[pis.Length];
        for (int i = 0; i < pis.Length; i++)
        {
            paramInstances[i] = CreateInstance(pis[i].ParameterType);
        }

        return Activator.CreateInstance(type, paramInstances);
    }

    public void SendListRoleRsp()
    {
        CSListRoleRsp roleRsp = new CSListRoleRsp();
        // if role list is empty, client will auto move to char creation
        roleRsp.RoleList.Role.Add(_roleBaseInfo);
        // roleRsp.RoleList.Role.Add(_roleBaseInfo2);
        _client.SendCsPacket(NewCsPacket.ListRoleRsp(roleRsp));
    }

    /// <summary>
    /// Changes the light settings of current scene
    /// </summary>
    public void SendTimeOfDayNtf()
    {
        CSTimeOfDayNtf timeOfDayNtf = new CSTimeOfDayNtf();
        timeOfDayNtf.time = 50;
        _client.SendCsPacket(NewCsPacket.TimeOfDayNtf(timeOfDayNtf));
    }

    public void SendTownSessionStart()
    {
        _client.SendCsPacket(NewCsPacket.TownSessionStart(new CSTownSessionStart()));
    }

    public void SendPlayerInitNtf()
    {
        _client.SendCsPacket(NewCsPacket.PlayerInitNtf(_playerInitInfo));
    }

    public void SendInstanceInitNtf()
    {
        _client.SendCsPacket(NewCsPacket.InstanceInitNtf(_instanceInitInfo));
    }

    public void SendPlayerLevelInitNtf()
    {
        _client.SendCsPacket(NewCsPacket.PlayerLevelInitNtf(_playerLevelInitInfo));
    }

    public void SendPlayerSpawn()
    {
        _client.SendCsPacket(NewCsPacket.SpawnPlayer(_spawnPlayer));
    }

    public void SendLoadLevelNtf()
    {
        _client.SendCsPacket(NewCsPacket.LoadLevelNtf(new CSLoadLevelNtf()));
    }

    public void SendTownServerInitNtf()
    {
        _client.SendCsPacket(NewCsPacket.TownServerInitNtf(_townInstanceVerifyRsp));
    }

    public void SendEnterInstanceRsp()
    {
        _client.SendCsPacket(NewCsPacket.EnterInstanceRsp(_enterInstanceRsp));
    }

    public void SendInstanceVerifyRsp()
    {
        _client.SendCsPacket(NewCsPacket.InstanceVerifyRsp(_verifyRsp));
    }

    public void SendReselectRoleRsp()
    {
        _client.SendCsPacket(NewCsPacket.ReselectRoleRsp(_reselectRoleRsp));
    }

}