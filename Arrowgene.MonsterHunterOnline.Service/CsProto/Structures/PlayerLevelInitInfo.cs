using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// Player Level initialize info
    /// </summary>
    public class PlayerLevelInitInfo : Structure, ICsStructure, CSICsRemoteDataInfo
    {
        public PlayerLevelInitInfo()
        {
            EntrustMoneyLastTm = 0;
            UnLockLevelData = new List<PlayerUnlockLevelInfo>();
            UnlockHubData = new List<PlayerUnLockHubInfo>();
            HubRewardData = new List<CSHubEntryRewardInfo>();
            LevelIntegrateData = new List<PlayerLevelIntegrateInfo>();
            LevelEntrustData = new List<PlayerLevelEntrustInfo>();
            PlayerLevelEnstrustRewardData = new List<PlayerLevelEnstrustRewardInfo>();
            PlayerLevelFormatPeriodLimitData = new CSLevelFormatPeriodLimitInfo();
            PlayerThousandLayerData = new List<CSThousandLayerDataInfo>();
            LevelStatDataPack = new List<byte>();
            EntrustGroupDataPack = new List<byte>();
            LevelWarningDataPack = new List<byte>();
            CSMonsterSizeList = new List<CSMonsterSize>();
            UnLockLevelGroupData = new List<PlayerUnlockLevelGroupInfo>();
            UnlockHubStarData = new List<PlayerUnLockHubStarInfo>();
            PlayerSuperHunterData = new PlayerSuperHunterInfo();
        }

        public ROMTE_DATA_TYPE DataType => ROMTE_DATA_TYPE.LEVELINFO_DATA_TYPE;

        /// <summary>
        /// 委托货币最后一次重置时间
        /// </summary>
        public int EntrustMoneyLastTm;

        public List<PlayerUnlockLevelInfo> UnLockLevelData;

        public List<PlayerUnLockHubInfo> UnlockHubData;

        public List<CSHubEntryRewardInfo> HubRewardData;

        public List<PlayerLevelIntegrateInfo> LevelIntegrateData;

        public List<PlayerLevelEntrustInfo> LevelEntrustData;

        public List<PlayerLevelEnstrustRewardInfo> PlayerLevelEnstrustRewardData;

        /// <summary>
        /// 赛制周期次数数据
        /// </summary>
        public CSLevelFormatPeriodLimitInfo PlayerLevelFormatPeriodLimitData;

        public List<CSThousandLayerDataInfo> PlayerThousandLayerData;

        /// <summary>
        /// 关卡统计数据
        /// </summary>
        public List<byte> LevelStatDataPack;

        /// <summary>
        /// 委托大组统计数据
        /// </summary>
        public List<byte> EntrustGroupDataPack;

        /// <summary>
        /// warning关卡数据
        /// </summary>
        public List<byte> LevelWarningDataPack;

        /// <summary>
        /// 怪物尺寸数据列表
        /// </summary>
        public List<CSMonsterSize> CSMonsterSizeList;

        public List<PlayerUnlockLevelGroupInfo> UnLockLevelGroupData;

        public List<PlayerUnLockHubStarInfo> UnlockHubStarData;

        /// <summary>
        /// 超级大连续信息
        /// </summary>
        public PlayerSuperHunterInfo PlayerSuperHunterData;

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, EntrustMoneyLastTm);
            WriteList(buffer, UnLockLevelData, CsProtoConstant.CS_LEVEL_ID_COUNT_MAX, WriteInt32, WriteCsStructure);
            WriteList(buffer, UnlockHubData, CsProtoConstant.MAX_HUB_NUM, WriteInt32, WriteCsStructure);
            WriteList(buffer, HubRewardData, CsProtoConstant.MAX_HUB_NUM, WriteInt32, WriteCsStructure);
            WriteList(buffer, LevelIntegrateData, CsProtoConstant.CS_LEVEL_ID_COUNT_MAX, WriteInt32, WriteCsStructure);
            WriteList(buffer, LevelEntrustData, LevelEntrustData.Count, WriteInt32, WriteCsStructure);
            WriteList(buffer,
                PlayerLevelEnstrustRewardData,
                CsProtoConstant.CS_LEVEL_SUB_GROUP_COUNT_MAX,
                WriteInt32,
                WriteCsStructure
            );
            WriteCsStructure(buffer, PlayerLevelFormatPeriodLimitData);
            WriteList(buffer,
                PlayerThousandLayerData,
                CsProtoConstant.CS_LEVEL_THOUSAND_LAYER_DATA_COUNT_MAX,
                WriteInt32,
                WriteCsStructure
            );
            WriteList(buffer, LevelStatDataPack, CsProtoConstant.CS_LEVEL_STAT_MAX_LEN, WriteInt32, WriteByte);
            WriteList(buffer, EntrustGroupDataPack, CsProtoConstant.CS_ENTRUST_STAT_MAX_LEN, WriteInt32, WriteByte);
            WriteList(buffer, LevelWarningDataPack, CsProtoConstant.CS_WARNING_DATA_MAX_LEN, WriteInt32, WriteByte);
            WriteList(buffer, CSMonsterSizeList, CsProtoConstant.CS_MONSTER_COUNT, WriteInt32, WriteCsStructure);
            WriteList(buffer,
                UnLockLevelGroupData,
                CsProtoConstant.CS_LEVEL_GROUP_COUNT_MAX,
                WriteInt32,
                WriteCsStructure
            );
            WriteList(buffer,
                UnlockHubStarData,
                CsProtoConstant.CS_LEVEL_HUB_STAR_COUNT_MAX,
                WriteInt32,
                WriteCsStructure
            );
            WriteCsStructure(buffer, PlayerSuperHunterData);
        }

        public void ReadCs(IBuffer buffer)
        {
            EntrustMoneyLastTm = ReadInt32(buffer);
            ReadList(buffer,
                UnLockLevelData,
                CsProtoConstant.CS_LEVEL_ID_COUNT_MAX,
                ReadInt32,
                ReadCsStructure<PlayerUnlockLevelInfo>
            );
            ReadList(buffer, UnlockHubData, CsProtoConstant.MAX_HUB_NUM, ReadInt32, ReadCsStructure<PlayerUnLockHubInfo>);
            ReadList(buffer,
                HubRewardData,
                CsProtoConstant.MAX_HUB_NUM,
                ReadInt32,
                ReadCsStructure<CSHubEntryRewardInfo>
            );
            ReadList(buffer,
                LevelIntegrateData,
                CsProtoConstant.CS_LEVEL_ID_COUNT_MAX,
                ReadInt32,
                ReadCsStructure<PlayerLevelIntegrateInfo>
            );
            ReadList(buffer,
                LevelEntrustData,
                LevelEntrustData.Count,
                ReadInt32,
                ReadCsStructure<PlayerLevelEntrustInfo>
            );
            ReadList(buffer,
                PlayerLevelEnstrustRewardData,
                CsProtoConstant.CS_LEVEL_SUB_GROUP_COUNT_MAX,
                ReadInt32,
                ReadCsStructure<PlayerLevelEnstrustRewardInfo>
            );
            PlayerLevelFormatPeriodLimitData = ReadCsStructure<CSLevelFormatPeriodLimitInfo>(buffer);
            ReadList(buffer,
                PlayerThousandLayerData,
                CsProtoConstant.CS_LEVEL_THOUSAND_LAYER_DATA_COUNT_MAX,
                ReadInt32,
                ReadCsStructure<CSThousandLayerDataInfo>
            );
            ReadList(buffer, LevelStatDataPack, CsProtoConstant.CS_LEVEL_STAT_MAX_LEN, ReadInt32, ReadByte);
            ReadList(buffer, EntrustGroupDataPack, CsProtoConstant.CS_ENTRUST_STAT_MAX_LEN, ReadInt32, ReadByte);
            ReadList(buffer, LevelWarningDataPack, CsProtoConstant.CS_WARNING_DATA_MAX_LEN, ReadInt32, ReadByte);
            ReadList(buffer,
                CSMonsterSizeList,
                CsProtoConstant.CS_MONSTER_COUNT,
                ReadInt32,
                ReadCsStructure<CSMonsterSize>
            );
            ReadList(buffer,
                UnLockLevelGroupData,
                CsProtoConstant.CS_LEVEL_GROUP_COUNT_MAX,
                ReadInt32,
                ReadCsStructure<PlayerUnlockLevelGroupInfo>
            );
            ReadList(buffer,
                UnlockHubStarData,
                CsProtoConstant.CS_LEVEL_HUB_STAR_COUNT_MAX,
                ReadInt32,
                ReadCsStructure<PlayerUnLockHubStarInfo>
            );
            PlayerSuperHunterData = ReadCsStructure<PlayerSuperHunterInfo>(buffer);
        }
    }
}