﻿using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdVipServiceExpireReqHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdVipServiceExpireReqHandler));


    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_VIP_SERVICE_EXPIRE_REQ;


    public void Handle(Client client, CsProtoPacket packet)
    {
        client.SendCsPacket(NewCsPacket.S2CVIPServiceExpireRsp(new CSVIPServiceExpireRsp()));
    }
}