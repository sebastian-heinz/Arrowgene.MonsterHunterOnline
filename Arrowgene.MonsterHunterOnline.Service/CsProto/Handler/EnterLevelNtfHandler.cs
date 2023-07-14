using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;

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
        List<AttrSync> attrs = _characterManager.GetAllAttrSync(client, client.Character);
        List<List<AttrSync>> attrChunks = Util.Chunk(attrs, CsProtoConstant.CS_ATTR_SYNC_LIST_MAX);

        foreach (List<AttrSync> attrChunk in attrChunks)
        {
            if (attrChunk.Count > CsProtoConstant.CS_ATTR_SYNC_LIST_MAX)
            {
                Logger.Error(client, "Chunk error");
            }

            CsProtoStructurePacket<AttrSyncList> attrSyncList = CsProtoResponse.AttrSyncList;
            for (int i = 0; i < attrChunk.Count; i++)
            {
                attrSyncList.Structure.Attr.Add(attrChunk[i]);
            }

            client.SendCsProtoStructurePacket(attrSyncList);
        }
    }
}