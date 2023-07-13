using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdWorldAccountReqHandler : CsProtoStructureHandler<AccountReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdWorldAccountReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_WORLD_ACCOUNT_REQ;


    public CsCmdWorldAccountReqHandler()
    {

    }

    public override void Handle(Client client, AccountReq req)
    {
        CsProtoStructurePacket<AccountRsp> rsp = CsProtoResponse.AccountRsp;
        rsp.Structure.FaceCount = 1;
        rsp.Structure.ChgSexCount = 1;
        rsp.Structure.Result = 0;
        client.SendCsProtoStructurePacket(rsp);
    }
}