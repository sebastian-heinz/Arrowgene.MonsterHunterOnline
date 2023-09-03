using System.Threading;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class MainInstanceAgreeOptReqHandler : CsProtoStructureHandler<MainInstanceAgreeOptReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(MainInstanceAgreeOptReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_MAIN_INSTANCE_AGREE_OPT_REQ;

    public MainInstanceAgreeOptReqHandler()
    {
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

        enterInstanceCountDown.Structure.Second = 20;
        enterInstanceCountDown.Structure.LevelId = client.State.MainInstanceLevelId;
        client.SendCsProtoStructurePacket(enterInstanceCountDown);

        // TODO dirty hack
        Thread.Sleep(1000);

        CsCsProtoStructurePacket<InstanceInitInfo>
            instanceInitInfo = CsProtoResponse.InstanceInitInfo;
        
        instanceInitInfo.Structure.BattleGroundId = 0;
        instanceInitInfo.Structure.LevelId = client.State.MainInstanceLevelId;
        instanceInitInfo.Structure.CreateMaxPlayerCount = 4;
        instanceInitInfo.Structure.GameMode = GameMode.Town;
        instanceInitInfo.Structure.TimeType = TimeType.Noon;
        instanceInitInfo.Structure.WeatherType = WeatherType.Sunny;
        instanceInitInfo.Structure.Time = 1;
        instanceInitInfo.Structure.LevelRandSeed = 1;
        instanceInitInfo.Structure.WarningFlag = 0;
        instanceInitInfo.Structure.CreatePlayerMaxLv = 99;

        client.SendCsProtoStructurePacket(instanceInitInfo);
    }
}