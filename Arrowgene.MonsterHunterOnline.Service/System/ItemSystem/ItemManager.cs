using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.System.ClientAssetSystem;

namespace Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

public class ItemManager
{
    private readonly IDatabase _database;
    private readonly AssetRepository _assets;

    public ItemManager(IDatabase database, AssetRepository assets)
    {
        _database = database;
        _assets = assets;
    }
}