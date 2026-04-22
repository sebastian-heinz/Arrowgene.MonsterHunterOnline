#nullable enable
using System;
using System.Collections.Generic;

namespace Arrowgene.MonsterHunterOnline.ClientTools.IIPS;

[Flags]
public enum IIPSArchiveEntryFlags : uint
{
    None = 0,
    Compressed = 0x00000200,
    Encrypted = 0x00010000,
    FixKey = 0x00020000,
    SingleUnit = 0x01000000,
    DeleteMarker = 0x02000000,
    HasSectorCrc = 0x04000000,
    Directory = 0x08000000,
    Exists = 0x80000000,
}

public enum IIPSArchiveStorageMode
{
    SingleUnit,
    SectorBased,
}

public sealed class IIPSArchiveMetadata
{
    internal IIPSArchiveMetadata()
    {
    }

    public ushort FormatVersion { get; internal set; }
    public ushort SectorSizeShift { get; internal set; }
    public uint SectorSize => IIPSArchiveFormat.GetSectorSize(SectorSizeShift);
    public uint Md5PieceSize { get; internal set; }
    public uint RawChunkSize { get; internal set; }
    public uint HetCapacity { get; internal set; }
    public string HeaderMd5 { get; internal set; } = string.Empty;
    public string BetMd5 { get; internal set; } = string.Empty;
    public string HetMd5 { get; internal set; } = string.Empty;
}

public sealed class IIPSArchiveOpenOptions
{
    public bool LoadListFile { get; set; } = true;
    public bool VerifyChecksums { get; set; } = true;
}

public sealed class IIPSArchiveCreationOptions
{
    public ushort FormatVersion { get; set; } = 0;
    public ushort SectorSizeShift { get; set; } = 5;
    public uint Md5PieceSize { get; set; } = 0x00004000;
    public uint RawChunkSize { get; set; } = 0x00004000;
}

public sealed class IIPSArchiveSaveOptions
{
    public bool IncludeListFile { get; set; } = true;
    public bool PreserveUnchangedEntries { get; set; } = true;
}

public sealed class IIPSArchiveEntryOptions
{
    public IIPSArchiveStorageMode StorageMode { get; set; } = IIPSArchiveStorageMode.SingleUnit;
    public bool Compress { get; set; }
    public bool Encrypt { get; set; }
    public bool UseFixedKey { get; set; }
}

public sealed class IIPSArchiveEntry
{
    private readonly IIPSArchive _archive;
    private readonly IIPSArchiveEntryRecord _record;

    internal IIPSArchiveEntry(IIPSArchive archive, IIPSArchiveEntryRecord record)
    {
        _archive = archive;
        _record = record;
    }

    internal IIPSArchiveEntryRecord Record => _record;
    public IIPSArchive Archive => _archive;

    public int Index => _record.Index;
    public string? ArchivePath => _record.FileName;
    public long Length => checked((long)_record.FileSize);
    public long StoredLength => checked((long)IIPSArchiveFormat.GetStoredLength(_record));
    public long FileOffset => checked((long)_record.FileOffset);
    public string Md5 => _record.Md5 == null ? string.Empty : Convert.ToHexString(_record.Md5).ToLowerInvariant();
    public IIPSArchiveEntryFlags Flags => (IIPSArchiveEntryFlags)_record.Flags;
    public IIPSArchiveStorageMode StorageMode => _record.IsSingleUnit ? IIPSArchiveStorageMode.SingleUnit : IIPSArchiveStorageMode.SectorBased;
    public bool Exists => _record.Exists;
    public bool IsCompressed => (Flags & IIPSArchiveEntryFlags.Compressed) != 0 && _record.CompressedSize != 0 && _record.CompressedSize != _record.FileSize;
    public bool IsEncrypted => _record.IsEncrypted;
    public bool IsSingleUnit => _record.IsSingleUnit;
    public bool IsDirectory => (Flags & IIPSArchiveEntryFlags.Directory) != 0;
    public bool IsDeleteMarker => (Flags & IIPSArchiveEntryFlags.DeleteMarker) != 0;
    public bool UsesFixedKey => _record.UsesFixedKey;

    public byte[] ReadAllBytes()
    {
        return _archive.Extract(this);
    }
}
