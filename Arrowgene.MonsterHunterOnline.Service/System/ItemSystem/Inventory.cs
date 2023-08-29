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
        Item[] collection = GetCollection(item.PosColumn);
        if (collection == null)
        {
            return false;
        }

        return Add(item, collection);
    }

    public bool Remove(Item item)
    {
        Item[] collection = GetCollection(item.PosColumn);
        if (collection == null)
        {
            return false;
        }

        return Remove(item, collection);
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
        if (item.PosGrid != null)
        {
            return false;
        }
        
        int freeSlot;
        lock (_lock)
        {
            freeSlot = GetFreeSlot(collection);
            if (freeSlot == NoFreeSlot)
            {
                Logger.Error("no free slot");
                return false;
            }

            collection[freeSlot] = item;
        }

        item.PosGrid = (ushort)freeSlot;
        return true;
    }

    private bool Remove(Item item, Item[] collection)
    {
        if (item.PosGrid == null)
        {
            return false;
        }

        ushort grid = item.PosGrid.Value;

        if (item.PosGrid >= collection.Length)
        {
            return false;
        }

        lock (_lock)
        {
            if (collection[item.PosGrid.Value] == null)
            {
                return false;
            }

            if (collection[grid] != item)
            {
                return false;
            }

            collection[grid] = null;
        }

        item.PosGrid = null;
        return true;
    }

    private int GetFreeSlot(Item[] collection)
    {
        lock (_lock)
        {
            for (int i = 0; i < collection.Length; i++)
            {
                if (collection[i] == null)
                {
                    return i;
                }
            }
        }

        return NoFreeSlot;
    }

    private Item[] GetCollection(ItemColumnType column)
    {
        switch (column)
        {
            case ItemColumnType.Item: return _item;
            case ItemColumnType.Store: return _store;
            case ItemColumnType.BoxEquip: return _equipment;
            case ItemColumnType.Equipment: return _equipment;
            case ItemColumnType.Quest: return _item;
        }

        return null;
    }
}