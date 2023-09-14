using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

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

        client.SendCsProtoStructurePacket(townServerInitNtf);
        client.State.prevLevelId = client.State.levelId;
        client.State.levelId = instanceInitInfo.LevelId;
    }
}