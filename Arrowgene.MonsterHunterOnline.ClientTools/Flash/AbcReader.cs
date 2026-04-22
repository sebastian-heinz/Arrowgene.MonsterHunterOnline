#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace Arrowgene.MonsterHunterOnline.ClientTools.Flash;

/// <summary>
/// Minimal AVM2 ABC bytecode parser that extracts class names from a DoABC tag.
/// </summary>
public static class AbcReader
{
    /// <summary>
    /// Extracts fully-qualified class names from raw ABC bytecode.
    /// </summary>
    public static List<string> ReadClassNames(ReadOnlySpan<byte> abcData)
    {
        int offset = 0;

        // Version
        ushort minorVersion = ReadU16(abcData, ref offset);
        ushort majorVersion = ReadU16(abcData, ref offset);

        // Constant pool — numeric pools first, then strings
        SkipS32Pool(abcData, ref offset);
        SkipU32Pool(abcData, ref offset);
        SkipDoublePool(abcData, ref offset);
        string[] strings = ReadStringPool(abcData, ref offset);
        NamespaceEntry[] namespaces = ReadNamespacePool(abcData, ref offset, strings);
        SkipNsSetPool(abcData, ref offset);
        MultinameEntry[] multinames = ReadMultinamePool(abcData, ref offset);

        // Skip method_info
        uint methodCount = ReadU30(abcData, ref offset);
        for (uint i = 0; i < methodCount; i++)
        {
            SkipMethodInfo(abcData, ref offset);
        }

        // Skip metadata_info
        uint metadataCount = ReadU30(abcData, ref offset);
        for (uint i = 0; i < metadataCount; i++)
        {
            SkipMetadataInfo(abcData, ref offset);
        }

        // Read instance_info (class names)
        uint classCount = ReadU30(abcData, ref offset);
        List<string> classNames = new((int)classCount);

        for (uint i = 0; i < classCount; i++)
        {
            uint nameIndex = ReadU30(abcData, ref offset); // multiname index
            string className = ResolveMultiname(nameIndex, multinames, namespaces, strings);
            classNames.Add(className);
            SkipInstanceInfoRemainder(abcData, ref offset);
        }

        return classNames;
    }

    /// <summary>
    /// Extracts class names from a DoABC tag's raw data (includes flags + name prefix).
    /// </summary>
    public static List<string> ReadClassNamesFromDoAbcTag(ReadOnlySpan<byte> tagData)
    {
        if (tagData.Length < 5)
        {
            return [];
        }

        int offset = 4; // skip flags (u32)

        // skip null-terminated name string
        while (offset < tagData.Length && tagData[offset] != 0)
        {
            offset++;
        }

        if (offset < tagData.Length)
        {
            offset++; // skip null terminator
        }

        if (offset >= tagData.Length)
        {
            return [];
        }

        try
        {
            return ReadClassNames(tagData[offset..]);
        }
        catch
        {
            return [];
        }
    }

    // ── Numeric pool skipping ──

    private static void SkipS32Pool(ReadOnlySpan<byte> data, ref int offset)
    {
        uint count = ReadU30(data, ref offset);
        for (uint i = 1; i < count; i++)
        {
            ReadU30(data, ref offset); // s32 uses same encoding as u30
        }
    }

    private static void SkipU32Pool(ReadOnlySpan<byte> data, ref int offset)
    {
        uint count = ReadU30(data, ref offset);
        for (uint i = 1; i < count; i++)
        {
            ReadU30(data, ref offset);
        }
    }

    private static void SkipDoublePool(ReadOnlySpan<byte> data, ref int offset)
    {
        uint count = ReadU30(data, ref offset);
        offset += (int)(count - 1) * 8; // each double is 8 bytes
    }

    // ── Constant pool parsing ──

