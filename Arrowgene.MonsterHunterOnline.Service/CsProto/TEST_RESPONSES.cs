using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto;

public static class TEST_RESPONSES
{
    public static void player_init(Client client)
    {
//	<struct name="CSPlayerInitInfo" version="1" desc="Player initialize info">
//		<entry name="AccountID" type="uint" desc="玩家QQ号"/>
//		<entry name="NetID" type="int" desc="logic entity id"/>
//		<entry name="DBId" type="uint64" desc="DBID"/>
//		<entry name="SessionID" type="uint" desc="Session id"/>
//		<entry name="WorldID" type="uint" desc="大区ID"/>
//		<entry name="ServerID" type="uint" desc="服ID"/>
//		<entry name="WorldSvrID" type="uint" desc="WorldSvrID"/>
//		<entry name="ServerTime" type="uint" desc="服务器时间"/>
//		<entry name="IsReConnect" type="uint" desc="本次登录是否断线重连"/>
//		<entry name="Name" type="string" size="CS_MAX_ROLE_NAME" desc="role name" sizeinfo="int"/>
//		<entry name="Gender" type="byte" desc="gender fo role"/>
//		<entry name="IsGM" type="byte" desc="是否是GM"/>
//		<entry name="Pose" type="CSQuatT" desc="Appear location"/>
//		<entry name="ParentEntityGUID" type="biguint" desc="parent entityGUID"/>
//		<entry name="AvatarSetID" type="byte" desc="Avatar Set"/>
//		<entry name="Faction" type="int" desc="阵营"/>
//		<entry name="RandSeed" type="uint" desc="随机数"/>
//		<entry name="Weapon" type="int"/>
//		<entry name="LastLoginTime" type="uint" desc="最后登录时间"/>
//		<entry name="CreateTime" type="uint" desc="角色创建时间"/>
//		<entry name="StoreSize" type="ushort" desc="仓库大小"/>
//		<entry name="NormalSize" type="ushort" desc="普通背包容量大小"/>
//		<entry name="MaterialStoreSize" type="ushort" desc="素材仓库容量大小"/>
//		<entry name="BagSize" type="int" desc="背包道具数据大小"/>
//		<entry name="BagItem" type="byte" count="CS_ITEM_BAG_DATA_LEN" desc="背包道具数据" refer="BagSize"/>
//		<entry name="EquipSize" type="ushort" desc="装备栏数据大小"/>
//		<entry name="EquipItem" type="byte" count="CS_ITEM_EQUIP_DATA_LEN" desc="装备数据" refer="EquipSize"/>
//		<entry name="StoreDataSize" type="int" desc="仓库大小"/>
//		<entry name="StoreItem" type="byte" count="CS_ITEM_STORE_DATA_LEN" desc="仓库数据" refer="StoreDataSize"/>
//		<entry name="ShortcutCount" type="ushort" desc="快捷栏数据数量"/>
//		<entry name="Shortcut" type="CSShortcut" count="CS_MAX_SHORTCUT_LEN" desc="快捷栏数据" refer="ShortcutCount"/>
//		<entry name="BuffSize" type="ushort" desc="buff数据大小"/>
//		<entry name="Buff" type="byte" count="CS_MAX_BUFF_DATA_LEN" desc="buff数据" refer="BuffSize"/>
//		<entry name="SkillSize" type="ushort" desc="skill数据大小"/>
//		<entry name="Skill" type="byte" count="CS_MAX_SKILL_DATA_LEN" desc="skill数据" refer="SkillSize"/>
//		<entry name="PetSize" type="ushort" desc="pet数据大小"/>
//		<entry name="Pet" type="byte" count="CS_MAX_PET_DATA_LEN" desc="pet数据" refer="PetSize"/>
//		<entry name="FriendCount" type="int" desc="好友数量"/>
//		<entry name="FriendData" type="FriendInfoPacket" count="CS_FRIEND_MAX" desc="好友列表" refer="FriendCount"/>
//		<entry name="PasserbyCount" type="int" desc="路人数量"/>
//		<entry name="PasserbyData" type="PasserbyInfoPacket" count="CS_PASSERBY_MAX" desc="路人列表" refer="PasserbyCount"/>
//		<entry name="BlacklistCount" type="int" desc="黑名单数量"/>
//		<entry name="BlacklistData" type="BlacklistInfoPacket" count="CS_BLACKLIST_MAX" desc="黑名单列表" refer="BlacklistCount"/>
//		<entry name="FriendGroupCount" type="int" desc="好友分组数量"/>
//		<entry name="FriendGroupData" type="FriendGroupPacket" count="CS_FRIENDGROUP_MAX" desc="好友分组列表" refer="FriendGroupCount"/>
//		<entry name="AttrSize" type="ushort" desc="属性数据大小"/>
//		<entry name="Attr" type="byte" count="CS_MAX_ATTR_DATA_LEN" desc="attr数据" refer="AttrSize"/>
//		<entry name="TaskLen" type="int" desc="任务数据长度"/>
//		<entry name="Task" type="byte" count="CS_MAX_TASK_DATA_LEN" desc="任务数据" refer="TaskLen"/>
//		<entry name="GuildLen" type="int" desc="猎团数据长度"/>
//		<entry name="Guild" type="byte" count="CS_MAX_GUILD_DATA_LEN" desc="猎团数据" refer="GuildLen"/>
//		<entry name="ActionPoint" type="CSActionPointData" desc="行动力数据"/>
//		<entry name="FirstEnterLevel" type="int" desc="是否第一次进入LEVEL"/>
//		<entry name="FirstEnterMap" type="int" desc="是否第一次进入MAP"/>
//		<entry name="PvpPrepareStageState" type="int" desc="PVP初始准备阶段状态 1表示结束，0表示开始"/>
//		<entry name="GuideStepCount" type="uint16" desc="新手引导完成数量"/>
//		<entry name="GuideSteps" type="CSGuideStep" count="CS_MAX_GUIDE_STEPS" desc="新手引导完成的步骤" refer="GuideStepCount"/>
//		<entry name="CDSize" type="ushort" desc="cd数据大小"/>
//		<entry name="CD" type="byte" count="CS_MAX_CD_DATA_LEN" desc="CD数据" refer="CDSize"/>
//		<entry name="SchedulePrizeLen" type="int" desc="任务数据长度"/>
//		<entry name="SchedulePrize" type="byte" count="CS_MAX_SCHEDULEPRIZE_DATA_LEN" desc="日程表奖励信息数据" refer="SchedulePrizeLen"/>
//		<entry name="MailInfoLen" type="int" desc="邮件数据长度"/>
//		<entry name="mailInfo" type="byte" count="CS_MAX_MAILINFO_DATA_LEN" desc="邮件数据" refer="MailInfoLen"/>
//		<entry name="NpcAtdInfoLen" type="int" desc="NPC组好感度数据长度"/>
//		<entry name="NpcAtdInfo" type="byte" count="CS_MAX_NPCATDINFO_DATA_LEN" desc="NPC组好感度数据" refer="NpcAtdInfoLen"/>
//		<entry name="CurPlayerUsedCatCarCount" type="int" desc="当前玩家已经使用的猫车次数"/>
//		<entry name="CatCuisineID" type="int" desc="剩余猫饭的ID"/>
//		<entry name="CatCuisineCount" type="uint16" desc="猫饭剩余次数"/>
//		<entry name="CatCuisineLevel" type="uint8" desc="剩余猫饭的等级"/>
//		<entry name="CatCuisineBuffs" type="uint8" desc="剩余猫饭的效果"/>
//		<entry name="CatCuisineLastTm" type="uint32" desc="最后一次吃猫饭的时间戳"/>
//		<entry name="CatCuisineFormulaCount" type="int" desc="猫饭配方解锁数量"/>
//		<entry name="CatCuisineFormulaData" type="CatCuisineDataInfo" count="CS_MAX_CAT_CUISINE_UNLOCK" refer="CatCuisineCount"/>
//		<entry name="ItemUseOnceCount" type="uint16" desc="唯一使用物品数量"/>
//		<entry name="ItemUseOnceList" type="uint16" count="CS_MAX_ITEM_USE_ONCE_LIST_COUNT" desc="唯一使用物品列表" refer="ItemUseOnceCount"/>
//		<entry name="StarLen" type="int" desc="猎人星级数据长度"/>
//		<entry name="Star" type="byte" count="CS_MAX_STAR_DATA_LEN" desc="猎人星级数据" refer="StarLen"/>
//		<entry name="VideoLen" type="ushort" desc="视屏观看数据长度"/>
//		<entry name="Video" type="byte" count="CS_MAX_SKILL_VIDEO_LEN" desc="视频观看数据" refer="VideoLen"/>
//		<entry name="ClientSettings" type="CSClientSettings" desc="客户端杂项数据"/>
//		<entry name="FarmLen" type="int" desc="农场数据长度"/>
//		<entry name="Farm" type="byte" count="CS_ROLE_FARMER_LEN" desc="农场数据" refer="FarmLen"/>
//		<entry name="FacialInfo" type="short" count="CS_MAX_FACIALINFO_COUNT" desc="捏脸数据集合"/>
//		<entry name="SpoorLen" type="int" desc="猎人之路数据长度"/>
//		<entry name="Spoor" type="byte" count="CS_MAX_SPOOR_DATA_LEN" desc="猎人之路数据" refer="SpoorLen"/>
//		<entry name="RapidHuntLen" type="int" desc="疾风狩猎数据长度"/>
//		<entry name="RapidHunt" type="byte" count="CS_MAX_RAPIDHUNT_DATA_LEN" desc="疾风狩猎数据" refer="RapidHuntLen"/>
//		<entry name="ActivityLen" type="int" desc="活动数据长度"/>
//		<entry name="Activity" type="byte" count="CS_MAX_ACTIVITY_DATA_LEN" desc="活动数据" refer="ActivityLen"/>
//		<entry name="IsSpectating" type="uchar" desc="是否在观战"/>
//		<entry name="ItemRebuildLen" type="int" desc="ItemRebuild数据长度"/>
//		<entry name="ItemRebuild" type="byte" count="CS_MAX_ITEMREBUILD_DATA_LEN" desc="ItemRebuild数据" refer="ItemRebuildLen"/>
//		<entry name="ItemBoxLen" type="int" desc="ItemBox数据长度"/>
//		<entry name="ItemBox" type="byte" count="CS_MAX_ITEMBOX_DATA_LEN" desc="ItemBox数据" refer="ItemBoxLen"/>
//		<entry name="ShopLen" type="int" desc="商店数据长度"/>
//		<entry name="Shop" type="byte" count="CS_MAX_SHOP_DATA_LEN" desc="商店数据" refer="ShopLen"/>
//		<entry name="EquipPlanLen" type="uint16" desc="装备方案数据"/>
//		<entry name="EquipPlanData" type="byte" count="CS_MAX_EQUIP_PLAN_LEN" desc="装备方案长度" refer="EquipPlanLen"/>
//		<entry name="TraceLen" type="int" desc="追踪数据长度"/>
//		<entry name="Trace" type="byte" count="CS_MAX_TRACE_DATA_LEN" desc="追踪数据" refer="TraceLen"/>
//		<entry name="StarStone" type="CSStarStoneInfo" desc="星蕴石信息"/>/
//		<entry name="SpeakLen" type="int" desc="Speak数据长度"/>
//		<entry name="Speak" type="byte" count="CS_MAX_SPEAK_DATA_LEN" desc="Speak数据" refer="SpeakLen"/>
//		<entry name="BattleItemUseLen" type="uint32" desc="BattleItemUse数据长度"/>
//		<entry name="BattleItemUse" type="byte" count="CS_MAX_BATTLE_ITEM_USE_LEN" desc="副本物品使用数量数据" refer="BattleItemUseLen"/>
//		<entry name="SuitSkillLen" type="uint32" desc="套装被动技能数据长度"/>
//		<entry name="SuitSkillData" type="byte" count="CS_SUIT_EQUIPSKIKLL_LEN" refer="SuitSkillLen"/>
//		<entry name="AstrolabeLen" type="int" desc="星盘数据长度"/>
//		<entry name="Astrolabe" type="byte" count="CS_MAX_ASTROLABE_DATA_LEN" desc="星盘数据" refer="AstrolabeLen"/>
//		<entry name="WildHuntLen" type="int" desc="红黄对抗数据长度"/>
//		<entry name="WildHunt" type="byte" count="CS_MAX_WILDHUNT_DATA_LEN" desc="红黄对抗数据" refer="WildHuntLen"/>
//		<entry name="SoulStoneLen" type="int" desc="狩魂石数据长度"/>
//		<entry name="SoulStone" type="byte" count="CS_MAX_SOULSTONE_DATA_LEN" desc="狩魂石数据" refer="SoulStoneLen"/>
//		<entry name="MonolopyLen" type="int" desc="大富翁数据长度"/>
//		<entry name="Monolopy" type="byte" count="CS_MAX_MONOLOPY_DATA_LEN" desc="大富翁数据" refer="MonolopyLen"/>
//		<entry name="AchieveLen" type="int" desc="成就数据长度"/>
//		<entry name="Achieve" type="byte" count="CS_MAX_ACHIEVE_DATA_LEN" desc="成就数据" refer="AchieveLen"/>
//		<entry name="UIOptionInfo" type="ClientUIOption" desc="UI自定义数据信息"/>
//		<entry name="IllustrateLen" type="int" desc="怪物图鉴数据长度"/>
//		<entry name="Illustrate" type="byte" count="CS_MAX_ILLUSTRATE_DATA_LEN" desc="怪物图鉴数据" refer="IllustrateLen"/>
//		<entry name="WeaponStyleInfo" type="S2CWeaponStyleInfo" desc="武器Style信息"/>
//		<entry name="WeaponHavenInfoCount" type="int" desc="武器是否获取过bit信息大小"/>
//		<entry name="WeaponHavenInfo" type="char" count="MAX_WEAPON_HAVEN_CNT" desc="武器是否获取过bit信息" refer="WeaponHavenInfoCount"/>
//		<entry name="SilverStorageBoxInfo" type="S2CSilverStorageBoxInfo" desc="银币收纳箱信息"/>
//		<entry name="GuideBookDataLen" type="int" desc="引导书数据长度"/>
//		<entry name="GuideBookData" type="byte" count="CS_MAX_GUIDE_BOOK_DATA_LEN" desc="引导书数据长度" refer="GuideBookDataLen"/>
//		<entry name="SecretResearchInitData" type="S2CSecretResearchLabDataSynchronizationRsp" desc="机密研究院数据"/>
//		<entry name="DragonShopBox" type="int" desc="小铺挂属的DrgonboxID"/>
//	</struct>

// count without referrer need to fill data

        IBuffer b = new StreamBuffer();
        b.WriteUInt32(1, Endianness.Big); //AccountID
        b.WriteInt32(1, Endianness.Big); //NetID
        b.WriteUInt64(1, Endianness.Big); //DBId
        b.WriteUInt32(1, Endianness.Big); //SessionID
        b.WriteUInt32(1, Endianness.Big); //WorldID
        b.WriteUInt32(1, Endianness.Big); //ServerID
        b.WriteUInt32(1, Endianness.Big); //WorldSvrID
        b.WriteUInt32(0, Endianness.Big); //ServerTime
        b.WriteUInt32(0, Endianness.Big); //IsReConnect
        b.WriteInt32(5, Endianness.Big); //Name
        b.WriteCString("Star"); //Name
        b.WriteByte(1); //Gender
        b.WriteByte(1); //IsGM
        //Pose
        //<entry name="Pose" type="CSQuatT" desc="Appear location"/>

        // <struct name="CSQuatT" version="1">
        // <entry name="q" type="CSQuat"/>
        // <entry name="t" type="CSVec3"/>
        // </struct>

        // <struct name="CSVec3" version="1">
        // <entry name="x" type="float"/>
        // <entry name="y" type="float"/>
        // <entry name="z" type="float"/>
        // </struct>

        // <struct name="CSQuat" version="1">
        // <entry name="v" type="CSVec3"/>
        // <entry name="w" type="float"/>
        // </struct>   
        b.WriteFloat(0, Endianness.Big); //qvx
        b.WriteFloat(0, Endianness.Big); //qvy
        b.WriteFloat(0, Endianness.Big); //qvz
        b.WriteFloat(0, Endianness.Big); //qw
        b.WriteFloat(0, Endianness.Big); //tx
        b.WriteFloat(0, Endianness.Big); //ty
        b.WriteFloat(0, Endianness.Big); //tz
        //Pose end
        b.WriteUInt64(0, Endianness.Big); //ParentEntityGUID
        b.WriteByte(0); //AvatarSetID
        b.WriteInt32(0, Endianness.Big); //Faction
        b.WriteUInt32(0, Endianness.Big); //RandSeed
        b.WriteInt32(0, Endianness.Big); //Weapon
        b.WriteUInt32(0, Endianness.Big); //LastLoginTime
        b.WriteUInt32(0, Endianness.Big); //CreateTime
        b.WriteUInt16(0, Endianness.Big); //StoreSize
        b.WriteUInt16(0, Endianness.Big); //NormalSize
        b.WriteUInt16(0, Endianness.Big); //MaterialStoreSize
        b.WriteInt32(0, Endianness.Big); //BagSize
        b.WriteByte(0); //BagItem
        b.WriteUInt16(0, Endianness.Big); //EquipSize
        b.WriteByte(0); //EquipItem
        b.WriteInt32(0, Endianness.Big); //StoreDataSize
        b.WriteByte(0); //StoreItem
        b.WriteUInt16(0, Endianness.Big); //ShortcutCount
        //<entry name="Shortcut" type="CSShortcut" count="CS_MAX_SHORTCUT_LEN" desc="快捷栏数据" refer="ShortcutCount"/>
        b.WriteUInt16(0, Endianness.Big); //BuffSize
        b.WriteByte(0); //Buff
        b.WriteUInt16(0, Endianness.Big); //SkillSize
        b.WriteByte(0); //Skill //count="CS_MAX_SKILL_DATA_LEN" desc="skill数据" refer="SkillSize"
        b.WriteUInt16(0, Endianness.Big); //PetSize
        b.WriteByte(0); //Pet //count="CS_MAX_PET_DATA_LEN" desc="pet数据" refer="PetSize"
        b.WriteInt32(0, Endianness.Big); //FriendCount
        //<entry name="FriendData" type="FriendInfoPacket" count="CS_FRIEND_MAX" desc="好友列表" refer="FriendCount"/>
        b.WriteInt32(0, Endianness.Big); //PasserbyCount
        //<entry name="PasserbyData" type="PasserbyInfoPacket" count="CS_PASSERBY_MAX" desc="路人列表" refer="PasserbyCount"/
        b.WriteInt32(0, Endianness.Big); //BlacklistCount
        //<entry name="BlacklistData" type="BlacklistInfoPacket" count="CS_BLACKLIST_MAX" desc="黑名单列表" refer="BlacklistCount"/>
        b.WriteInt32(0, Endianness.Big); //FriendGroupCount
        //<entry name="FriendGroupData" type="FriendGroupPacket" count="CS_FRIENDGROUP_MAX" desc="好友分组列表" refer="FriendGroupCount"/>
        b.WriteUInt16(0, Endianness.Big); //AttrSize
        //<entry name="Attr" type="byte" count="CS_MAX_ATTR_DATA_LEN" desc="attr数据" refer="AttrSize"/>
        b.WriteInt32(0, Endianness.Big); //TaskLen
        //<entry name="Task" type="byte" count="CS_MAX_TASK_DATA_LEN" desc="任务数据" refer="TaskLen"/>
        b.WriteInt32(0, Endianness.Big); //GuildLen
        //<entry name="Guild" type="byte" count="CS_MAX_GUILD_DATA_LEN" desc="猎团数据" refer="GuildLen"/>
        //ActionPoint start
        //<entry name="ActionPoint" type="CSActionPointData" desc="行动力数据"/>'

        // <struct name="CSActionPointData" version="1" desc="行动同步数据">
        //     <entry name="ActionPoint" type="int" count="ACTION_POINT_TYPE_COUNT" desc="普通行动力"/>
        //     <entry name="AdditionalActionPoint" type="int" desc="附加行动力"/>
        //     <entry name="NextResetTime" type="int" desc="下次重置时间"/>
        //     <entry name="ActionPointFlags" type="uint32" desc="行动力各项标签"/>
        // </struct>
        // TODO check ACTION_POINT_TYPE_COUNT
        // <macro name="ACTION_POINT_TYPE_COUNT" value="2" desc="行动力类型数量" />
        b.WriteInt32(0, Endianness.Big); //ActionPoint
        b.WriteInt32(0, Endianness.Big); //ActionPoint
        b.WriteInt32(0, Endianness.Big); //AdditionalActionPoint
        b.WriteInt32(0, Endianness.Big); //NextResetTime
        b.WriteUInt32(0, Endianness.Big); //ActionPointFlags

        //ActionPoint end
        b.WriteInt32(0, Endianness.Big); //FirstEnterLevel
        b.WriteInt32(0, Endianness.Big); //FirstEnterMap    
        b.WriteInt32(0, Endianness.Big); //PvpPrepareStageState      
        b.WriteUInt16(0, Endianness.Big); //GuideStepCount
        //<entry name="GuideSteps" type="CSGuideStep" count="CS_MAX_GUIDE_STEPS" desc="新手引导完成的步骤" refer="GuideStepCount"/>
        b.WriteUInt16(0, Endianness.Big); //CDSize
        //<entry name="CD" type="byte" count="CS_MAX_CD_DATA_LEN" desc="CD数据" refer="CDSize"/>
        b.WriteInt32(0, Endianness.Big); //SchedulePrizeLen    
        //<entry name="SchedulePrize" type="byte" count="CS_MAX_SCHEDULEPRIZE_DATA_LEN" desc="日程表奖励信息数据" refer="SchedulePrizeLen"/>
        b.WriteInt32(0, Endianness.Big); //MailInfoLen    
        //<entry name="mailInfo" type="byte" count="CS_MAX_MAILINFO_DATA_LEN" desc="邮件数据" refer="MailInfoLen"/>
        b.WriteInt32(0, Endianness.Big); //NpcAtdInfoLen 
        //<entry name="NpcAtdInfo" type="byte" count="CS_MAX_NPCATDINFO_DATA_LEN" desc="NPC组好感度数据" refer="NpcAtdInfoLen"/>
        b.WriteInt32(0, Endianness.Big); //CurPlayerUsedCatCarCount  
        b.WriteInt32(0, Endianness.Big); //CatCuisineID  
        b.WriteInt16(0, Endianness.Big); //CatCuisineCount  
        b.WriteByte(0); //CatCuisineLevel
        b.WriteByte(0); //CatCuisineBuffs
        b.WriteUInt32(0, Endianness.Big); //CatCuisineLastTm  
        b.WriteInt32(0, Endianness.Big); //CatCuisineFormulaCount  
        //<entry name="CatCuisineFormulaData" type="CatCuisineDataInfo" count="CS_MAX_CAT_CUISINE_UNLOCK" refer="CatCuisineCount"/>
        b.WriteUInt16(0, Endianness.Big); //ItemUseOnceCount
        //<entry name="ItemUseOnceList" type="uint16" count="CS_MAX_ITEM_USE_ONCE_LIST_COUNT" desc="唯一使用物品列表" refer="ItemUseOnceCount"/>
        b.WriteInt32(0, Endianness.Big); //StarLen  
        //<entry name="Star" type="byte" count="CS_MAX_STAR_DATA_LEN" desc="猎人星级数据" refer="StarLen"/>
        b.WriteUInt16(0, Endianness.Big); //VideoLen   
        //<entry name="Video" type="byte" count="CS_MAX_SKILL_VIDEO_LEN" desc="视频观看数据" refer="VideoLen"/>
        /////!!!///<entry name="ClientSettings" type="CSClientSettings" desc="客户端杂项数据"/>
        b.WriteInt32(0, Endianness.Big); //FarmLen  
        //<entry name="Farm" type="byte" count="CS_ROLE_FARMER_LEN" desc="农场数据" refer="FarmLen"/>
        for (int i = 0; i < 46; i++)
        {
            //<entry name="FacialInfo" type="short" count="CS_MAX_FACIALINFO_COUNT" desc="捏脸数据集合"/>
            b.WriteInt16(0, Endianness.Big); //FacialInfo
        }

        b.WriteInt32(0, Endianness.Big); //SpoorLen  
        //<entry name="Spoor" type="byte" count="CS_MAX_SPOOR_DATA_LEN" desc="猎人之路数据" refer="SpoorLen"/>
        b.WriteInt32(0, Endianness.Big); //RapidHuntLen  
        //<entry name="RapidHunt" type="byte" count="CS_MAX_RAPIDHUNT_DATA_LEN" desc="疾风狩猎数据" refer="RapidHuntLen"/>
        b.WriteInt32(0, Endianness.Big); //ActivityLen  
        //<entry name="Activity" type="byte" count="CS_MAX_ACTIVITY_DATA_LEN" desc="活动数据" refer="ActivityLen"/>
        b.WriteByte(0); //IsSpectating
        b.WriteInt32(0, Endianness.Big); //ItemRebuildLen  
        //<entry name="ItemRebuild" type="byte" count="CS_MAX_ITEMREBUILD_DATA_LEN" desc="ItemRebuild数据" refer="ItemRebuildLen"/>
        b.WriteInt32(0, Endianness.Big); //ItemBoxLen  
        //<entry name="ItemBox" type="byte" count="CS_MAX_ITEMBOX_DATA_LEN" desc="ItemBox数据" refer="ItemBoxLen"/>
        b.WriteInt32(0, Endianness.Big); //ShopLen  
        //<entry name="Shop" type="byte" count="CS_MAX_SHOP_DATA_LEN" desc="商店数据" refer="ShopLen"/>
        b.WriteUInt16(0, Endianness.Big); //EquipPlanLen  
        //<entry name="EquipPlanData" type="byte" count="CS_MAX_EQUIP_PLAN_LEN" desc="装备方案长度" refer="EquipPlanLen"/>
        b.WriteInt32(0, Endianness.Big); //TraceLen  
        //<entry name="Trace" type="byte" count="CS_MAX_TRACE_DATA_LEN" desc="追踪数据" refer="TraceLen"/>
        // StarStone Start
        //<entry name="StarStone" type="CSStarStoneInfo" desc="星蕴石信息"/>/
        // <struct name="CSStarStoneInfo" version="1" desc="星蕴石信息">
        //     <entry name="WaterExp" type="int" desc="水属性经验"/>
        //     <entry name="FireExp" type="int" desc="火属性经验"/>
        //     <entry name="ThunderExp" type="int" desc="雷属性经验"/>
        //     <entry name="DragonExp" type="int" desc="龙属性经验"/>
        //     <entry name="IceExp" type="int" desc="冰属性经验"/>
        //     <entry name="Duration" type="int" desc="耐久度"/>
        //     <entry name="SlotsCnt" type="int" desc="所有打开的SLOT数量"/>
        //     <entry name="Slots" type="CSStarStoneSlotInfo" count="CS_MAX_STARSTONE_SLOT_NUM" desc="所有打开的SLOT的注入信息" refer="SlotsCnt"/>
        //     </struct>
        b.WriteInt32(0, Endianness.Big); //WaterExp  
        b.WriteInt32(0, Endianness.Big); //FireExp  
        b.WriteInt32(0, Endianness.Big); //ThunderExp  
        b.WriteInt32(0, Endianness.Big); //DragonExp  
        b.WriteInt32(0, Endianness.Big); //IceExp  
        b.WriteInt32(0, Endianness.Big); //Duration  
        b.WriteInt32(0, Endianness.Big); //SlotsCnt  
        //<entry name="Slots" type="CSStarStoneSlotInfo" count="CS_MAX_STARSTONE_SLOT_NUM" desc="所有打开的SLOT的注入信息" refer="SlotsCnt"/>
        // StarStone End 
        b.WriteInt32(0, Endianness.Big); //SpeakLen  
        //<entry name="Speak" type="byte" count="CS_MAX_SPEAK_DATA_LEN" desc="Speak数据" refer="SpeakLen"/>
        b.WriteUInt32(0, Endianness.Big); //BattleItemUseLen  
        //<entry name="BattleItemUse" type="byte" count="CS_MAX_BATTLE_ITEM_USE_LEN" desc="副本物品使用数量数据" refer="BattleItemUseLen"/>
        b.WriteUInt32(0, Endianness.Big); //SuitSkillLen  
        //<entry name="SuitSkillData" type="byte" count="CS_SUIT_EQUIPSKIKLL_LEN" refer="SuitSkillLen"/>
        b.WriteInt32(0, Endianness.Big); //AstrolabeLen  
        //<entry name="Astrolabe" type="byte" count="CS_MAX_ASTROLABE_DATA_LEN" desc="星盘数据" refer="AstrolabeLen"/>
        b.WriteInt32(0, Endianness.Big); //WildHuntLen  
        //<entry name="WildHunt" type="byte" count="CS_MAX_WILDHUNT_DATA_LEN" desc="红黄对抗数据" refer="WildHuntLen"/>
        b.WriteInt32(0, Endianness.Big); //SoulStoneLen  
        //<entry name="SoulStone" type="byte" count="CS_MAX_SOULSTONE_DATA_LEN" desc="狩魂石数据" refer="SoulStoneLen"/>
        b.WriteInt32(0, Endianness.Big); //MonolopyLen  
        //<entry name="Monolopy" type="byte" count="CS_MAX_MONOLOPY_DATA_LEN" desc="大富翁数据" refer="MonolopyLen"/>
        b.WriteInt32(0, Endianness.Big); //AchieveLen  
        //<entry name="Achieve" type="byte" count="CS_MAX_ACHIEVE_DATA_LEN" desc="成就数据" refer="AchieveLen"/>
        // UIOptionInfo start
        //<entry name="UIOptionInfo" type="ClientUIOption" desc="UI自定义数据信息"/>
        // <struct name="ClientUIOption" version="1" desc="客户端UI自定义信息">
        //     <entry name="DataLen" type="int" desc="数据长度"/>
        //     <entry name="OptionData" type="byte" count="CS_UIOPTION_SIZE" desc="实际数据" refer="DataLen"/>
        //     </struct>
        b.WriteInt32(0, Endianness.Big); //DataLen  
        //<entry name="OptionData" type="byte" count="CS_UIOPTION_SIZE" desc="实际数据" refer="DataLen"/>
        // UIOptionInfo end
        b.WriteInt32(0, Endianness.Big); //IllustrateLen
        //<entry name="Illustrate" type="byte" count="CS_MAX_ILLUSTRATE_DATA_LEN" desc="怪物图鉴数据" refer="IllustrateLen"/>
        //WeaponStyleInfo start
        //<entry name="WeaponStyleInfo" type="S2CWeaponStyleInfo" desc="武器Style信息"/>
        //  <struct name="S2CWeaponStyleInfo" version="1" desc="个人武器Style数据">
        //      <entry name="WeaponStyleData" type="int" count="MAX_WEAPOSTYLE_TYPE" desc="各类武器的Style"/>
        //      </struct>
        //	<macro name="MAX_WEAPOSTYLE_TYPE" value="20" desc="武器Style最大类型" />
        for (int i = 0; i < 20; i++)
        {
            b.WriteInt32(0, Endianness.Big); //WeaponStyleData  
        }

        //WeaponStyleInfo end
        b.WriteInt32(0, Endianness.Big); //WeaponHavenInfoCount
        //<entry name="WeaponHavenInfo" type="char" count="MAX_WEAPON_HAVEN_CNT" desc="武器是否获取过bit信息" refer="WeaponHavenInfoCount"/>
        //SilverStorageBoxInfo start
        //<entry name="SilverStorageBoxInfo" type="S2CSilverStorageBoxInfo" desc="银币收纳箱信息"/>
        // <struct name="S2CSilverStorageBoxInfo" version="1" desc="银币收纳箱">
        //     <entry name="SilverCount" type="int" desc="银币收纳箱中的当前银币量"/>
        //     <entry name="WeekFreeFetchTimes" type="int" desc="银币收纳箱本周已免费提取银币次数"/>
        //     <entry name="WeekBuyFetchTimes" type="int" desc="银币收纳箱本周已付费提取银币次数"/>
        //     <entry name="EnlargeTimes" type="int" desc="累计扩容次数"/>
        //     </struct>
        b.WriteInt32(0, Endianness.Big); //SilverCount
        b.WriteInt32(0, Endianness.Big); //WeekFreeFetchTimes
        b.WriteInt32(0, Endianness.Big); //WeekBuyFetchTimes
        b.WriteInt32(0, Endianness.Big); //EnlargeTimes
        //SilverStorageBoxInfo end
        b.WriteInt32(0, Endianness.Big); //GuideBookDataLen
        //<entry name="GuideBookData" type="byte" count="CS_MAX_GUIDE_BOOK_DATA_LEN" desc="引导书数据长度" refer="GuideBookDataLen"/>
        //SecretResearchInitData start
        //<entry name="SecretResearchInitData" type="S2CSecretResearchLabDataSynchronizationRsp" desc="机密研究院数据"/>
        //  <struct name="S2CSecretResearchLabDataSynchronizationRsp" version="1" desc="机密研究院数据同步 服务端->客户端">
        //      <entry name="ItemBoxList" type="ItemBoxType" count="SECRET_RESEARCH_LAB_ITEM_BOX_MAX_LEN" desc="抽奖盒子列表"/>
        //      </struct>
        //<macro name="SECRET_RESEARCH_LAB_ITEM_BOX_MAX_LEN" value="3" desc="机密研究院抽奖盒子数量" />
        //<struct name="ItemBoxType" version="1" desc="机密研究院 抽奖盒子信息">
        //    <entry name="BoxId" type="int" desc="盒子ID"/>
        //    <entry name="LotteryItemList" type="LotteryItemType" count="SECRET_RESEARCH_LAB_LOTTERY_ITEM_MAX_LEN" desc="奖励槽信息"/>
        //    <entry name="VipRefrshCount" type="int" desc="VIP刷新次数"/>
        //    <entry name="RefreshCount" type="int" desc="刷新次数"/>
        //    <entry name="ResearchCount" type="int" desc="研究次数"/>
        //    </struct>
        //	<macro name="SECRET_RESEARCH_LAB_LOTTERY_ITEM_MAX_LEN" value="8" desc="奖励槽数量" />
        // <struct name="LotteryItemType" version="1" desc="机密研究院 奖励物品信息">
        //     <entry name="Position" type="int" desc="奖品所在位置"/>
        //     <entry name="ItemId" type="int" desc="奖品ID"/>
        //     <entry name="ItemNum" type="int" desc="奖品数量"/>
        //     </struct>
        for (int i = 0; i < 3; i++)
        {
            b.WriteInt32(0, Endianness.Big); //BoxId  
            for (int j = 0; j < 8; j++)
            {
                b.WriteInt32(0, Endianness.Big); //Position  
                b.WriteInt32(0, Endianness.Big); //ItemId  
                b.WriteInt32(0, Endianness.Big); //ItemNum  
            }
            b.WriteInt32(0, Endianness.Big); //VipRefrshCount  
            b.WriteInt32(0, Endianness.Big); //RefreshCount  
            b.WriteInt32(0, Endianness.Big); //ResearchCount  
        }

        //SecretResearchInitData end
        b.WriteInt32(0, Endianness.Big); //DragonShopBox


        CsProtoPacket r = new CsProtoPacket();
        r.Body = b.GetAllBytes();
        r.Cmd = CsProtoCmd.CS_CMD_PLAYER_INIT_NTF;
        client.SendCsProto(r);
    }


