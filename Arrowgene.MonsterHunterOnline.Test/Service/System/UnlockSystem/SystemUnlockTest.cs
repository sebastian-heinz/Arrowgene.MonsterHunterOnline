using Arrowgene.MonsterHunterOnline.Service.System.UnlockSystem;
using Xunit;

namespace Arrowgene.MonsterHunterOnline.Test.Service.System.UnlockSystem;

public class SystemUnlockTest
{
    [Fact]
    public void Test_SystemUnlockExtFlags_ToInt32()
    {
        SystemUnlockExtFlags extFlags;

        extFlags = SystemUnlockExtFlags.EnhancedBreakthroughTransfer;
        Assert.Equal(0b1, extFlags.ToInt32());

        extFlags = SystemUnlockExtFlags.WeaponTeachingHandleMode;
        Assert.Equal(0b10, extFlags.ToInt32());

        extFlags = SystemUnlockExtFlags.Hunting;
        Assert.Equal(0b100, extFlags.ToInt32());

        extFlags = SystemUnlockExtFlags.Casting;
        Assert.Equal(0b10000000, extFlags.ToInt32());

        extFlags = SystemUnlockExtFlags.LegendarySkillOrb | SystemUnlockExtFlags.EnhancedBreakthroughTransfer;
        Assert.Equal(0b10000000001, extFlags.ToInt32());
    }

    [Fact]
    public void Test_SystemUnlockFlags_ToInt32()
    {
        SystemUnlockFlags flags;

        flags = SystemUnlockFlags.PetSystem;
        Assert.Equal(0b1, flags.ToInt32());

        flags = SystemUnlockFlags.ProductionSystem;
        Assert.Equal(0b10, flags.ToInt32());

        flags = SystemUnlockFlags.PetSystem | SystemUnlockFlags.ProductionSystem;
        Assert.Equal(0b11, flags.ToInt32());

        flags = SystemUnlockFlags.PetSystem | SystemUnlockFlags.ProductionSystem;
        Assert.Equal(0b11, flags.ToInt32());

        flags = SystemUnlockFlags.PetSystem | SystemUnlockFlags.ArmorUpgrade;
        int intFlags = flags.ToInt32();
        Assert.Equal(~0b0111_1111_1111_1111_1111_1111_1111_1110, intFlags);

        flags.FromInt32(intFlags);
        Assert.Equal(~0b0111_1111_1111_1111_1111_1111_1111_1110, intFlags);

        // assume flags above `1UL << 31` are dropped
        flags = SystemUnlockFlags.PetSystem | SystemUnlockFlags.ArmorUpgrade |
                SystemUnlockFlags.EnhancedBreakthroughTransfer;
        intFlags = flags.ToInt32();
        Assert.Equal(~0b0111_1111_1111_1111_1111_1111_1111_1110, intFlags);
        flags = flags.FromInt32(intFlags);
        Assert.Equal(~0b0111_1111_1111_1111_1111_1111_1111_1110, intFlags);
        Assert.Equal(SystemUnlockFlags.PetSystem | SystemUnlockFlags.ArmorUpgrade, flags);
    }
    
    [Fact]
    public void Test_SystemUnlockFlags_ToExtFlags()
    {
        // assume only flags above `1UL << 31` are carried over
        SystemUnlockFlags flags = SystemUnlockFlags.PetSystem |
                                  SystemUnlockFlags.ArmorUpgrade |
                                  SystemUnlockFlags.EnhancedBreakthroughTransfer |
                                  SystemUnlockFlags.WeaponTeachingHandleMode;
        SystemUnlockExtFlags extFlags = flags.ToExtFlags();
        Assert.Equal(SystemUnlockExtFlags.EnhancedBreakthroughTransfer |
                     SystemUnlockExtFlags.WeaponTeachingHandleMode,
            extFlags);
    }
}