using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class CsPlayerInitInfo : IStructure
{
    public CsPlayerInitInfo()
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
        IsGM = 0;
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
        BagItems = new List<byte>();
        EquipItems = new List<byte>();
        StoreItems = new List<byte>();
        Shortcuts = new List<CSShortcut>();
        Buffs = new List<byte>();
        Skills = new List<byte>();
        Pets = new List<byte>();
        FriendData = new List<FriendInfoPacket>();
        PasserbyData = new List<PasserbyInfoPacket>();
        BlacklistData = new List<BlacklistInfoPacket>();
        FriendGroupData = new List<FriendGroupPacket>();
        Attrs = new List<byte>();
        Tasks = new List<byte>();
        Guilds = new List<byte>();
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
        CatCuisineCount = 0;
        CatCuisineLevel = 0;
        CatCuisineBuffs = 0;
        CatCuisineLastTm = 0;
        CatCuisineFormulaData = new List<CatCuisineDataInfo>();
        ItemUseOnceList = new List<ushort>();
        Stars = new List<byte>();
        Videos = new List<byte>();
        ClientSettings = new CSClientSettings();
        Farms = new List<byte>();
        FacialInfo = new short[CsConstant.CS_MAX_FACIALINFO_COUNT];
        Spoors = new List<byte>();
        RapidHunts = new List<byte>();
        Activities = new List<byte>();
        IsSpectating = 0;
        ItemRebuilds = new List<byte>();
        ItemBoxes = new List<byte>();
        Shops = new List<byte>();
        EquipPlanData = new List<byte>();
        Traces = new List<byte>();
        StarStone = new CSStarStoneInfo();
        Speaks = new List<byte>();
        BattleItemUses = new List<byte>();
        SuitSkillData = new List<byte>();
        Astrolabes = new List<byte>();
        WildHunts = new List<byte>();
        SoulStones = new List<byte>();
        Monolopies = new List<byte>();
        Achieves = new List<byte>();
        UIOptionInfo = new ClientUIOption();
        Illustrates = new List<byte>();
        WeaponStyleInfo = new S2CWeaponStyleInfo();
        WeaponHavenInfo = new List<byte>();
        SilverStorageBoxInfo = new S2CSilverStorageBoxInfo();
        GuideBookData = new List<byte>();
        SecretResearchInitData = new S2CSecretResearchLabDataSynchronizationRsp();
        DragonShopBox = 0;
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

    public uint SessionId { get; set; }

    /// <summary>
    /// 大区ID
    /// </summary>
    public uint WorldId { get; set; }

    /// <summary>
    /// 服ID
    /// </summary>
    public uint ServerId { get; set; }

    public uint WorldSvrId { get; set; }

    /// <summary>
    /// 服务器时间
    /// </summary>
    public uint ServerTime { get; set; }

    /// <summary>
    /// 本次登录是否断线重连
    /// </summary>
    public uint IsReConnect { get; set; }

    public string Name { get; set; }
    public byte Gender { get; set; }

    /// <summary>
    /// 是否是GM
    /// </summary>
    public byte IsGM { get; set; }

    public CSQuatT Pose { get; set; }
    public ulong ParentEntityGuid { get; set; }
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
    public List<byte> BagItems { get; set; }

    /// <summary>
    /// 装备数据
    /// </summary>
    public List<byte> EquipItems { get; set; }

    /// <summary>
    /// 仓库数据
    /// </summary>
    public List<byte> StoreItems { get; set; }

    /// <summary>
    /// 快捷栏数据
    /// </summary>
    public List<CSShortcut> Shortcuts { get; set; }

    /// <summary>
    /// buff数据
    /// </summary>
    public List<byte> Buffs { get; set; }

    /// <summary>
    /// skill数据
    /// </summary>
    public List<byte> Skills { get; set; }

    /// <summary>
    /// pet数据
    /// </summary>
    public List<byte> Pets { get; set; }

    /// <summary>
    /// 好友列表
    /// </summary>
    public List<FriendInfoPacket> FriendData { get; set; }

    /// <summary>
    /// 路人列表
    /// </summary>
    public List<PasserbyInfoPacket> PasserbyData { get; set; }

    /// <summary>
    /// 黑名单列表
    /// </summary>
    public List<BlacklistInfoPacket> BlacklistData { get; set; }

    /// <summary>
    /// 好友分组列表
    /// </summary>
    public List<FriendGroupPacket> FriendGroupData { get; set; }

    /// <summary>
    /// attr数据
    /// </summary>
    public List<byte> Attrs { get; set; }

    /// <summary>
    /// 任务数据
    /// </summary>
    public List<byte> Tasks { get; set; }

    /// <summary>
    /// 猎团数据
    /// </summary>
    public List<byte> Guilds { get; set; }

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
    /// PVP初始准备阶段状态
    /// </summary>
    public int PvpPrepareStageState { get; set; }

    /// <summary>
    /// 新手引导完成的步骤
    /// </summary>
    public List<CSGuideStep> GuideSteps { get; set; }

    /// <summary>
    /// CD数据
    /// </summary>
    public List<byte> Cd { get; set; }

    /// <summary>
    /// 日程表奖励信息数据
    /// </summary>
    public List<byte> SchedulePrize { get; set; }

    /// <summary>
    /// 邮件数据
    /// </summary>
    public List<byte> MailInfo { get; set; }

    /// <summary>
    /// NPC组好感度数据
    /// </summary>
    public List<byte> NpcAtdInfo { get; set; }

    /// <summary>
    /// 当前玩家已经使用的猫车次数
    /// </summary>
    public int CurPlayerUsedCatCarCount { get; set; }

    /// <summary>
    /// 剩余猫饭的ID
    /// </summary>
    public int CatCuisineId { get; set; }

    /// <summary>
    /// 猫饭剩余次数
    /// </summary>
    public ushort CatCuisineCount { get; set; }

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

    public List<CatCuisineDataInfo> CatCuisineFormulaData { get; set; }

    /// <summary>
    /// 唯一使用物品列表
    /// </summary>
    public List<ushort> ItemUseOnceList { get; set; }

    /// <summary>
    /// 猎人星级数据
    /// </summary>
    public List<byte> Stars { get; set; }

    /// <summary>
    /// 视频观看数据
    /// </summary>
    public List<byte> Videos { get; set; }

    /// <summary>
    /// 客户端杂项数据
    /// </summary>
    public CSClientSettings ClientSettings { get; set; }

    /// <summary>
    /// 农场数据
    /// </summary>
    public List<byte> Farms { get; set; }

    /// <summary>
    /// 捏脸数据集合
    /// </summary>
    public short[] FacialInfo { get; }

    /// <summary>
    /// 猎人之路数据
    /// </summary>
    public List<byte> Spoors { get; set; }

    /// <summary>
    /// 疾风狩猎数据
    /// </summary>
    public List<byte> RapidHunts { get; set; }

    /// <summary>
    /// 活动数据
    /// </summary>
    public List<byte> Activities { get; set; }

    /// <summary>
    /// 是否在观战
    /// </summary>
    public byte IsSpectating { get; set; }

    /// <summary>
    /// ItemRebuild数据
    /// </summary>
    public List<byte> ItemRebuilds { get; set; }

    /// <summary>
    /// ItemBox数据
    /// </summary>
    public List<byte> ItemBoxes { get; set; }

    /// <summary>
    /// 商店数据
    /// </summary>
    public List<byte> Shops { get; set; }

    /// <summary>
    /// 装备方案长度
    /// </summary>
    public List<byte> EquipPlanData { get; set; }

    /// <summary>
    /// 追踪数据
    /// </summary>
    public List<byte> Traces { get; set; }

    /// <summary>
    /// 星蕴石信息
    /// </summary>
    public CSStarStoneInfo StarStone { get; set; }

    /// <summary>
    /// Speak数据
    /// </summary>
    public List<byte> Speaks { get; set; }

    /// <summary>
    /// 副本物品使用数量数据
    /// </summary>
    public List<byte> BattleItemUses { get; set; }

    public List<byte> SuitSkillData { get; set; }

    /// <summary>
    /// 星盘数据
    /// </summary>
    public List<byte> Astrolabes { get; set; }

    /// <summary>
    /// 红黄对抗数据
    /// </summary>
    public List<byte> WildHunts { get; set; }

    /// <summary>
    /// 狩魂石数据
    /// </summary>
    public List<byte> SoulStones { get; set; }

    /// <summary>
    /// 大富翁数据
    /// </summary>
    public List<byte> Monolopies { get; set; }

    /// <summary>
    /// 成就数据
    /// </summary>
    public List<byte> Achieves { get; set; }

    /// <summary>
    /// UI自定义数据信息
    /// </summary>
    public ClientUIOption UIOptionInfo { get; set; }

    /// <summary>
    /// 怪物图鉴数据
    /// </summary>
    public List<byte> Illustrates { get; set; }

    /// <summary>
    /// 武器Style信息
    /// </summary>
    public S2CWeaponStyleInfo WeaponStyleInfo { get; set; }

    /// <summary>
    /// 武器是否获取过bit信息
    /// </summary>
    public List<byte> WeaponHavenInfo { get; set; }

    /// <summary>
    /// 银币收纳箱信息
    /// </summary>
    public S2CSilverStorageBoxInfo SilverStorageBoxInfo { get; set; }

    /// <summary>
    /// 引导书数据长度
    /// </summary>
    public List<byte> GuideBookData { get; set; }

    /// <summary>
    /// 机密研究院数据
    /// </summary>
    public S2CSecretResearchLabDataSynchronizationRsp SecretResearchInitData { get; set; }

    /// <summary>
    /// 小铺挂属的DrgonboxID
    /// </summary>
    public int DragonShopBox { get; set; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteUInt32(AccountId, Endianness.Big);
        buffer.WriteInt32(NetId, Endianness.Big);
        buffer.WriteUInt64(DbId, Endianness.Big);
        buffer.WriteUInt32(SessionId, Endianness.Big);
        buffer.WriteUInt32(WorldId, Endianness.Big);
        buffer.WriteUInt32(ServerId, Endianness.Big);
        buffer.WriteUInt32(WorldSvrId, Endianness.Big);
        buffer.WriteUInt32(ServerTime, Endianness.Big);
        buffer.WriteUInt32(IsReConnect, Endianness.Big);

        buffer.WriteInt32(Name.Length + 1, Endianness.Big);
        buffer.WriteCString(Name);

        buffer.WriteByte(Gender);
        buffer.WriteByte(IsGM);
        Pose.Write(buffer);
        buffer.WriteUInt64(ParentEntityGuid, Endianness.Big);
        buffer.WriteByte(AvatarSetId);
        buffer.WriteInt32(Faction, Endianness.Big);
        buffer.WriteUInt32(RandSeed, Endianness.Big);
        buffer.WriteInt32(Weapon, Endianness.Big);
        buffer.WriteUInt32(LastLoginTime, Endianness.Big);
        buffer.WriteUInt32(CreateTime, Endianness.Big);
        buffer.WriteUInt16(StoreSize, Endianness.Big);
        buffer.WriteUInt16(NormalSize, Endianness.Big);
        buffer.WriteUInt16(MaterialStoreSize, Endianness.Big);

        int bagItemsSize = BagItems.Count;
        buffer.WriteInt32(bagItemsSize, Endianness.Big);
        for (int i = 0; i < bagItemsSize; i++)
        {
            buffer.WriteByte(BagItems[i]);
        }


        ushort equipItemsSize = (ushort)EquipItems.Count;
        buffer.WriteUInt16(equipItemsSize, Endianness.Big);
        for (int i = 0; i < equipItemsSize; i++)
        {
            buffer.WriteByte(EquipItems[i]);
        }

        int storeItemsSize = StoreItems.Count;
        buffer.WriteInt32(storeItemsSize, Endianness.Big);
        for (int i = 0; i < storeItemsSize; i++)
        {
            buffer.WriteByte(StoreItems[i]);
        }

        ushort shortcutsSize = (ushort)Shortcuts.Count;
        buffer.WriteUInt16(shortcutsSize, Endianness.Big);
        for (int i = 0; i < shortcutsSize; i++)
        {
            Shortcuts[i].Write(buffer);
        }

        ushort buffsSize = (ushort)Buffs.Count;
        buffer.WriteUInt16(buffsSize, Endianness.Big);
        for (int i = 0; i < buffsSize; i++)
        {
            buffer.WriteByte(Buffs[i]);
        }

        ushort skillsSize = (ushort)Skills.Count;
        buffer.WriteUInt16(skillsSize, Endianness.Big);
        for (int i = 0; i < skillsSize; i++)
        {
            buffer.WriteByte(Skills[i]);
        }

        ushort petsSize = (ushort)Pets.Count;
        buffer.WriteUInt16(petsSize, Endianness.Big);
        for (int i = 0; i < petsSize; i++)
        {
            buffer.WriteByte(Pets[i]);
        }

        int friendDataSize = FriendData.Count;
        buffer.WriteInt32(friendDataSize, Endianness.Big);
        for (int i = 0; i < friendDataSize; i++)
        {
            FriendData[i].Write(buffer);
        }

        int passerbyDataSize = PasserbyData.Count;
        buffer.WriteInt32(passerbyDataSize, Endianness.Big);
        for (int i = 0; i < passerbyDataSize; i++)
        {
            PasserbyData[i].Write(buffer);
        }

        int blacklistDataSize = BlacklistData.Count;
        buffer.WriteInt32(blacklistDataSize, Endianness.Big);
        for (int i = 0; i < blacklistDataSize; i++)
        {
            BlacklistData[i].Write(buffer);
        }

        int friendGroupDataSize = FriendGroupData.Count;
        buffer.WriteInt32(friendGroupDataSize, Endianness.Big);
        for (int i = 0; i < friendGroupDataSize; i++)
        {
            FriendGroupData[i].Write(buffer);
        }

        ushort attrsSize = (ushort)Attrs.Count;
        buffer.WriteUInt16(attrsSize, Endianness.Big);
        for (int i = 0; i < attrsSize; i++)
        {
            buffer.WriteByte(Attrs[i]);
        }

        int tasksSize = Tasks.Count;
        buffer.WriteInt32(tasksSize, Endianness.Big);
        for (int i = 0; i < tasksSize; i++)
        {
            buffer.WriteByte(Tasks[i]);
        }

        int guildsSize = Guilds.Count;
        buffer.WriteInt32(guildsSize, Endianness.Big);
        for (int i = 0; i < guildsSize; i++)
        {
            buffer.WriteByte(Guilds[i]);
        }

        ActionPoint.Write(buffer);
        buffer.WriteInt32(FirstEnterLevel, Endianness.Big);
        buffer.WriteInt32(FirstEnterMap, Endianness.Big);
        buffer.WriteInt32(PvpPrepareStageState, Endianness.Big);


        ushort guideStepsSize = (ushort)GuideSteps.Count;
        buffer.WriteUInt16(guideStepsSize, Endianness.Big);
        for (int i = 0; i < guideStepsSize; i++)
        {
            GuideSteps[i].Write(buffer);
        }

        ushort cdSize = (ushort)Cd.Count;
        buffer.WriteUInt16(cdSize, Endianness.Big);
        for (int i = 0; i < cdSize; i++)
        {
            buffer.WriteByte(Cd[i]);
        }

        int schedulePrizeSize = SchedulePrize.Count;
        buffer.WriteInt32(schedulePrizeSize, Endianness.Big);
        for (int i = 0; i < schedulePrizeSize; i++)
        {
            buffer.WriteByte(SchedulePrize[i]);
        }

        int mailInfoSize = MailInfo.Count;
        buffer.WriteInt32(mailInfoSize, Endianness.Big);
        for (int i = 0; i < mailInfoSize; i++)
        {
            buffer.WriteByte(MailInfo[i]);
        }

        int npcAtdInfoSize = NpcAtdInfo.Count;
        buffer.WriteInt32(npcAtdInfoSize, Endianness.Big);
        for (int i = 0; i < npcAtdInfoSize; i++)
        {
            buffer.WriteByte(NpcAtdInfo[i]);
        }

        buffer.WriteInt32(CurPlayerUsedCatCarCount, Endianness.Big);
        buffer.WriteInt32(CatCuisineId, Endianness.Big);
        buffer.WriteUInt16(CatCuisineCount, Endianness.Big);
        buffer.WriteByte(CatCuisineLevel);
        buffer.WriteByte(CatCuisineBuffs);
        buffer.WriteUInt32(CatCuisineLastTm, Endianness.Big);

        int catCuisineFormulaDataSize = CatCuisineFormulaData.Count;
        buffer.WriteInt32(catCuisineFormulaDataSize, Endianness.Big);
        for (int i = 0; i < catCuisineFormulaDataSize; i++)
        {
            CatCuisineFormulaData[i].Write(buffer);
        }

        ushort itemUseOnceListSize = (ushort)ItemUseOnceList.Count;
        buffer.WriteUInt16(itemUseOnceListSize, Endianness.Big);
        for (int i = 0; i < itemUseOnceListSize; i++)
        {
            buffer.WriteUInt16(ItemUseOnceList[i], Endianness.Big);
        }

        int starsSize = Stars.Count;
        buffer.WriteInt32(starsSize, Endianness.Big);
        for (int i = 0; i < starsSize; i++)
        {
            buffer.WriteByte(Stars[i]);
        }

        ushort videosSize = (ushort)Videos.Count;
        buffer.WriteUInt16(videosSize, Endianness.Big);
        for (int i = 0; i < videosSize; i++)
        {
            buffer.WriteByte(Videos[i]);
        }

        ClientSettings.Write(buffer);

        int farmsSize = Farms.Count;
        buffer.WriteInt32(farmsSize, Endianness.Big);
        for (int i = 0; i < farmsSize; i++)
        {
            buffer.WriteByte(Farms[i]);
        }

        for (int i = 0; i < CsConstant.CS_MAX_FACIALINFO_COUNT; i++)
        {
            buffer.WriteInt16(FacialInfo[i], Endianness.Big);
        }

        int spoorsSize = Spoors.Count;
        buffer.WriteInt32(spoorsSize, Endianness.Big);
        for (int i = 0; i < spoorsSize; i++)
        {
            buffer.WriteByte(Spoors[i]);
        }

        int rapidHuntsSize = RapidHunts.Count;
        buffer.WriteInt32(rapidHuntsSize, Endianness.Big);
        for (int i = 0; i < rapidHuntsSize; i++)
        {
            buffer.WriteByte(RapidHunts[i]);
        }

        int activitiesSize = Activities.Count;
        buffer.WriteInt32(activitiesSize, Endianness.Big);
        for (int i = 0; i < activitiesSize; i++)
        {
            buffer.WriteByte(Activities[i]);
        }

        buffer.WriteByte(IsSpectating);

        int itemRebuildsSize = ItemRebuilds.Count;
        buffer.WriteInt32(itemRebuildsSize, Endianness.Big);
        for (int i = 0; i < itemRebuildsSize; i++)
        {
            buffer.WriteByte(ItemRebuilds[i]);
        }

        int itemBoxesSize = ItemBoxes.Count;
        buffer.WriteInt32(itemBoxesSize, Endianness.Big);
        for (int i = 0; i < itemBoxesSize; i++)
        {
            buffer.WriteByte(ItemBoxes[i]);
        }

        int shopsSize = Shops.Count;
        buffer.WriteInt32(shopsSize, Endianness.Big);
        for (int i = 0; i < shopsSize; i++)
        {
            buffer.WriteByte(Shops[i]);
        }

        ushort equipPlanDataSize = (ushort)EquipPlanData.Count;
        buffer.WriteUInt16(equipPlanDataSize, Endianness.Big);
        for (int i = 0; i < equipPlanDataSize; i++)
        {
            buffer.WriteByte(EquipPlanData[i]);
        }

        int tracesSize = Traces.Count;
        buffer.WriteInt32(tracesSize, Endianness.Big);
        for (int i = 0; i < tracesSize; i++)
        {
            buffer.WriteByte(Traces[i]);
        }

        StarStone.Write(buffer);

        int speaksSize = Speaks.Count;
        buffer.WriteInt32(speaksSize, Endianness.Big);
        for (int i = 0; i < speaksSize; i++)
        {
            buffer.WriteByte(Speaks[i]);
        }

        uint battleItemUsesSize = (uint)BattleItemUses.Count;
        buffer.WriteUInt32(battleItemUsesSize, Endianness.Big);
        for (int i = 0; i < battleItemUsesSize; i++)
        {
            buffer.WriteByte(BattleItemUses[i]);
        }

        uint suitSkillDataSize = (uint)SuitSkillData.Count;
        buffer.WriteUInt32(suitSkillDataSize, Endianness.Big);
        for (int i = 0; i < suitSkillDataSize; i++)
        {
            buffer.WriteByte(SuitSkillData[i]);
        }

        int astrolabesSize = Astrolabes.Count;
        buffer.WriteInt32(astrolabesSize, Endianness.Big);
        for (int i = 0; i < astrolabesSize; i++)
        {
            buffer.WriteByte(Astrolabes[i]);
        }

        int wildHuntsSize = WildHunts.Count;
        buffer.WriteInt32(wildHuntsSize, Endianness.Big);
        for (int i = 0; i < wildHuntsSize; i++)
        {
            buffer.WriteByte(WildHunts[i]);
        }

        int soulStonesSize = SoulStones.Count;
        buffer.WriteInt32(soulStonesSize, Endianness.Big);
        for (int i = 0; i < soulStonesSize; i++)
        {
            buffer.WriteByte(SoulStones[i]);
        }

        int monolopiesSize = Monolopies.Count;
        buffer.WriteInt32(monolopiesSize, Endianness.Big);
        for (int i = 0; i < monolopiesSize; i++)
        {
            buffer.WriteByte(Monolopies[i]);
        }

        int achievesSize = Achieves.Count;
        buffer.WriteInt32(achievesSize, Endianness.Big);
        for (int i = 0; i < achievesSize; i++)
        {
            buffer.WriteByte(Achieves[i]);
        }

        UIOptionInfo.Write(buffer);

        int illustratesSize = Illustrates.Count;
        buffer.WriteInt32(illustratesSize, Endianness.Big);
        for (int i = 0; i < illustratesSize; i++)
        {
            buffer.WriteByte(Illustrates[i]);
        }

        WeaponStyleInfo.Write(buffer);

        int weaponHavenInfoSize = WeaponHavenInfo.Count;
        buffer.WriteInt32(weaponHavenInfoSize, Endianness.Big);
        for (int i = 0; i < weaponHavenInfoSize; i++)
        {
            buffer.WriteByte(WeaponHavenInfo[i]);
        }

        SilverStorageBoxInfo.Write(buffer);

        int guideBookDataSize = GuideBookData.Count;
        buffer.WriteInt32(guideBookDataSize, Endianness.Big);
        for (int i = 0; i < guideBookDataSize; i++)
        {
            buffer.WriteByte(GuideBookData[i]);
        }

        SecretResearchInitData.Write(buffer);
        buffer.WriteInt32(DragonShopBox, Endianness.Big);
        
        
        // TODO 4 bytes required, perhaps i miss some somewhere or the struct is updated
        buffer.WriteByte(0);
        buffer.WriteByte(0);
        buffer.WriteByte(0);
        buffer.WriteByte(0);
    }
}