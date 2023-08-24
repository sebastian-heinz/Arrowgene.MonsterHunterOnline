using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Arrowgene.MonsterHunterOnline.Service.System.UnlockSystem;

public static class SystemUnlock
{
    /// <summary>
    /// Returns `ExtFlags` (bits 33 - 64)
    /// </summary>
    public static SystemUnlockExtFlags ToExtFlags(this SystemUnlockFlags flags)
    {
        return (SystemUnlockExtFlags)(flags.ToUInt64() >> 32);
    }

    /// <summary>
    /// Returns the ulong value
    /// </summary>
    public static ulong ToUInt64(this SystemUnlockFlags flags)
    {
        return (ulong)flags;
    }

    /// <summary>
    /// Returns the first 32bits as int value
    /// </summary>
    public static int ToInt32(this SystemUnlockFlags flags)
    {
        return (int)(flags.ToUInt64() & uint.MaxValue);
    }

    /// <summary>
    /// Returns the first 32bits as int value
    /// </summary>
    public static SystemUnlockFlags FromInt32(this SystemUnlockFlags flags, int value)
    {
        return (SystemUnlockFlags)(uint)value;
    }

    /// <summary>
    /// Returns the int value
    /// </summary>
    public static int ToInt32(this SystemUnlockExtFlags flags)
    {
        return (int)flags;
    }
    
    /// <summary>
    /// Returns flags set based on level, by determinating which systems are available for the given level.
    /// </summary>
    public static SystemUnlockFlags GetForLevel(uint level)
    {
        ulong systemUnlockvalue = 0;

        string staticFolder = Path.Combine(Util.ExecutingDirectory(), "Files\\Static");
        string csvPath = Path.Combine(staticFolder, "SystemUnlock.csv");
        using (TextFieldParser parser = new TextFieldParser(csvPath))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // Skip the header line
            parser.ReadLine();
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                string id = fields[0];
                string unlockLevel = fields[2]; // level to unlock
                string defaultUnlock = fields[7]; // is unlocked by default

                if (defaultUnlock == "1" || (unlockLevel != "" && level >= int.Parse(unlockLevel)))
                {
                    systemUnlockvalue += (ulong)Math.Pow(2, int.Parse(id) - 1);
                }
            }
        }

        return (SystemUnlockFlags)systemUnlockvalue;
    }
}