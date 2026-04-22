#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.ClientTools.IIPS;

internal static class IIPSArchiveReader
{
    public static void Load(IIPSArchive archive, string path, IIPSArchiveOpenOptions options)
    {
        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        BinaryReader reader = new BinaryReader(stream);

        try
        {
            IIPSArchiveHeaderData header = ReadHeader(stream, reader, options.VerifyChecksums, archive.ArchiveLogger);

            stream.Position = (long)header.HetOffset;
            uint hetMagic = reader.ReadUInt32();
            uint hetVersion = reader.ReadUInt32();
            uint hetDataLength = reader.ReadUInt32();
            byte[] hetData = reader.ReadBytes((int)hetDataLength);
            IIPSArchiveCrypto.IfsSectionDecrypt(hetData);
            IIPSArchiveLookup lookup = ParseHetTable(hetData, archive.ArchiveLogger, out HetState hetState);

            stream.Position = (long)header.BetOffset;
            uint betMagic = reader.ReadUInt32();
            uint betVersion = reader.ReadUInt32();
            uint betDataLength = reader.ReadUInt32();
            byte[] betData = reader.ReadBytes((int)betDataLength);
            IIPSArchiveCrypto.IfsSectionDecrypt(betData);
            List<IIPSArchiveEntryRecord> records = ParseBetEntries(betData, hetState, archive.ArchiveLogger);

            IIPSArchiveMetadata metadata = new IIPSArchiveMetadata
            {
                FormatVersion = header.FormatVersion,
                SectorSizeShift = header.SectorSizeShift,
                Md5PieceSize = header.Md5PieceSize,
                RawChunkSize = header.RawChunkSize,
                HeaderMd5 = header.HeaderMd5,
                BetMd5 = header.BetMd5,
                HetMd5 = header.HetMd5,
            };

            archive.ReplaceState(path, stream, reader, metadata, records, lookup);

            if (options.LoadListFile)
            {
                TryLoadEmbeddedListFile(archive);
            }
        }
        catch
        {
            reader.Dispose();
            stream.Dispose();
            throw;
        }
    }

    private static IIPSArchiveHeaderData ReadHeader(FileStream stream, BinaryReader reader, bool verifyChecksums, Arrowgene.Logging.ILogger logger)
    {
        stream.Position = 0;
        IIPSArchiveHeaderData header = new IIPSArchiveHeaderData
        {
            Magic = reader.ReadUInt32(),
            HeaderLength = reader.ReadUInt32(),
            FormatVersion = reader.ReadUInt16(),
            SectorSizeShift = reader.ReadUInt16(),
            ArchiveSize = reader.ReadUInt64(),
            BetOffset = reader.ReadUInt64(),
            HetOffset = reader.ReadUInt64(),
            Md5TableOffset = reader.ReadUInt64(),
            BitmapOffset = reader.ReadUInt64(),
            HetLength = reader.ReadUInt64(),
            BetLength = reader.ReadUInt64(),
            Md5TableLength = reader.ReadUInt64(),
            BitmapLength = reader.ReadUInt64(),
            Md5PieceSize = reader.ReadUInt32(),
            RawChunkSize = reader.ReadUInt32(),
            Md5PatchBaseTag = reader.ReadBytes(16),
            Md5PatchedTag = reader.ReadBytes(16),
            BetMd5 = Convert.ToHexString(reader.ReadBytes(16)).ToLowerInvariant(),
            HetMd5 = Convert.ToHexString(reader.ReadBytes(16)).ToLowerInvariant(),
            HeaderMd5 = Convert.ToHexString(reader.ReadBytes(16)).ToLowerInvariant(),
        };

        if (header.Magic != IIPSArchiveFormat.Magic)
        {
            logger.Info("MAGIC mismatch");
        }

        if (header.HeaderLength != IIPSArchiveFormat.HeaderLength)
        {
            logger.Info("Header length mismatch");
        }

        if (!verifyChecksums)
        {
            return header;
        }

        byte[] headerBytes = ReadBytesAt(stream, reader, 0, (int)header.HeaderLength - 16);
        string headerMd5 = IIPSArchiveCrypto.Md5(headerBytes);
        if (!string.Equals(headerMd5, header.HeaderMd5, StringComparison.OrdinalIgnoreCase))
        {
            logger.Info("Header MD5 mismatch");
        }

        if (header.BetOffset + header.BetLength <= (ulong)stream.Length)
        {
            string betMd5 = IIPSArchiveCrypto.Md5(ReadBytesAt(stream, reader, (long)header.BetOffset, (int)header.BetLength));
            if (!string.Equals(betMd5, header.BetMd5, StringComparison.OrdinalIgnoreCase))
            {
                logger.Info("BET MD5 mismatch");
            }
        }
        else
        {
            logger.Info("BET overflow");
        }

        if (header.HetOffset + header.HetLength <= (ulong)stream.Length)
        {
            string hetMd5 = IIPSArchiveCrypto.Md5(ReadBytesAt(stream, reader, (long)header.HetOffset, (int)header.HetLength));
            if (!string.Equals(hetMd5, header.HetMd5, StringComparison.OrdinalIgnoreCase))
            {
                logger.Info("HET MD5 mismatch");
            }
        }
        else
        {
            logger.Info("HET overflow");
        }

        return header;
    }

