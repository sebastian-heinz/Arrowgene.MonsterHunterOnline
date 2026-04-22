#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.ClientTools.IIPS;

internal static class IIPSArchiveFormat
{
    public const uint HeaderLength = 0xAC;
    public const uint Magic = 0x7366696E; // "nifs"
    public const uint HetSignature = 0x1A544548; // "HET\x1A"
    public const uint BetSignature = 0x1A544542; // "BET\x1A"
    public const uint SectionVersion = 1;
    public const uint HashBits = 64;
    public const uint BetHashBits = 56;

    public static ulong MaskNameHash(ulong rawHash)
    {
        return rawHash | (1UL << 63);
    }

    public static uint GetSectorSize(ushort sectorSizeShift)
    {
        return 0x200u << (sectorSizeShift & 0x1F);
    }

    public static int BitsRequired(ulong value)
    {
        return value == 0 ? 1 : BitOperations.Log2(value) + 1;
    }

    public static ulong GetStoredLength(IIPSArchiveEntryRecord record)
    {
        return record.CompressedSize == 0 ? record.FileSize : record.CompressedSize;
    }

    public static ulong GetPhysicalStoredLength(IIPSArchiveEntryRecord record, uint sectorSize)
    {
        ulong baseLen = record.CompressedSize == 0 ? record.FileSize : record.CompressedSize;
        bool singleUnit = (record.Flags & (uint)IIPSArchiveEntryFlags.SingleUnit) != 0;
        if (singleUnit || record.FileSize == 0 || sectorSize == 0)
        {
            return baseLen;
        }

        ulong numSectors = (record.FileSize + sectorSize - 1) / sectorSize;
        return baseLen + numSectors * 16;
    }

    public static byte[] Decompress(byte[] compressed, int expectedSize, ILogger logger)
    {
        if (compressed.Length == 0)
        {
            return Array.Empty<byte>();
        }

        byte method = compressed[0];
        byte[] payload = new byte[compressed.Length - 1];
        Array.Copy(compressed, 1, payload, 0, payload.Length);

        if ((method & 0x02) != 0)
        {
            return ZlibDecompress(payload, expectedSize, logger);
        }

        if ((method & 0x10) != 0)
        {
            logger.Info($"Bzip2 compression (0x{method:X2}) not fully supported, trying zlib");
            return ZlibDecompress(payload, expectedSize, logger);
        }

        if (method == 0x00)
        {
            return payload;
        }

        logger.Info($"Unknown compression type 0x{method:X2}, trying zlib");
        return ZlibDecompress(payload, expectedSize, logger);
    }

    public static byte[] Compress(byte[] input)
    {
        using MemoryStream ms = new MemoryStream();
        ms.WriteByte(0x02);
        using (DeflateStream deflate = new DeflateStream(ms, CompressionLevel.SmallestSize, leaveOpen: true))
        {
            deflate.Write(input, 0, input.Length);
        }

        return ms.ToArray();
    }

    public static ulong ReadBits(byte[] data, long bitOffset, int bitCount)
    {
        if (bitCount == 0 || bitCount > 64)
        {
            return 0;
        }

        ulong result = 0;
        for (int i = 0; i < bitCount; i++)
        {
            long currentBit = bitOffset + i;
            int byteIndex = (int)(currentBit / 8);
            int bitIndex = (int)(currentBit % 8);
            if (byteIndex >= data.Length)
            {
                break;
            }

            if ((data[byteIndex] & (1 << bitIndex)) != 0)
            {
                result |= 1UL << i;
            }
        }

        return result;
    }

    public static byte[] ReadBitsAsBytes(byte[] data, long bitOffset, int bitCount)
    {
        int byteCount = (bitCount + 7) / 8;
        byte[] result = new byte[byteCount];
        for (int i = 0; i < bitCount; i++)
        {
            long currentBit = bitOffset + i;
            int sourceByteIndex = (int)(currentBit / 8);
            int sourceBitIndex = (int)(currentBit % 8);
            if (sourceByteIndex >= data.Length)
            {
                break;
            }

            if ((data[sourceByteIndex] & (1 << sourceBitIndex)) != 0)
            {
                result[i / 8] |= (byte)(1 << (i % 8));
            }
        }

        return result;
    }

