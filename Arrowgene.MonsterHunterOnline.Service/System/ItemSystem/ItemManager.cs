using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;
using Arrowgene.MonsterHunterOnline.Service.System.ClientAssetSystem;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

public class ItemManager
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(ItemManager));

    private readonly IDatabase _database;
    private readonly AssetRepository _assets;

    public ItemManager(IDatabase database, AssetRepository assets)
    {
        _database = database;
        _assets = assets;
    }

    public Inventory GetInventory(uint characterId)
    {
        List<Item> items = _database.SelectItemsByCharacterId(characterId);
        Inventory inventory = new Inventory(items);
        return inventory;
    }

    /// <summary>
    /// Adds specified item to client
    /// </summary>
    public bool AddItem(Client client, uint itemId)
    {
        Inventory inventory = client.Inventory;
        if (inventory == null)
        {
            Logger.Error(client, "AddItem::inventory null");
            return false;
        }

        Character character = client.Character;
        if (character == null)
        {
            Logger.Error(client, "AddItem::character null");
            return false;
        }

        Item item = MakeItem(character.Id, itemId, inventory);
        if (item == null)
        {
            Logger.Error(client, "AddItem::failed to make item");
            return false;
        }

        CsCsProtoStructurePacket<ItemMgrAddItemNtf> itemMgrAddItemNtf = CsProtoResponse.ItemMgrAddItemNtf;
        // TODO check if replay with error code for client works
        itemMgrAddItemNtf.Structure.Reason = 0;
        itemMgrAddItemNtf.Structure.ItemList.Add(new ItemData(item));
        client.SendCsProtoStructurePacket(itemMgrAddItemNtf);

        return true;
    }

    /// <summary>
    /// Brings a item into existence.
    /// </summary>
    public Item MakeItem(uint characterId, uint itemId, Inventory inventory)
    {
        if (!_assets.Items.ContainsKey(itemId))
        {
            Logger.Error("MakeItem::item data not found");
            return null;
        }

        ItemInfo itemInfo = _assets.Items[itemId];


        Item item = new Item();
        switch (itemInfo.MainClass)
        {
            case ItemClass.Item:
            {
                // TODO check Quest type etc..
                item.PosColumn = ItemColumnType.Item;
                break;
            }
            case ItemClass.Equipment:
            {
                item.PosColumn = ItemColumnType.BoxEquip;
                break;
            }
            default:
            {
                return null;
            }
        }

        item.CharacterId = characterId;
        item.CreatedBy = $"{characterId}";
        item.Quantity = 1;
        item.ItemId = itemId;
        inventory.Add(item);
        if (!_database.CreateItem(item))
        {
            inventory.Remove(item);
            return null;
        }

        return item;
    }
}