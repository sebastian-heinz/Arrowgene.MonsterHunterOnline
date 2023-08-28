using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.System.ClientAssetSystem;

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

    /// <summary>
    /// Adds specified item to client
    /// </summary>
    public bool AddItem(Client client, int itemId)
    {
        Inventory inventory = client.Inventory;
        if (inventory == null)
        {
            Logger.Error(client, "AddItem::inventory null");
            return false;
        }

        if (!_assets.Items.ContainsKey(itemId))
        {
            Logger.Error(client, "AddItem::item data not found");
            return false;
        }

        ItemInfo itemInfo = _assets.Items[itemId];

        Item item = MakeItem(itemInfo);
        if (item == null)
        {
            Logger.Error(client, "AddItem::failed to make item");
            return false;
        }

        if (!inventory.Add(item))
        {
            Logger.Error(client, "AddItem::failed to add item to inventory");
            return false;
        }

        CsProtoStructurePacket<ItemMgrAddItemNtf> itemMgrAddItemNtf = CsProtoResponse.ItemMgrAddItemNtf;
        itemMgrAddItemNtf.Structure.Reason = 0;
        ItemData itemData = new ItemData(item);
        itemMgrAddItemNtf.Structure.ItemList.Add(itemData);
        client.SendCsProtoStructurePacket(itemMgrAddItemNtf);

        return true;
    }

    public Item MakeItem(ItemInfo itemInfo)
    {
        Item item = new Item(itemInfo);
        return item;
    }
}