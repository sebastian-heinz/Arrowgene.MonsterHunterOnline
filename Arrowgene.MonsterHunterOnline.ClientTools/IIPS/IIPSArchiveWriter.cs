#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Arrowgene.MonsterHunterOnline.ClientTools.IIPS;

internal static class IIPSArchiveWriter
{
    public static void Save(IIPSArchive archive, string path, IIPSArchiveSaveOptions options)
    {
        string targetPath = Path.GetFullPath(path);
        string? targetDirectory = Path.GetDirectoryName(targetPath);
        if (string.IsNullOrEmpty(targetDirectory))
        {
            throw new InvalidOperationException($"Could not determine output directory for path '{path}'.");
        }

        Directory.CreateDirectory(targetDirectory);
        string tempPath = Path.Combine(targetDirectory, Path.GetRandomFileName());
        List<IIPSArchiveEntryRecord> records = PrepareRecords(archive, options);

        try
        {
            using (FileStream output = new FileStream(tempPath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                output.Position = IIPSArchiveFormat.HeaderLength;
                foreach (IIPSArchiveEntryRecord record in records)
                {
                    ulong sourceFileOffset = record.FileOffset;
                    bool isDirectory = (record.Flags & (uint)IIPSArchiveEntryFlags.Directory) != 0;
                    if (isDirectory || record.FileSize == 0)
                    {
                        record.FileOffset = 0;
                    }
                    else
                    {
                        record.FileOffset = (ulong)output.Position;
                    }
                    ulong preservedCompressedSize = record.CompressedSize;
                    bool preserveCompressedSize = CanPreserveStoredBytes(record, options);
                    byte[] storedData = BuildStoredData(archive, record, options, sourceFileOffset);
                    output.Write(storedData, 0, storedData.Length);
                    if (preserveCompressedSize)
                    {
                        record.CompressedSize = preservedCompressedSize;
                    }
                    else
                    {
                        record.CompressedSize = (ulong)storedData.Length;
                    }
                }

                byte[] hetSection = IIPSArchiveSerialization.BuildSection(IIPSArchiveFormat.HetSignature, BuildHetData(records));
                ulong hetOffset = (ulong)output.Position;
                output.Write(hetSection, 0, hetSection.Length);
                byte[] hetMd5 = System.Security.Cryptography.MD5.HashData(hetSection);
                output.Write(hetMd5, 0, hetMd5.Length);

                byte[] betSection = IIPSArchiveSerialization.BuildSection(IIPSArchiveFormat.BetSignature, BuildBetData(records));
                ulong betOffset = (ulong)output.Position;
                output.Write(betSection, 0, betSection.Length);
                byte[] betMd5 = System.Security.Cryptography.MD5.HashData(betSection);
                output.Write(betMd5, 0, betMd5.Length);

                uint md5PieceSize = archive.Metadata.Md5PieceSize == 0 ? 0x00004000u : archive.Metadata.Md5PieceSize;
                uint rawChunkSize = archive.Metadata.RawChunkSize == 0 ? 0x00004000u : archive.Metadata.RawChunkSize;
                uint sectorSize = IIPSArchiveFormat.GetSectorSize(archive.Metadata.SectorSizeShift);
                ulong dataEnd = (ulong)output.Position;
                ulong archiveSize = ((dataEnd + sectorSize - 1) / sectorSize) * sectorSize;

                if (archiveSize > dataEnd)
                {
                    byte[] padding = new byte[archiveSize - dataEnd];
                    output.Write(padding, 0, padding.Length);
                }

                uint pieceCount = (uint)((archiveSize + md5PieceSize - 1) / md5PieceSize);
                uint chunkCount = (uint)((archiveSize + rawChunkSize - 1) / rawChunkSize);
                int md5TableLength = ((int)pieceCount + 1) * 16;
                int bitmapLength = (int)chunkCount;

                ulong md5TableOffset = archiveSize;
                ulong bitmapOffset = md5TableOffset + (ulong)md5TableLength;

                IIPSArchiveHeaderData header = new IIPSArchiveHeaderData
                {
                    Magic = IIPSArchiveFormat.Magic,
                    HeaderLength = IIPSArchiveFormat.HeaderLength,
                    FormatVersion = archive.Metadata.FormatVersion,
                    SectorSizeShift = archive.Metadata.SectorSizeShift,
                    ArchiveSize = archiveSize,
                    BetOffset = betOffset,
                    HetOffset = hetOffset,
                    Md5TableOffset = md5TableOffset,
                    BitmapOffset = bitmapOffset,
                    HetLength = (ulong)hetSection.Length,
                    BetLength = (ulong)betSection.Length,
                    Md5TableLength = (ulong)md5TableLength,
                    BitmapLength = (ulong)bitmapLength,
                    Md5PieceSize = md5PieceSize,
                    RawChunkSize = rawChunkSize,
                    Md5PatchBaseTag = new byte[16],
                    Md5PatchedTag = new byte[16],
                    BetMd5 = IIPSArchiveCrypto.Md5(betSection),
                    HetMd5 = IIPSArchiveCrypto.Md5(hetSection),
                };

                byte[] headerBytes = IIPSArchiveSerialization.BuildHeader(header);
                output.Position = 0;
                output.Write(headerBytes, 0, headerBytes.Length);

                byte[] md5Table = BuildMd5Table(output, archiveSize, md5PieceSize);
                output.Position = (long)md5TableOffset;
                output.Write(md5Table, 0, md5Table.Length);

                byte[] bitmap = BuildBitmap(archiveSize, rawChunkSize);
                output.Position = (long)bitmapOffset;
                output.Write(bitmap, 0, bitmap.Length);
            }

            bool overwriteCurrentSource = archive.CurrentSourcePath != null &&
                                          string.Equals(Path.GetFullPath(archive.CurrentSourcePath), targetPath, StringComparison.OrdinalIgnoreCase);
            if (overwriteCurrentSource)
            {
                archive.ReleaseSourceHandles();
            }

            File.Move(tempPath, targetPath, overwrite: true);
            IIPSArchiveReader.Load(archive, targetPath, new IIPSArchiveOpenOptions());
        }
        finally
        {
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }
        }
    }

