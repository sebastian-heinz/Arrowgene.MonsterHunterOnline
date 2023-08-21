using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class MainInstanceOptSynReqHandler : CsProtoStructureHandler<MainInstanceOptSynReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(MainInstanceOptSynReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_MAIN_INSTANCE_OPT_SYN_REQ;


    public MainInstanceOptSynReqHandler()
    {
    }

    public override void Handle(Client client, MainInstanceOptSynReq req)
    {
        CsProtoStructurePacket<MainInstanceOptSynRsp> rsp = CsProtoResponse.MainInstanceOptSynRsp;
        rsp.Structure.TriggerId = req.TriggerId;
        rsp.Structure.InstancePoint = req.InstancePoint;
        rsp.Structure.LevelId = req.LevelId;
        rsp.Structure.LevelDiff = req.LevelDiff;
        rsp.Structure.LevelMode = req.LevelMode;
        client.SendCsProtoStructurePacket(rsp);
    }
}