    private static string[] ReadStringPool(ReadOnlySpan<byte> data, ref int offset)
    {
        uint count = ReadU30(data, ref offset);
        if (count == 0)
        {
            return [string.Empty];
        }

        string[] strings = new string[count];
        strings[0] = string.Empty; // index 0 is always empty
        for (uint i = 1; i < count; i++)
        {
            uint length = ReadU30(data, ref offset);
            strings[i] = Encoding.UTF8.GetString(data.Slice(offset, (int)length));
            offset += (int)length;
        }

        return strings;
    }

    private static NamespaceEntry[] ReadNamespacePool(ReadOnlySpan<byte> data, ref int offset, string[] strings)
    {
        uint count = ReadU30(data, ref offset);
        if (count == 0)
        {
            return [new NamespaceEntry(0, string.Empty)];
        }

        NamespaceEntry[] namespaces = new NamespaceEntry[count];
        namespaces[0] = new NamespaceEntry(0, string.Empty);
        for (uint i = 1; i < count; i++)
        {
            byte kind = data[offset++];
            uint nameIndex = ReadU30(data, ref offset);
            string name = nameIndex < (uint)strings.Length ? strings[nameIndex] : string.Empty;
            namespaces[i] = new NamespaceEntry(kind, name);
        }

        return namespaces;
    }

    private static void SkipNsSetPool(ReadOnlySpan<byte> data, ref int offset)
    {
        uint count = ReadU30(data, ref offset);
        for (uint i = 1; i < count; i++)
        {
            uint nsCount = ReadU30(data, ref offset);
            for (uint j = 0; j < nsCount; j++)
            {
                ReadU30(data, ref offset);
            }
        }
    }

    private static MultinameEntry[] ReadMultinamePool(ReadOnlySpan<byte> data, ref int offset)
    {
        uint count = ReadU30(data, ref offset);
        if (count == 0)
        {
            return [new MultinameEntry(0, 0, 0)];
        }

        MultinameEntry[] multinames = new MultinameEntry[count];
        multinames[0] = new MultinameEntry(0, 0, 0);
        for (uint i = 1; i < count; i++)
        {
            byte kind = data[offset++];
            uint ns = 0, name = 0;

            switch (kind)
            {
                case 0x07: // QName
                case 0x0D: // QNameA
                    ns = ReadU30(data, ref offset);
                    name = ReadU30(data, ref offset);
                    break;
                case 0x0F: // RTQName
                case 0x10: // RTQNameA
                    name = ReadU30(data, ref offset);
                    break;
                case 0x11: // RTQNameL
                case 0x12: // RTQNameLA
                    break;
                case 0x09: // Multiname
                case 0x0E: // MultinameA
                    name = ReadU30(data, ref offset);
                    ReadU30(data, ref offset); // ns_set
                    break;
                case 0x1B: // MultinameL
                case 0x1C: // MultinameLA
                    ReadU30(data, ref offset); // ns_set
                    break;
                case 0x1D: // TypeName (generic)
                    uint typeName = ReadU30(data, ref offset);
                    uint paramCount = ReadU30(data, ref offset);
                    for (uint p = 0; p < paramCount; p++)
                    {
                        ReadU30(data, ref offset);
                    }
                    break;
            }

            multinames[i] = new MultinameEntry(kind, ns, name);
        }

        return multinames;
    }

    // ── Skip helpers ──

    private static void SkipMethodInfo(ReadOnlySpan<byte> data, ref int offset)
    {
        uint paramCount = ReadU30(data, ref offset); // param_count
        ReadU30(data, ref offset); // return_type
        for (uint i = 0; i < paramCount; i++)
        {
            ReadU30(data, ref offset); // param_type
        }

        ReadU30(data, ref offset); // name
        byte flags = data[offset++];

        if ((flags & 0x08) != 0) // HAS_OPTIONAL
        {
            uint optionCount = ReadU30(data, ref offset);
            for (uint i = 0; i < optionCount; i++)
            {
                ReadU30(data, ref offset); // val
                offset++; // kind
            }
        }

        if ((flags & 0x80) != 0) // HAS_PARAM_NAMES
        {
            for (uint i = 0; i < paramCount; i++)
            {
                ReadU30(data, ref offset);
            }
        }
    }