    private static List<IIPSArchiveEntryRecord> PrepareRecords(IIPSArchive archive, IIPSArchiveSaveOptions options)
    {
        List<IIPSArchiveEntryRecord> records = archive.Records.Select(CloneRecord).ToList();
        if (!options.IncludeListFile)
        {
            Reindex(records);
            return records;
        }

        List<string> fileNames = records
            .Where(record => !string.IsNullOrEmpty(record.FileName) && !string.Equals(record.FileName, "(listfile)", StringComparison.OrdinalIgnoreCase))
            .Select(record => record.FileName!)
            .ToList();

        List<string> listFileEntries = new List<string>(fileNames.Count + 1) { "(listfile)" };
        listFileEntries.AddRange(fileNames);
        byte[] listFileContent = Encoding.UTF8.GetBytes(string.Join('\n', listFileEntries) + '\n');

        IIPSArchiveEntryRecord? listFileRecord = records.FirstOrDefault(record => string.Equals(record.FileName, "(listfile)", StringComparison.OrdinalIgnoreCase));
        if (listFileRecord == null)
        {
            listFileRecord = new IIPSArchiveEntryRecord
            {
                FileName = "(listfile)",
                SourceKind = IIPSArchiveEntrySourceKind.Memory,
                WriteOptions = new IIPSArchiveEntryOptions(),
            };
            records.Add(listFileRecord);
        }

        listFileRecord.FileName = "(listfile)";
        listFileRecord.NameHash = IIPSArchiveFormat.MaskNameHash(IIPSArchiveCrypto.ComputeNameHash("(listfile)"));
        listFileRecord.SourceKind = IIPSArchiveEntrySourceKind.Memory;
        listFileRecord.Content = listFileContent;
        listFileRecord.FileSize = (ulong)listFileContent.Length;
        listFileRecord.CompressedSize = (ulong)listFileContent.Length;
        listFileRecord.Md5 = MD5.HashData(listFileContent);
        listFileRecord.Extra = 0;
        listFileRecord.WriteOptions = new IIPSArchiveEntryOptions();
        listFileRecord.Flags = (uint)IIPSArchiveEntryFlags.Exists | (uint)IIPSArchiveEntryFlags.SingleUnit;

        Reindex(records);
        return records;
    }

