using System;

namespace Arrowgene.MonsterHunterOnline.Service.System.UnlockSystem;

[Flags]
public enum SystemUnlockFlags : ulong
{
    None = 0,

    /// <summary>
    /// Palico menu (O key)
    /// </summary>
    PetSystem = 1 << 0,
    ProductionSystem = 1 << 1,

    /// <summary>
    ///  Achievements (J key)
    /// </summary>
    HunterStar = 1 << 2,
    FarmSystem = 1 << 3,
    StrangeRecords = 1 << 4,
    GuildEntrustment = 1 << 5,
    NPCFriendship = 1 << 6,
    WeaponTalent = 1 << 7,
    MaofanSystem = 1 << 8,
    PassiveSkills = 1 << 9,
    TitleSystem = 1 << 10,
    EquipmentForging = 1 << 11,
    RankUpgrade = 1 << 12,
    EquipmentPrecision = 1 << 13,
    EquipmentTessellation = 1 << 14,
    EquipmentEnhancement = 1 << 15,
    MotionDisplay = 1 << 16,
    WeaponAwakening = 1 << 17,
    WeaponTraining = 1 << 18,
    MelaleucaTower = 1 << 19,
    BountyBoard = 1 << 20,
    EquipmentDerivent = 1 << 21,
    PetDismissal = 1 << 22,
    ArmorDerivation = 1 << 23,

    /// <summary>
    /// Clan (T key)
    /// </summary>
    TeamSystem = 1 << 24,
    HuntingGame = 1 << 25,
    ItemDisassembly = 1 << 26,
    AuctionHouse = 1 << 27,
    CurrencyExchange = 1 << 28,
    Competition = 1 << 29,
    StoneProtectionCasting = 1 << 30,
    ArmorUpgrade = 1UL << 31,
    EnhancedBreakthroughTransfer = 1UL << 32,
    WeaponTeachingHandleMode = 1UL << 33,
    Hunting = 1UL << 34,
    AttributeWhetstone = 1UL << 35,
    BreakthroughQualificationCertification = 1UL << 36,
    EquipmentRefining = 1UL << 37,
    RandomMatch = 1UL << 38,
    Casting = 1UL << 39,
    Unbind = 1UL << 40,
    GuardStoneSmelting = 1UL << 41,
    LegendarySkillOrb = 1UL << 42,
}