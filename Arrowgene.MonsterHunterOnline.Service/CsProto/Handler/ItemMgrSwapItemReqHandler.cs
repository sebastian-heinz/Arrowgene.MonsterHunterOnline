using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ItemMgrSwapItemReqHandler : CsProtoStructureHandler<ItemMgrSwapItemReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ItemMgrSwapItemReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_ITEMMGR_SWAP_ITEM_REQ;

    public override void Handle(Client client, ItemMgrSwapItemReq req)
    {
        Inventory inventory = client.Inventory;
        if (inventory == null)
        {
            Logger.Error(client, "inventory null");
            return;
        }

        CsCsProtoStructurePacket<ItemMgrSwapItemNtf> itemMgrSwapItemNtf = CsProtoResponse.ItemMgrSwapItemNtf;

        if (!inventory.Swap(
                req.ItemId,
                req.ItemColumn,
                req.ItemGrid,
                req.DstItemId,
                req.DstColumn,
                req.DstGrid
            ))
        {
            Logger.Error(client, $"failed to swap item {req.ItemId}");
            return;
        }

        Logger.Info(req.JsonDump());

        itemMgrSwapItemNtf.Structure.ItemId = req.ItemId;
        itemMgrSwapItemNtf.Structure.ItemColumn = req.ItemColumn;
        itemMgrSwapItemNtf.Structure.ItemGrid = req.ItemGrid;
        itemMgrSwapItemNtf.Structure.DstColumn = req.DstColumn;
        itemMgrSwapItemNtf.Structure.DstGrid = req.DstGrid;
        client.SendCsProtoStructurePacket(itemMgrSwapItemNtf);
    }
}