    private static byte[] BuildStoredData(IIPSArchive archive, IIPSArchiveEntryRecord record, IIPSArchiveSaveOptions options, ulong sourceFileOffset)
    {
        if (CanPreserveStoredBytes(record, options))
        {
            record.NameHash = EnsureNameHash(record);
            ulong targetFileOffset = record.FileOffset;
            record.FileOffset = sourceFileOffset;
            try
            {
                record.Md5 ??= record.FileSize == 0
                    ? MD5.HashData(Array.Empty<byte>())
                    : MD5.HashData(archive.ExtractRecord(record));
                return archive.ReadStoredBytes(record);
            }
            finally
            {
                record.FileOffset = targetFileOffset;
            }
        }

        byte[] content;
        if (record.SourceKind == IIPSArchiveEntrySourceKind.Memory)
        {
            content = record.Content == null ? Array.Empty<byte>() : (byte[])record.Content.Clone();
        }
        else
        {
            ulong targetFileOffset = record.FileOffset;
            record.FileOffset = sourceFileOffset;
            try
            {
                content = archive.ExtractRecord(record);
            }
            finally
            {
                record.FileOffset = targetFileOffset;
            }
        }

        record.NameHash = EnsureNameHash(record);
        record.FileSize = (ulong)content.Length;
        record.Md5 = MD5.HashData(content);
        record.Extra = 0;

        if (content.Length == 0)
        {
            uint preserved = record.Flags & (uint)IIPSArchiveEntryFlags.DeleteMarker;
            record.Flags = (uint)IIPSArchiveEntryFlags.Exists | (uint)IIPSArchiveEntryFlags.SingleUnit | preserved;
            record.CompressedSize = 0;
            return Array.Empty<byte>();
        }

        return record.WriteOptions.StorageMode == IIPSArchiveStorageMode.SectorBased
            ? EncodeSectorBased(record, content, archive.Metadata.SectorSize)
            : EncodeSingleUnit(record, content);
    }

    private static byte[] EncodeSingleUnit(IIPSArchiveEntryRecord record, byte[] content)
    {
        IIPSArchiveEntryOptions options = record.WriteOptions;
        byte[] stored = content;
        bool compressed = false;
        if (options.Compress)
        {
            byte[] compressedCandidate = IIPSArchiveFormat.Compress(content);
            if (compressedCandidate.Length < content.Length)
            {
                stored = compressedCandidate;
                compressed = true;
            }
        }

        if (options.Encrypt)
        {
            stored = (byte[])stored.Clone();
            IIPSArchiveCrypto.MpqEncryptBlock(stored, ComputeFileKey(record.FileName!, options.UseFixedKey, record.FileOffset, (ulong)content.Length));
        }

        uint flags = (uint)IIPSArchiveEntryFlags.Exists | (uint)IIPSArchiveEntryFlags.SingleUnit;
        if (compressed)
        {
            flags |= (uint)IIPSArchiveEntryFlags.Compressed;
        }

        if (options.Encrypt)
        {
            flags |= (uint)IIPSArchiveEntryFlags.Encrypted;
        }

        if (options.UseFixedKey)
        {
            flags |= (uint)IIPSArchiveEntryFlags.FixKey;
        }

        record.Flags = flags;
        record.CompressedSize = (ulong)stored.Length;
        return stored;
    }

