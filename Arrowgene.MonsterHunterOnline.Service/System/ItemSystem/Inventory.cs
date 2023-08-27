using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;
using Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

public class Inventory
{
    private Item[][] _item;
    private Item[][] _store;
    private Item[][] _box;
    private Item[] _equipment;
    private Item[][] _quest;

    public Inventory()
    {
    }


    public bool Add(Item item)
    {
        return true;
    }

    public bool Move(ulong reqItemId,
        ItemColumnType reqItemColumn,
        ushort reqItemGrid,
        ItemColumnType reqDstColumn,
        ushort reqDstGrid)
    {
        return true;
    }

    public void PopulateItemListProperties(IItemListProperties properties)
    {
        properties.NormalSize = 30;
        properties.MaterialStoreSize = 30;
        properties.StoreSize = 30;
        properties.EquipItem.UnknownA = 1;
        properties.EquipItem.UnknownB = 1;
        properties.EquipItem.Items.Add(new TlvItem()
        {
            ItemId = 1,
            PosColumn = ItemColumnType.Equipment,
            PosGridEquipment = ItemEquipmentType.Weapon,
            ItemType = 120005,
            Quantity = 1,
        });
        properties.EquipItem.Items.Add(new TlvItem()
        {
            ItemId = 2,
            PosColumn = ItemColumnType.Equipment,
            PosGridEquipment = ItemEquipmentType.Helmet,
            ItemType = 60011,
            Quantity = 1,
        });
        properties.BagItem.Items.Add(new TlvItem()
        {
            ItemId = 3,
            PosColumn = ItemColumnType.BoxEquip,
            PosGrid = 0,
            ItemType = 55784,
            Quantity = 1,
        });
        properties.BagItem.Items.Add(new TlvItem()
        {
            ItemId = 4,
            PosColumn = ItemColumnType.BoxEquip,
            PosGrid = 1,
            ItemType = 120006,
            Quantity = 1,
        });
    }
}