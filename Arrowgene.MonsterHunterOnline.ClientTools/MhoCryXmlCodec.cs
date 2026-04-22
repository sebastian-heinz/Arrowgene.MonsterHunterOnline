#nullable enable
using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Arrowgene.MonsterHunterOnline.ClientTools;

public enum MhoCryXmlFormat
{
    NotXml,
    PlainXml,
    CryXmlBinary,
    EncryptedXml,
    EncryptedCryXmlBinary,
}

/// <summary>
/// Decodes Monster Hunter Online XML assets that use the MHO encrypted file envelope
/// and optionally the CryEngine binary XML format (<c>CryXmlB</c>).
/// </summary>
public static class MhoCryXmlCodec
{
    private static readonly byte[] EncryptedHeader = [0xFF, 0xFF, 0x6D, 0x68];

    // Ported from Andoryuuta/MHOFileDecrypt with the same corrected trailing bytes.
    private static readonly byte[] DecryptTable0 =
    [
        0x09, 0x40, 0x48, 0x19, 0xC1, 0x8F, 0x83, 0xF5, 0x60, 0x09, 0x6F, 0x14, 0x0F, 0xBE, 0x51, 0xEA,
        0x7A, 0x81, 0x08, 0xB4, 0x76, 0xB6, 0x1A, 0x91, 0x5A, 0x74, 0x70, 0xC9, 0xDD, 0x83, 0xE5, 0x04,
        0x9F, 0x48, 0xC8, 0x48, 0xA0, 0x9E, 0x9B, 0x8F, 0x8B, 0x0F, 0x9C, 0x01, 0x94, 0x34, 0x62, 0x29,
        0x99, 0xB7, 0xDC, 0x77, 0xFC, 0x87, 0xB2, 0x39, 0xFB, 0x8F, 0x6D, 0xD6, 0x51, 0x97, 0x6C, 0xD8,
        0x91,
    ];

    // Ported from Andoryuuta/MHOFileDecrypt with the same corrected trailing bytes.
    private static readonly byte[] DecryptTable1 =
    [
        0xC7, 0xAB, 0x19, 0x5A, 0x77, 0x88, 0xFA, 0x21, 0xAB, 0x5D, 0x7D, 0x33, 0xAA, 0x3A, 0x75, 0x0A,
        0xF9, 0x7C, 0x76, 0xB6, 0x6A, 0xE3, 0x05, 0xD5, 0x77, 0xCF, 0xF2, 0xFB, 0x2D, 0xB2, 0x1B, 0x29,
        0x17, 0x50, 0x04, 0xDA, 0x4A, 0xC7, 0x8C, 0x31, 0x4A, 0x51, 0xA8, 0x3B, 0x9E, 0xE5, 0xDE, 0x4B,
        0x75, 0x7C, 0x47, 0x54, 0xFB, 0x03, 0x24, 0xA6, 0x13, 0x4A, 0xCB, 0xE9, 0x5E, 0x34, 0xE1, 0xA1,
        0x80,
    ];

    public static bool IsEncrypted(ReadOnlySpan<byte> data)
    {
        return data.Length >= EncryptedHeader.Length && data[..EncryptedHeader.Length].SequenceEqual(EncryptedHeader);
    }

    public static bool IsCryXmlBinary(ReadOnlySpan<byte> data)
    {
        return data.Length >= 7 && data[..7].SequenceEqual("CryXmlB"u8);
    }

    public static MhoCryXmlFormat DetectFormat(ReadOnlySpan<byte> data)
    {
        if (IsEncrypted(data))
        {
            byte[] decrypted = Decrypt(data);
            if (IsCryXmlBinary(decrypted))
            {
                return MhoCryXmlFormat.EncryptedCryXmlBinary;
            }

            return LooksLikeXml(decrypted)
                ? MhoCryXmlFormat.EncryptedXml
                : MhoCryXmlFormat.NotXml;
        }

        if (IsCryXmlBinary(data))
        {
            return MhoCryXmlFormat.CryXmlBinary;
        }

        return LooksLikeXml(data)
            ? MhoCryXmlFormat.PlainXml
            : MhoCryXmlFormat.NotXml;
    }

    public static bool IsCryXmlCodec(ReadOnlySpan<byte> data)
    {
        return DetectFormat(data) != MhoCryXmlFormat.NotXml;
    }

    // Alias kept for UI call sites that use the requested "Codex" naming.
    public static bool IsCryXmlCodex(ReadOnlySpan<byte> data)
    {
        return IsCryXmlCodec(data);
    }

    public static bool NeedsDecryption(ReadOnlySpan<byte> data)
    {
        return DetectFormat(data) is MhoCryXmlFormat.EncryptedXml or MhoCryXmlFormat.EncryptedCryXmlBinary;
    }

