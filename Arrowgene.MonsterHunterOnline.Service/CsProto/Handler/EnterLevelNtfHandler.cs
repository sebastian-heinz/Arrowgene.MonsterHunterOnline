using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class EnterLevelNtfHandler : CsProtoStructureHandler<EnterLevelNtf>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(EnterLevelNtfHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_ENTER_LEVEL_NTF;

    private readonly CharacterManager _characterManager;

    public EnterLevelNtfHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, EnterLevelNtf req)
    {
       // _characterManager.SyncAllAttr(client);
    }
}