    private static byte[] EncodeSectorBased(IIPSArchiveEntryRecord record, byte[] content, uint sectorSize)
    {
        IIPSArchiveEntryOptions options = record.WriteOptions;
        int sectorCount = (content.Length + (int)sectorSize - 1) / (int)sectorSize;
        if (sectorCount == 0)
        {
            return EncodeSingleUnit(record, content);
        }

        uint fileKey = options.Encrypt
            ? ComputeFileKey(record.FileName!, options.UseFixedKey, record.FileOffset, (ulong)content.Length)
            : 0;

        List<byte[]> sectors = new List<byte[]>(sectorCount);
        uint[] offsets = new uint[sectorCount + 1];
        uint currentOffset = (uint)(offsets.Length * sizeof(uint));
        bool anyCompressed = false;

        for (int sectorIndex = 0; sectorIndex < sectorCount; sectorIndex++)
        {
            int start = sectorIndex * (int)sectorSize;
            int rawLength = Math.Min((int)sectorSize, content.Length - start);
            byte[] rawSector = new byte[rawLength];
            Array.Copy(content, start, rawSector, 0, rawLength);

            byte[] storedSector = rawSector;
            if (options.Compress)
            {
                byte[] compressedSector = IIPSArchiveFormat.Compress(rawSector);
                if (compressedSector.Length < rawSector.Length)
                {
                    storedSector = compressedSector;
                    anyCompressed = true;
                }
            }

            if (options.Encrypt && storedSector.Length > 0)
            {
                storedSector = (byte[])storedSector.Clone();
                IIPSArchiveCrypto.MpqEncryptBlock(storedSector, fileKey + (uint)sectorIndex);
            }

            offsets[sectorIndex] = currentOffset;
            currentOffset += (uint)storedSector.Length;
            sectors.Add(storedSector);
        }

        offsets[sectorCount] = currentOffset;

        byte[] offsetTable = new byte[offsets.Length * sizeof(uint)];
        for (int i = 0; i < offsets.Length; i++)
        {
            BitConverter.GetBytes(offsets[i]).CopyTo(offsetTable, i * sizeof(uint));
        }

        if (options.Encrypt && offsetTable.Length > 0)
        {
            IIPSArchiveCrypto.MpqEncryptBlock(offsetTable, fileKey - 1);
        }

        using MemoryStream ms = new MemoryStream((int)currentOffset);
        ms.Write(offsetTable, 0, offsetTable.Length);
        foreach (byte[] sector in sectors)
        {
            ms.Write(sector, 0, sector.Length);
        }

        uint flags = (uint)IIPSArchiveEntryFlags.Exists;
        if (anyCompressed)
        {
            flags |= (uint)IIPSArchiveEntryFlags.Compressed;
        }

        if (options.Encrypt)
        {
            flags |= (uint)IIPSArchiveEntryFlags.Encrypted;
        }

        if (options.UseFixedKey)
        {
            flags |= (uint)IIPSArchiveEntryFlags.FixKey;
        }

        record.Flags = flags;
        record.CompressedSize = (ulong)ms.Length;
        return ms.ToArray();
    }

    private static byte[] BuildHetData(List<IIPSArchiveEntryRecord> records)
    {
        uint actualEntries = (uint)records.Count;
        uint capacity = actualEntries + 263;
        uint entryCount = capacity;
        uint slotCount = (capacity * 4) / 3;
        uint indexBits = (uint)IIPSArchiveFormat.BitsRequired(capacity == 0 ? 0 : capacity - 1);
        uint indexStrideBits = indexBits;
        int indexTableBytes = (int)((slotCount * indexStrideBits + 7) / 8);

        byte[] nameHashBytes = new byte[slotCount];
        byte[] fileIndexData = new byte[indexTableBytes];
        for (int i = 0; i < fileIndexData.Length; i++) fileIndexData[i] = 0xFF;

        foreach (IIPSArchiveEntryRecord record in records)
        {
            ulong nameHash = EnsureNameHash(record);
            byte hashByte = (byte)(nameHash >> 56);
            uint slot = slotCount == 0 ? 0 : (uint)(nameHash % slotCount);
            while (nameHashBytes[slot] != 0)
            {
                slot = (slot + 1) % slotCount;
            }

            nameHashBytes[slot] = hashByte;
            record.HetIndex = (int)slot;
            for (int b = 0; b < indexBits; b++)
            {
                long bitPos = (long)slot * indexStrideBits + b;
                int byteIdx = (int)(bitPos / 8);
                int bitIdx = (int)(bitPos % 8);
                if (((record.Index >> b) & 1) != 0)
                {
                    fileIndexData[byteIdx] |= (byte)(1 << bitIdx);
                }
                else
                {
                    fileIndexData[byteIdx] &= (byte)~(1 << bitIdx);
                }
            }
        }

        using MemoryStream ms = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
        writer.Write((uint)(32 + nameHashBytes.Length + fileIndexData.Length));
        writer.Write(entryCount);
        writer.Write(slotCount);
        writer.Write(IIPSArchiveFormat.HashBits);
        writer.Write(indexStrideBits);
        writer.Write(0u);
        writer.Write(indexBits);
        writer.Write((uint)fileIndexData.Length);
        writer.Write(nameHashBytes);
        writer.Write(fileIndexData);
        return ms.ToArray();
    }

