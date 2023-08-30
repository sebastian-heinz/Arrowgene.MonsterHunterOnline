using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;
using Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

public class Inventory
{
    private const int NoFreeSlot = -1;
    private const int MaxItemSize = 100;

    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(Inventory));

    private readonly Item[] _item;
    private readonly Item[] _store;
    private readonly Item[] _equipment;
    private readonly Item[] _boxEquipment;
    private readonly object _lock;
    private readonly IDatabase _database;
    private readonly uint _characterId;

    public Inventory(uint characterId, IDatabase database)
    {
        _characterId = characterId;
        _database = database;
        _item = new Item[MaxItemSize];
        _store = new Item[MaxItemSize];
        _equipment = new Item[MaxItemSize];
        _boxEquipment = new Item[MaxItemSize];
        _lock = new object();

        List<Item> items = _database.SelectItemsByCharacterId(_characterId);
        foreach (Item item in items)
        {
            if (item.Id == null)
            {
                continue;
            }

            if (item.PosGrid == null)
            {
                continue;
            }

            Item[] collection = GetCollection(item.PosColumn);
            collection[item.PosGrid.Value] = item;
        }
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

    public bool Move(ulong id,
        ItemColumnType srcColumn,
        ushort srcGrid,
        ItemColumnType dstColumn,
        ushort dstGrid)
    {
        lock (_lock)
        {
            Item[] srcCollection = GetCollection(srcColumn);
            if (srcCollection == null)
            {
                return false;
            }

            if (srcGrid >= srcCollection.Length)
            {
                return false;
            }

            Item srcItem = srcCollection[srcGrid];
            if (srcItem == null)
            {
                return false;
            }

            if (srcItem.Id != id)
            {
                return false;
            }

            // check if dst is free
            Item[] dstCollection = GetCollection(dstColumn);
            if (dstCollection == null)
            {
                return false;
            }

            if (dstGrid >= dstCollection.Length)
            {
                return false;
            }

            if (dstCollection[dstGrid] != null)
            {
                // slot occupied
                return false;
            }

            // update item
            srcCollection[srcGrid] = null;
            dstCollection[dstGrid] = srcItem;
            srcItem.PosColumn = dstColumn;
            srcItem.PosGrid = dstGrid;

            if (!_database.UpdateItem(srcItem))
            {
                // commit failed, roll back
                srcCollection[srcGrid] = srcItem;
                dstCollection[dstGrid] = null;
                srcItem.PosColumn = srcColumn;
                srcItem.PosGrid = srcGrid;
                return false;
            }
        }

        return true;
    }

    public void PopulateItemListProperties(IItemListProperties properties)
    {
        lock (_lock)
        {
            properties.NormalSize = MaxItemSize;
            properties.MaterialStoreSize = MaxItemSize;
            properties.StoreSize = MaxItemSize;
            PopulateItemListProperties(properties.StoreItem, _store);
            PopulateItemListProperties(properties.EquipItem, _equipment);
            // TODO not sure about this
            // I think every tab of inventory goes into BagItem
            PopulateItemListProperties(properties.BagItem, _item);
            PopulateItemListProperties(properties.BagItem, _boxEquipment);
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
        if (item.CharacterId != _characterId)
        {
            // not our item
            return false;
        }

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

        if (!_database.CreateItem(item))
        {
            Remove(item);
            return false;
        }

        return true;
    }

    private bool Remove(Item item, Item[] collection)
    {
        if (item.CharacterId != _characterId)
        {
            // not our item
            return false;
        }

        if (item.Id != null)
        {
            // delete from db to ensure it is gone in any case
            _database.DeleteItem(item.ItemId);
        }

        if (item.PosGrid == null)
        {
            return false;
        }

        ushort grid = item.PosGrid.Value;

        if (grid >= collection.Length)
        {
            return false;
        }

        lock (_lock)
        {
            if (collection[grid] == null)
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
            case ItemColumnType.BoxEquip: return _boxEquipment;
            case ItemColumnType.Equipment: return _equipment;
            case ItemColumnType.Quest: return _item;
        }

        return null;
    }
}