    public static byte[] Encrypt(ReadOnlySpan<byte> plainData)
    {
        byte[] payload = plainData.ToArray();
        EncryptInPlace(payload);

        byte[] result = new byte[EncryptedHeader.Length + payload.Length];
        EncryptedHeader.CopyTo(result, 0);
        payload.CopyTo(result, EncryptedHeader.Length);
        return result;
    }

    public static byte[] Decrypt(ReadOnlySpan<byte> data)
    {
        if (!IsEncrypted(data))
        {
            throw new InvalidDataException("Expected an MHO encrypted XML file starting with 0xFF 0xFF 0x6D 0x68.");
        }

        byte[] decrypted = data[EncryptedHeader.Length..].ToArray();
        DecryptInPlace(decrypted);
        return decrypted;
    }

    public static XDocument LoadDocument(ReadOnlySpan<byte> data)
    {
        MhoCryXmlFormat format = DetectFormat(data);
        byte[] payload = format is MhoCryXmlFormat.EncryptedXml or MhoCryXmlFormat.EncryptedCryXmlBinary
            ? Decrypt(data)
            : data.ToArray();

        if (format is MhoCryXmlFormat.CryXmlBinary or MhoCryXmlFormat.EncryptedCryXmlBinary)
        {
            return ParseCryXmlBinary(payload);
        }

        if (format is MhoCryXmlFormat.NotXml)
        {
            throw new InvalidDataException("The decoded payload is neither text XML nor CryXmlB.");
        }

        using MemoryStream stream = new(payload, writable: false);
        return XDocument.Load(stream, LoadOptions.PreserveWhitespace);
    }

    public static XDocument LoadFile(string path)
    {
        return LoadDocument(File.ReadAllBytes(path));
    }

    public static string ReadXml(ReadOnlySpan<byte> data, bool indent = true)
    {
        return SerializeXml(LoadDocument(data), indent);
    }

    public static string ReadXmlFile(string path, bool indent = true)
    {
        return ReadXml(File.ReadAllBytes(path), indent);
    }