    private static byte[] BuildBetData(List<IIPSArchiveEntryRecord> records)
    {
        ulong maxFileOffset = records.Count == 0 ? 0 : records.Max(record => record.FileOffset);
        ulong maxFileSize = records.Count == 0 ? 0 : records.Max(record => record.FileSize);
        ulong maxStoredSize = records.Count == 0 ? 0 : records.Max(record => record.CompressedSize);

        uint filePosBits = (uint)IIPSArchiveFormat.BitsRequired(maxFileOffset);
        uint fileSizeBits = (uint)IIPSArchiveFormat.BitsRequired(maxFileSize);
        uint compressedSizeBits = (uint)IIPSArchiveFormat.BitsRequired(maxStoredSize);
        const uint flagsBits = 32;
        const uint md5Bits = 128;
        const uint extraBits = 64;
        const uint betHashBits = IIPSArchiveFormat.BetHashBits;

        uint bitIndexFilePos = 0;
        uint bitIndexFileSize = bitIndexFilePos + filePosBits;
        uint bitIndexCompressedSize = bitIndexFileSize + fileSizeBits;
        uint bitIndexFlags = bitIndexCompressedSize + compressedSizeBits;
        uint bitIndexMd5 = bitIndexFlags + flagsBits;
        uint bitIndexExtra = bitIndexMd5 + md5Bits;
        uint totalEntryBits = bitIndexExtra + extraBits;

        byte[] entryData = new byte[(int)((records.Count * (long)totalEntryBits + 7) / 8)];
        byte[] hashData = new byte[(int)((records.Count * (long)betHashBits + 7) / 8)];

        for (int i = 0; i < records.Count; i++)
        {
            IIPSArchiveEntryRecord record = records[i];
            long entryBitOffset = (long)i * totalEntryBits;
            IIPSArchiveFormat.WriteBits(entryData, entryBitOffset + bitIndexFilePos, (int)filePosBits, record.FileOffset);
            IIPSArchiveFormat.WriteBits(entryData, entryBitOffset + bitIndexFileSize, (int)fileSizeBits, record.FileSize);
            IIPSArchiveFormat.WriteBits(entryData, entryBitOffset + bitIndexCompressedSize, (int)compressedSizeBits, record.CompressedSize);
            IIPSArchiveFormat.WriteBits(entryData, entryBitOffset + bitIndexFlags, (int)flagsBits, record.Flags);
            IIPSArchiveFormat.WriteBits(entryData, entryBitOffset + bitIndexMd5, record.Md5 ?? new byte[16]);
            IIPSArchiveFormat.WriteBits(entryData, entryBitOffset + bitIndexExtra, (int)extraBits, record.Extra);
            IIPSArchiveFormat.WriteBits(hashData, (long)i * betHashBits, (int)betHashBits, record.NameHash & 0x00FFFFFFFFFFFFFFUL);
        }

        using MemoryStream ms = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
        writer.Write((uint)(84 + entryData.Length + hashData.Length));
        writer.Write((uint)records.Count);
        writer.Write(totalEntryBits);
        writer.Write(bitIndexFilePos);
        writer.Write(bitIndexFileSize);
        writer.Write(bitIndexCompressedSize);
        writer.Write(bitIndexFlags);
        writer.Write(bitIndexMd5);
        writer.Write(bitIndexMd5);
        writer.Write(filePosBits);
        writer.Write(fileSizeBits);
        writer.Write(compressedSizeBits);
        writer.Write(flagsBits);
        writer.Write(md5Bits);
        writer.Write(0u);
        writer.Write(betHashBits);
        writer.Write(0u);
        writer.Write(betHashBits);
        writer.Write((uint)records.Count * 7);
        writer.Write(bitIndexExtra);
        writer.Write(extraBits);
        writer.Write(entryData);
        writer.Write(hashData);
        return ms.ToArray();
    }

