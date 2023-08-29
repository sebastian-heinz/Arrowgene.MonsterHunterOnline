using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// Player initialize info
    /// </summary>
    public class PlayerInitInfo : Structure, ICsStructure, IItemListProperties
    {
        public PlayerInitInfo()
        {
            AccountId = 0;
            NetId = 0;
            DbId = 0;
            SessionId = 0;
            WorldId = 0;
            ServerId = 0;
            WorldSvrId = 0;
            ServerTime = 0;
            IsReConnect = 0;
            Name = "";
            Gender = 0;
            IsGm = 0;
            Pose = new CSQuatT();
            ParentEntityGuid = 0;
            AvatarSetId = 0;
            Faction = 0;
            RandSeed = 0;
            Weapon = 0;
            LastLoginTime = 0;
            CreateTime = 0;
            StoreSize = 0;
            NormalSize = 0;
            MaterialStoreSize = 0;
            BagItem = new TlvItemList();
            EquipItem = new TlvItemList();
            StoreItem = new TlvItemList();
            Shortcut = new List<CSShortcut>();
            Buff = new List<byte>();
            Skill = new List<byte>();
            Pet = new List<byte>();
            FriendData = new List<FriendInfoPacket>();
            PasserbyData = new List<PasserbyInfoPacket>();
            BlacklistData = new List<BlacklistInfoPacket>();
            FriendGroupData = new List<FriendGroupPacket>();
            Attr = new TlvAttr();
            Task = new TlvTaskData();
            Guild = new List<byte>();
            ActionPoint = new CSActionPointData();
            FirstEnterLevel = 0;
            FirstEnterMap = 0;
            PvpPrepareStageState = 0;
            GuideSteps = new List<CSGuideStep>();
            Cd = new List<byte>();
            SchedulePrize = new List<byte>();
            MailInfo = new List<byte>();
            NpcAtdInfo = new List<byte>();
            CurPlayerUsedCatCarCount = 0;
            CatCuisineId = 0;
            CatCuisineLevel = 0;
            CatCuisineBuffs = 0;
            CatCuisineLastTm = 0;
            CatCuisineFormulaCount = 0;
            CatCuisineFormulaData = new List<CatCuisineDataInfo>();
            ItemUseOnceList = new List<ushort>();
            Star = new List<byte>();
            Video = new List<byte>();
            ClientSettings = new CSClientSettings();
            Farm = new List<byte>();
            FacialInfo = new short[CsProtoConstant.CS_MAX_FACIALINFO_COUNT];
            Spoor = new List<byte>();
            RapidHunt = new List<byte>();
            Activity = new List<byte>();
            IsSpectating = 0;
            ItemRebuild = new List<byte>();
            ItemBox = new List<byte>();
            Shop = new List<byte>();
            EquipPlanData = new List<byte>();
            Trace = new List<byte>();
            StarStone = new CSStarStoneInfo();
            Speak = new List<byte>();
            BattleItemUse = new List<byte>();
            SuitSkillData = new List<byte>();
            Astrolabe = new List<byte>();
            WildHunt = new List<byte>();
            SoulStone = new List<byte>();
            Monolopy = new List<byte>();
            Achieve = new List<byte>();
            UiOptionInfo = new ClientUIOption();
            Illustrate = new List<byte>();
            WeaponStyleInfo = new S2CWeaponStyleInfo();
            WeaponHavenInfo = new List<byte>();
            SilverStorageBoxInfo = new S2CSilverStorageBoxInfo();
            GuideBookData = new List<byte>();
            SecretResearchInitData = new S2CSecretResearchLabDataSynchronizationRsp();
            DragonShopBox = 0;
            CanGetRewarded = 0;
        }

        /// <summary>
        /// 玩家QQ号
        /// </summary>
        public uint AccountId { get; set; }

        /// <summary>
        /// logic entity id
        /// </summary>
        public int NetId { get; set; }

        /// <summary>
        /// DBID
        /// </summary>
        public ulong DbId { get; set; }

        /// <summary>
        /// Session id
        /// </summary>
        public uint SessionId { get; set; }

        /// <summary>
        /// 大区ID
        /// </summary>
        public uint WorldId { get; set; }

        /// <summary>
        /// 服ID
        /// </summary>
        public uint ServerId { get; set; }

        /// <summary>
        /// WorldSvrID
        /// </summary>
        public uint WorldSvrId { get; set; }

        /// <summary>
        /// 服务器时间
        /// </summary>
        public uint ServerTime { get; set; }

        /// <summary>
        /// 本次登录是否断线重连
        /// </summary>
        public uint IsReConnect { get; set; }

        /// <summary>
        /// role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// gender fo role
        /// </summary>
        public byte Gender { get; set; }

        /// <summary>
        /// 是否是GM
        /// </summary>
        public byte IsGm { get; set; }

        /// <summary>
        /// Appear location
        /// </summary>
        public CSQuatT Pose { get; set; }

        /// <summary>
        /// parent entityGUID
        /// </summary>
        public ulong ParentEntityGuid { get; set; }

        /// <summary>
        /// Avatar Set
        /// </summary>
        public byte AvatarSetId { get; set; }

        /// <summary>
        /// 阵营
        /// </summary>
        public int Faction { get; set; }

        /// <summary>
        /// 随机数
        /// </summary>
        public uint RandSeed { get; set; }

        public int Weapon { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public uint LastLoginTime { get; set; }

        /// <summary>
        /// 角色创建时间
        /// </summary>
        public uint CreateTime { get; set; }

        /// <summary>
        /// 仓库大小
        /// </summary>
        public ushort StoreSize { get; set; }

        /// <summary>
        /// 普通背包容量大小
        /// </summary>
        public ushort NormalSize { get; set; }

        /// <summary>
        /// 素材仓库容量大小
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
        /// 快捷栏数据
        /// </summary>
        public List<CSShortcut> Shortcut { get; }

        /// <summary>
        /// buff数据
        /// </summary>
        public List<byte> Buff { get; }

        /// <summary>
        /// skill数据
        /// </summary>
        public List<byte> Skill { get; }

        /// <summary>
        /// pet数据
        /// </summary>
        public List<byte> Pet { get; }

        /// <summary>
        /// 好友列表
        /// </summary>
        public List<FriendInfoPacket> FriendData { get; }

        /// <summary>
        /// 路人列表
        /// </summary>
        public List<PasserbyInfoPacket> PasserbyData { get; }

        /// <summary>
        /// 黑名单列表
        /// </summary>
        public List<BlacklistInfoPacket> BlacklistData { get; }

        /// <summary>
        /// 好友分组列表
        /// </summary>
        public List<FriendGroupPacket> FriendGroupData { get; }

        /// <summary>
        /// attr数据
        /// </summary>
        public TlvAttr Attr { get; }

        /// <summary>
        /// 任务数据
        /// </summary>
        public TlvTaskData Task { get; }

        /// <summary>
        /// 猎团数据
        /// </summary>
        public List<byte> Guild { get; }

        /// <summary>
        /// 行动力数据
        /// </summary>
        public CSActionPointData ActionPoint { get; set; }

        /// <summary>
        /// 是否第一次进入LEVEL
        /// </summary>
        public int FirstEnterLevel { get; set; }

        /// <summary>
        /// 是否第一次进入MAP
        /// </summary>
        public int FirstEnterMap { get; set; }

        /// <summary>
        /// PVP初始准备阶段状态 1表示结束，0表示开始
        /// </summary>
        public int PvpPrepareStageState { get; set; }

        /// <summary>
        /// 新手引导完成的步骤
        /// </summary>
        public List<CSGuideStep> GuideSteps { get; }

        /// <summary>
        /// CD数据
        /// </summary>
        public List<byte> Cd { get; }

        /// <summary>
        /// 日程表奖励信息数据
        /// </summary>
        public List<byte> SchedulePrize { get; }

        /// <summary>
        /// 邮件数据
        /// </summary>
        public List<byte> MailInfo { get; }

        /// <summary>
        /// NPC组好感度数据
        /// </summary>
        public List<byte> NpcAtdInfo { get; }

        /// <summary>
        /// 当前玩家已经使用的猫车次数
        /// </summary>
        public int CurPlayerUsedCatCarCount { get; set; }

        /// <summary>
        /// 剩余猫饭的ID
        /// </summary>
        public int CatCuisineId { get; set; }

        /// <summary>
        /// 剩余猫饭的等级
        /// </summary>
        public byte CatCuisineLevel { get; set; }

        /// <summary>
        /// 剩余猫饭的效果
        /// </summary>
        public byte CatCuisineBuffs { get; set; }

        /// <summary>
        /// 最后一次吃猫饭的时间戳
        /// </summary>
        public uint CatCuisineLastTm { get; set; }

        /// <summary>
        /// 猫饭配方解锁数量
        /// </summary>
        public int CatCuisineFormulaCount { get; set; }

        public List<CatCuisineDataInfo> CatCuisineFormulaData { get; }

        /// <summary>
        /// 唯一使用物品列表
        /// </summary>
        public List<ushort> ItemUseOnceList { get; }

        /// <summary>
        /// 猎人星级数据
        /// </summary>
        public List<byte> Star { get; }

        /// <summary>
        /// 视频观看数据
        /// </summary>
        public List<byte> Video { get; }

        /// <summary>
        /// 客户端杂项数据
        /// </summary>
        public CSClientSettings ClientSettings { get; set; }

        /// <summary>
        /// 农场数据
        /// </summary>
        public List<byte> Farm { get; }

        /// <summary>
        /// 捏脸数据集合
        /// </summary>
        public short[] FacialInfo { get; }

        /// <summary>
        /// 猎人之路数据
        /// </summary>
        public List<byte> Spoor { get; }

        /// <summary>
        /// 疾风狩猎数据
        /// </summary>
        public List<byte> RapidHunt { get; }

        /// <summary>
        /// 活动数据
        /// </summary>
        public List<byte> Activity { get; }

        /// <summary>
        /// 是否在观战
        /// </summary>
        public byte IsSpectating { get; set; }

        /// <summary>
        /// ItemRebuild数据
        /// </summary>
        public List<byte> ItemRebuild { get; }

        /// <summary>
        /// ItemBox数据
        /// </summary>
        public List<byte> ItemBox { get; }

        /// <summary>
        /// 商店数据
        /// </summary>
        public List<byte> Shop { get; }

        /// <summary>
        /// 装备方案长度
        /// </summary>
        public List<byte> EquipPlanData { get; }

        /// <summary>
        /// 追踪数据
        /// </summary>
        public List<byte> Trace { get; }

        /// <summary>
        /// 星蕴石信息
        /// </summary>
        public CSStarStoneInfo StarStone { get; set; }

        /// <summary>
        /// Speak数据
        /// </summary>
        public List<byte> Speak { get; }

        /// <summary>
        /// 副本物品使用数量数据
        /// </summary>
        public List<byte> BattleItemUse { get; }

        public List<byte> SuitSkillData { get; }

        /// <summary>
        /// 星盘数据
        /// </summary>
        public List<byte> Astrolabe { get; }

        /// <summary>
        /// 红黄对抗数据
        /// </summary>
        public List<byte> WildHunt { get; }

        /// <summary>
        /// 狩魂石数据
        /// </summary>
        public List<byte> SoulStone { get; }

        /// <summary>
        /// 大富翁数据
        /// </summary>
        public List<byte> Monolopy { get; }

        /// <summary>
        /// 成就数据
        /// </summary>
        public List<byte> Achieve { get; }

        /// <summary>
        /// UI自定义数据信息
        /// </summary>
        public ClientUIOption UiOptionInfo { get; set; }

        /// <summary>
        /// 怪物图鉴数据
        /// </summary>
        public List<byte> Illustrate { get; }

        /// <summary>
        /// 武器Style信息
        /// </summary>
        public S2CWeaponStyleInfo WeaponStyleInfo { get; set; }

        /// <summary>
        /// 武器是否获取过bit信息
        /// </summary>
        public List<byte> WeaponHavenInfo { get; }

        /// <summary>
        /// 银币收纳箱信息
        /// </summary>
        public S2CSilverStorageBoxInfo SilverStorageBoxInfo { get; set; }

        /// <summary>
        /// 引导书数据长度
        /// </summary>
        public List<byte> GuideBookData { get; }

        /// <summary>
        /// 机密研究院数据
        /// </summary>
        public S2CSecretResearchLabDataSynchronizationRsp SecretResearchInitData;

        /// <summary>
        /// 小铺挂属的DrgonboxID
        /// </summary>
        public int DragonShopBox { get; set; }

        /// <summary>
        /// 返利活动是否有可领取奖励
        /// </summary>
        public int CanGetRewarded { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteUInt32(buffer, AccountId);
            WriteInt32(buffer, NetId);
            WriteUInt64(buffer, DbId);
            WriteUInt32(buffer, SessionId);
            WriteUInt32(buffer, WorldId);
            WriteUInt32(buffer, ServerId);
            WriteUInt32(buffer, WorldSvrId);
            WriteUInt32(buffer, ServerTime);
            WriteUInt32(buffer, IsReConnect);
            WriteString(buffer, Name);
            WriteByte(buffer, Gender);
            WriteByte(buffer, IsGm);
            WriteCsStructure(buffer, Pose);
            WriteUInt64(buffer, ParentEntityGuid);
            WriteByte(buffer, AvatarSetId);
            WriteInt32(buffer, Faction);
            WriteUInt32(buffer, RandSeed);
            WriteInt32(buffer, Weapon);
            WriteUInt32(buffer, LastLoginTime);
            WriteUInt32(buffer, CreateTime);
            WriteUInt16(buffer, StoreSize);
            WriteUInt16(buffer, NormalSize);
            WriteUInt16(buffer, MaterialStoreSize);
            WriteTlvStructure(buffer, BagItem, CsProtoConstant.CS_ITEM_BAG_DATA_LEN, WriteInt32);
            WriteTlvStructure(buffer, EquipItem, (ushort)CsProtoConstant.CS_ITEM_EQUIP_DATA_LEN, WriteUInt16);
            WriteTlvStructure(buffer, StoreItem, CsProtoConstant.CS_ITEM_STORE_DATA_LEN, WriteInt32);
            WriteList(buffer, Shortcut, (ushort)CsProtoConstant.CS_MAX_SHORTCUT_LEN, WriteUInt16, WriteCsStructure);
            WriteList(buffer, Buff, (ushort)CsProtoConstant.CS_MAX_BUFF_DATA_LEN, WriteUInt16, WriteByte);
            WriteList(buffer, Skill, (ushort)CsProtoConstant.CS_MAX_SKILL_DATA_LEN, WriteUInt16, WriteByte);
            WriteList(buffer, Pet, (ushort)CsProtoConstant.CS_MAX_PET_DATA_LEN, WriteUInt16, WriteByte);
            WriteList(buffer, FriendData, CsProtoConstant.CS_FRIEND_MAX, WriteInt32, WriteCsStructure);
            WriteList(buffer, PasserbyData, CsProtoConstant.CS_PASSERBY_MAX, WriteInt32, WriteCsStructure);
            WriteList(buffer, BlacklistData, CsProtoConstant.CS_BLACKLIST_MAX, WriteInt32, WriteCsStructure);
            WriteList(buffer, FriendGroupData, CsProtoConstant.CS_FRIENDGROUP_MAX, WriteInt32, WriteCsStructure);
            WriteTlvStructure(buffer, Attr, (ushort)CsProtoConstant.CS_MAX_ATTR_DATA_LEN, WriteUInt16);
            WriteTlvStructure(buffer, Task, CsProtoConstant.CS_MAX_TASK_DATA_LEN, WriteInt32);
            WriteList(buffer, Guild, CsProtoConstant.CS_MAX_GUILD_DATA_LEN, WriteInt32, WriteByte);
            WriteCsStructure(buffer, ActionPoint);
            WriteInt32(buffer, FirstEnterLevel);
            WriteInt32(buffer, FirstEnterMap);
            WriteInt32(buffer, PvpPrepareStageState);
            WriteList(buffer, GuideSteps, (ushort)CsProtoConstant.CS_MAX_GUIDE_STEPS, WriteUInt16, WriteCsStructure);
            WriteList(buffer, Cd, (ushort)CsProtoConstant.CS_MAX_CD_DATA_LEN, WriteUInt16, WriteByte);
            WriteList(buffer, SchedulePrize, CsProtoConstant.CS_MAX_SCHEDULEPRIZE_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, MailInfo, CsProtoConstant.CS_MAX_MAILINFO_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, NpcAtdInfo, CsProtoConstant.CS_MAX_NPCATDINFO_DATA_LEN, WriteInt32, WriteByte);
            WriteInt32(buffer, CurPlayerUsedCatCarCount);
            WriteInt32(buffer, CatCuisineId);
            WriteByte(buffer, CatCuisineLevel);
            WriteByte(buffer, CatCuisineBuffs);
            WriteUInt32(buffer, CatCuisineLastTm);
            WriteInt32(buffer, CatCuisineFormulaCount);
            WriteList(buffer,
                CatCuisineFormulaData,
                (ushort)CsProtoConstant.CS_MAX_CAT_CUISINE_UNLOCK,
                WriteUInt16,
                WriteCsStructure
            );
            WriteList(buffer,
                ItemUseOnceList,
                (ushort)CsProtoConstant.CS_MAX_ITEM_USE_ONCE_LIST_COUNT,
                WriteUInt16,
                WriteUInt16
            );
            WriteList(buffer, Star, CsProtoConstant.CS_MAX_STAR_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, Video, (ushort)CsProtoConstant.CS_MAX_SKILL_VIDEO_LEN, WriteUInt16, WriteByte);
            WriteCsStructure(buffer, ClientSettings);
            WriteList(buffer, Farm, CsProtoConstant.CS_ROLE_FARMER_LEN, WriteInt32, WriteByte);
            WriteArray(buffer, FacialInfo, CsProtoConstant.CS_MAX_FACIALINFO_COUNT, WriteInt16);
            WriteList(buffer, Spoor, CsProtoConstant.CS_MAX_SPOOR_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, RapidHunt, CsProtoConstant.CS_MAX_RAPIDHUNT_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, Activity, CsProtoConstant.CS_MAX_ACTIVITY_DATA_LEN, WriteInt32, WriteByte);
            WriteByte(buffer, IsSpectating);
            WriteList(buffer, ItemRebuild, CsProtoConstant.CS_MAX_ITEMREBUILD_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, ItemBox, CsProtoConstant.CS_MAX_ITEMBOX_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, Shop, CsProtoConstant.CS_MAX_SHOP_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, EquipPlanData, (ushort)CsProtoConstant.CS_MAX_EQUIP_PLAN_LEN, WriteUInt16, WriteByte);
            WriteList(buffer, Trace, CsProtoConstant.CS_MAX_TRACE_DATA_LEN, WriteInt32, WriteByte);
            WriteCsStructure(buffer, StarStone);
            WriteList(buffer, Speak, CsProtoConstant.CS_MAX_SPEAK_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, BattleItemUse, (uint)CsProtoConstant.CS_MAX_BATTLE_ITEM_USE_LEN, WriteUInt32, WriteByte);
            WriteList(buffer, SuitSkillData, (uint)CsProtoConstant.CS_SUIT_EQUIPSKIKLL_LEN, WriteUInt32, WriteByte);
            WriteList(buffer, Astrolabe, CsProtoConstant.CS_MAX_ASTROLABE_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, WildHunt, CsProtoConstant.CS_MAX_WILDHUNT_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, SoulStone, CsProtoConstant.CS_MAX_SOULSTONE_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, Monolopy, CsProtoConstant.CS_MAX_MONOLOPY_DATA_LEN, WriteInt32, WriteByte);
            WriteList(buffer, Achieve, CsProtoConstant.CS_MAX_ACHIEVE_DATA_LEN, WriteInt32, WriteByte);
            WriteCsStructure(buffer, UiOptionInfo);
            WriteList(buffer, Illustrate, CsProtoConstant.CS_MAX_ACHIEVE_DATA_LEN, WriteInt32, WriteByte);
            WriteCsStructure(buffer, WeaponStyleInfo);
            WriteList(buffer, WeaponHavenInfo, CsProtoConstant.MAX_WEAPON_HAVEN_CNT, WriteInt32, WriteByte);
            WriteCsStructure(buffer, SilverStorageBoxInfo);
            WriteList(buffer, GuideBookData, CsProtoConstant.CS_MAX_GUIDE_BOOK_DATA_LEN, WriteInt32, WriteByte);
            WriteCsStructure(buffer, SecretResearchInitData);
            WriteInt32(buffer, DragonShopBox);
            WriteInt32(buffer, CanGetRewarded);
        }

        public void ReadCs(IBuffer buffer)
        {
            AccountId = ReadUInt32(buffer);
            NetId = ReadInt32(buffer);
            DbId = ReadUInt64(buffer);
            SessionId = ReadUInt32(buffer);
            WorldId = ReadUInt32(buffer);
            ServerId = ReadUInt32(buffer);
            WorldSvrId = ReadUInt32(buffer);
            ServerTime = ReadUInt32(buffer);
            IsReConnect = ReadUInt32(buffer);
            Name = ReadString(buffer);
            Gender = ReadByte(buffer);
            IsGm = ReadByte(buffer);
            Pose = ReadCsStructure<CSQuatT>(buffer);
            ParentEntityGuid = ReadUInt64(buffer);
            AvatarSetId = ReadByte(buffer);
            Faction = ReadInt32(buffer);
            RandSeed = ReadUInt32(buffer);
            Weapon = ReadInt32(buffer);
            LastLoginTime = ReadUInt32(buffer);
            CreateTime = ReadUInt32(buffer);
            StoreSize = ReadUInt16(buffer);
            NormalSize = ReadUInt16(buffer);
            MaterialStoreSize = ReadUInt16(buffer);
            ReadTlvStructure(buffer, BagItem, CsProtoConstant.CS_ITEM_BAG_DATA_LEN, ReadInt32);
            ReadTlvStructure(buffer, EquipItem, (ushort)CsProtoConstant.CS_ITEM_EQUIP_DATA_LEN, ReadUInt16);
            ReadTlvStructure(buffer, StoreItem, CsProtoConstant.CS_ITEM_STORE_DATA_LEN, ReadInt32);
            ReadList(buffer, Shortcut, (ushort)CsProtoConstant.CS_MAX_SHORTCUT_LEN, ReadUInt16,
                ReadCsStructure<CSShortcut>);
            ReadList(buffer, Buff, (ushort)CsProtoConstant.CS_MAX_BUFF_DATA_LEN, ReadUInt16, ReadByte);
            ReadList(buffer, Skill, (ushort)CsProtoConstant.CS_MAX_SKILL_DATA_LEN, ReadUInt16, ReadByte);
            ReadList(buffer, Pet, (ushort)CsProtoConstant.CS_MAX_PET_DATA_LEN, ReadUInt16, ReadByte);
            ReadList(buffer, FriendData, CsProtoConstant.CS_FRIEND_MAX, ReadInt32, ReadCsStructure<FriendInfoPacket>);
            ReadList(buffer, PasserbyData, CsProtoConstant.CS_PASSERBY_MAX, ReadInt32,
                ReadCsStructure<PasserbyInfoPacket>);
            ReadList(buffer, BlacklistData, CsProtoConstant.CS_BLACKLIST_MAX, ReadInt32,
                ReadCsStructure<BlacklistInfoPacket>);
            ReadList(buffer, FriendGroupData, CsProtoConstant.CS_FRIENDGROUP_MAX, ReadInt32,
                ReadCsStructure<FriendGroupPacket>);
            ReadTlvStructure(buffer, Attr, (ushort)CsProtoConstant.CS_MAX_ATTR_DATA_LEN, ReadUInt16);
            ReadTlvStructure(buffer, Task, CsProtoConstant.CS_MAX_TASK_DATA_LEN, ReadInt32);
            ReadList(buffer, Guild, CsProtoConstant.CS_MAX_GUILD_DATA_LEN, ReadInt32, ReadByte);
            ActionPoint = ReadCsStructure<CSActionPointData>(buffer);
            FirstEnterLevel = ReadInt32(buffer);
            FirstEnterMap = ReadInt32(buffer);
            PvpPrepareStageState = ReadInt32(buffer);
            ReadList(buffer, GuideSteps, (ushort)CsProtoConstant.CS_MAX_GUIDE_STEPS, ReadUInt16,
                ReadCsStructure<CSGuideStep>);
            ReadList(buffer, Cd, (ushort)CsProtoConstant.CS_MAX_CD_DATA_LEN, ReadUInt16, ReadByte);
            ReadList(buffer, SchedulePrize, CsProtoConstant.CS_MAX_SCHEDULEPRIZE_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, MailInfo, CsProtoConstant.CS_MAX_MAILINFO_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, NpcAtdInfo, CsProtoConstant.CS_MAX_NPCATDINFO_DATA_LEN, ReadInt32, ReadByte);
            CurPlayerUsedCatCarCount = ReadInt32(buffer);
            CatCuisineId = ReadInt32(buffer);
            CatCuisineLevel = ReadByte(buffer);
            CatCuisineBuffs = ReadByte(buffer);
            CatCuisineLastTm = ReadUInt32(buffer);
            CatCuisineFormulaCount = ReadInt32(buffer);
            ReadList(buffer,
                CatCuisineFormulaData,
                (ushort)CsProtoConstant.CS_MAX_CAT_CUISINE_UNLOCK,
                ReadUInt16,
                ReadCsStructure<CatCuisineDataInfo>
            );
            ReadList(buffer,
                ItemUseOnceList,
                (ushort)CsProtoConstant.CS_MAX_ITEM_USE_ONCE_LIST_COUNT,
                ReadUInt16,
                ReadUInt16
            );
            ReadList(buffer, Star, CsProtoConstant.CS_MAX_STAR_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, Video, (ushort)CsProtoConstant.CS_MAX_SKILL_VIDEO_LEN, ReadUInt16, ReadByte);
            ClientSettings = ReadCsStructure<CSClientSettings>(buffer);
            ReadList(buffer, Farm, CsProtoConstant.CS_ROLE_FARMER_LEN, ReadInt32, ReadByte);
            ReadArray(buffer, FacialInfo, CsProtoConstant.CS_MAX_FACIALINFO_COUNT, ReadInt16);
            ReadList(buffer, Spoor, CsProtoConstant.CS_MAX_SPOOR_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, RapidHunt, CsProtoConstant.CS_MAX_RAPIDHUNT_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, Activity, CsProtoConstant.CS_MAX_ACTIVITY_DATA_LEN, ReadInt32, ReadByte);
            IsSpectating = ReadByte(buffer);
            ReadList(buffer, ItemRebuild, CsProtoConstant.CS_MAX_ITEMREBUILD_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, ItemBox, CsProtoConstant.CS_MAX_ITEMBOX_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, Shop, CsProtoConstant.CS_MAX_SHOP_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, EquipPlanData, (ushort)CsProtoConstant.CS_MAX_EQUIP_PLAN_LEN, ReadUInt16, ReadByte);
            ReadList(buffer, Trace, CsProtoConstant.CS_MAX_TRACE_DATA_LEN, ReadInt32, ReadByte);
            StarStone = ReadCsStructure<CSStarStoneInfo>(buffer);
            ReadList(buffer, Speak, CsProtoConstant.CS_MAX_SPEAK_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, BattleItemUse, (uint)CsProtoConstant.CS_MAX_BATTLE_ITEM_USE_LEN, ReadUInt32, ReadByte);
            ReadList(buffer, SuitSkillData, (uint)CsProtoConstant.CS_SUIT_EQUIPSKIKLL_LEN, ReadUInt32, ReadByte);
            ReadList(buffer, Astrolabe, CsProtoConstant.CS_MAX_ASTROLABE_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, WildHunt, CsProtoConstant.CS_MAX_WILDHUNT_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, SoulStone, CsProtoConstant.CS_MAX_SOULSTONE_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, Monolopy, CsProtoConstant.CS_MAX_MONOLOPY_DATA_LEN, ReadInt32, ReadByte);
            ReadList(buffer, Achieve, CsProtoConstant.CS_MAX_ACHIEVE_DATA_LEN, ReadInt32, ReadByte);
            UiOptionInfo = ReadCsStructure<ClientUIOption>(buffer);
            ReadList(buffer, Illustrate, CsProtoConstant.CS_MAX_ACHIEVE_DATA_LEN, ReadInt32, ReadByte);
            WeaponStyleInfo = ReadCsStructure<S2CWeaponStyleInfo>(buffer);
            ReadList(buffer, WeaponHavenInfo, CsProtoConstant.MAX_WEAPON_HAVEN_CNT, ReadInt32, ReadByte);
            SilverStorageBoxInfo = ReadCsStructure<S2CSilverStorageBoxInfo>(buffer);
            ReadList(buffer, GuideBookData, CsProtoConstant.CS_MAX_GUIDE_BOOK_DATA_LEN, ReadInt32, ReadByte);
            SecretResearchInitData = ReadCsStructure<S2CSecretResearchLabDataSynchronizationRsp>(buffer);
            DragonShopBox = ReadInt32(buffer);
            CanGetRewarded = ReadInt32(buffer);
        }
    }
}