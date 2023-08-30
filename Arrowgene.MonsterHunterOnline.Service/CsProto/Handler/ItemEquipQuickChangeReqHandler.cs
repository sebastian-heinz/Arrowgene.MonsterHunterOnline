using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ItemEquipQuickChangeReqHandler : CsProtoStructureHandler<QuickChangeEquipReq>
{
    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_ITEM_EQUIP_QUICK_CHANGE_REQ;

    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ItemEquipQuickChangeReqHandler));

    public override void Handle(Client client, QuickChangeEquipReq req)
    {
        Inventory inventory = client.Inventory;
        if (inventory == null)
        {
            Logger.Error(client, "inventory null");
            return;
        }

        // not sure how to do this, lets just move them around
        foreach (EquipParam equipParam in req.EquipParamList)
        {
            CsCsProtoStructurePacket<ItemMgrMoveItemNtf> itemMgrMoveItemNtf = CsProtoResponse.ItemMgrMoveItemNtf;

            if (!inventory.Move(
                    equipParam.ItemId,
                    equipParam.SrcColumn,
                    (ushort)equipParam.SrcGrid,
                    equipParam.DstColumn,
                    (ushort)equipParam.DstGrid
                ))
            {
                Logger.Error(client, $"failed to move item {equipParam.ItemId}");
                return;
            }

            Logger.Info(req.JsonDump());

            itemMgrMoveItemNtf.Structure.ItemId = equipParam.ItemId;
            itemMgrMoveItemNtf.Structure.ItemColumn = equipParam.SrcColumn;
            itemMgrMoveItemNtf.Structure.ItemGrid = (ushort)equipParam.SrcGrid;
            itemMgrMoveItemNtf.Structure.DstColumn = equipParam.DstColumn;
            itemMgrMoveItemNtf.Structure.DstGrid = (ushort)equipParam.DstGrid;
            client.SendCsProtoStructurePacket(itemMgrMoveItemNtf);
        }
    }
}