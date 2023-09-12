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

        instanceInitInfo.Structure.BattleGroundId = 0;
        instanceInitInfo.Structure.LevelId = client.State.MainInstanceLevelId;
        instanceInitInfo.Structure.CreateMaxPlayerCount = 4;
        instanceInitInfo.Structure.GameMode = GameMode.Casual;
        instanceInitInfo.Structure.TimeType = TimeType.Noon;
        instanceInitInfo.Structure.WeatherType = WeatherType.Sunny;
        instanceInitInfo.Structure.Time = 1;
        instanceInitInfo.Structure.LevelRandSeed = 1;
        instanceInitInfo.Structure.WarningFlag = 0;
        instanceInitInfo.Structure.CreatePlayerMaxLv = 99;
        

       // client.SendCsProtoStructurePacket(instanceInitInfo);
        
        
        CsCsProtoStructurePacket<TownInstanceVerifyRsp> townServerInitNtf = CsProtoResponse.TownServerInitNtf;
        townServerInitNtf.Structure.ErrNo = 0;
        townServerInitNtf.Structure.LineId = 0;
        townServerInitNtf.Structure.LevelEnterType = 0;
        townServerInitNtf.Structure.InstanceInitInfo = instanceInitInfo.Structure;
        client.SendCsProtoStructurePacket(townServerInitNtf);
        client.State.prevLevelId = client.State.levelId;
        client.State.levelId =  client.State.MainInstanceLevelId;

        CsCsProtoStructurePacket<PlayerTeleport> PlayerTeleport = CsProtoResponse.PlayerTeleport;
        PlayerTeleport.Structure.SyncTime = 1;
        //One day netobjid would not be the character id ?
        PlayerTeleport.Structure.NetObjId = client.Character.Id;
        PlayerTeleport.Structure.Region = client.State.MainInstanceLevelId;
        PlayerTeleport.Structure.TargetPos.t.x = 1681.2958f;
        PlayerTeleport.Structure.TargetPos.t.y = 346.80392f;
        PlayerTeleport.Structure.TargetPos.t.z = 205.375f;
        PlayerTeleport.Structure.ParentGuid = 1;
        PlayerTeleport.Structure.InitState = 1;
        client.SendCsProtoStructurePacket(PlayerTeleport);
        
    }
}