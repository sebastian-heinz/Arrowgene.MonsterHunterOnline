using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class WorldAccountReqHandler : CsProtoStructureHandler<AccountReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(WorldAccountReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_WORLD_ACCOUNT_REQ;


    public WorldAccountReqHandler()
    {
    }

    public override void Handle(Client client, AccountReq req)
    {
        CsCsProtoStructurePacket<AccountRsp> rsp = CsProtoResponse.AccountRsp;
        rsp.Structure.FaceCount = 1;
        rsp.Structure.ChgSexCount = 1;
        rsp.Structure.Result = 0;
        client.SendCsProtoStructurePacket(rsp);
    }
}