    public static void WriteXmlFile(string inputPath, string outputPath, bool indent = true)
    {
        string xml = ReadXmlFile(inputPath, indent);
        File.WriteAllText(outputPath, xml, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
    }

    private static void DecryptInPlace(byte[] data)
    {
        byte[] scratch = new byte[129];
        int offset = 0;
        int remaining = data.Length;

        while (remaining >= 129)
        {
            int i = 0;
            for (; i < 64; i++)
            {
                scratch[i + 0x41] = (byte)(data[offset + i] ^ DecryptTable0[i]);
            }

            int j = 0;
            for (; i < 129; j++, i++)
            {
                scratch[j] = (byte)(data[offset + i] ^ DecryptTable1[j]);
            }

            Buffer.BlockCopy(scratch, 0, data, offset, 129);
            offset += 129;
            remaining -= 129;
        }

        if (remaining <= 0)
        {
            return;
        }

        if (remaining <= 65)
        {
            for (int i = 0; i < remaining; i++)
            {
                scratch[i] = (byte)(data[offset + i] ^ DecryptTable1[i]);
            }
        }
        else
        {
            int rotatedPrefixLength = remaining - 65;
            int i = 0;
            for (; i < rotatedPrefixLength; i++)
            {
                scratch[i + 0x41] = (byte)(data[offset + i] ^ DecryptTable0[i]);
            }

            int j = 0;
            for (; i < remaining; j++, i++)
            {
                scratch[j] = (byte)(data[offset + i] ^ DecryptTable1[j]);
            }
        }

        Buffer.BlockCopy(scratch, 0, data, offset, remaining);
    }

    private static void EncryptInPlace(byte[] data)
    {
        byte[] scratch = new byte[129];
        int offset = 0;
        int remaining = data.Length;

        while (remaining >= 129)
        {
            for (int i = 0; i < 64; i++)
            {
                scratch[i] = (byte)(data[offset + i + 65] ^ DecryptTable0[i]);
            }

            for (int j = 0; j < 65; j++)
            {
                scratch[64 + j] = (byte)(data[offset + j] ^ DecryptTable1[j]);
            }

            Buffer.BlockCopy(scratch, 0, data, offset, 129);
            offset += 129;
            remaining -= 129;
        }

        if (remaining <= 0)
        {
            return;
        }

        if (remaining <= 65)
        {
            for (int i = 0; i < remaining; i++)
            {
                scratch[i] = (byte)(data[offset + i] ^ DecryptTable1[i]);
            }
        }
        else
        {
            int rotatedPrefixLength = remaining - 65;
            for (int i = 0; i < rotatedPrefixLength; i++)
            {
                scratch[i] = (byte)(data[offset + 65 + i] ^ DecryptTable0[i]);
            }

            for (int j = 0; j < 65; j++)
            {
                scratch[rotatedPrefixLength + j] = (byte)(data[offset + j] ^ DecryptTable1[j]);
            }
        }

        Buffer.BlockCopy(scratch, 0, data, offset, remaining);
    }

    private static XDocument ParseCryXmlBinary(ReadOnlySpan<byte> data)
    {
        int cursor = 0;
        string header = ReadCString(data, ref cursor);
        if (!string.Equals(header, "CryXmlB", StringComparison.Ordinal))
        {
            throw new InvalidDataException($"Unexpected CryXml header '{header}'.");
        }

        int fileSize = ReadInt32(data, ref cursor);
        int nodeTableOffset = ReadInt32(data, ref cursor);
        int nodeTableCount = ReadInt32(data, ref cursor);
        int attrTableOffset = ReadInt32(data, ref cursor);
        int attrTableCount = ReadInt32(data, ref cursor);
        int childTableOffset = ReadInt32(data, ref cursor);
        int childTableCount = ReadInt32(data, ref cursor);
        int dataTableOffset = ReadInt32(data, ref cursor);
        int dataTableSize = ReadInt32(data, ref cursor);

        if (fileSize > data.Length)
        {
            throw new InvalidDataException($"CryXmlB header declares {fileSize} bytes, but the buffer only has {data.Length} bytes.");
        }

        ValidateTableBounds(nodeTableOffset, nodeTableCount, 28, data.Length, "node");
        ValidateTableBounds(attrTableOffset, attrTableCount, 8, data.Length, "attribute");
        ValidateTableBounds(childTableOffset, childTableCount, 4, data.Length, "child");
        ValidateSliceBounds(dataTableOffset, dataTableSize, data.Length, "string");

        CryXmlNode[] nodes = new CryXmlNode[nodeTableCount];
        for (int i = 0; i < nodeTableCount; i++)
        {
            int offset = nodeTableOffset + (i * 28);
            nodes[i] = new CryXmlNode(
                BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4)),
                BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset + 4, 4)),
                BinaryPrimitives.ReadInt16LittleEndian(data.Slice(offset + 8, 2)),
                BinaryPrimitives.ReadInt16LittleEndian(data.Slice(offset + 10, 2)),
                BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset + 12, 4)),
                BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset + 16, 4)),
                BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset + 20, 4)));
        }

        CryXmlAttributeRef[] attributes = new CryXmlAttributeRef[attrTableCount];
        for (int i = 0; i < attrTableCount; i++)
        {
            int offset = attrTableOffset + (i * 8);
            attributes[i] = new CryXmlAttributeRef(
                BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4)),
                BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset + 4, 4)));
        }

        int[] childTable = new int[childTableCount];
        for (int i = 0; i < childTableCount; i++)
        {
            int offset = childTableOffset + (i * 4);
            childTable[i] = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4));
        }

        ReadOnlySpan<byte> stringTable = data.Slice(dataTableOffset, dataTableSize);
        XElement[] elements = new XElement[nodeTableCount];
        for (int i = 0; i < nodeTableCount; i++)
        {
            CryXmlNode node = nodes[i];
            XElement element = new(ReadCString(stringTable, node.NameOffset));

            for (int j = 0; j < node.AttributeCount; j++)
            {
                int attributeIndex = checked(node.FirstAttributeIndex + j);
                if ((uint)attributeIndex >= (uint)attributes.Length)
                {
                    throw new InvalidDataException($"CryXml attribute index {attributeIndex} is out of range.");
                }

                CryXmlAttributeRef attribute = attributes[attributeIndex];
                element.SetAttributeValue(
                    ReadCString(stringTable, attribute.NameOffset),
                    ReadCString(stringTable, attribute.ValueOffset));
            }

            string content = ReadCString(stringTable, node.ContentOffset);
            if (content.Length > 0)
            {
                element.Add(new XText(content));
            }

            elements[i] = element;
        }

        int rootIndex = -1;
        bool[] attached = new bool[nodeTableCount];

        for (int i = 0; i < nodeTableCount; i++)
        {
            CryXmlNode node = nodes[i];
            if (node.ParentId == -1)
            {
                if (rootIndex != -1)
                {
                    throw new InvalidDataException("CryXml contains multiple root nodes.");
                }

                rootIndex = i;
                continue;
            }

            if ((uint)node.ParentId >= (uint)elements.Length)
            {
                throw new InvalidDataException($"CryXml parent index {node.ParentId} is out of range.");
            }

        }

        if (rootIndex == -1)
        {
            throw new InvalidDataException("CryXml does not contain a root node.");
        }

        for (int i = 0; i < nodeTableCount; i++)
        {
            CryXmlNode node = nodes[i];
            if (node.ChildCount == 0)
            {
                continue;
            }

            int firstChildIndex = node.FirstChildIndex;
            if (firstChildIndex < 0)
            {
                throw new InvalidDataException("CryXml child table index cannot be negative when child_count > 0.");
            }

            int childEnd = checked(firstChildIndex + node.ChildCount);
            if (childEnd > childTable.Length)
            {
                throw new InvalidDataException("CryXml child table extends past the declared child table.");
            }

            for (int childSlot = firstChildIndex; childSlot < childEnd; childSlot++)
            {
                int childIndex = childTable[childSlot];
                if ((uint)childIndex >= (uint)elements.Length)
                {
                    throw new InvalidDataException($"CryXml child node index {childIndex} is out of range.");
                }

                if (nodes[childIndex].ParentId != i)
                {
                    throw new InvalidDataException($"CryXml child node {childIndex} does not point back to parent node {i}.");
                }

                elements[i].Add(elements[childIndex]);
                attached[childIndex] = true;
            }
        }

        for (int i = 0; i < attached.Length; i++)
        {
            if (i != rootIndex && !attached[i])
            {
                throw new InvalidDataException($"CryXml node {i} is not attached to the root.");
            }
        }

        return new XDocument(new XDeclaration("1.0", "utf-8", null), elements[rootIndex]);
    }

    private static bool LooksLikeXml(ReadOnlySpan<byte> data)
    {
        int offset = 0;
        if (data.Length >= 3 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
        {
            offset = 3;
        }

        while (offset < data.Length && char.IsWhiteSpace((char)data[offset]))
        {
            offset++;
        }

        return offset < data.Length && data[offset] == (byte)'<';
    }

    private static string SerializeXml(XDocument document, bool indent)
    {
        XmlWriterSettings settings = new()
        {
            Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
            Indent = indent,
            OmitXmlDeclaration = false,
        };

        using MemoryStream stream = new();
        using (XmlWriter xmlWriter = XmlWriter.Create(stream, settings))
        {
            document.Save(xmlWriter);
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static void ValidateTableBounds(int offset, int count, int elementSize, int length, string tableName)
    {
        if (offset < 0 || count < 0)
        {
            throw new InvalidDataException($"CryXml {tableName} table has a negative offset or count.");
        }

        long end = (long)offset + ((long)count * elementSize);
        if (end > length)
        {
            throw new InvalidDataException($"CryXml {tableName} table extends past the end of the buffer.");
        }
    }

    private static void ValidateSliceBounds(int offset, int size, int length, string sliceName)
    {
        if (offset < 0 || size < 0)
        {
            throw new InvalidDataException($"CryXml {sliceName} table has a negative offset or size.");
        }

        long end = (long)offset + size;
        if (end > length)
        {
            throw new InvalidDataException($"CryXml {sliceName} table extends past the end of the buffer.");
        }
    }

    private static int ReadInt32(ReadOnlySpan<byte> data, ref int offset)
    {
        if (offset + 4 > data.Length)
        {
            throw new InvalidDataException("Unexpected end of CryXmlB header.");
        }

        int value = BinaryPrimitives.ReadInt32LittleEndian(data.Slice(offset, 4));
        offset += 4;
        return value;
    }

    private static string ReadCString(ReadOnlySpan<byte> data, ref int offset)
    {
        if ((uint)offset >= (uint)data.Length)
        {
            throw new InvalidDataException("Encountered an invalid string offset in CryXml data.");
        }

        int end = data[offset..].IndexOf((byte)0);
        if (end < 0)
        {
            throw new InvalidDataException("Encountered an unterminated string in CryXml data.");
        }

        string value = Encoding.UTF8.GetString(data.Slice(offset, end));
        offset += end + 1;
        return value;
    }

    private static string ReadCString(ReadOnlySpan<byte> data, int offset)
    {
        if (offset < 0)
        {
            return string.Empty;
        }

        if (offset >= data.Length)
        {
            throw new InvalidDataException($"CryXml string offset {offset} is out of range.");
        }

        int end = data[offset..].IndexOf((byte)0);
        if (end < 0)
        {
            throw new InvalidDataException($"CryXml string at offset {offset} is missing a null terminator.");
        }

        return Encoding.UTF8.GetString(data.Slice(offset, end));
    }

    private readonly record struct CryXmlNode(
        int NameOffset,
        int ContentOffset,
        short AttributeCount,
        short ChildCount,
        int ParentId,
        int FirstAttributeIndex,
        int FirstChildIndex);

    private readonly record struct CryXmlAttributeRef(int NameOffset, int ValueOffset);
}