    public static void send(Client client)
    {
        //  <struct name="CSBTDebugBGListRsp" version="1">
        //   <entry name="Index" type="uint32"/>
        //   <entry name="ErrCode" type="short"/>
        //   <entry name="Count" type="short"/>
        //   <entry name="InstanceInfo" type="CSBGInfo" count="CS_MAX_BTDEBUG_BG_LIST_COUNT" refer="Count"/>
        //   </struct>

        // <struct name="CSBSInfo" version="1">
        //  <entry name="BattleSvrID" type="uint32"/>
        //  <entry name="BattleSvrIP" type="uint32"/>
        //  <entry name="SendPort" type="uint32"/>
        //  <entry name="RecvPort" type="uint32"/>
        //  <entry name="BattleSvrBusID" type="uint32"/>
        //  </struct>

        IBuffer res75 = new StreamBuffer();
        res75.WriteUInt32(1, Endianness.Big);
        res75.WriteInt16(0, Endianness.Big);
        res75.WriteInt16(1, Endianness.Big); //CSBSInfo count
        //CSBSInfo
        res75.WriteUInt32(1, Endianness.Big);
        res75.WriteUInt32(0x7F000001, Endianness.Big); //ip
        res75.WriteUInt32(8283, Endianness.Big); //port
        res75.WriteUInt32(8284, Endianness.Big);
        res75.WriteUInt32(1, Endianness.Big);

        CsProtoPacket resp75 = new CsProtoPacket();
        resp75.Body = res75.GetAllBytes();
        resp75.Cmd = CsProtoCmd.CS_CMD_DEBUGSERVICE_BG_LIST_RSP;
        //  client.SendCsProto(resp75);

        resp75.Cmd = CsProtoCmd.CS_CMD_DEBUGSERVICE_BS_LIST_RSP;
//   client.SendCsProto(resp75);


        // <struct name="CSRoomInitInfo" version="1" desc="玩家进入BS时，重新发送房间必要的信息">
        //  <entry name="RoomSlotNum" type="int"/>
        //  <entry name="RoomSlots" type="uint" count="CS_MAX_BSM_CANDIDATE_COUNT" refer="RoomSlotNum"/>
        //  </struct>
        IBuffer res1 = new StreamBuffer();
        res1.WriteInt32(1, Endianness.Big);
        res1.WriteUInt32(2, Endianness.Big);

        CsProtoPacket resp1 = new CsProtoPacket();
        resp1.Body = res1.GetAllBytes();
        resp1.Cmd = CsProtoCmd.CS_CMD_ROOM_INIT_INFO;
        //    client.SendCsProto(resp1);


        //
        //
        // <struct name="CSShakeHand" version="1" desc="连接握手消息">
        //     <entry name="VerifyCode" type="int32" desc="验证码"/>
        //     </struct>
        IBuffer res3 = new StreamBuffer();
        res3.WriteInt32(0, Endianness.Big);

        CsProtoPacket resp3 = new CsProtoPacket();
        resp3.Body = res3.GetAllBytes();
        resp3.Cmd = CsProtoCmd.CS_CMD_SYSTEM_SHAKEHAND;
        //   client.SendCsProto(resp3);


        // <struct name="CSAutoStartProcess" version="1" desc="自动发起某些流程">
        //     <entry name="ProcessType" type="int32" desc="流程类型" bindmacrosgroup="CS_AUTO_START_FUNCTION"/>
        //     </struct>
        IBuffer res4 = new StreamBuffer();
        //   <macrosgroup name="CS_AUTO_START_FUNCTION" desc="客户端自动发起的功能">
        //   <macro name="CS_AUTO_START_BACKTOROLELIST" value="0" desc="自动返回角色列表" />
        //   <macro name="CS_AUTO_START_BACKTOTOWN" value="1" desc="自动返回城镇" />
        //   </macrosgroup>
        res4.WriteInt32(0, Endianness.Big);

        CsProtoPacket resp4 = new CsProtoPacket();
        resp4.Body = res4.GetAllBytes();
        resp4.Cmd = CsProtoCmd.CS_CMD_AUTO_START_PROCESS;
        //    client.SendCsProto(resp4);


        IBuffer res5 = new StreamBuffer();
        // <struct name="CSMainInstanceOptSynReq" version="1" desc="副本主UI操作同步请求">
        // <entry name="TriggerId" type="int" desc="触发点"/>
        // <entry name="InstancePoint" type="int" desc="副本UI点"/>
        // <entry name="LevelID" type="int" desc="levelId"/>
        // <entry name="LevelDiff" type="int" desc="难度"/>
        // <entry name="LevelMode" type="int" desc="模式"/>
        // </struct>
        res5.WriteInt32(1, Endianness.Big);
        res5.WriteInt32(2, Endianness.Big);
        res5.WriteInt32(1, Endianness.Big);
        res5.WriteInt32(1, Endianness.Big);
        res5.WriteInt32(1, Endianness.Big);
        CsProtoPacket resp5 = new CsProtoPacket();
        resp5.Body = res5.GetAllBytes();
        resp5.Cmd = CsProtoCmd.CS_CMD_MAIN_INSTANCE_OPT_SYN_REQ;
        //       client.SendCsProto(resp5);


        //CS_CMD_SYSTEM_HEARTBEAT
        //CS_CMD_SYSTEM_GAME_MANAGER_CMD
        //

        //<struct name="CSPing" version="1" desc="Ping消息">
        //    <entry name="PingID" type="uint32" desc="ping消息序列号"/>
        //    <entry name="Delay" type="uint16" desc="统计延迟，毫秒"/>
        //    <entry name="CurDelay" type="uint16" desc="最近延迟，毫秒"/>
        //    <entry name="AverageDelay" type="uint16" desc="最近平均延迟，毫秒"/>
        //    <entry name="ServerFps" type="char" desc="最近的服务器fps"/>
        //    <entry name="ServerTm" type="uint32" desc="服务器时间戳"/>
        //    </struct>
        IBuffer res6 = new StreamBuffer();
        res6.WriteUInt32(0, Endianness.Big);
        res6.WriteInt16(0, Endianness.Big);
        res6.WriteInt16(0, Endianness.Big);
        res6.WriteInt16(0, Endianness.Big);
        res6.WriteByte(30);
        res6.WriteUInt32(0, Endianness.Big);

        CsProtoPacket resp6 = new CsProtoPacket();
        resp6.Body = res6.GetAllBytes();
        resp6.Cmd = CsProtoCmd.CS_CMD_SYSTEM_PING;
        //    client.SendCsProto(resp6);


        // <struct name="CSRoleDataErrorRsp" version="1" desc="角色数据错误">
        //     <entry name="ErrNo" type="int" desc="0为成功"/>
        //     </struct>
        IBuffer res7 = new StreamBuffer();
        res7.WriteUInt32(0, Endianness.Big);
        CsProtoPacket resp7 = new CsProtoPacket();
        resp7.Body = res7.GetAllBytes();
        resp7.Cmd = CsProtoCmd.CS_CMD_ROLEDATA_ERR_RSP;
        //  client.SendCsProto(resp7);


        //   <struct name="CSRoomInitInfo" version="1" desc="玩家进入BS时，重新发送房间必要的信息">
        //       <entry name="RoomSlotNum" type="int"/>
        //       <entry name="RoomSlots" type="uint" count="CS_MAX_BSM_CANDIDATE_COUNT" refer="RoomSlotNum"/>
        //       </struct>

        IBuffer res8 = new StreamBuffer();
        res7.WriteInt32(0, Endianness.Big);
        res7.WriteUInt32(1, Endianness.Big);

        CsProtoPacket resp8 = new CsProtoPacket();
        resp8.Body = res8.GetAllBytes();
        resp8.Cmd = CsProtoCmd.CS_CMD_ROOM_INIT_INFO;
        //   client.SendCsProto(resp7);


        //<struct name="CSBackToTown" version="1" desc="进入副本失败返回town">
        //        <entry name="Uin" type="uint" desc="QQ号"/>
        //        <entry name="RoleID" type="int" desc="角色ID"/>
        //        </struct>
        IBuffer res9 = new StreamBuffer();
        res9.WriteUInt32(1, Endianness.Big);
        res9.WriteInt32(1, Endianness.Big);

        CsProtoPacket resp9 = new CsProtoPacket();
        resp9.Body = res9.GetAllBytes();
        resp9.Cmd = CsProtoCmd.CS_CMD_PLAYER_BACK_TO_TOWN;
        //      client.SendCsProto(resp9);


        //      <struct name="CSTownInstanceVerifyRsp" version="1" desc="进入TOWN场景验证响应">
        //      <entry name="ErrNo" type="int" desc="响应码, 0为成功"/>
        //      <entry name="IntanceInitInfo" type="CSInstanceInitInfo" desc="Instance initialize info"/>
        //      <entry name="LineID" type="uint16" desc="所在线ID"/>
        //      <entry name="LevelEnterType" type="int" desc="进入关卡类型，如Townsvr新进，副本返回townsvr，townsvrHub切换等"/>
        //      </struct>
        //<struct name="CSInstanceInitInfo" version="1" desc="Instance initialize info">
        //        <entry name="BattleGroundID" type="int" desc="BattleGround ID"/>
        //        <entry name="LevelID" type="int" desc="Level id"/>
        //        <entry name="CreateMaxPlayerCount" type="int" desc="创建副本的玩家数量"/>
        //        <entry name="GameMode" type="int"/>
        //        <entry name="TimeType" type="int" desc="关卡时间"/>
        //        <entry name="WeatherType" type="int" desc="关卡天气"/>
        //        <entry name="time" type="float"/>
        //        <entry name="LevelRandSeed" type="int" desc="关卡内数据随机种子，特定系统专用"/>
        //        <entry name="WarningFlag" type="uint8" desc="是否是warning"/>
        //        <entry name="CreatePlayerMaxLv" type="int" desc="副本的玩家最大等级"/>
        //        </struct>
        IBuffer res11 = new StreamBuffer();
        res11.WriteInt32(0, Endianness.Big); //ErrNo
        //CSInstanceInitInfo
        res11.WriteInt32(1, Endianness.Big); //BattleGroundID
        res11.WriteInt32(1, Endianness.Big); //LevelID
        res11.WriteInt32(2, Endianness.Big); //CreateMaxPlayerCount
        res11.WriteInt32(0, Endianness.Big); //GameMode
        res11.WriteInt32(0, Endianness.Big); //TimeType
        res11.WriteInt32(0, Endianness.Big); //WeatherType
        res11.WriteFloat(0, Endianness.Big); //time
        res11.WriteInt32(0, Endianness.Big); //LevelRandSeed
        res11.WriteByte(0); //WarningFlag
        res11.WriteInt32(0, Endianness.Big); //CreatePlayerMaxLv

        //CSInstanceInitInfo
        res11.WriteUInt16(0, Endianness.Big); //LineID
        res11.WriteInt32(0, Endianness.Big); //LevelEnterType


        CsProtoPacket resp11 = new CsProtoPacket();
        resp11.Body = res11.GetAllBytes();
        resp11.Cmd = CsProtoCmd.CS_CMD_TOWN_SERVER_INIT_NTF;
        //    client.SendCsProto(resp11);


        //        <struct name="CSSpawnNewPlayer" version="1" desc="创建客户端玩家消息请求">
        //        <entry name="Name" type="string" size="CS_MAX_ROLE_NAME" desc="角色名" sizeinfo="int"/>
        //        </struct>

        IBuffer res12 = new StreamBuffer();
        res12.WriteCString("ShiBa");
        CsProtoPacket resp12 = new CsProtoPacket();
        resp12.Body = res12.GetAllBytes();
        resp12.Cmd = CsProtoCmd.CS_CMD_SPAWN_NEWPLAYER;
        //    client.SendCsProto(resp12);


        //    <struct name="CSAssignPlayerId" version="1" desc="分配新创建玩家的ID号">
        //        <entry name="PlayerId" type="uint" desc="ID号"/>
        //        </struct>
        IBuffer res13 = new StreamBuffer();
        res13.WriteInt32(1, Endianness.Big);
        CsProtoPacket resp13 = new CsProtoPacket();
        resp13.Body = res13.GetAllBytes();
        resp13.Cmd = CsProtoCmd.CS_CMD_ASSIGN_PLAYERID;
        //   client.SendCsProto(resp13);


        //  <struct name="CSNewLineInfo" version="1" desc="c-->s,请求切换TS">
        //        <entry name="LineID" type="uint16" desc="目标线ID"/>
        //        </struct>
        IBuffer res14 = new StreamBuffer();
        res14.WriteUInt16(1, Endianness.Big);
        CsProtoPacket resp14 = new CsProtoPacket();
        resp14.Body = res14.GetAllBytes();
        resp14.Cmd = CsProtoCmd.CS_CMD_SWITCH_LINE;
        //  client.SendCsProto(resp14);


        //   <struct name="CSPlayCutSceneNtf" version="1" desc="服务器通知客户端播放过场动画">
        //   <entry name="CutSceneID" type="uint32" desc="过场动画ID"/>
        //   <entry name="CutSceneType" type="uint32" desc="过场动画类型"/>
        //   </struct>
        IBuffer res15 = new StreamBuffer();
        res15.WriteUInt32(2, Endianness.Big);
        res15.WriteUInt32(1, Endianness.Big);
        CsProtoPacket resp15 = new CsProtoPacket();
        resp15.Body = res15.GetAllBytes();
        resp15.Cmd = CsProtoCmd.CS_CMD_PLAY_CUTSCENE_NTF;
        //client.SendCsProto(resp15);


        //  <struct name="CSSvrStatusNtf" version="1" desc="下发服务器组状态">
        //      <entry name="OnlinePlayerNum" type="int" desc="GM命令名称"/>
        //      <entry name="PlayerInTown" type="int" desc="GM参数1"/>
        //      <entry name="PlayerInBattle" type="int" desc="GM参数2"/>
        //      <entry name="ActiveInstanceCount" type="int" desc="公告内容"/>
        //      <entry name="PlayerInWaitQueue" type="int" desc="公告内容"/>
        //      <entry name="WorldFPS" type="float" desc="WorldSvr FPS"/>
        //      <entry name="TownFPS" type="float" count="10" desc="TownSvr FPS"/>
        //      <entry name="BattleFPS" type="float" count="10" desc="BattleSvr FPS"/>
        //      </struct>
        IBuffer res16 = new StreamBuffer();
        res16.WriteInt32(2, Endianness.Big);
        res16.WriteInt32(1, Endianness.Big);
        res16.WriteInt32(1, Endianness.Big);
        res16.WriteInt32(1, Endianness.Big);
        res16.WriteInt32(1, Endianness.Big);
        res16.WriteFloat(30, Endianness.Big);
        res16.WriteFloat(30, Endianness.Big);
        res16.WriteFloat(30, Endianness.Big);
        CsProtoPacket resp16 = new CsProtoPacket();
        resp16.Body = res16.GetAllBytes();
        resp16.Cmd = CsProtoCmd.CS_CMD_SVRSTATUS_NTF;
        //client.SendCsProto(resp16);
        // return;
        
        
        CsProtoPacket resp17 = new CsProtoPacket();
        resp17.Body = res16.GetAllBytes();
        resp17.Cmd = CsProtoCmd.CS_CMD_SVRSTATUS_NTF;
        

        for (int i = 513; i < 2000; i++)
        {
            IBuffer rT = new StreamBuffer();
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            CsProtoPacket rTs = new CsProtoPacket();
            rTs.Body = rT.GetAllBytes();
            rTs.Cmd = (CsProtoCmd)i;
          //  client.SendCsProto(rTs);
        }
    }
}