    private static bool CanPreserveStoredBytes(IIPSArchiveEntryRecord record, IIPSArchiveSaveOptions options)
    {
        return options.PreserveUnchangedEntries &&
               record.SourceKind == IIPSArchiveEntrySourceKind.ExistingArchive &&
               !record.UsesFixedKey;
    }

    private static ulong EnsureNameHash(IIPSArchiveEntryRecord record)
    {
        if (record.NameHash != 0)
        {
            return record.NameHash;
        }

        if (string.IsNullOrEmpty(record.FileName))
        {
            throw new InvalidOperationException($"Entry {record.Index} has no name and no reconstructed hash.");
        }

        record.NameHash = IIPSArchiveFormat.MaskNameHash(IIPSArchiveCrypto.ComputeNameHash(record.FileName));
        return record.NameHash;
    }

    private static uint ComputeFileKey(string fileName, bool useFixedKey, ulong fileOffset, ulong fileSize)
    {
        uint key = IIPSArchiveCrypto.ComputeFileKey(fileName);
        if (useFixedKey)
        {
            key = (key + (uint)fileOffset) ^ (uint)fileSize;
        }

        return key;
    }

    private static IIPSArchiveEntryRecord CloneRecord(IIPSArchiveEntryRecord record)
    {
        return new IIPSArchiveEntryRecord
        {
            Index = record.Index,
            FileOffset = record.FileOffset,
            FileSize = record.FileSize,
            CompressedSize = record.CompressedSize,
            Flags = record.Flags,
            NameHash = record.NameHash,
            HetIndex = record.HetIndex,
            FileName = record.FileName,
            Md5 = record.Md5 == null ? null : (byte[])record.Md5.Clone(),
            Extra = record.Extra,
            SourceKind = record.SourceKind,
            Content = record.Content == null ? null : (byte[])record.Content.Clone(),
            WriteOptions = new IIPSArchiveEntryOptions
            {
                StorageMode = record.WriteOptions.StorageMode,
                Compress = record.WriteOptions.Compress,
                Encrypt = record.WriteOptions.Encrypt,
                UseFixedKey = record.WriteOptions.UseFixedKey,
            },
        };
    }

    private static void Reindex(List<IIPSArchiveEntryRecord> records)
    {
        for (int i = 0; i < records.Count; i++)
        {
            records[i].Index = i;
        }
    }

    private static uint NextSlotCount(uint entryCount)
    {
        uint capacity = Math.Max(entryCount, 128u);
        return (uint)Math.Ceiling(capacity / 0.75d);
    }

    private static byte[] BuildMd5Table(FileStream output, ulong archiveDataSize, uint md5PieceSize)
    {
        if (archiveDataSize == 0 || md5PieceSize == 0)
        {
            return Array.Empty<byte>();
        }

        uint pieceCount = (uint)((archiveDataSize + md5PieceSize - 1) / md5PieceSize);
        byte[] result = new byte[(pieceCount + 1) * 16];
        byte[] pieceBuffer = new byte[md5PieceSize];

        for (uint i = 0; i < pieceCount; i++)
        {
            ulong offset = (ulong)i * md5PieceSize;
            int length = (int)Math.Min(md5PieceSize, archiveDataSize - offset);
            output.Position = (long)offset;
            int total = 0;
            while (total < length)
            {
                int read = output.Read(pieceBuffer, total, length - total);
                if (read <= 0) break;
                total += read;
            }

            byte[] md5 = MD5.HashData(pieceBuffer.AsSpan(0, total));
            md5.CopyTo(result, (int)i * 16);
        }

        byte[] master = MD5.HashData(result.AsSpan(0, (int)pieceCount * 16));
        master.CopyTo(result, (int)pieceCount * 16);

        output.Position = output.Length;
        return result;
    }

    private static byte[] BuildBitmap(ulong archiveDataSize, uint rawChunkSize)
    {
        if (archiveDataSize == 0 || rawChunkSize == 0)
        {
            return Array.Empty<byte>();
        }

        uint chunkCount = (uint)((archiveDataSize + rawChunkSize - 1) / rawChunkSize);
        byte[] bitmap = new byte[chunkCount];
        for (uint i = 0; i < chunkCount; i++)
        {
            bitmap[i] = 0x01;
        }

        return bitmap;
    }
}
