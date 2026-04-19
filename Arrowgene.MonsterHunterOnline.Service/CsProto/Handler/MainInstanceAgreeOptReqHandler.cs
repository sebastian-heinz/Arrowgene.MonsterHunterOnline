using System.Threading;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Constant;
using Arrowgene.MonsterHunterOnline.Protocol.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class MainInstanceAgreeOptReqHandler : CsProtoStructureHandler<MainInstanceAgreeOptReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(MainInstanceAgreeOptReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_MAIN_INSTANCE_AGREE_OPT_REQ;

    private readonly Setting _setting;
    
    public MainInstanceAgreeOptReqHandler(Setting setting)
    {
        _setting = setting;
    }

    public override void Handle(Client client, MainInstanceAgreeOptReq req)
    {
        CsCsProtoStructurePacket<MainInstanceAgreeOptRsp> rsp = CsProtoResponse.MainInstanceAgreeOptRsp;
        rsp.Structure.Reason = 0;
        rsp.Structure.LevelId = client.State.MainInstanceLevelId;
        rsp.Structure.NetId = (int)client.Character.Id;
        rsp.Structure.Agree = req.Agree;
        rsp.Structure.RoleName = client.Character.Name;
        client.SendCsProtoStructurePacket(rsp);

        CsCsProtoStructurePacket<EnterInstanceCountDown>
            enterInstanceCountDown = CsProtoResponse.EnterInstanceCountDown;

        enterInstanceCountDown.Structure.Second = 5;
        enterInstanceCountDown.Structure.LevelId = client.State.MainInstanceLevelId;
        client.SendCsProtoStructurePacket(enterInstanceCountDown);

        // TODO dirty hack
        Thread.Sleep(5000);

        //client.SendCsPacket(NewCsPacket.MainInstanceClose(new CSMainInstanceClose()
        //    {
        //        LevelID = client.State.levelId,
        //        RoomID = 0,
        //        Reason = 0,
        //        TriggerNetID = client.Character.Id,
        //        RoleName = client.Character.Name,
        //    }
        //));

        CsCsProtoStructurePacket<InstanceInitInfo>
            instanceInitInfo = CsProtoResponse.InstanceInitInfo;

        instanceInitInfo.Structure.BattleGroundId = 1;
        instanceInitInfo.Structure.LevelId = client.State.MainInstanceLevelId;
        instanceInitInfo.Structure.CreateMaxPlayerCount = 4;
        instanceInitInfo.Structure.GameMode = GameMode.Standard;
        instanceInitInfo.Structure.TimeType = TimeType.Noon;
        instanceInitInfo.Structure.WeatherType = WeatherType.Sunny;
        instanceInitInfo.Structure.Time = 1;
        instanceInitInfo.Structure.LevelRandSeed = 1;
        instanceInitInfo.Structure.WarningFlag = 0;
        instanceInitInfo.Structure.CreatePlayerMaxLv = 99;

        //CsCsProtoStructurePacket<TownInstanceVerifyRsp> townServerInitNtf = CsProtoResponse.TownServerInitNtf;
        //townServerInitNtf.Structure.ErrNo = 0;
        //townServerInitNtf.Structure.LineId = 0;
        //townServerInitNtf.Structure.LevelEnterType = 0;
        //townServerInitNtf.Structure.InstanceInitInfo = instanceInitInfo.Structure;
        // client.SendCsProtoStructurePacket(townServerInitNtf);

        //client.State.prevLevelId = client.State.levelId;
        //client.State.levelId = client.State.MainInstanceLevelId;
        client.State.prevLevelId = client.State.MainInstanceLevelId;
        client.State.levelId = client.State.InitLevelId;

        CsCsProtoStructurePacket<EnterInstanceRsp> enterInstanceRsp = CsProtoResponse.EnterInstanceRsp;
        enterInstanceRsp.Structure.ErrNo = 0;
        enterInstanceRsp.Structure.RoleId = (int)client.Character.Id;
        enterInstanceRsp.Structure.InstanceId = 1;
        enterInstanceRsp.Structure.BattleSvr = $"127.0.0.1:{_setting.BattleServerPort}";
        //enterInstanceRsp.Structure.ServiceId = 1;
        enterInstanceRsp.Structure.ServiceId = client.State.MainInstanceLevelId; //Hack to pass the level ID to InstanceVerifyReqHandler
        enterInstanceRsp.Structure.Key = "BtlSvr01";
        enterInstanceRsp.Structure.InstanceInfo = instanceInitInfo.Structure;
        enterInstanceRsp.Structure.SameBS = 1;
        enterInstanceRsp.Structure.CrossRegion = 0;
        enterInstanceRsp.Structure.MatchRoom = 0;
        client.SendCsProtoStructurePacket(enterInstanceRsp);
    }
}