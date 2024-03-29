﻿using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class C2SCmdActivityAddSecretQuestHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(C2SCmdActivityAddSecretQuestHandler));


    public CS_CMD_ID Cmd => CS_CMD_ID.C2S_CMD_ACTIVITY_ADDSECRETQUEST;


    public void Handle(Client client, CsProtoPacket packet)
    {
       // client.SendCsPacket(NewCsPacket.S2CActivityAddSecretQuest(new S2CActivityAddSecretQuest()));
    }
}