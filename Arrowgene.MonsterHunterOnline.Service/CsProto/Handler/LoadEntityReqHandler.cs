using System.Globalization;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class LoadEntityReqHandler : CsProtoStructureHandler<LoadEntityReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(LoadEntityReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_LOAD_ENTITY_REQ;


    public LoadEntityReqHandler()
    {
    }

    public override void Handle(Client client, LoadEntityReq req)
    {
        Logger.Debug(req.JsonDump());


        LogicEntityId id = new LogicEntityId();
        id.Id = req.LogicEntityId[0];
        id.Type = (LogicEntityType)req.LogicEntityType[0];


        SceneObjAppearNtf ntf = new SceneObjAppearNtf();
        ntf.NetId = 39002;
        ntf.EntityName = "UncleMerchant";
        ntf.ClassName = "EmCommon";
        ntf.Pose.t = client.State.Position;
        ntf.SubTypeId = 1;
        ntf.Sync2CE = 0;
        ntf.SpawnType = 0; // Spawn type, 0 absolute position, 1 relative bone position
        ntf.Bone = 0;
        ntf.Holder = 0;
        ntf.Owner = 0;
        ntf.Faction = 0;
        ntf.RegionId = 0;
        //ntf.UsrData = id.Id;
        //  ntf.EntGuid = ulong.Parse("44454658350C7106", NumberStyles.HexNumber);
        ntf.PropertityFile = "";
        ntf.MHSpawnType = 0;
        ntf.BTState = "";
        // ntf.BBVars = id.Id;
        //  ntf.Buff = id.Id;
        ntf.ParentId = 63;
        ntf.ParentGuid = ulong.Parse("44454658350C7105", NumberStyles.HexNumber);


        CsCsProtoStructurePacket<SceneObjAppearNtfList> sceneObjAppearNtfList = CsProtoResponse.SceneObjAppearNtfList;
        sceneObjAppearNtfList.Structure.Appear.Add(ntf);


        client.SendCsProtoStructurePacket(sceneObjAppearNtfList);


        return;
        CsCsProtoStructurePacket<MonsterAppearNtf> monsterAppearNtf = CsProtoResponse.MonsterAppearNtf;
        monsterAppearNtf.Structure.NetId = (int)50080;
        monsterAppearNtf.Structure.SpawnType = (short)2;
        monsterAppearNtf.Structure.MonsterInfoId = 50080;
        monsterAppearNtf.Structure.Name = "M008_RaptorCrimson";
        monsterAppearNtf.Structure.Class = "EmCommon";
        monsterAppearNtf.Structure.EntGuid = 12345;
        monsterAppearNtf.Structure.Pose.t = client.State.Position;

        monsterAppearNtf.Structure.LcmState.MonsterID = 50080;
        monsterAppearNtf.Structure.LcmState.MonsterPos = client.State.Position;
        monsterAppearNtf.Structure.LcmState.TargetSrvID = 1;
        monsterAppearNtf.Structure.LcmState.SyncTime = 0;

        //   Logger.Debug(monsterAppearNtf.Structure.JsonDump());
        CsCsProtoStructurePacket<MonsterAppearNtfList> monsterAppearNtfList = CsProtoResponse.MonsterAppearNtfList;
        monsterAppearNtfList.Structure.Appear.Add(monsterAppearNtf.Structure);
        client.SendCsProtoStructurePacket(monsterAppearNtfList);
    }
}