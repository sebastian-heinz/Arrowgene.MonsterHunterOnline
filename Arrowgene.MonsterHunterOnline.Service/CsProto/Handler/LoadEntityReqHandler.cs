using System;
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