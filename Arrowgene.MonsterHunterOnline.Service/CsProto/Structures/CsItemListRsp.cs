using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class CsItemListRsp : IRemoteDataInfo
{
    public CsItemListRsp()
    {
        StoreSize = 0;
        NormalSize = 0;
        MaterialStoreSize = 0;
        BagItems = new List<byte>();
        EquipItems = new List<byte>();
        StoreItems = new List<byte>();
        ItemUseOnceList = new List<ushort>();
    }


    public RemoteDataType DataType => RemoteDataType.ITEMMGR_DATA_TYPE;

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
    public List<byte> BagItems { get; }

    /// <summary>
    /// 装备数据
    /// </summary>
    public List<byte> EquipItems { get; }

    /// <summary>
    /// 仓库数据
    /// </summary>
    public List<byte> StoreItems { get; }

    /// <summary>
    /// 唯一使用物品列表
    /// </summary>
    public List<ushort> ItemUseOnceList { get; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteUInt16(StoreSize, Endianness.Big);
        buffer.WriteUInt16(NormalSize, Endianness.Big);
        buffer.WriteUInt16(MaterialStoreSize, Endianness.Big);
        int bagSize = BagItems.Count;
        buffer.WriteInt32(bagSize, Endianness.Big);
        for (int i = 0; i < bagSize; i++)
        {
            buffer.WriteByte(BagItems[i]);
        }

        ushort equipSize = (ushort)EquipItems.Count;
        buffer.WriteUInt16(equipSize, Endianness.Big);
        for (int i = 0; i < equipSize; i++)
        {
            buffer.WriteByte(EquipItems[i]);
        }

        int storeDataSize = StoreItems.Count;
        buffer.WriteInt32(storeDataSize, Endianness.Big);
        for (int i = 0; i < storeDataSize; i++)
        {
            buffer.WriteByte(StoreItems[i]);
        }

        ushort itemUseOnceSize = (ushort)ItemUseOnceList.Count;
        buffer.WriteUInt16(itemUseOnceSize, Endianness.Big);
        for (int i = 0; i < itemUseOnceSize; i++)
        {
            buffer.WriteUInt16(ItemUseOnceList[i], Endianness.Big);
        }
    }
}