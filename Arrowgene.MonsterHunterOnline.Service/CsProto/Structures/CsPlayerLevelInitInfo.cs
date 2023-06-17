using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// Player Level initialize info
/// </summary>
public class CsPlayerLevelInitInfo : IRemoteDataInfo
{
    public CsPlayerLevelInitInfo()
    {
        EntrustMoneyLastTm = 0;
        UnLockLevelData = new List<PlayerUnlockLevelInfo>();
        UnlockHubData = new List<PlayerUnLockHubInfo>();
        HubRewardData = new List<CsHubEntryRewardInfo>();
        LevelIntegrateData = new List<PlayerLevelIntegrateInfo>();
        LevelEntrustData = new List<PlayerLevelEntrustInfo>();
        PlayerLevelEnstrustRewardData = new List<PlayerLevelEnstrustRewardInfo>();
        PlayerLevelFormatPeriodLimitData = new CsLevelFormatPeriodLimitInfo();
        PlayerThousandLayerData = new List<CsThousandLayerDataInfo>();
        LevelStatDataPack = new List<byte>();
        EntrustGroupDataPack = new List<byte>();
        LevelWarningDataPack = new List<byte>();
        CSMonsterSizeList = new List<CsMonsterSize>();
        UnLockLevelGroupData = new List<PlayerUnlockLevelGroupInfo>();
        UnlockHubStarData = new List<PlayerUnLockHubStarInfo>();
        PlayerSuperHunterData = new PlayerSuperHunterInfo();
    }

    public RemoteDataType DataType => RemoteDataType.LEVELINFO_DATA_TYPE;


    /// <summary>
    /// 委托货币最后一次重置时间
    /// </summary>
    public int EntrustMoneyLastTm { get; set; }

    /// <summary>
    /// 解锁副本数量
    /// </summary>
    public List<PlayerUnlockLevelInfo> UnLockLevelData { get; set; }

    public List<PlayerUnLockHubInfo> UnlockHubData { get; set; }
    public List<CsHubEntryRewardInfo> HubRewardData { get; set; }
    public List<PlayerLevelIntegrateInfo> LevelIntegrateData { get; set; }
    public List<PlayerLevelEntrustInfo> LevelEntrustData { get; set; }
    public List<PlayerLevelEnstrustRewardInfo> PlayerLevelEnstrustRewardData { get; set; }

    /// <summary>
    /// 赛制周期次数数据
    /// </summary>
    public CsLevelFormatPeriodLimitInfo PlayerLevelFormatPeriodLimitData { get; set; }

    public List<CsThousandLayerDataInfo> PlayerThousandLayerData { get; set; }

    /// <summary>
    /// 关卡统计数据
    /// </summary>
    public List<byte> LevelStatDataPack { get; set; }

    /// <summary>
    /// 委托大组统计数据
    /// </summary>
    public List<byte> EntrustGroupDataPack { get; set; }

    /// <summary>
    /// warning关卡数据
    /// </summary>
    public List<byte> LevelWarningDataPack { get; set; }


    /// <summary>
    /// 怪物尺寸数据列表
    /// </summary>
    public List<CsMonsterSize> CSMonsterSizeList { get; set; }

    public List<PlayerUnlockLevelGroupInfo> UnLockLevelGroupData { get; set; }
    public List<PlayerUnLockHubStarInfo> UnlockHubStarData { get; set; }

    /// <summary>
    /// 超级大连续信息
    /// </summary>
    public PlayerSuperHunterInfo PlayerSuperHunterData { get; set; }

    public void Write(IBuffer buffer)
    {
        buffer.WriteInt32(EntrustMoneyLastTm, Endianness.Big);
        int unLockLevelSize = UnLockLevelData.Count;
        buffer.WriteInt32(unLockLevelSize, Endianness.Big);
        for (int i = 0; i < unLockLevelSize; i++)
        {
            UnLockLevelData[i].Write(buffer);
        }

        int unlockHubDataSize = UnlockHubData.Count;
        buffer.WriteInt32(unlockHubDataSize, Endianness.Big);
        for (int i = 0; i < unlockHubDataSize; i++)
        {
            UnlockHubData[i].Write(buffer);
        }

        int hubRewardDataSize = HubRewardData.Count;
        buffer.WriteInt32(hubRewardDataSize, Endianness.Big);
        for (int i = 0; i < hubRewardDataSize; i++)
        {
            HubRewardData[i].Write(buffer);
        }

        int levelIntegrateDataSize = LevelIntegrateData.Count;
        buffer.WriteInt32(levelIntegrateDataSize, Endianness.Big);
        for (int i = 0; i < levelIntegrateDataSize; i++)
        {
            LevelIntegrateData[i].Write(buffer);
        }

        int levelEntrustDataSize = LevelEntrustData.Count;
        buffer.WriteInt32(levelEntrustDataSize, Endianness.Big);
        for (int i = 0; i < levelEntrustDataSize; i++)
        {
            LevelEntrustData[i].Write(buffer);
        }

        int playerLevelEnstrustRewardDataSize = PlayerLevelEnstrustRewardData.Count;
        buffer.WriteInt32(playerLevelEnstrustRewardDataSize, Endianness.Big);
        for (int i = 0; i < playerLevelEnstrustRewardDataSize; i++)
        {
            PlayerLevelEnstrustRewardData[i].Write(buffer);
        }

        PlayerLevelFormatPeriodLimitData.Write(buffer);

        int playerThousandLayerDataSize = PlayerThousandLayerData.Count;
        buffer.WriteInt32(playerThousandLayerDataSize, Endianness.Big);
        for (int i = 0; i < playerThousandLayerDataSize; i++)
        {
            PlayerThousandLayerData[i].Write(buffer);
        }

        int levelStatDataPackSize = LevelStatDataPack.Count;
        buffer.WriteInt32(levelStatDataPackSize, Endianness.Big);
        for (int i = 0; i < levelStatDataPackSize; i++)
        {
            buffer.WriteByte(LevelStatDataPack[i]);
        }

        int entrustGroupDataPack = EntrustGroupDataPack.Count;
        buffer.WriteInt32(entrustGroupDataPack, Endianness.Big);
        for (int i = 0; i < entrustGroupDataPack; i++)
        {
            buffer.WriteByte(EntrustGroupDataPack[i]);
        }

        int levelWarningDataPack = LevelWarningDataPack.Count;
        buffer.WriteInt32(levelWarningDataPack, Endianness.Big);
        for (int i = 0; i < levelWarningDataPack; i++)
        {
            buffer.WriteByte(LevelWarningDataPack[i]);
        }

        int csMonsterSizeListSize = CSMonsterSizeList.Count;
        buffer.WriteInt32(csMonsterSizeListSize, Endianness.Big);
        for (int i = 0; i < csMonsterSizeListSize; i++)
        {
            CSMonsterSizeList[i].Write(buffer);
        }

        int unLockLevelGroupDataSize = UnLockLevelGroupData.Count;
        buffer.WriteInt32(unLockLevelGroupDataSize, Endianness.Big);
        for (int i = 0; i < unLockLevelGroupDataSize; i++)
        {
            UnLockLevelGroupData[i].Write(buffer);
        }

        int unlockHubStarData = UnlockHubStarData.Count;
        buffer.WriteInt32(unlockHubStarData, Endianness.Big);
        for (int i = 0; i < unlockHubStarData; i++)
        {
            UnlockHubStarData[i].Write(buffer);
        }

        PlayerSuperHunterData.Write(buffer);
    }
}