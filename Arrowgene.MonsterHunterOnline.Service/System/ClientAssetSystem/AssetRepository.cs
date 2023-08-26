﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;

namespace Arrowgene.MonsterHunterOnline.Service.System.ClientAssetSystem;

public class AssetRepository
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(AssetRepository));

    private DirectoryInfo _directory;

    public AssetRepository()
    {
        _items = new Dictionary<ulong, ItemData>();
    }

    public ReadOnlyDictionary<ulong, ItemData> Items => _items.AsReadOnly();
    private readonly Dictionary<ulong, ItemData> _items;

    public void Initialize(string folder)
    {
        _directory = new DirectoryInfo(folder);
        if (!_directory.Exists)
        {
            Logger.Error($"Could not initialize repository, '{folder}' does not exist");
            return;
        }

        LoadEquipment();
        LoadItems();
    }

    private void LoadEquipment()
    {
        List<ItemData> items = new List<ItemData>();
        Load(items, "Static/equipdata.dat_双刀.csv", new EquipDataCsv());
        Load(items, "Static/equipdata.dat_大剑.csv", new EquipDataCsv());
        Load(items, "Static/equipdata.dat_太刀.csv", new EquipDataCsv());
        Load(items, "Static/equipdata.dat_弓.csv", new EquipDataCsv());
        Load(items, "Static/equipdata.dat_弩.csv", new EquipDataCsv());
        Load(items, "Static/equipdata.dat_斩斧.csv", new EquipDataCsv());
        Load(items, "Static/equipdata.dat_测试.csv", new EquipDataCsv());
        Load(items, "Static/equipdata2.dat_片手.csv", new EquipDataCsv());
        Load(items, "Static/equipdata2.dat_狩猎笛.csv", new EquipDataCsv());
        Load(items, "Static/equipdata2.dat_铳枪.csv", new EquipDataCsv());
        Load(items, "Static/equipdata2.dat_锤.csv", new EquipDataCsv());
        Load(items, "Static/equipdata2.dat_长枪.csv", new EquipDataCsv());
        Load(items, "Static/equipdata_armor.dat_防具.csv", new EquipDataCsv());
        Load(items, "Static/equipdata_clothes.dat_时装.csv", new EquipDataCsv());
        Load(items, "Static/equipdata_jewelry.dat_首饰.csv", new EquipDataCsv());
        foreach (ItemData item in items)
        {
            _items.Add(item.ItemId, item);
        }
    }

    private void LoadItems()
    {
        List<ItemData> items = new List<ItemData>();
        Load(items, "Static/itemdata.dat_1消耗品.csv", new ItemDataCsv());
        Load(items, "Static/itemdata.dat_4弹药.csv", new ItemDataCsv());
        Load(items, "Static/itemdata.dat_7宝石.csv", new ItemDataCsv());
        Load(items, "Static/itemdata.dat_9农场.csv", new ItemDataCsv());
        Load(items, "Static/itemdata.dat_10护身符.csv", new ItemDataCsv());
        Load(items, "Static/itemdata.dat_11礼物.csv", new ItemDataCsv());
        Load(items, "Static/itemdata_legendpearl.dat_传奇技能珠.csv", new ItemDataCsv());
        Load(items, "Static/itemdata_levelgroup.dat_1消耗品.csv", new ItemDataCsv());
        Load(items, "Static/itemdata_levelgroup.dat_4弹药.csv", new ItemDataCsv());
        Load(items, "Static/itemdata_mart.dat_商城道具.csv", new ItemDataCsv());
        Load(items, "Static/itemdata_material.dat_素材.csv", new ItemDataCsv());
        Load(items, "Static/itemdata_quest.dat_任务道具.csv", new ItemDataCsv());
        Load(items, "Static/itemdata_skillpearl.dat_技能珠.csv", new ItemDataCsv());
        Load(items, "Static/itemdata_tool.dat_工具.csv", new ItemDataCsv());
        foreach (ItemData item in items)
        {
            _items.Add(item.ItemId, item);
        }
    }

    private void Load<T>(List<T> list, string subPath, IAssetDeserializer<T> readerWriter)
    {
        string path = Path.Combine(_directory.FullName, subPath);
        FileInfo file = new FileInfo(path);
        if (!file.Exists)
        {
            Logger.Error($"Could not load '{path}', file does not exist");
        }

        List<T> assets = readerWriter.ReadPath(file.FullName);
        list.AddRange(assets);
    }
}