    private static IIPSArchiveLookup ParseHetTable(byte[] hetData, Arrowgene.Logging.ILogger logger, out HetState state)
    {
        if (hetData.Length < 32)
        {
            throw new InvalidDataException("HET table is truncated.");
        }

        StreamBuffer het = new StreamBuffer(hetData);
        het.SetPositionStart();

        uint tableSize = het.ReadUInt32();
        uint entryCount = het.ReadUInt32();
        uint totalCount = het.ReadUInt32();
        uint hashBitSize = het.ReadUInt32();
        uint indexSizeTotal = het.ReadUInt32();
        uint indexSizeExtra = het.ReadUInt32();
        uint indexSize = het.ReadUInt32();
        uint indexTableSize = het.ReadUInt32();

        byte[] nameHashes = het.ReadBytes((int)totalCount);
        byte[] fileIndexData = het.ReadBytes((int)indexTableSize);

        logger.Info($"HET: entries={entryCount}, slots={totalCount}, hashBits={hashBitSize}, indexBits={indexSize}");

        state = new HetState
        {
            TotalCount = totalCount,
            IndexBits = indexSize,
            IndexStrideBits = indexSizeTotal,
            HashBits = hashBitSize,
            NameHashes = nameHashes,
            FileIndexData = fileIndexData,
        };

        return new IIPSArchiveLookup(totalCount, indexSize, indexSizeTotal, hashBitSize, nameHashes, fileIndexData);
    }

    private static List<IIPSArchiveEntryRecord> ParseBetEntries(byte[] betData, HetState hetState, Arrowgene.Logging.ILogger logger)
    {
        if (betData.Length < 84)
        {
            throw new InvalidDataException("BET table is truncated.");
        }

        StreamBuffer bet = new StreamBuffer(betData);
        bet.SetPositionStart();

        uint[] header = new uint[21];
        for (int i = 0; i < header.Length; i++)
        {
            header[i] = bet.ReadUInt32();
        }

        uint entryCount = header[1];
        uint tableEntrySize = header[2];
        uint bitIndexFilePos = header[3];
        uint bitIndexFileSize = header[4];
        uint bitIndexCompressedSize = header[5];
        uint bitIndexFlags = header[6];
        uint bitIndexMd5 = header[7];
        uint bitCountFilePos = header[9];
        uint bitCountFileSize = header[10];
        uint bitCountCompressedSize = header[11];
        uint bitCountFlags = header[12];
        uint bitCountMd5 = header[13];
        uint betHashStrideBits = header[15];
        uint betHashBits = header[17];
        uint bitCountExtra = header[20];
        uint bitIndexExtra = bitIndexMd5 + bitCountMd5;

        uint computedEntryBits = bitCountFilePos + bitCountFileSize + bitCountCompressedSize + bitCountFlags + bitCountMd5 + bitCountExtra;
        if (tableEntrySize > 0 && tableEntrySize != computedEntryBits)
        {
            logger.Info($"BET: field[2]={tableEntrySize} != computed={computedEntryBits}, using field[2]");
            computedEntryBits = tableEntrySize;
        }

        logger.Info($"BET header: entries={entryCount}, bitsPerEntry={computedEntryBits}, bcFilePos={bitCountFilePos}, bcFileSize={bitCountFileSize}, bcCmpSize={bitCountCompressedSize}, bcMd5={bitCountMd5}, bcExtra={bitCountExtra}, bcFlags={bitCountFlags}, betHashSize={betHashBits}/{betHashStrideBits}");

        byte[] allBitData = new byte[betData.Length - 84];
        Array.Copy(betData, 84, allBitData, 0, allBitData.Length);

        int entryDataBits = (int)((long)entryCount * computedEntryBits);
        int entryDataBytes = (entryDataBits + 7) / 8;
        byte[] hashData = new byte[Math.Max(0, allBitData.Length - entryDataBytes)];
        if (hashData.Length > 0)
        {
            Array.Copy(allBitData, entryDataBytes, hashData, 0, hashData.Length);
        }

        List<IIPSArchiveEntryRecord> records = new List<IIPSArchiveEntryRecord>((int)entryCount);
        for (uint i = 0; i < entryCount; i++)
        {
            long baseBitOffset = (long)i * computedEntryBits;
            ulong filePos = IIPSArchiveFormat.ReadBits(allBitData, baseBitOffset + bitIndexFilePos, (int)bitCountFilePos);
            ulong fileSize = IIPSArchiveFormat.ReadBits(allBitData, baseBitOffset + bitIndexFileSize, (int)bitCountFileSize);
            ulong compressedSize = IIPSArchiveFormat.ReadBits(allBitData, baseBitOffset + bitIndexCompressedSize, (int)bitCountCompressedSize);
            uint flags = (uint)IIPSArchiveFormat.ReadBits(allBitData, baseBitOffset + bitIndexFlags, (int)bitCountFlags);
            byte[]? md5 = bitCountMd5 == 0 ? null : IIPSArchiveFormat.ReadBitsAsBytes(allBitData, baseBitOffset + bitIndexMd5, (int)bitCountMd5);
            ulong extra = bitCountExtra == 0 ? 0 : IIPSArchiveFormat.ReadBits(allBitData, baseBitOffset + bitIndexExtra, (int)bitCountExtra);

            records.Add(new IIPSArchiveEntryRecord
            {
                Index = (int)i,
                FileOffset = filePos,
                FileSize = fileSize,
                CompressedSize = compressedSize,
                Flags = flags,
                Md5 = md5,
                Extra = extra,
                SourceKind = IIPSArchiveEntrySourceKind.ExistingArchive,
                WriteOptions = new IIPSArchiveEntryOptions
                {
                    StorageMode = (flags & (uint)IIPSArchiveEntryFlags.SingleUnit) != 0 ? IIPSArchiveStorageMode.SingleUnit : IIPSArchiveStorageMode.SectorBased,
                    Compress = (flags & (uint)IIPSArchiveEntryFlags.Compressed) != 0,
                    Encrypt = (flags & (uint)IIPSArchiveEntryFlags.Encrypted) != 0,
                    UseFixedKey = (flags & (uint)IIPSArchiveEntryFlags.FixKey) != 0,
                },
            });
        }

        ReconstructNameHashes(records, hetState, hashData, betHashStrideBits, betHashBits);
        return records;
    }

