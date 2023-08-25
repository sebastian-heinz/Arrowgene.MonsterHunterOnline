using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ItemMgrMoveItemHandler : CsProtoStructureHandler<ItemMgrMoveItemReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ItemMgrMoveItemHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_ITEMMGR_MOVE_ITEM_REQ;

    public ItemMgrMoveItemHandler()
    {
    }

    public override void Handle(Client client, ItemMgrMoveItemReq req)
    {
 
    }
}