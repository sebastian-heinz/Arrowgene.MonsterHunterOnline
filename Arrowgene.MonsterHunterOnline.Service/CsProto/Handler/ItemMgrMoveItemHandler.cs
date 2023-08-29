using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ItemMgrMoveItemHandler : CsProtoStructureHandler<ItemMgrMoveItemReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ItemMgrMoveItemHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_ITEMMGR_MOVE_ITEM_REQ;

    public override void Handle(Client client, ItemMgrMoveItemReq req)
    {
        Inventory inventory = client.Inventory;
        if (inventory == null)
        {
            Logger.Error(client, "inventory null");
            return;
        }

        CsCsProtoStructurePacket<ItemMgrMoveItemNtf> itemMgrMoveItemNtf = CsProtoResponse.ItemMgrMoveItemNtf;

        if (!inventory.Move(
                (ulong)req.ItemId,
                req.ItemColumn,
                req.ItemGrid,
                req.DstColumn,
                req.DstGrid
            ))
        {
            Logger.Error(client, $"failed to move item {req.ItemId}");
            return;
        }
        
        Logger.Info(req.JsonDump());

        itemMgrMoveItemNtf.Structure.ItemId = req.ItemId;
        itemMgrMoveItemNtf.Structure.ItemColumn = req.ItemColumn;
        itemMgrMoveItemNtf.Structure.ItemGrid = req.ItemGrid;
        itemMgrMoveItemNtf.Structure.DstColumn = req.DstColumn;
        itemMgrMoveItemNtf.Structure.DstGrid = req.DstGrid;
        client.SendCsProtoStructurePacket(itemMgrMoveItemNtf);
    }
}