    public static void WriteBits(byte[] destination, long bitOffset, int bitCount, ulong value)
    {
        for (int i = 0; i < bitCount; i++)
        {
            if (((value >> i) & 1UL) == 0)
            {
                continue;
            }

            long currentBit = bitOffset + i;
            int byteIndex = (int)(currentBit / 8);
            int bitIndex = (int)(currentBit % 8);
            destination[byteIndex] |= (byte)(1 << bitIndex);
        }
    }

    public static void WriteBits(byte[] destination, long bitOffset, ReadOnlySpan<byte> value)
    {
        for (int i = 0; i < value.Length * 8; i++)
        {
            if ((value[i / 8] & (1 << (i % 8))) == 0)
            {
                continue;
            }

            long currentBit = bitOffset + i;
            int byteIndex = (int)(currentBit / 8);
            int bitIndex = (int)(currentBit % 8);
            destination[byteIndex] |= (byte)(1 << bitIndex);
        }
    }

    public static IEnumerable<string> ParseFileNames(string content)
    {
        string[] lines = content.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string name = line.Trim();
            if (string.IsNullOrEmpty(name))
            {
                continue;
            }

            int tildeIndex = name.IndexOf('~');
            if (tildeIndex >= 0)
            {
                name = name[..tildeIndex];
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }
            }

            yield return name;
        }
    }

    private static byte[] ZlibDecompress(byte[] data, int expectedSize, ILogger logger)
    {
        try
        {
            using MemoryStream ms = new MemoryStream(data);
            if (data.Length >= 2 && (data[0] & 0x0F) == 0x08)
            {
                ms.Position = 2;
            }

            using DeflateStream deflate = new DeflateStream(ms, CompressionMode.Decompress);
            byte[] result = new byte[expectedSize];
            int total = 0;
            while (total < expectedSize)
            {
                int read = deflate.Read(result, total, expectedSize - total);
                if (read == 0)
                {
                    break;
                }

                total += read;
            }

            return result;
        }
        catch (Exception ex)
        {
            logger.Error($"Zlib decompress failed: {ex.Message}");
            return data;
        }
    }
}

internal sealed class IIPSArchiveEntryRecord
{
    public int Index { get; set; }
    public ulong FileOffset { get; set; }
    public ulong FileSize { get; set; }
    public ulong CompressedSize { get; set; }
    public uint Flags { get; set; }
    public ulong NameHash { get; set; }
    public int HetIndex { get; set; } = -1;
    public string? FileName { get; set; }
    public byte[]? Md5 { get; set; }
    public ulong Extra { get; set; }
    public IIPSArchiveEntrySourceKind SourceKind { get; set; }
    public byte[]? Content { get; set; }
    public IIPSArchiveEntryOptions WriteOptions { get; set; } = new();

    public bool Exists => (Flags & (uint)IIPSArchiveEntryFlags.Exists) != 0;
    public bool IsEncrypted => (Flags & (uint)IIPSArchiveEntryFlags.Encrypted) != 0;
    public bool UsesFixedKey => (Flags & (uint)IIPSArchiveEntryFlags.FixKey) != 0;
    public bool IsSingleUnit => (Flags & (uint)IIPSArchiveEntryFlags.SingleUnit) != 0;
}

internal enum IIPSArchiveEntrySourceKind
{
    ExistingArchive,
    Memory,
}

internal sealed class IIPSArchiveLookup
{
    private readonly uint _totalCount;
    private readonly uint _indexBits;
    private readonly uint _indexStrideBits;
    private readonly uint _hashBits;
    private readonly byte[] _nameHashBytes;
    private readonly byte[] _fileIndexData;
    private readonly ulong _andMask;
    private readonly ulong _orMask;

    public IIPSArchiveLookup(
        uint totalCount,
        uint indexBits,
        uint indexStrideBits,
        uint hashBits,
        byte[] nameHashBytes,
        byte[] fileIndexData)
    {
        _totalCount = totalCount;
        _indexBits = indexBits;
        _indexStrideBits = indexStrideBits;
        _hashBits = hashBits;
        _nameHashBytes = nameHashBytes;
        _fileIndexData = fileIndexData;
        _andMask = hashBits >= 64 ? ulong.MaxValue : (1UL << (int)hashBits) - 1;
        _orMask = 1UL << ((int)hashBits - 1);
    }