    private static void ReconstructNameHashes(
        List<IIPSArchiveEntryRecord> records,
        HetState hetState,
        byte[] betHashData,
        uint betHashStrideBits,
        uint betHashBits)
    {
        int shiftAmount = (int)hetState.HashBits - 8;
        for (uint slot = 0; slot < hetState.TotalCount; slot++)
        {
            byte hetByte = hetState.NameHashes[slot];
            if (hetByte == 0)
            {
                continue;
            }

            int entryIndex = (int)IIPSArchiveFormat.ReadBits(hetState.FileIndexData, (long)slot * hetState.IndexStrideBits, (int)hetState.IndexBits);
            if (entryIndex < 0 || entryIndex >= records.Count)
            {
                continue;
            }

            ulong hashBits = IIPSArchiveFormat.ReadBits(betHashData, (long)entryIndex * betHashStrideBits, (int)betHashBits);
            ulong fullHash = hashBits + ((ulong)hetByte << shiftAmount);
            records[entryIndex].NameHash = fullHash;
            records[entryIndex].HetIndex = (int)slot;
        }
    }

    private static void TryLoadEmbeddedListFile(IIPSArchive archive)
    {
        if (!archive.TryGetEntry("(listfile)", out IIPSArchiveEntry? listFile) || listFile == null)
        {
            archive.ArchiveLogger.Info("No embedded archive path list found. Use LoadArchivePaths() to supply names externally.");
            return;
        }

        try
        {
            string content = Encoding.UTF8.GetString(archive.Extract(listFile));
            archive.LoadArchivePaths(content);
        }
        catch (Exception ex)
        {
            archive.ArchiveLogger.Error($"Failed to parse (listfile): {ex.Message}");
        }
    }

    private static byte[] ReadBytesAt(FileStream stream, BinaryReader reader, long offset, int count)
    {
        stream.Position = offset;
        return reader.ReadBytes(count);
    }

    private sealed class HetState
    {
        public uint TotalCount { get; init; }
        public uint IndexBits { get; init; }
        public uint IndexStrideBits { get; init; }
        public uint HashBits { get; init; }
        public byte[] NameHashes { get; init; } = Array.Empty<byte>();
        public byte[] FileIndexData { get; init; } = Array.Empty<byte>();
    }
}