    private static void SkipMetadataInfo(ReadOnlySpan<byte> data, ref int offset)
    {
        ReadU30(data, ref offset); // name
        uint itemCount = ReadU30(data, ref offset);
        for (uint i = 0; i < itemCount; i++)
        {
            ReadU30(data, ref offset); // key
            ReadU30(data, ref offset); // value
        }
    }

    private static void SkipInstanceInfoRemainder(ReadOnlySpan<byte> data, ref int offset)
    {
        // name was already read by caller
        ReadU30(data, ref offset); // super_name
        byte flags = data[offset++];

        if ((flags & 0x08) != 0) // CLASSFLAG_PROTECTEDNS
        {
            ReadU30(data, ref offset);
        }

        uint interfaceCount = ReadU30(data, ref offset);
        for (uint i = 0; i < interfaceCount; i++)
        {
            ReadU30(data, ref offset);
        }

        ReadU30(data, ref offset); // iinit (method index)
        SkipTraits(data, ref offset);
    }

    private static void SkipTraits(ReadOnlySpan<byte> data, ref int offset)
    {
        uint traitCount = ReadU30(data, ref offset);
        for (uint i = 0; i < traitCount; i++)
        {
            ReadU30(data, ref offset); // name
            byte kindByte = data[offset++];
            int kind = kindByte & 0x0F;
            int attr = (kindByte >> 4) & 0x0F;

            switch (kind)
            {
                case 0: // Slot
                case 6: // Const
                    ReadU30(data, ref offset); // slot_id
                    ReadU30(data, ref offset); // type_name
                    uint vindex = ReadU30(data, ref offset);
                    if (vindex != 0)
                    {
                        offset++; // vkind
                    }

                    break;
                case 1: // Method
                case 2: // Getter
                case 3: // Setter
                    ReadU30(data, ref offset); // disp_id
                    ReadU30(data, ref offset); // method
                    break;
                case 4: // Class
                    ReadU30(data, ref offset); // slot_id
                    ReadU30(data, ref offset); // classi
                    break;
                case 5: // Function
                    ReadU30(data, ref offset); // slot_id
                    ReadU30(data, ref offset); // function
                    break;
            }

            if ((attr & 0x04) != 0) // ATTR_Metadata
            {
                uint metadataCount = ReadU30(data, ref offset);
                for (uint m = 0; m < metadataCount; m++)
                {
                    ReadU30(data, ref offset);
                }
            }
        }
    }

    // ── Name resolution ──

    private static string ResolveMultiname(uint index, MultinameEntry[] multinames, NamespaceEntry[] namespaces,
        string[] strings)
    {
        if (index == 0 || index >= (uint)multinames.Length)
        {
            return $"<multiname_{index}>";
        }

        MultinameEntry mn = multinames[index];
        string name = mn.NameIndex < (uint)strings.Length ? strings[mn.NameIndex] : $"<name_{mn.NameIndex}>";

        if (mn.Kind is 0x07 or 0x0D) // QName
        {
            string ns = mn.NsIndex < (uint)namespaces.Length ? namespaces[mn.NsIndex].Name : string.Empty;
            return string.IsNullOrEmpty(ns) ? name : $"{ns}.{name}";
        }

        return name;
    }

    // ── Primitives ──

    private static uint ReadU30(ReadOnlySpan<byte> data, ref int offset)
    {
        uint result = 0;
        int shift = 0;
        for (int i = 0; i < 5; i++)
        {
            byte b = data[offset++];
            result |= (uint)(b & 0x7F) << shift;
            if ((b & 0x80) == 0)
            {
                break;
            }

            shift += 7;
        }

        return result;
    }

    private static ushort ReadU16(ReadOnlySpan<byte> data, ref int offset)
    {
        ushort value = (ushort)(data[offset] | (data[offset + 1] << 8));
        offset += 2;
        return value;
    }

    // ── Data types ──

    private readonly record struct NamespaceEntry(byte Kind, string Name);

    private readonly record struct MultinameEntry(byte Kind, uint NsIndex, uint NameIndex);
}
