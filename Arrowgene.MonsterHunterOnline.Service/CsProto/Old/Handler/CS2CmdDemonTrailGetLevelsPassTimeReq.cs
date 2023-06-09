﻿using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CS2CmdDemonTrailGetLevelsPassTimeReq : ICsProtoHandler
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CS2CmdDemonTrailGetLevelsPassTimeReq));


    public CS_CMD_ID Cmd => CS_CMD_ID.C2S_CMD_DEMON_TRIAL_GET_LEVELS_PASS_TIME_REQ;


    public void Handle(Client client, CsProtoPacket packet)
    {
        client.SendCsPacket(NewCsPacket.DemonTrialGetLevelPassTimeRsp(new SCDemonTrialGetLevelPassTimeRsp()));
    }
}