    public int FindFileIndex(string fileName, IReadOnlyList<IIPSArchiveEntryRecord> entries)
    {
        if (_nameHashBytes.Length == 0 || _fileIndexData.Length == 0 || _totalCount == 0)
        {
            return -1;
        }

        ulong maskedHash = (IIPSArchiveCrypto.ComputeNameHash(fileName) & _andMask) | _orMask;
        byte expectedByte = (byte)(maskedHash >> ((int)_hashBits - 8));
        uint startSlot = (uint)(maskedHash % _totalCount);

        for (uint attempt = 0; attempt < _totalCount; attempt++)
        {
            uint slot = (startSlot + attempt) % _totalCount;
            byte hashByte = _nameHashBytes[slot];
            if (hashByte == 0x00)
            {
                return -1;
            }

            if (hashByte != expectedByte)
            {
                continue;
            }

            int fileIndex = (int)IIPSArchiveFormat.ReadBits(_fileIndexData, (long)slot * _indexStrideBits, (int)_indexBits);
            if (fileIndex < 0 || fileIndex >= entries.Count)
            {
                continue;
            }

            if (entries[fileIndex].NameHash == maskedHash)
            {
                return fileIndex;
            }
        }

        return -1;
    }
}

internal sealed class IIPSArchiveHeaderData
{
    public uint Magic { get; set; }
    public uint HeaderLength { get; set; }
    public ushort FormatVersion { get; set; }
    public ushort SectorSizeShift { get; set; }
    public ulong ArchiveSize { get; set; }
    public ulong BetOffset { get; set; }
    public ulong HetOffset { get; set; }
    public ulong Md5TableOffset { get; set; }
    public ulong BitmapOffset { get; set; }
    public ulong HetLength { get; set; }
    public ulong BetLength { get; set; }
    public ulong Md5TableLength { get; set; }
    public ulong BitmapLength { get; set; }
    public uint Md5PieceSize { get; set; }
    public uint RawChunkSize { get; set; }
    public byte[] Md5PatchBaseTag { get; set; } = new byte[16];
    public byte[] Md5PatchedTag { get; set; } = new byte[16];
    public string BetMd5 { get; set; } = string.Empty;
    public string HetMd5 { get; set; } = string.Empty;
    public string HeaderMd5 { get; set; } = string.Empty;
}

internal static class IIPSArchiveSerialization
{
    public static byte[] BuildHeader(IIPSArchiveHeaderData header)
    {
        byte[] betMd5 = Convert.FromHexString(header.BetMd5);
        byte[] hetMd5 = Convert.FromHexString(header.HetMd5);

        using MemoryStream ms = new MemoryStream((int)IIPSArchiveFormat.HeaderLength);
        using BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
        writer.Write(header.Magic);
        writer.Write(header.HeaderLength);
        writer.Write(header.FormatVersion);
        writer.Write(header.SectorSizeShift);
        writer.Write(header.ArchiveSize);
        writer.Write(header.BetOffset);
        writer.Write(header.HetOffset);
        writer.Write(header.Md5TableOffset);
        writer.Write(header.BitmapOffset);
        writer.Write(header.HetLength);
        writer.Write(header.BetLength);
        writer.Write(header.Md5TableLength);
        writer.Write(header.BitmapLength);
        writer.Write(header.Md5PieceSize);
        writer.Write(header.RawChunkSize);
        writer.Write(header.Md5PatchBaseTag);
        writer.Write(header.Md5PatchedTag);
        writer.Write(betMd5);
        writer.Write(hetMd5);
        writer.Write(new byte[16]);

        byte[] headerBytes = ms.ToArray();
        byte[] headerMd5 = MD5.HashData(headerBytes.AsSpan(0, headerBytes.Length - 16));
        headerMd5.CopyTo(headerBytes, headerBytes.Length - 16);
        header.HeaderMd5 = Convert.ToHexString(headerMd5).ToLowerInvariant();
        return headerBytes;
    }

    public static byte[] BuildSection(uint signature, byte[] decryptedData)
    {
        byte[] encryptedData = (byte[])decryptedData.Clone();
        IIPSArchiveCrypto.IfsSectionEncrypt(encryptedData);
        using MemoryStream ms = new MemoryStream(12 + encryptedData.Length);
        using BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
        writer.Write(signature);
        writer.Write(IIPSArchiveFormat.SectionVersion);
        writer.Write((uint)encryptedData.Length);
        writer.Write(encryptedData);
        return ms.ToArray();
    }
}
