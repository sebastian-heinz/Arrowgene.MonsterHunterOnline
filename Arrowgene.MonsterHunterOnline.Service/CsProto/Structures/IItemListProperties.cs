using System.Collections.Generic;
using Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public interface IItemListProperties
{
    /// <summary>
    /// 仓库大小
    /// </summary>
    public ushort StoreSize { get; set; }

    /// <summary>
    /// 普通背包容量大小
    /// </summary>
    public ushort NormalSize { get; set; }

    /// <summary>
    /// 素材仓库大小
    /// </summary>
    public ushort MaterialStoreSize { get; set; }

    /// <summary>
    /// 背包道具数据
    /// </summary>
    public TlvItemList BagItem { get; }

    /// <summary>
    /// 装备数据
    /// </summary>
    public TlvItemList EquipItem { get; }

    /// <summary>
    /// 仓库数据
    /// </summary>
    public TlvItemList StoreItem { get; }

    /// <summary>
    /// 唯一使用物品列表
    /// </summary>
    public List<ushort> ItemUseOnceList { get; }
}