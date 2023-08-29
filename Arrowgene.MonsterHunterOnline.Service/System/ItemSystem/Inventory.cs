using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;
using Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

public class Inventory
{
    private const int NoFreeSlot = -1;
    private const int MaxItemSize = 100;

    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(Inventory));

    private Item[] _item;
    private Item[] _store;
    private Item[] _equipment;
    private object _lock;
    
    public Inventory()
    {
        _item = new Item[MaxItemSize];
        _store = new Item[MaxItemSize];
        _equipment = new Item[MaxItemSize];
        _lock = new object();
    }
    
    public bool Add(Item item)
    {
        lock (_lock)
        {
            switch (item.PosColumn)
            {
                case ItemColumnType.Item: return Add(item, _item);
                case ItemColumnType.Store: return Add(item, _store);
                case ItemColumnType.BoxEquip:  return Add(item, _equipment);
                case ItemColumnType.Equipment: return Add(item, _equipment);
                case ItemColumnType.Quest: return Add(item, _item);
            }

            return false;
        }
    }

    public bool Move(ulong itemId,
        ItemColumnType srcColumn,
        ushort srcGrid,
        ItemColumnType dstColumn,
        ushort dstGrid)
    {
        return true;
    }

    public void PopulateItemListProperties(IItemListProperties properties)
    {
        lock (_lock)
        {
            properties.NormalSize = MaxItemSize;
            properties.MaterialStoreSize = MaxItemSize;
            properties.StoreSize = MaxItemSize;
            PopulateItemListProperties(properties.EquipItem, _equipment);
            // TODO not sure about this
            PopulateItemListProperties(properties.BagItem, _item);
            PopulateItemListProperties(properties.StoreItem, _store);
        }
    }

    private void PopulateItemListProperties(TlvItemList itemList, Item[] collection)
    {
        itemList.UnknownA = 0;
        itemList.UnknownB = 0;
        for (int i = 0; i < MaxItemSize; i++)
        {
            Item item = collection[i];
            if (item == null)
            {
                continue;
            }

            itemList.Items.Add(item);
        }
    }

    private bool Add(Item item, Item[] collection)
    {
        int freeSlot = GetFreeSlot(collection);
        if (freeSlot == NoFreeSlot)
        {
            Logger.Error("no free slot");
            return false;
        }

        // TODO
        collection[freeSlot] = item;
        item.Id = (uint)freeSlot;
        item.PosGrid = (ushort)freeSlot;
        return true;
    }

    private int GetFreeSlot(Item[] collection)
    {
        for (int i = 0; i < collection.Length; i++)
        {
            if (collection[i] == null)
            {
                return i;
            }
        }

        return NoFreeSlot;
    }
}