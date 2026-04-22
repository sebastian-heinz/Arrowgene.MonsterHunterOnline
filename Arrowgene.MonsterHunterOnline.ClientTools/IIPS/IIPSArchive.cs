#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.ClientTools.IIPS;

public sealed class IIPSArchive : IDisposable
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(IIPSArchive));

    private readonly List<IIPSArchiveEntryRecord> _records;
    private readonly List<IIPSArchiveEntry> _entries;
    private readonly Dictionary<string, IIPSArchiveEntry> _entriesByName;
    private FileStream? _sourceStream;
    private BinaryReader? _sourceReader;
    private IIPSArchiveLookup? _lookup;
    private bool _disposed;

    private IIPSArchive()
    {
        _records = [];
        _entries = [];
        _entriesByName = new Dictionary<string, IIPSArchiveEntry>(StringComparer.OrdinalIgnoreCase);
        Metadata = new IIPSArchiveMetadata();
    }

    public static IIPSArchive Open(string path, IIPSArchiveOpenOptions? options = null)
    {
        IIPSArchive archive = new IIPSArchive();
        IIPSArchiveReader.Load(archive, path, options ?? new IIPSArchiveOpenOptions());
        return archive;
    }

    public static IIPSArchive CreateNew(IIPSArchiveCreationOptions? options = null)
    {
        options ??= new IIPSArchiveCreationOptions();
        IIPSArchive archive = new IIPSArchive();
        archive.Metadata.FormatVersion = options.FormatVersion;
        archive.Metadata.SectorSizeShift = options.SectorSizeShift;
        archive.Metadata.Md5PieceSize = options.Md5PieceSize;
        archive.Metadata.RawChunkSize = options.RawChunkSize;
        return archive;
    }

    public string? SourcePath { get; private set; }
    public bool HasPendingChanges { get; private set; }
    public IIPSArchiveMetadata Metadata { get; }
    public IReadOnlyList<IIPSArchiveEntry> Entries => _entries;
    public IReadOnlyList<string> ArchivePaths => _entries.Where(entry => !string.IsNullOrEmpty(entry.ArchivePath)).Select(entry => entry.ArchivePath!).ToList();

    public bool TryGetEntry(string archivePath, out IIPSArchiveEntry? entry)
    {
        ThrowIfDisposed();
        string normalizedPath = NormalizeAndValidatePath(archivePath);

        if (_entriesByName.TryGetValue(normalizedPath, out entry))
        {
            return true;
        }

        if (_lookup == null)
        {
            entry = null;
            return false;
        }

        int index = _lookup.FindFileIndex(normalizedPath, _records);
        if (index < 0 || index >= _entries.Count)
        {
            entry = null;
            return false;
        }

        entry = _entries[index];
        CacheResolvedArchivePath(entry.Record, normalizedPath);
        return true;
    }

    public IIPSArchiveEntry GetEntry(string archivePath)
    {
        if (!TryGetEntry(archivePath, out IIPSArchiveEntry? entry) || entry == null)
        {
            throw new FileNotFoundException($"File not found in archive: {archivePath}");
        }

        return entry;
    }

    public byte[] Extract(string archivePath)
    {
        return Extract(GetEntry(archivePath));
    }

    public byte[] Extract(IIPSArchiveEntry entry)
    {
        ThrowIfDisposed();
        return ExtractRecord(entry.Record);
    }

    public void ExtractAll(string outputDirectory)
    {
        ThrowIfDisposed();

        Directory.CreateDirectory(outputDirectory);
        string? unnamedDirectory = null;

        foreach (IIPSArchiveEntry entry in _entries)
        {
            if (!entry.Exists || entry.Length == 0)
            {
                continue;
            }

            byte[] data = Extract(entry);
            string outputPath;
            if (!string.IsNullOrEmpty(entry.ArchivePath))
            {
                outputPath = Path.Combine(outputDirectory, entry.ArchivePath!.Replace('\\', Path.DirectorySeparatorChar));
                string? directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            else
            {
                Logger.Error($"Unnamed archive entry {entry.Index} extracted as fallback bin ({entry.Record.NameHash:X16})");
                unnamedDirectory ??= Path.Combine(outputDirectory, "_unnamed");
                Directory.CreateDirectory(unnamedDirectory);
                outputPath = Path.Combine(unnamedDirectory, $"{entry.Index:D6}_{entry.Record.NameHash:X16}.bin");
            }

            File.WriteAllBytes(outputPath, data);
        }
    }

    public void LoadArchivePaths(string archivePathList)
    {
        ThrowIfDisposed();
        foreach (string archivePath in IIPSArchiveFormat.ParseFileNames(archivePathList))
        {
            if (_entriesByName.ContainsKey(archivePath))
            {
                continue;
            }

            if (_lookup == null)
            {
                continue;
            }

            int index = _lookup.FindFileIndex(archivePath, _records);
            if (index < 0 || index >= _records.Count)
            {
                continue;
            }

            CacheResolvedArchivePath(_records[index], archivePath);
        }
    }

    public void LoadArchivePathsFromFile(string path)
    {
        LoadArchivePaths(File.ReadAllText(path));
    }

    public IIPSArchiveEntry Add(string archivePath, byte[] content, IIPSArchiveEntryOptions? options = null)
    {
        ThrowIfDisposed();
        string normalizedPath = NormalizeAndValidatePath(archivePath);
        if (_entriesByName.ContainsKey(normalizedPath))
        {
            throw new InvalidOperationException($"Entry already exists: {archivePath}");
        }

        IIPSArchiveEntryRecord record = CreateMemoryRecord(normalizedPath, content, options);
        AddRecord(record);
        _lookup = null;
        HasPendingChanges = true;
        return _entries[record.Index];
    }

    public IIPSArchiveEntry MarkDeleted(string archivePath)
    {
        ThrowIfDisposed();
        string normalizedPath = NormalizeAndValidatePath(archivePath);
        if (_entriesByName.TryGetValue(normalizedPath, out IIPSArchiveEntry? existing) && existing != null)
        {
            Remove(existing);
        }

        IIPSArchiveEntryOptions options = new IIPSArchiveEntryOptions
        {
            StorageMode = IIPSArchiveStorageMode.SingleUnit,
            Compress = false,
            Encrypt = false,
            UseFixedKey = false,
        };

        IIPSArchiveEntryRecord record = CreateMemoryRecord(normalizedPath, Array.Empty<byte>(), options);
        record.Flags = (uint)IIPSArchiveEntryFlags.Exists
                       | (uint)IIPSArchiveEntryFlags.SingleUnit
                       | (uint)IIPSArchiveEntryFlags.DeleteMarker;
        AddRecord(record);
        _lookup = null;
        HasPendingChanges = true;
        return _entries[record.Index];
    }

    public IIPSArchiveEntry AddFile(string sourcePath, string? archivePath = null, IIPSArchiveEntryOptions? options = null)
    {
        string resolvedArchivePath = archivePath ?? Path.GetFileName(sourcePath);
        return Add(resolvedArchivePath, File.ReadAllBytes(sourcePath), options);
    }

    public IReadOnlyList<IIPSArchiveEntry> AddDirectory(string sourceDirectory, string? archiveRoot = null, IIPSArchiveEntryOptions? options = null)
    {
        ThrowIfDisposed();

        if (!Directory.Exists(sourceDirectory))
        {
            throw new DirectoryNotFoundException($"Directory does not exist: {sourceDirectory}");
        }

        string rootPath = Path.GetFullPath(sourceDirectory);
        List<IIPSArchiveEntry> addedEntries = new List<IIPSArchiveEntry>();
        foreach (string file in Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories))
        {
            string relativePath = Path.GetRelativePath(rootPath, file).Replace('/', '\\');
            string archivePath = string.IsNullOrEmpty(archiveRoot)
                ? relativePath
                : Path.Combine(archiveRoot, relativePath).Replace('/', '\\');
            addedEntries.Add(Add(archivePath, File.ReadAllBytes(file), options));
        }

        return addedEntries;
    }

    public IIPSArchiveEntry Replace(string archivePath, byte[] content, IIPSArchiveEntryOptions? options = null)
    {
        ThrowIfDisposed();
        IIPSArchiveEntry entry = GetEntry(archivePath);
        return Replace(entry, content, options);
    }

    public IIPSArchiveEntry Replace(IIPSArchiveEntry entry, byte[] content, IIPSArchiveEntryOptions? options = null)
    {
        ThrowIfDisposed();
        if (!_records.Contains(entry.Record))
        {
            throw new InvalidOperationException("The provided entry does not belong to this archive.");
        }

        entry.Record.Content = (byte[])content.Clone();
        entry.Record.SourceKind = IIPSArchiveEntrySourceKind.Memory;
        entry.Record.WriteOptions = CloneOptions(options ?? DeriveOptions(entry.Record));
        entry.Record.FileSize = (ulong)content.Length;
        entry.Record.CompressedSize = (ulong)content.Length;
        entry.Record.Flags = BuildBaseFlags(entry.Record.WriteOptions);
        entry.Record.Md5 = MD5.HashData(content);
        _lookup = null;
        HasPendingChanges = true;
        return entry;
    }

    public IIPSArchiveEntry Modify(string archivePath, byte[] content, IIPSArchiveEntryOptions? options = null)
    {
        return Replace(archivePath, content, options);
    }

    public IIPSArchiveEntry Modify(IIPSArchiveEntry entry, byte[] content, IIPSArchiveEntryOptions? options = null)
    {
        return Replace(entry, content, options);
    }

    public IIPSArchiveEntry ReplaceWithFile(string archivePath, string sourcePath, IIPSArchiveEntryOptions? options = null)
    {
        return Replace(archivePath, File.ReadAllBytes(sourcePath), options);
    }

    public bool Remove(string archivePath)
    {
        ThrowIfDisposed();
        if (!TryGetEntry(archivePath, out IIPSArchiveEntry? entry) || entry == null)
        {
            return false;
        }

        return Remove(entry);
    }

    public bool Remove(IIPSArchiveEntry entry)
    {
        ThrowIfDisposed();
        int recordIndex = _records.IndexOf(entry.Record);
        int entryIndex = _entries.IndexOf(entry);
        if (recordIndex < 0 || entryIndex < 0)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(entry.ArchivePath))
        {
            _entriesByName.Remove(entry.ArchivePath!);
        }

        _records.RemoveAt(recordIndex);
        _entries.RemoveAt(entryIndex);
        Reindex();
        _lookup = null;
        HasPendingChanges = true;
        return true;
    }

    public void Save(string path, IIPSArchiveSaveOptions? options = null)
    {
        ThrowIfDisposed();
        IIPSArchiveWriter.Save(this, path, options ?? new IIPSArchiveSaveOptions());
        HasPendingChanges = false;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _sourceReader?.Dispose();
        _sourceStream?.Dispose();
        _disposed = true;
    }

    internal void ReplaceState(
        string? sourcePath,
        FileStream? sourceStream,
        BinaryReader? sourceReader,
        IIPSArchiveMetadata metadata,
        List<IIPSArchiveEntryRecord> records,
        IIPSArchiveLookup? lookup)
    {
        _sourceReader?.Dispose();
        _sourceStream?.Dispose();

        SourcePath = sourcePath;
        _sourceStream = sourceStream;
        _sourceReader = sourceReader;
        _lookup = lookup;

        Metadata.FormatVersion = metadata.FormatVersion;
        Metadata.SectorSizeShift = metadata.SectorSizeShift;
        Metadata.Md5PieceSize = metadata.Md5PieceSize;
        Metadata.RawChunkSize = metadata.RawChunkSize;
        Metadata.HeaderMd5 = metadata.HeaderMd5;
        Metadata.BetMd5 = metadata.BetMd5;
        Metadata.HetMd5 = metadata.HetMd5;

        _records.Clear();
        _entries.Clear();
        _entriesByName.Clear();

        foreach (IIPSArchiveEntryRecord record in records)
        {
            _records.Add(record);
            IIPSArchiveEntry entry = new IIPSArchiveEntry(this, record);
            _entries.Add(entry);
            if (!string.IsNullOrEmpty(record.FileName))
            {
                _entriesByName[record.FileName!] = entry;
            }
        }

        Reindex();
    }

    internal byte[] ReadStoredBytes(IIPSArchiveEntryRecord record)
    {
        if (_sourceStream == null || _sourceReader == null)
        {
            throw new InvalidOperationException("Archive is not backed by a readable source stream.");
        }

        ulong storedLength = IIPSArchiveFormat.GetPhysicalStoredLength(record, Metadata.SectorSize);
        if (storedLength == 0)
        {
            return Array.Empty<byte>();
        }

        byte[] buffer = new byte[(int)storedLength];
        _sourceStream.Position = (long)record.FileOffset;
        int read = _sourceReader.Read(buffer, 0, buffer.Length);
        if (read != buffer.Length)
        {
            throw new EndOfStreamException($"Failed to read stored data for entry {record.Index}");
        }

        return buffer;
    }

    internal byte[] ExtractRecord(IIPSArchiveEntryRecord record)
    {
        if (record.SourceKind == IIPSArchiveEntrySourceKind.Memory)
        {
            return record.Content == null ? Array.Empty<byte>() : (byte[])record.Content.Clone();
        }

        if (_sourceStream == null || _sourceReader == null || record.FileSize == 0)
        {
            return Array.Empty<byte>();
        }

        if ((long)record.FileOffset >= _sourceStream.Length)
        {
            Logger.Error($"File offset 0x{record.FileOffset:X} out of range for entry {record.Index}");
            return Array.Empty<byte>();
        }

        bool encrypted = (record.Flags & (uint)IIPSArchiveEntryFlags.Encrypted) != 0;
        bool compressed = (record.Flags & (uint)IIPSArchiveEntryFlags.Compressed) != 0;
        bool singleUnit = (record.Flags & (uint)IIPSArchiveEntryFlags.SingleUnit) != 0;
        uint fileKey = encrypted ? ComputeFileKey(record) : 0;
        uint sectorSize = Metadata.SectorSize;

        if (singleUnit || record.CompressedSize == 0 || record.CompressedSize == record.FileSize)
        {
            ulong readSize = record.CompressedSize > 0 && record.CompressedSize < record.FileSize
                ? record.CompressedSize
                : record.FileSize;
            byte[] data = ReadBytesAt((long)record.FileOffset, (int)readSize);
            if (encrypted)
            {
                IIPSArchiveCrypto.MpqDecryptBlock(data, fileKey);
            }

            if (compressed && record.CompressedSize > 0 && record.CompressedSize < record.FileSize)
            {
                return IIPSArchiveFormat.Decompress(data, (int)record.FileSize, Logger);
            }

            return data;
        }

        int numSectors = ((int)record.FileSize + (int)sectorSize - 1) / (int)sectorSize;
        int offsetTableEntries = numSectors + 1;
        if ((record.Flags & (uint)IIPSArchiveEntryFlags.HasSectorCrc) != 0)
        {
            offsetTableEntries += numSectors;
        }

        byte[] offsetTableBytes = ReadBytesAt((long)record.FileOffset, offsetTableEntries * 4);
        if (encrypted)
        {
            IIPSArchiveCrypto.MpqDecryptBlock(offsetTableBytes, fileKey - 1);
        }

        uint[] sectorOffsets = new uint[offsetTableEntries];
        for (int i = 0; i < offsetTableEntries; i++)
        {
            sectorOffsets[i] = BitConverter.ToUInt32(offsetTableBytes, i * 4);
        }

        using MemoryStream result = new MemoryStream();
        for (int sectorIndex = 0; sectorIndex < numSectors; sectorIndex++)
        {
            uint sectorStart = sectorOffsets[sectorIndex];
            uint sectorEnd = sectorOffsets[sectorIndex + 1];
            int sectorCompressedSize = (int)(sectorEnd - sectorStart);
            int sectorRawSize = (int)Math.Min(sectorSize, record.FileSize - (ulong)sectorIndex * sectorSize);
            if (sectorCompressedSize <= 0 || sectorCompressedSize > _sourceStream.Length)
            {
                throw new InvalidDataException($"Bad sector {sectorIndex}: offset={sectorStart}-{sectorEnd}");
            }

            byte[] sectorData = ReadBytesAt((long)record.FileOffset + sectorStart, sectorCompressedSize);
            if (encrypted)
            {
                IIPSArchiveCrypto.MpqDecryptBlock(sectorData, fileKey + (uint)sectorIndex);
            }

            if (compressed && sectorCompressedSize < sectorRawSize)
            {
                byte[] decompressed = IIPSArchiveFormat.Decompress(sectorData, sectorRawSize, Logger);
                result.Write(decompressed, 0, decompressed.Length);
            }
            else
            {
                result.Write(sectorData, 0, sectorData.Length);
            }
        }

        return result.ToArray();
    }

    internal uint ComputeFileKey(IIPSArchiveEntryRecord record)
    {
        if (string.IsNullOrEmpty(record.FileName))
        {
            throw new InvalidOperationException($"Cannot compute encryption key for unnamed entry {record.Index}");
        }

        uint key = IIPSArchiveCrypto.ComputeFileKey(record.FileName);
        if (record.UsesFixedKey)
        {
            key = (key + (uint)record.FileOffset) ^ (uint)record.FileSize;
        }

        return key;
    }

    internal IReadOnlyList<IIPSArchiveEntryRecord> Records => _records;
    internal ILogger ArchiveLogger => Logger;
    internal string? CurrentSourcePath => SourcePath;

    internal void ReleaseSourceHandles()
    {
        _sourceReader?.Dispose();
        _sourceStream?.Dispose();
        _sourceReader = null;
        _sourceStream = null;
        SourcePath = null;
    }

    private IIPSArchiveEntryRecord CreateMemoryRecord(string archivePath, byte[] content, IIPSArchiveEntryOptions? options)
    {
        options = CloneOptions(options ?? new IIPSArchiveEntryOptions());
        return new IIPSArchiveEntryRecord
        {
            FileName = archivePath.Replace('/', '\\'),
            NameHash = IIPSArchiveFormat.MaskNameHash(IIPSArchiveCrypto.ComputeNameHash(archivePath)),
            Content = (byte[])content.Clone(),
            FileSize = (ulong)content.Length,
            CompressedSize = (ulong)content.Length,
            Md5 = MD5.HashData(content),
            Flags = BuildBaseFlags(options),
            SourceKind = IIPSArchiveEntrySourceKind.Memory,
            WriteOptions = options,
        };
    }

    private void AddRecord(IIPSArchiveEntryRecord record)
    {
        record.Index = _records.Count;
        _records.Add(record);
        IIPSArchiveEntry entry = new IIPSArchiveEntry(this, record);
        _entries.Add(entry);
        if (!string.IsNullOrEmpty(record.FileName))
        {
            _entriesByName[record.FileName!] = entry;
        }
    }

    private byte[] ReadBytesAt(long offset, int count)
    {
        if (_sourceStream == null || _sourceReader == null)
        {
            return Array.Empty<byte>();
        }

        _sourceStream.Position = offset;
        return _sourceReader.ReadBytes(count);
    }

    private void CacheResolvedArchivePath(IIPSArchiveEntryRecord record, string archivePath)
    {
        string normalizedPath = NormalizeAndValidatePath(archivePath);
        record.FileName = normalizedPath;
        if (record.Index < _entries.Count)
        {
            _entriesByName[normalizedPath] = _entries[record.Index];
        }
    }

    private void Reindex()
    {
        for (int i = 0; i < _records.Count; i++)
        {
            _records[i].Index = i;
        }
    }

    private static string NormalizeAndValidatePath(string archivePath)
    {
        if (string.IsNullOrWhiteSpace(archivePath))
        {
            throw new ArgumentException("Archive path must not be empty.", nameof(archivePath));
        }

        return archivePath.Replace('/', '\\');
    }

    private static IIPSArchiveEntryOptions DeriveOptions(IIPSArchiveEntryRecord record)
    {
        return new IIPSArchiveEntryOptions
        {
            StorageMode = record.IsSingleUnit ? IIPSArchiveStorageMode.SingleUnit : IIPSArchiveStorageMode.SectorBased,
            Compress = (record.Flags & (uint)IIPSArchiveEntryFlags.Compressed) != 0,
            Encrypt = record.IsEncrypted,
            UseFixedKey = record.UsesFixedKey,
        };
    }

    private static IIPSArchiveEntryOptions CloneOptions(IIPSArchiveEntryOptions options)
    {
        return new IIPSArchiveEntryOptions
        {
            StorageMode = options.StorageMode,
            Compress = options.Compress,
            Encrypt = options.Encrypt,
            UseFixedKey = options.UseFixedKey,
        };
    }

    private static uint BuildBaseFlags(IIPSArchiveEntryOptions options)
    {
        uint flags = (uint)IIPSArchiveEntryFlags.Exists;
        if (options.StorageMode == IIPSArchiveStorageMode.SingleUnit)
        {
            flags |= (uint)IIPSArchiveEntryFlags.SingleUnit;
        }

        if (options.Compress)
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

        return flags;
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(IIPSArchive));
        }
    }
}
