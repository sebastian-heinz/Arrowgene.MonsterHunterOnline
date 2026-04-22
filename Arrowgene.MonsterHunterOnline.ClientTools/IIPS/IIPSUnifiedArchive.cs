#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.ClientTools.IIPS;

public sealed class IIPSUnifiedArchive : IDisposable
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(IIPSUnifiedArchive));

    private readonly List<IIPSArchive> _archives = [];
    private readonly List<string> _loadedArchives = [];
    private readonly List<string> _missingArchives = [];
    private readonly List<IIPSArchiveEntry> _mergedEntries = [];
    private bool _disposed;

    public string? Version { get; private set; }
    public string? SourceDirectory { get; private set; }
    public string? SourceFileListPath { get; private set; }
    public IReadOnlyList<IIPSArchiveEntry> MergedEntries => _mergedEntries;
    public IReadOnlyList<string> LoadedArchives => _loadedArchives;
    public IReadOnlyList<string> MissingArchives => _missingArchives;
    public int TotalArchiveCount => _loadedArchives.Count + _missingArchives.Count;
    public IReadOnlyList<IIPSArchive> Archives => _archives;

    public static IIPSUnifiedArchive Open(string fileListPath)
    {
        IIPSUnifiedArchive unified = new IIPSUnifiedArchive();
        unified.SourceFileListPath = fileListPath;
        unified.SourceDirectory = Path.GetDirectoryName(fileListPath);

        IIPSFileList fileList = IIPSFileList.Parse(fileListPath);
        IIPSFileListResolvedState resolved = fileList.Resolve();
        unified.Version = resolved.Version;

        string iipsDir = unified.SourceDirectory ?? ".";

        // Look for the ifs files in the same directory as the lst,
        // or in an iipsdownload subdirectory (common MHO client layout)
        string[] searchDirs = [iipsDir, Path.Combine(iipsDir, "iipsdownload")];

        // Use ALL referenced files from the entire lst history, not just the resolved latest.
        // This handles version mismatches where the lst references newer archives that don't exist
        // but older versions of the same archives are available on disk.
        // The merge logic (later overwrites earlier) produces the correct result.
        List<string> allFiles = fileList.GetAllReferencedFiles();

        Dictionary<string, IIPSArchiveEntry> merged = new(StringComparer.OrdinalIgnoreCase);

        foreach (string ifsFileName in allFiles)
        {
            string? foundPath = null;
            foreach (string dir in searchDirs)
            {
                string candidate = Path.Combine(dir, ifsFileName);
                if (File.Exists(candidate))
                {
                    foundPath = candidate;
                    break;
                }
            }

            if (foundPath == null)
            {
                unified._missingArchives.Add(ifsFileName);
                Logger.Info($"Skipping missing archive: {ifsFileName}");
                continue;
            }

            try
            {
                IIPSArchive archive = IIPSArchive.Open(foundPath);
                unified._archives.Add(archive);
                unified._loadedArchives.Add(ifsFileName);

                foreach (IIPSArchiveEntry entry in archive.Entries)
                {
                    if (!entry.Exists || string.IsNullOrWhiteSpace(entry.ArchivePath))
                    {
                        continue;
                    }

                    // Later archives overwrite earlier entries (patch-over-base)
                    merged[entry.ArchivePath!] = entry;
                }

                Logger.Info($"Loaded {ifsFileName}: {archive.Entries.Count} entries");
            }
            catch (Exception ex)
            {
                unified._missingArchives.Add(ifsFileName);
                Logger.Error($"Failed to open {ifsFileName}: {ex.Message}");
            }
        }

        unified._mergedEntries.AddRange(merged.Values.OrderBy(e => e.ArchivePath, StringComparer.OrdinalIgnoreCase));
        Logger.Info($"Unified archive: {unified._mergedEntries.Count} merged entries from {unified._loadedArchives.Count} archives ({unified._missingArchives.Count} missing)");

        return unified;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        foreach (IIPSArchive archive in _archives)
        {
            archive.Dispose();
        }

        _archives.Clear();
        _disposed = true;
    }
}
