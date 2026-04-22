using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia.Media.Imaging;
using Arrowgene.Lua.Decompiler;
using Arrowgene.Lua.Decompiler.Decompile;
using Arrowgene.Lua.Decompiler.Parse;
using Arrowgene.MonsterHunterOnline.ClientTools;
using Arrowgene.MonsterHunterOnline.ClientTools.Dat;
using Arrowgene.MonsterHunterOnline.ClientTools.FileProvider;
using Arrowgene.MonsterHunterOnline.ClientTools.Flash;
using Arrowgene.MonsterHunterOnline.ClientTools.IIPS;
using Arrowgene.MonsterHunterOnline.UI.Infrastructure;
using Arrowgene.MonsterHunterOnline.UI.ViewModels;

namespace Arrowgene.MonsterHunterOnline.UI.Components;

public sealed class DatSheetViewModel
{
    public string Name { get; init; } = string.Empty;
    public string[] Headers { get; init; } = Array.Empty<string>();
    public List<string[]> Rows { get; init; } = [];
}

public sealed class IIPSArchiveFileExplorerViewModel : ViewModelBase, IDisposable
{
    private const string UnnamedFolderName = "_unnamed";
    private const int MaxPreviewDecodeWidth = 960;
    private const long MaxImagePreviewBytes = 32L * 1024L * 1024L;
    private const long MaxTextPreviewBytes = 512L * 1024L;
    private static readonly HashSet<string> PreviewableImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".webp", ".dds", ".tif", ".tiff"
    };
    private static readonly HashSet<string> PreviewableTextExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".txt", ".json", ".xml", ".ini", ".cfg", ".lua", ".as"
    };
    private static readonly UTF8Encoding Utf8Strict = new(false, true);
    private static readonly UnicodeEncoding Utf16LeStrict = new(false, false, true);
    private static readonly UnicodeEncoding Utf16BeStrict = new(true, false, true);
    private static readonly UTF32Encoding Utf32LeStrict = new(false, false, true);
    private static readonly UTF32Encoding Utf32BeStrict = new(true, false, true);
    private static readonly Encoding GbkEncoding = Encoding.GetEncoding("GBK", EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);

    private IIPSArchive? _archive;
    private IIPSUnifiedArchive? _unifiedArchive;
    private IFileProvider? _fileProvider;
    private bool _isUnifiedMode;
    private string _archiveFileName = "No archive loaded";
    private string _archiveFilePath = "Select an IIPS archive to inspect its contents.";
    private string _statusText = "Open an IIPS archive to browse archive paths, inspect metadata, and extract entries.";
    private string _formatVersionText = "-";
    private string _sectorSizeText = "-";
    private string _headerHashText = "-";
    private string _betHashText = "-";
    private string _hetHashText = "-";
    private int _entryCount;
    private int _namedEntryCount;
    private int _unnamedEntryCount;
    private int _directoryCount;
    private bool _hasArchive;
    private bool _hasUnsavedChanges;
    private bool _canExtractSelection;
    private bool _canAddFile;
    private bool _canModifySelection;
    private bool _canRemoveSelection;
    private bool _canSaveArchive;
    private IReadOnlyList<IIPSArchiveTreeNodeViewModel> _allFileNodes = Array.Empty<IIPSArchiveTreeNodeViewModel>();
    private ObservableCollection<IIPSArchiveTreeNodeViewModel> _rootNodes = [];
    private IIPSArchiveTreeNodeViewModel? _selectedNode;
    private string? _lastSelectionPath;
    private string _filterText = string.Empty;
    private string _filterSummaryText = "Open an archive to browse files.";
    private bool _hasVisibleNodes;
    private string _treeEmptyStateText = "Open an archive to browse files.";
    private string _selectedName = "No selection";
    private string _selectedPath = "Select an entry from the archive tree.";
    private string _selectedType = "-";
    private string _selectedLength = "-";
    private string _selectedStoredLength = "-";
    private string _selectedFlags = "-";
    private string _selectedXmlFormat = "-";
    private string _selectedLuaFormat = "-";
    private string _selectedHash = "-";
    private string _selectedIndex = "-";
    private Bitmap? _previewBitmap;
    private bool _hasImagePreview;
    private bool _hasTextPreview;
    private bool _hasTablePreview;
    private string _textPreviewContent = string.Empty;
    private string _previewStatus = "Select a previewable file to preview.";
    private string _previewDetails = "-";
    private string[] _tablePreviewHeaders = Array.Empty<string>();
    private List<string[]> _tablePreviewRows = [];
    private bool _hasDatPreview;
    private List<DatSheetViewModel> _datPreviewSheets = [];
    private int _datSelectedSheetIndex;
    private bool _isHexMode;
    private bool _userToggledHex;
    private byte[]? _currentPreviewData;
    private long _currentPreviewLength;
    private bool _isInsideSwf;
    private bool _canBrowseSwf;
    private bool _canEditFile;
    private string _swfBreadcrumb = string.Empty;
    private IReadOnlyList<IIPSArchiveTreeNodeViewModel>? _savedAllFileNodes;
    private ObservableCollection<IIPSArchiveTreeNodeViewModel>? _savedRootNodes;
    private string? _savedFilterText;
    private string? _savedFilterSummary;
    private string? _savedSelectionPath;

    public string ArchiveFileName
    {
        get => _archiveFileName;
        private set => SetProperty(ref _archiveFileName, value);
    }

    public string ArchiveFilePath
    {
        get => _archiveFilePath;
        private set => SetProperty(ref _archiveFilePath, value);
    }

    public string StatusText
    {
        get => _statusText;
        private set => SetProperty(ref _statusText, value);
    }

    public string FormatVersionText
    {
        get => _formatVersionText;
        private set => SetProperty(ref _formatVersionText, value);
    }

    public string SectorSizeText
    {
        get => _sectorSizeText;
        private set => SetProperty(ref _sectorSizeText, value);
    }

    public string HeaderHashText
    {
        get => _headerHashText;
        private set => SetProperty(ref _headerHashText, value);
    }

    public string BetHashText
    {
        get => _betHashText;
        private set => SetProperty(ref _betHashText, value);
    }

    public string HetHashText
    {
        get => _hetHashText;
        private set => SetProperty(ref _hetHashText, value);
    }

    public string HashSummaryTooltip => $"Header: {HeaderHashText}\nBET: {BetHashText}\nHET: {HetHashText}";

    public int EntryCount
    {
        get => _entryCount;
        private set => SetProperty(ref _entryCount, value);
    }

    public int NamedEntryCount
    {
        get => _namedEntryCount;
        private set => SetProperty(ref _namedEntryCount, value);
    }

    public int UnnamedEntryCount
    {
        get => _unnamedEntryCount;
        private set => SetProperty(ref _unnamedEntryCount, value);
    }

    public int DirectoryCount
    {
        get => _directoryCount;
        private set => SetProperty(ref _directoryCount, value);
    }

    public bool HasArchive
    {
        get => _hasArchive;
        private set => SetProperty(ref _hasArchive, value);
    }

    public bool HasUnsavedChanges
    {
        get => _hasUnsavedChanges;
        private set => SetProperty(ref _hasUnsavedChanges, value);
    }

    public bool IsUnifiedMode
    {
        get => _isUnifiedMode;
        private set
        {
            if (SetProperty(ref _isUnifiedMode, value))
            {
                OnPropertyChanged(nameof(IsNotUnifiedMode));
            }
        }
    }

    public bool IsNotUnifiedMode => !IsUnifiedMode;

    public bool CanExtractSelection
    {
        get => _canExtractSelection;
        private set => SetProperty(ref _canExtractSelection, value);
    }

    public bool CanAddFile
    {
        get => _canAddFile;
        private set => SetProperty(ref _canAddFile, value);
    }

    public bool CanModifySelection
    {
        get => _canModifySelection;
        private set => SetProperty(ref _canModifySelection, value);
    }

    public bool CanRemoveSelection
    {
        get => _canRemoveSelection;
        private set => SetProperty(ref _canRemoveSelection, value);
    }

    public bool CanSaveArchive
    {
        get => _canSaveArchive;
        private set => SetProperty(ref _canSaveArchive, value);
    }

    public bool IsInsideSwf
    {
        get => _isInsideSwf;
        private set => SetProperty(ref _isInsideSwf, value);
    }

    public string SwfBreadcrumb
    {
        get => _swfBreadcrumb;
        private set => SetProperty(ref _swfBreadcrumb, value);
    }

    public bool CanBrowseSwf
    {
        get => _canBrowseSwf;
        private set => SetProperty(ref _canBrowseSwf, value);
    }

    public bool CanEditFile
    {
        get => _canEditFile;
        private set => SetProperty(ref _canEditFile, value);
    }

    public ObservableCollection<IIPSArchiveTreeNodeViewModel> RootNodes
    {
        get => _rootNodes;
        private set => SetProperty(ref _rootNodes, value);
    }

    public string FilterText
    {
        get => _filterText;
        set
        {
            if (SetProperty(ref _filterText, value))
            {
                OnPropertyChanged(nameof(HasActiveFilter));
                ApplyFilter();
            }
        }
    }

    public bool HasActiveFilter => !string.IsNullOrWhiteSpace(FilterText);

    public string FilterSummaryText
    {
        get => _filterSummaryText;
        private set => SetProperty(ref _filterSummaryText, value);
    }

    public bool HasVisibleNodes
    {
        get => _hasVisibleNodes;
        private set
        {
            if (SetProperty(ref _hasVisibleNodes, value))
            {
                OnPropertyChanged(nameof(ShowTreeEmptyState));
            }
        }
    }

    public bool ShowTreeEmptyState => !HasVisibleNodes;

    public string TreeEmptyStateText
    {
        get => _treeEmptyStateText;
        private set => SetProperty(ref _treeEmptyStateText, value);
    }

    public IIPSArchiveTreeNodeViewModel? SelectedNode
    {
        get => _selectedNode;
        set
        {
            if (SetProperty(ref _selectedNode, value))
            {
                if (value != null)
                {
                    _lastSelectionPath = value.DisplayPath;
                }

                UpdateSelectionState(value);
            }
        }
    }

    public string SelectedName
    {
        get => _selectedName;
        private set => SetProperty(ref _selectedName, value);
    }

    public string SelectedPath
    {
        get => _selectedPath;
        private set => SetProperty(ref _selectedPath, value);
    }

    public string SelectedType
    {
        get => _selectedType;
        private set => SetProperty(ref _selectedType, value);
    }

    public string SelectedLength
    {
        get => _selectedLength;
        private set => SetProperty(ref _selectedLength, value);
    }

    public string SelectedStoredLength
    {
        get => _selectedStoredLength;
        private set => SetProperty(ref _selectedStoredLength, value);
    }

    public string SelectedFlags
    {
        get => _selectedFlags;
        private set => SetProperty(ref _selectedFlags, value);
    }

    public string SelectedXmlFormat
    {
        get => _selectedXmlFormat;
        private set => SetProperty(ref _selectedXmlFormat, value);
    }

    public string SelectedLuaFormat
    {
        get => _selectedLuaFormat;
        private set => SetProperty(ref _selectedLuaFormat, value);
    }

    public string SelectedHash
    {
        get => _selectedHash;
        private set => SetProperty(ref _selectedHash, value);
    }

    public string SelectedIndex
    {
        get => _selectedIndex;
        private set => SetProperty(ref _selectedIndex, value);
    }

    public Bitmap? PreviewBitmap
    {
        get => _previewBitmap;
        private set => SetProperty(ref _previewBitmap, value);
    }

    public bool HasImagePreview
    {
        get => _hasImagePreview;
        private set => SetProperty(ref _hasImagePreview, value);
    }

    public bool HasTextPreview
    {
        get => _hasTextPreview;
        private set => SetProperty(ref _hasTextPreview, value);
    }

    public bool HasTablePreview
    {
        get => _hasTablePreview;
        private set => SetProperty(ref _hasTablePreview, value);
    }

    public string[] TablePreviewHeaders
    {
        get => _tablePreviewHeaders;
        private set => SetProperty(ref _tablePreviewHeaders, value);
    }

    public List<string[]> TablePreviewRows
    {
        get => _tablePreviewRows;
        private set => SetProperty(ref _tablePreviewRows, value);
    }

    public bool HasDatPreview
    {
        get => _hasDatPreview;
        private set => SetProperty(ref _hasDatPreview, value);
    }

    public List<DatSheetViewModel> DatPreviewSheets
    {
        get => _datPreviewSheets;
        private set => SetProperty(ref _datPreviewSheets, value);
    }

    public int DatSelectedSheetIndex
    {
        get => _datSelectedSheetIndex;
        set => SetProperty(ref _datSelectedSheetIndex, value);
    }

    public bool IsHexMode
    {
        get => _isHexMode;
        private set => SetProperty(ref _isHexMode, value);
    }

    public bool CanToggleHex => _selectedNode != null && !_selectedNode.IsDirectory;

    public string TextPreviewContent
    {
        get => _textPreviewContent;
        private set => SetProperty(ref _textPreviewContent, value);
    }

    public string PreviewStatus
    {
        get => _previewStatus;
        private set => SetProperty(ref _previewStatus, value);
    }

    public string PreviewDetails
    {
        get => _previewDetails;
        private set => SetProperty(ref _previewDetails, value);
    }

    public bool TryOpenArchive(string path)
    {
        try
        {
            IIPSArchive archive = IIPSArchive.Open(path);
            SwapArchive(archive, path);
            return true;
        }
        catch (Exception ex)
        {
            StatusText = $"Failed to open archive: {ex.Message}";
            return false;
        }
    }

    public bool TryOpenFileList(string path)
    {
        try
        {
            IIPSUnifiedArchive unified = IIPSUnifiedArchive.Open(path);
            SwapUnifiedArchive(unified, path);
            return true;
        }
        catch (Exception ex)
        {
            StatusText = $"Failed to open file list: {ex.Message}";
            return false;
        }
    }

    public bool TryOpenFileProvider(IFileProvider provider, string label)
    {
        try
        {
            DisposeArchive();
            _fileProvider = provider;
            IsUnifiedMode = true;
            SetFilterTextSilently(string.Empty);
            _lastSelectionPath = null;

            ArchiveFileName = Path.GetFileName(label.TrimEnd(Path.DirectorySeparatorChar, '/'));
            ArchiveFilePath = label;
            FormatVersionText = "Directory";
            SectorSizeText = "-";
            HeaderHashText = "-";
            BetHashText = "-";
            HetHashText = "-";
            OnPropertyChanged(nameof(HashSummaryTooltip));
            HasArchive = true;
            HasUnsavedChanges = false;

            BuildFileProviderTree(provider);

            StatusText = $"Loaded {EntryCount} files from {ArchiveFileName}.";
            return true;
        }
        catch (Exception ex)
        {
            StatusText = $"Failed to open directory: {ex.Message}";
            return false;
        }
    }

    public bool TryExtractAll(string outputDirectory)
    {
        if (!HasArchive)
        {
            StatusText = "Open an archive before extracting.";
            return false;
        }

        try
        {
            if (_isUnifiedMode && _unifiedArchive != null)
            {
                Directory.CreateDirectory(outputDirectory);
                int extracted = 0;
                foreach (IIPSArchiveEntry entry in _unifiedArchive.MergedEntries)
                {
                    if (!entry.Exists || entry.Length == 0 || string.IsNullOrEmpty(entry.ArchivePath))
                    {
                        continue;
                    }

                    string outputPath = Path.Combine(outputDirectory, entry.ArchivePath!.Replace('\\', Path.DirectorySeparatorChar));
                    string? directory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    File.WriteAllBytes(outputPath, entry.ReadAllBytes());
                    extracted++;
                }

                StatusText = $"Extracted {extracted} entries to {outputDirectory}.";
            }
            else if (_archive != null)
            {
                _archive.ExtractAll(outputDirectory);
                StatusText = $"Extracted {EntryCount} archive entries to {outputDirectory}.";
            }

            return true;
        }
        catch (Exception ex)
        {
            StatusText = $"Failed to extract archive: {ex.Message}";
            return false;
        }
    }

    public bool TryExtractSelection(string outputDirectory)
    {
        if (!HasArchive)
        {
            StatusText = "Open an archive before extracting.";
            return false;
        }

        if (SelectedNode == null)
        {
            StatusText = "Select an archive entry or folder first.";
            return false;
        }

        try
        {
            int extractedFiles = 0;
            foreach (IIPSArchiveTreeNodeViewModel fileNode in SelectedNode.EnumerateFileNodes())
            {
                byte[]? data = ReadNodeBytes(fileNode);
                if (data == null || data.Length == 0)
                {
                    continue;
                }

                string outputPath = Path.Combine(outputDirectory, fileNode.OutputRelativePath.Replace('\\', Path.DirectorySeparatorChar));
                string? directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllBytes(outputPath, data);
                extractedFiles++;
            }

            StatusText = extractedFiles == 0
                ? "The current selection did not contain extractable file entries."
                : $"Extracted {extractedFiles} item(s) to {outputDirectory}.";
            return extractedFiles > 0;
        }
        catch (Exception ex)
        {
            StatusText = $"Failed to extract selection: {ex.Message}";
            return false;
        }
    }

    public bool TryAddFile(string sourcePath, string archivePath)
    {
        if (_archive == null)
        {
            StatusText = "Open an archive before adding files.";
            return false;
        }

        try
        {
            _archive.AddFile(sourcePath, archivePath);
            HasUnsavedChanges = true;
            RebuildTree(archivePath);
            StatusText = $"Added {archivePath} from {Path.GetFileName(sourcePath)}.";
            return true;
        }
        catch (Exception ex)
        {
            StatusText = $"Failed to add file: {ex.Message}";
            return false;
        }
    }

    public bool TryModifySelection(string sourcePath)
    {
        if (_archive == null)
        {
            StatusText = "Open an archive before modifying files.";
            return false;
        }

        if (SelectedNode?.Entry == null)
        {
            StatusText = "Select a file entry to modify.";
            return false;
        }

        try
        {
            string displayPath = SelectedNode.DisplayPath;
            string name = SelectedNode.Name;
            _archive.Modify(SelectedNode.Entry, File.ReadAllBytes(sourcePath));
            HasUnsavedChanges = true;
            RebuildTree(displayPath);
            StatusText = $"Updated {name} from {Path.GetFileName(sourcePath)}.";
            return true;
        }
        catch (Exception ex)
        {
            StatusText = $"Failed to modify file: {ex.Message}";
            return false;
        }
    }

    public bool TryRemoveSelection()
    {
        if (_archive == null)
        {
            StatusText = "Open an archive before removing files.";
            return false;
        }

        if (SelectedNode == null)
        {
            StatusText = "Select an archive entry or folder first.";
            return false;
        }

        List<IIPSArchiveEntry> entries = SelectedNode.EnumerateFileNodes()
            .Select(static node => node.Entry)
            .Where(static entry => entry != null)
            .Cast<IIPSArchiveEntry>()
            .ToList();

        if (entries.Count == 0)
        {
            StatusText = "The current selection does not contain removable file entries.";
            return false;
        }

        int removed = 0;
        foreach (IIPSArchiveEntry entry in entries)
        {
            if (_archive.Remove(entry))
            {
                removed++;
            }
        }

        if (removed == 0)
        {
            StatusText = "No archive entries were removed.";
            return false;
        }

        HasUnsavedChanges = true;
        RebuildTree();
        StatusText = removed == 1
            ? "Removed 1 archive entry."
            : $"Removed {removed} archive entries.";
        return true;
    }

    public bool TrySaveArchive()
    {
        if (_archive != null)
        {
            try
            {
                string? selectionPath = SelectedNode?.DisplayPath;
                _archive.Save(ArchiveFilePath);
                HasUnsavedChanges = false;
                RebuildTree(selectionPath);
                StatusText = $"Saved archive to {ArchiveFilePath}.";
                return true;
            }
            catch (Exception ex)
            {
                StatusText = $"Failed to save archive: {ex.Message}";
                return false;
            }
        }

        if (_unifiedArchive != null)
        {
            try
            {
                int saved = 0;
                foreach (IIPSArchive childArchive in _unifiedArchive.Archives)
                {
                    if (!childArchive.HasPendingChanges || childArchive.SourcePath == null)
                    {
                        continue;
                    }

                    childArchive.Save(childArchive.SourcePath);
                    saved++;
                }

                if (saved == 0)
                {
                    StatusText = "No archives had pending changes.";
                    return false;
                }

                HasUnsavedChanges = false;
                StatusText = $"Saved {saved} modified archive(s).";
                return true;
            }
            catch (Exception ex)
            {
                StatusText = $"Failed to save: {ex.Message}";
                return false;
            }
        }

        StatusText = "Open an archive before saving.";
        return false;
    }

    public bool TryUpdateFileByPath(string archivePath, byte[] newData)
    {
        IIPSArchiveEntry? entry = FindEntryByPath(archivePath);
        if (entry == null)
        {
            StatusText = $"Entry not found: {Path.GetFileName(archivePath)}";
            return false;
        }

        try
        {
            entry.Archive.Modify(entry, newData);
            HasUnsavedChanges = true;
            CanSaveArchive = true;
            string archiveName = entry.Archive.SourcePath != null ? Path.GetFileName(entry.Archive.SourcePath) : "archive";
            StatusText = $"Updated {Path.GetFileName(archivePath)} in {archiveName} (unsaved).";
            return true;
        }
        catch (Exception ex)
        {
            StatusText = $"Failed to update {Path.GetFileName(archivePath)}: {ex.Message}";
            return false;
        }
    }

    public bool TryUpdateSwfTag(string swfArchivePath, int tagIndex, byte[] newTagData)
    {
        IIPSArchiveEntry? swfEntry = FindEntryByPath(swfArchivePath);
        if (swfEntry == null)
        {
            StatusText = $"SWF entry not found: {Path.GetFileName(swfArchivePath)}";
            return false;
        }

        try
        {
            byte[] swfBytes = swfEntry.ReadAllBytes();
            SwfFile swf = SwfFile.Open(swfBytes, Path.GetFileName(swfArchivePath));
            swf.ReplaceTagData(tagIndex, newTagData);
            byte[] rebuilt = swf.Build(ClientTools.Flash.SwfCompression.Zlib);
            swfEntry.Archive.Modify(swfEntry, rebuilt);
            HasUnsavedChanges = true;
            CanSaveArchive = true;
            string archiveName = swfEntry.Archive.SourcePath != null ? Path.GetFileName(swfEntry.Archive.SourcePath) : "archive";
            StatusText = $"Updated tag {tagIndex} in {Path.GetFileName(swfArchivePath)} ({archiveName}, unsaved).";
            return true;
        }
        catch (Exception ex)
        {
            StatusText = $"Failed to update SWF tag: {ex.Message}";
            return false;
        }
    }

    private IIPSArchiveEntry? FindEntryByPath(string archivePath)
    {
        if (_archive != null && _archive.TryGetEntry(archivePath, out IIPSArchiveEntry? entry) && entry != null)
        {
            return entry;
        }

        if (_unifiedArchive != null)
        {
            return _unifiedArchive.MergedEntries
                .FirstOrDefault(e => string.Equals(e.ArchivePath, archivePath, StringComparison.OrdinalIgnoreCase));
        }

        return null;
    }

    public byte[]? ReadSelectedNodeBytes()
    {
        return SelectedNode == null ? null : ReadNodeBytes(SelectedNode);
    }

    private static readonly HashSet<string> EditableTextExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".txt", ".xml", ".json", ".ini", ".cfg", ".lua", ".csv", ".as"
    };

    private static bool IsEditableExtension(string extension)
    {
        return EditableTextExtensions.Contains(extension);
    }

    public bool TryNavigateIntoSwf(IIPSArchiveTreeNodeViewModel? node)
    {
        if (node == null || node.IsDirectory)
        {
            return false;
        }

        string extension = Path.GetExtension(node.DisplayPath ?? node.Name);
        if (!string.Equals(extension, ".swf", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        byte[]? swfBytes = ReadNodeBytes(node);
        if (swfBytes == null || swfBytes.Length < 8)
        {
            return false;
        }

        try
        {
            SwfFile swf = SwfFile.Open(swfBytes, node.Name);
            List<IIPSArchiveTreeNodeViewModel> tagNodes = BuildSwfTagNodes(swf, node.DisplayPath ?? node.Name);

            _savedAllFileNodes = _allFileNodes;
            _savedRootNodes = RootNodes;
            _savedFilterText = FilterText;
            _savedFilterSummary = FilterSummaryText;
            _savedSelectionPath = node.DisplayPath;

            _allFileNodes = tagNodes;
            SetFilterTextSilently(string.Empty);
            RootNodes = new ObservableCollection<IIPSArchiveTreeNodeViewModel>(tagNodes);
            HasVisibleNodes = tagNodes.Count > 0;
            FilterSummaryText = $"{tagNodes.Count} asset(s) in SWF";
            TreeEmptyStateText = "This SWF has no extractable assets.";
            IsInsideSwf = true;
            SwfBreadcrumb = node.Name;
            SelectedNode = null;
            StatusText = $"Opened SWF: {node.Name} — {swf.Tags.Count} tags, {tagNodes.Count} assets.";
            return true;
        }
        catch (Exception ex)
        {
            StatusText = $"Failed to open SWF: {ex.Message}";
            return false;
        }
    }

    public void NavigateBackFromSwf()
    {
        if (!IsInsideSwf || _savedRootNodes == null || _savedAllFileNodes == null)
        {
            return;
        }

        _allFileNodes = _savedAllFileNodes;
        RootNodes = _savedRootNodes;
        FilterSummaryText = _savedFilterSummary ?? string.Empty;
        SetFilterTextSilently(_savedFilterText ?? string.Empty);
        IsInsideSwf = false;
        SwfBreadcrumb = string.Empty;
        HasVisibleNodes = RootNodes.Count > 0;
        StatusText = $"Returned to archive.";

        SelectedNode = string.IsNullOrWhiteSpace(_savedSelectionPath)
            ? null
            : FindNodeByDisplayPath(RootNodes, _savedSelectionPath);

        _savedAllFileNodes = null;
        _savedRootNodes = null;
        _savedFilterText = null;
        _savedFilterSummary = null;
        _savedSelectionPath = null;
    }

    private static List<IIPSArchiveTreeNodeViewModel> BuildSwfTagNodes(SwfFile swf, string parentPath)
    {
        List<IIPSArchiveTreeNodeViewModel> nodes = [];

        byte[]? renderedFrame = SwfSceneRenderer.RenderFirstFrame(swf);
        if (renderedFrame != null && renderedFrame.Length > 0)
        {
            string framePath = $"{parentPath}\\_rendered_frame.png";
            nodes.Add(IIPSArchiveTreeNodeViewModel.CreateFile("_rendered_frame.png", framePath, framePath, renderedFrame));
        }

        foreach (SwfTag tag in swf.Tags)
        {
            if (tag.Code == 0 || tag.Code == 1 || tag.Length == 0)
            {
                continue;
            }

            // Expand DoABC tags into individual class nodes
            if (tag.Code == 82)
            {
                ExpandDoAbcTag(tag, parentPath, nodes);
                continue;
            }

            string stem = $"{tag.Index:D4}_{tag.Name}";
            if (tag.CharacterId.HasValue)
            {
                stem += $"_{tag.CharacterId.Value:D5}";
            }

            string? assetName = tag.ExportName ?? tag.SymbolClassName;
            if (!string.IsNullOrWhiteSpace(assetName))
            {
                stem += $"_{assetName}";
            }

            byte[]? assetData = ExtractSwfTagAsset(tag);
            if (assetData == null)
            {
                continue;
            }

            string ext = DetectSwfAssetExtension(tag, assetData);
            string fileName = stem + ext;
            string filePath = $"{parentPath}\\{fileName}";
            nodes.Add(IIPSArchiveTreeNodeViewModel.CreateSwfChild(fileName, filePath, filePath, assetData, parentPath, tag.Index));
        }

        return nodes;
    }

    private static void ExpandDoAbcTag(SwfTag tag, string parentPath, List<IIPSArchiveTreeNodeViewModel> nodes)
    {
        byte[] tagData = tag.Data.ToArray();
        List<string> classNames = ClientTools.Flash.AbcReader.ReadClassNamesFromDoAbcTag(tagData);

        if (classNames.Count == 0)
        {
            // Fallback: show the raw ABC tag
            string fileName = $"{tag.Index:D4}_DoABC.abc";
            string filePath = $"{parentPath}\\{fileName}";
            nodes.Add(IIPSArchiveTreeNodeViewModel.CreateSwfChild(fileName, filePath, filePath, tagData, parentPath, tag.Index));
            return;
        }

        foreach (string className in classNames)
        {
            // Convert package.ClassName to path: package/ClassName.as
            string asPath = className.Replace('.', '/');
            string fileName = $"{asPath}.as";
            string filePath = $"{parentPath}\\scripts\\{fileName}";

            // Each class node references the same DoABC tag (tag.Index)
            // The data is the full ABC blob — individual class extraction isn't practical
            nodes.Add(IIPSArchiveTreeNodeViewModel.CreateSwfChild(
                fileName, filePath, filePath, tagData, parentPath, tag.Index));
        }
    }

    private static byte[]? ExtractSwfTagAsset(SwfTag tag)
    {
        ReadOnlySpan<byte> data = tag.Data.Span;

        switch (tag.Code)
        {
            case 21: // DefineBitsJPEG2
            {
                if (data.Length < 4) return null;
                return data[2..].ToArray();
            }
            case 35: // DefineBitsJPEG3
            {
                if (data.Length < 6) return null;
                uint alphaOffset = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(data[2..]);
                if (alphaOffset == 0 || 6 + alphaOffset > (uint)data.Length) return data[6..].ToArray();
                return data.Slice(6, (int)alphaOffset).ToArray();
            }
            case 90: // DefineBitsJPEG4
            {
                if (data.Length < 8) return null;
                uint alphaOffset = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(data[2..]);
                int imageStart = 8; // skip characterId(2) + alphaOffset(4) + deblock(2)
                if (alphaOffset == 0) return data[imageStart..].ToArray();
                return data.Slice(imageStart, (int)alphaOffset).ToArray();
            }
            case 20: // DefineBitsLossless
            case 36: // DefineBitsLossless2
            case 82: // DoABC
            case 87: // DefineBinaryData
            {
                return data.ToArray();
            }
            default:
            {
                if (data.Length > 0)
                {
                    return data.ToArray();
                }
                return null;
            }
        }
    }

    private static string DetectSwfAssetExtension(SwfTag tag, byte[] data)
    {
        return tag.Code switch
        {
            21 or 35 or 90 => DetectImageExtension(data),
            20 or 36 => ".lossless.bin",
            82 => ".abc",
            87 => data.Length > 6 ? DetectImageExtension(data.AsSpan(6)) : ".bin",
            2 or 22 or 32 or 83 => ".shape.bin",
            39 => ".sprite.bin",
            10 or 48 or 75 => ".font.bin",
            14 => ".sound.bin",
            _ => ".bin",
        };
    }

    private static string DetectImageExtension(ReadOnlySpan<byte> data)
    {
        if (data.Length >= 8 && data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4E && data[3] == 0x47)
            return ".png";
        if (data.Length >= 3 && data[0] == 0xFF && data[1] == 0xD8 && data[2] == 0xFF)
            return ".jpg";
        if (data.Length >= 4 && data[0] == (byte)'G' && data[1] == (byte)'I' && data[2] == (byte)'F')
            return ".gif";
        return ".bin";
    }

    public void Dispose()
    {
        ClearImagePreview();
        DisposeArchive();
    }

    private void SwapArchive(IIPSArchive archive, string path)
    {
        DisposeArchive();
        _archive = archive;
        IsUnifiedMode = false;
        SetFilterTextSilently(string.Empty);
        _lastSelectionPath = null;

        ArchiveFileName = Path.GetFileName(path);
        ArchiveFilePath = path;
        FormatVersionText = $"Version {archive.Metadata.FormatVersion}";
        SectorSizeText = FormatSize(archive.Metadata.SectorSize);
        HeaderHashText = archive.Metadata.HeaderMd5;
        BetHashText = archive.Metadata.BetMd5;
        HetHashText = archive.Metadata.HetMd5;
        OnPropertyChanged(nameof(HashSummaryTooltip));
        HasArchive = true;
        HasUnsavedChanges = false;

        RebuildTree();

        StatusText = UnnamedEntryCount == 0
            ? $"Loaded {EntryCount} entries from {ArchiveFileName}."
            : $"Loaded {EntryCount} entries from {ArchiveFileName}. {UnnamedEntryCount} entries are still unnamed.";
    }

    private void SwapUnifiedArchive(IIPSUnifiedArchive unified, string path)
    {
        DisposeArchive();
        _unifiedArchive = unified;
        IsUnifiedMode = true;
        SetFilterTextSilently(string.Empty);
        _lastSelectionPath = null;

        ArchiveFileName = Path.GetFileName(path);
        ArchiveFilePath = path;
        FormatVersionText = unified.Version ?? "-";
        SectorSizeText = $"{unified.LoadedArchives.Count} archives";
        HeaderHashText = "-";
        BetHashText = "-";
        HetHashText = "-";
        OnPropertyChanged(nameof(HashSummaryTooltip));
        HasArchive = true;
        HasUnsavedChanges = false;

        RebuildUnifiedTree(unified);

        string missingText = unified.MissingArchives.Count > 0
            ? $" ({unified.MissingArchives.Count} missing)"
            : "";
        StatusText = $"Loaded {EntryCount} merged entries from {unified.LoadedArchives.Count} archives{missingText}.";
    }

    public string SuggestArchivePath(string sourcePath)
    {
        string fileName = Path.GetFileName(sourcePath);
        if (SelectedNode == null)
        {
            return fileName;
        }

        if (SelectedNode.IsDirectory && !string.IsNullOrWhiteSpace(SelectedNode.ArchivePath))
        {
            return $"{SelectedNode.ArchivePath}\\{fileName}";
        }

        if (SelectedNode.Entry != null && !string.IsNullOrWhiteSpace(SelectedNode.Entry.ArchivePath))
        {
            string? parent = GetArchiveDirectory(SelectedNode.Entry.ArchivePath);
            return string.IsNullOrWhiteSpace(parent) ? fileName : $"{parent}\\{fileName}";
        }

        return fileName;
    }

    private void RebuildTree(string? preferredSelectionPath = null)
    {
        if (_archive == null)
        {
            ResetArchiveState();
            return;
        }

        Dictionary<string, IIPSArchiveTreeNodeViewModel> directories = new(StringComparer.OrdinalIgnoreCase);
        List<IIPSArchiveTreeNodeViewModel> roots = [];
        IIPSArchiveTreeNodeViewModel? unnamedRoot = null;

        foreach (IIPSArchiveEntry entry in _archive.Entries.Where(static entry => entry.Exists))
        {
            if (!string.IsNullOrWhiteSpace(entry.ArchivePath))
            {
                AddNamedEntryNode(entry, entry.ArchivePath!, directories, roots);
                continue;
            }

            unnamedRoot ??= IIPSArchiveTreeNodeViewModel.CreateDirectory(UnnamedFolderName, null, UnnamedFolderName);
            if (!roots.Contains(unnamedRoot))
            {
                roots.Add(unnamedRoot);
            }

            string fileName = $"{entry.Index:D6}_{entry.Md5}.bin";
            string outputPath = Path.Combine(UnnamedFolderName, fileName).Replace('/', '\\');
            unnamedRoot.Children.Add(IIPSArchiveTreeNodeViewModel.CreateFile(fileName, null, outputPath, entry));
        }

        foreach (IIPSArchiveTreeNodeViewModel root in roots)
        {
            root.SortRecursive();
        }

        IReadOnlyList<IIPSArchiveTreeNodeViewModel> sortedRoots = roots
            .OrderByDescending(node => node.IsDirectory)
            .ThenBy(node => node.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        _allFileNodes = sortedRoots
            .SelectMany(static root => root.EnumerateFileNodes())
            .OrderBy(node => node.DisplayPath, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        EntryCount = _archive.Entries.Count(static entry => entry.Exists);
        NamedEntryCount = _archive.Entries.Count(static entry => entry.Exists && !string.IsNullOrWhiteSpace(entry.ArchivePath));
        UnnamedEntryCount = _archive.Entries.Count(static entry => entry.Exists && string.IsNullOrWhiteSpace(entry.ArchivePath));
        DirectoryCount = directories.Count + (unnamedRoot == null ? 0 : 1);
        UpdateToolbarState();
        ApplyFilter(preferredSelectionPath);
    }

    private void RebuildUnifiedTree(IIPSUnifiedArchive unified)
    {
        Dictionary<string, IIPSArchiveTreeNodeViewModel> directories = new(StringComparer.OrdinalIgnoreCase);
        List<IIPSArchiveTreeNodeViewModel> roots = [];

        foreach (IIPSArchiveEntry entry in unified.MergedEntries)
        {
            if (!string.IsNullOrWhiteSpace(entry.ArchivePath))
            {
                AddNamedEntryNode(entry, entry.ArchivePath!, directories, roots);
            }
        }

        foreach (IIPSArchiveTreeNodeViewModel root in roots)
        {
            root.SortRecursive();
        }

        IReadOnlyList<IIPSArchiveTreeNodeViewModel> sortedRoots = roots
            .OrderByDescending(node => node.IsDirectory)
            .ThenBy(node => node.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        _allFileNodes = sortedRoots
            .SelectMany(static root => root.EnumerateFileNodes())
            .OrderBy(node => node.DisplayPath, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        EntryCount = unified.MergedEntries.Count;
        NamedEntryCount = EntryCount;
        UnnamedEntryCount = 0;
        DirectoryCount = directories.Count;
        UpdateToolbarState();
        ApplyFilter();
    }

    private void BuildFileProviderTree(IFileProvider provider)
    {
        Dictionary<string, IIPSArchiveTreeNodeViewModel> directories = new(StringComparer.OrdinalIgnoreCase);
        List<IIPSArchiveTreeNodeViewModel> roots = [];
        int fileCount = 0;

        EnumerateProvider(provider, "", directories, roots, ref fileCount);

        foreach (IIPSArchiveTreeNodeViewModel root in roots)
        {
            root.SortRecursive();
        }

        IReadOnlyList<IIPSArchiveTreeNodeViewModel> sortedRoots = roots
            .OrderByDescending(node => node.IsDirectory)
            .ThenBy(node => node.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
        _allFileNodes = sortedRoots
            .SelectMany(static root => root.EnumerateFileNodes())
            .OrderBy(node => node.DisplayPath, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        EntryCount = fileCount;
        NamedEntryCount = fileCount;
        UnnamedEntryCount = 0;
        DirectoryCount = directories.Count;
        UpdateToolbarState();
        ApplyFilter();
    }

    private static void EnumerateProvider(IFileProvider provider, string relativeDir,
        Dictionary<string, IIPSArchiveTreeNodeViewModel> directories,
        List<IIPSArchiveTreeNodeViewModel> roots, ref int fileCount)
    {
        foreach (string dirPath in provider.EnumerateDirectories(relativeDir))
        {
            string name = Path.GetFileName(dirPath);
            string normalizedPath = dirPath.Replace('/', '\\');
            IIPSArchiveTreeNodeViewModel dirNode = IIPSArchiveTreeNodeViewModel.CreateDirectory(name, normalizedPath, normalizedPath);
            directories[normalizedPath] = dirNode;

            string? parentKey = GetArchiveDirectory(normalizedPath);
            if (parentKey != null && directories.TryGetValue(parentKey, out IIPSArchiveTreeNodeViewModel? parent))
            {
                parent.Children.Add(dirNode);
            }
            else
            {
                roots.Add(dirNode);
            }

            EnumerateProvider(provider, dirPath, directories, roots, ref fileCount);
        }

        foreach (string filePath in provider.EnumerateFiles(relativeDir, "*"))
        {
            string name = Path.GetFileName(filePath);
            string normalizedPath = filePath.Replace('/', '\\');

            long fileSize = 0;
            if (provider is DirectoryFileProvider dirProvider)
            {
                try
                {
                    string fullPath = Path.Combine(dirProvider.Root, filePath.Replace('/', Path.DirectorySeparatorChar));
                    fileSize = new FileInfo(fullPath).Length;
                }
                catch
                {
                }
            }

            IIPSArchiveTreeNodeViewModel fileNode = IIPSArchiveTreeNodeViewModel.CreateFile(name, normalizedPath, normalizedPath, fileSize);

            string? parentKey = GetArchiveDirectory(normalizedPath);
            if (parentKey != null && directories.TryGetValue(parentKey, out IIPSArchiveTreeNodeViewModel? parent))
            {
                parent.Children.Add(fileNode);
            }
            else
            {
                roots.Add(fileNode);
            }

            fileCount++;
        }
    }

    private byte[]? ReadNodeBytes(IIPSArchiveTreeNodeViewModel node)
    {
        if (node.InlineData != null)
        {
            return node.InlineData;
        }

        if (node.Entry != null)
        {
            return node.Entry.ReadAllBytes();
        }

        if (_fileProvider != null && !node.IsDirectory)
        {
            string path = node.DisplayPath.Replace('\\', '/');
            if (_fileProvider.Exists(path))
            {
                return _fileProvider.ReadAllBytes(path);
            }
        }

        return null;
    }

    private static void AddNamedEntryNode(IIPSArchiveEntry entry,
        string archivePath,
        Dictionary<string, IIPSArchiveTreeNodeViewModel> directories,
        List<IIPSArchiveTreeNodeViewModel> roots)
    {
        string normalizedPath = archivePath.Replace('/', '\\').Trim('\\');
        string[] segments = normalizedPath.Split('\\', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 0)
        {
            return;
        }

        IIPSArchiveTreeNodeViewModel? parent = null;
        int directorySegmentCount = entry.IsDirectory ? segments.Length : segments.Length - 1;
        string currentPath = string.Empty;

        for (int i = 0; i < directorySegmentCount; i++)
        {
            currentPath = currentPath.Length == 0 ? segments[i] : $"{currentPath}\\{segments[i]}";
            if (!directories.TryGetValue(currentPath, out IIPSArchiveTreeNodeViewModel? directoryNode))
            {
                directoryNode = IIPSArchiveTreeNodeViewModel.CreateDirectory(segments[i], currentPath, currentPath);
                directories.Add(currentPath, directoryNode);
                AttachNode(parent, directoryNode, roots);
            }

            parent = directoryNode;
        }

        if (entry.IsDirectory)
        {
            return;
        }

        string fileName = segments[^1];
        IIPSArchiveTreeNodeViewModel fileNode = IIPSArchiveTreeNodeViewModel.CreateFile(fileName, normalizedPath, normalizedPath, entry);
        AttachNode(parent, fileNode, roots);
    }

    private static void AttachNode(IIPSArchiveTreeNodeViewModel? parent,
        IIPSArchiveTreeNodeViewModel node,
        List<IIPSArchiveTreeNodeViewModel> roots)
    {
        if (parent == null)
        {
            roots.Add(node);
            return;
        }

        parent.Children.Add(node);
    }

    private void UpdateSelectionState(IIPSArchiveTreeNodeViewModel? node)
    {
        CanExtractSelection = HasArchive && node != null;
        CanAddFile = HasArchive;
        CanModifySelection = HasArchive && node?.Entry != null;
        CanRemoveSelection = HasArchive && node != null && node.EnumerateFileNodes().Any(static fileNode => fileNode.Entry != null);
        CanSaveArchive = HasArchive && HasUnsavedChanges;
        CanBrowseSwf = HasArchive && !IsInsideSwf && node != null && !node.IsDirectory &&
            string.Equals(Path.GetExtension(node.DisplayPath ?? node.Name), ".swf", StringComparison.OrdinalIgnoreCase);

        string? editExt = node != null && !node.IsDirectory ? Path.GetExtension(node.DisplayPath ?? node.Name) : null;
        CanEditFile = HasArchive && editExt != null && IsEditableExtension(editExt);

        if (node == null)
        {
            SelectedName = "No selection";
            SelectedPath = "Select an entry from the archive tree.";
            SelectedType = "-";
            SelectedLength = "-";
            SelectedStoredLength = "-";
            SelectedFlags = "-";
            SelectedXmlFormat = "-";
            SelectedLuaFormat = "-";
            SelectedHash = "-";
            SelectedIndex = "-";
            ResetPreview("Select a previewable file to preview.");
            return;
        }

        SelectedName = node.Name;
        SelectedPath = node.DisplayPath;
        SelectedType = node.IsDirectory ? "Directory" : "File";

        if (node.IsDirectory)
        {
            SelectedLength = $"{node.EnumerateFileNodes().Count()} child files";
            SelectedStoredLength = "-";
            SelectedFlags = "Virtual folder";
            SelectedXmlFormat = "-";
            SelectedLuaFormat = "-";
            SelectedHash = "-";
            SelectedIndex = "-";
            ResetPreview("Folders do not have previews.");
            return;
        }

        if (node.Entry != null)
        {
            SelectedLength = FormatSize(node.Entry.Length);
            SelectedStoredLength = FormatSize(node.Entry.StoredLength);
            SelectedFlags = FormatFlags(node.Entry);
            SelectedXmlFormat = DescribeXmlFormat(node.Entry);
            SelectedLuaFormat = DescribeLuaFormat(node.Entry);
            SelectedHash = node.Entry.Md5;
            SelectedIndex = node.Entry.Index.ToString();
        }
        else
        {
            SelectedLength = node.FileSize.HasValue ? FormatSize(node.FileSize.Value) : "-";
            SelectedStoredLength = "-";
            SelectedFlags = "-";
            SelectedXmlFormat = "-";
            SelectedLuaFormat = "-";
            SelectedHash = "-";
            SelectedIndex = "-";
        }

        UpdatePreview(node);
    }

    private void DisposeArchive()
    {
        ClearImagePreview();
        _archive?.Dispose();
        _archive = null;
        _unifiedArchive?.Dispose();
        _unifiedArchive = null;
        _fileProvider = null;
    }

    private void ResetArchiveState()
    {
        HasArchive = false;
        HasUnsavedChanges = false;
        IsUnifiedMode = false;
        CanExtractSelection = false;
        CanAddFile = false;
        CanModifySelection = false;
        CanRemoveSelection = false;
        CanSaveArchive = false;
        ArchiveFileName = "No archive loaded";
        ArchiveFilePath = "Select an IIPS archive to inspect its contents.";
        FormatVersionText = "-";
        SectorSizeText = "-";
        HeaderHashText = "-";
        BetHashText = "-";
        HetHashText = "-";
        OnPropertyChanged(nameof(HashSummaryTooltip));
        EntryCount = 0;
        NamedEntryCount = 0;
        UnnamedEntryCount = 0;
        DirectoryCount = 0;
        _allFileNodes = Array.Empty<IIPSArchiveTreeNodeViewModel>();
        _lastSelectionPath = null;
        SetFilterTextSilently(string.Empty);
        FilterSummaryText = "Open an archive to browse files.";
        TreeEmptyStateText = "Open an archive to browse files.";
        HasVisibleNodes = false;
        ResetPreview("Select a previewable file to preview.");
        RootNodes = [];
        SelectedNode = null;
    }

    private static string FormatFlags(IIPSArchiveEntry entry)
    {
        List<string> flags =
        [
            entry.StorageMode == IIPSArchiveStorageMode.SingleUnit ? "SingleUnit" : "SectorBased"
        ];

        if (entry.IsCompressed)
        {
            flags.Add("Compressed");
        }

        if (entry.IsEncrypted)
        {
            flags.Add("Encrypted");
        }

        if (entry.UsesFixedKey)
        {
            flags.Add("FixedKey");
        }

        if (entry.IsDirectory)
        {
            flags.Add("Directory");
        }

        return string.Join(", ", flags);
    }

    private static string? GetArchiveDirectory(string? archivePath)
    {
        if (string.IsNullOrWhiteSpace(archivePath))
        {
            return null;
        }

        string normalizedPath = archivePath.Replace('/', '\\');
        int separatorIndex = normalizedPath.LastIndexOf('\\');
        return separatorIndex <= 0 ? null : normalizedPath[..separatorIndex];
    }

    private void UpdateToolbarState()
    {
        CanAddFile = HasArchive;
        CanSaveArchive = HasArchive && HasUnsavedChanges;
    }

    private void ApplyFilter(string? preferredSelectionPath = null)
    {
        if (_allFileNodes.Count == 0)
        {
            RootNodes = [];
            HasVisibleNodes = false;
            FilterSummaryText = HasArchive ? "No files to display." : "Open an archive to browse files.";
            TreeEmptyStateText = HasArchive ? "This archive has no entries." : "Open an archive to browse files.";
            SelectedNode = null;
            return;
        }

        IReadOnlyList<IIPSArchiveTreeNodeViewModel> visibleFiles;
        if (!HasActiveFilter)
        {
            visibleFiles = _allFileNodes;
        }
        else
        {
            string filter = FilterText.Trim();
            visibleFiles = _allFileNodes
                .Where(node => node.Name.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                    node.DisplayPath.Contains(filter, StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }

        RootNodes = new ObservableCollection<IIPSArchiveTreeNodeViewModel>(visibleFiles);
        UpdateFilterPresentation(visibleFiles);

        string? selectionPath = preferredSelectionPath ?? SelectedNode?.DisplayPath ?? _lastSelectionPath;
        SelectedNode = string.IsNullOrWhiteSpace(selectionPath)
            ? null
            : FindNodeByDisplayPath(RootNodes, selectionPath);
    }

    private void UpdateFilterPresentation(IReadOnlyCollection<IIPSArchiveTreeNodeViewModel> visibleFiles)
    {
        int visibleFileCount = visibleFiles.Count;
        HasVisibleNodes = visibleFileCount > 0;

        if (!HasArchive)
        {
            FilterSummaryText = "Open an archive to browse files.";
            TreeEmptyStateText = "Open an archive to browse files.";
            return;
        }

        if (!HasActiveFilter)
        {
            FilterSummaryText = $"{EntryCount} file(s) sorted by archive path";
            TreeEmptyStateText = EntryCount == 0 ? "This archive has no entries." : "No files to display.";
            return;
        }

        if (!HasVisibleNodes)
        {
            FilterSummaryText = "No matches";
            TreeEmptyStateText = $"No files or folders match \"{FilterText.Trim()}\".";
            return;
        }

        FilterSummaryText = $"Showing {visibleFileCount} of {EntryCount} file(s)";
        TreeEmptyStateText = string.Empty;
    }

    private void SetFilterTextSilently(string value)
    {
        if (string.Equals(_filterText, value, StringComparison.Ordinal))
        {
            return;
        }

        _filterText = value;
        OnPropertyChanged(nameof(FilterText));
        OnPropertyChanged(nameof(HasActiveFilter));
    }

    private static IIPSArchiveTreeNodeViewModel? FindNodeByDisplayPath(IEnumerable<IIPSArchiveTreeNodeViewModel> nodes, string displayPath)
    {
        foreach (IIPSArchiveTreeNodeViewModel node in nodes)
        {
            if (string.Equals(node.DisplayPath, displayPath, StringComparison.OrdinalIgnoreCase))
            {
                return node;
            }

            IIPSArchiveTreeNodeViewModel? descendant = FindNodeByDisplayPath(node.Children, displayPath);
            if (descendant != null)
            {
                return descendant;
            }
        }

        return null;
    }

    public void ToggleHexMode()
    {
        _userToggledHex = !_userToggledHex;
        if (_selectedNode != null)
        {
            UpdatePreview(_selectedNode);
        }
    }

    private void UpdatePreview(IIPSArchiveTreeNodeViewModel node)
    {
        OnPropertyChanged(nameof(CanToggleHex));

        try
        {
            _currentPreviewData = ReadNodeBytes(node);
        }
        catch
        {
            _currentPreviewData = null;
        }

        if (_currentPreviewData == null || _currentPreviewData.Length == 0)
        {
            _currentPreviewData = null;
            IsHexMode = false;
            ResetPreview(_currentPreviewData == null ? "Preview unavailable." : "Empty file.");
            return;
        }

        _currentPreviewLength = _currentPreviewData.Length;

        if (_userToggledHex)
        {
            IsHexMode = true;
            ShowHexPreview(_currentPreviewData, _currentPreviewLength);
            return;
        }

        string? extension = Path.GetExtension(node.Entry?.ArchivePath ?? node.DisplayPath ?? node.Name);

        if (!string.IsNullOrWhiteSpace(extension))
        {
            if (PreviewableImageExtensions.Contains(extension))
            {
                IsHexMode = false;
                UpdateImagePreview(node);
                return;
            }

            if (string.Equals(extension, ".dat", StringComparison.OrdinalIgnoreCase))
            {
                IsHexMode = false;
                UpdateDatPreview(node);
                return;
            }

            if (string.Equals(extension, ".csv", StringComparison.OrdinalIgnoreCase))
            {
                IsHexMode = false;
                UpdateCsvPreview(node);
                return;
            }

            if (PreviewableTextExtensions.Contains(extension))
            {
                IsHexMode = false;
                UpdateTextPreview(node);
                return;
            }
        }

        // No recognized preview - fall back to hex
        IsHexMode = true;
        ShowHexPreview(_currentPreviewData, _currentPreviewLength);
    }

    private void UpdateImagePreview(IIPSArchiveTreeNodeViewModel node)
    {
        byte[]? data = _currentPreviewData;
        if (data == null)
        {
            ResetPreview("Preview unavailable.");
            return;
        }

        if (data.Length > MaxImagePreviewBytes)
        {
            ResetPreview($"Preview skipped for files larger than {FormatSize(MaxImagePreviewBytes)}.");
            return;
        }

        try
        {
            string archivePath = node.Entry?.ArchivePath ?? node.DisplayPath ?? node.Name;
            if (!ImagePreviewDecoder.TryDecode(data, archivePath, MaxPreviewDecodeWidth, out ImagePreviewDecodeResult decodedImage, out string? error))
            {
                ResetPreview($"Preview unavailable: {error}");
                return;
            }

            ReplacePreviewBitmap(decodedImage.Bitmap);
            HasImagePreview = true;
            HasTextPreview = false;
            TextPreviewContent = string.Empty;
            PreviewDetails = $"{decodedImage.SourceSize.Width} x {decodedImage.SourceSize.Height}";
            PreviewStatus = $"Image preview • {PreviewDetails}";
        }
        catch (Exception ex)
        {
            ResetPreview($"Preview unavailable: {ex.Message}");
        }
    }

    private void UpdateTextPreview(IIPSArchiveTreeNodeViewModel node)
    {
        byte[]? data = _currentPreviewData;
        if (data == null)
        {
            ResetPreview("Preview unavailable.");
            return;
        }

        if (data.Length > MaxTextPreviewBytes)
        {
            ResetPreview($"Preview skipped for text files larger than {FormatSize(MaxTextPreviewBytes)}.");
            return;
        }

        try
        {
            string archivePath = node.Entry?.ArchivePath ?? node.DisplayPath ?? node.Name;
            string extension = Path.GetExtension(archivePath);

            if (string.Equals(extension, ".xml", StringComparison.OrdinalIgnoreCase) &&
                MhoCryXmlCodec.IsCryXmlCodex(data))
            {
                MhoCryXmlFormat xmlFormat = MhoCryXmlCodec.DetectFormat(data);
                string xmlText = MhoCryXmlCodec.ReadXml(data).Replace("\r\n", "\n").Replace('\r', '\n');

                ClearImagePreview();
                HasImagePreview = false;
                HasTextPreview = true;
                TextPreviewContent = xmlText;
                PreviewDetails = $"{FormatXmlFormat(xmlFormat)} • {FormatSize(data.Length)}";
                PreviewStatus = $"XML preview • {CountLines(xmlText)} line(s)";
                return;
            }

            if (string.Equals(extension, ".lua", StringComparison.OrdinalIgnoreCase))
            {
                LuaFileInfo luaInfo = LuaFile.Identify(data);
                if (luaInfo.Type == LuaFileType.LuaCompiled)
                {
                    if (TryDecompileLua(data, out string luaText, out string? luaError))
                    {
                        ClearImagePreview();
                        HasImagePreview = false;
                        HasTextPreview = true;
                        TextPreviewContent = luaText;
                        string version = luaInfo.GetVersionString() ?? "?";
                        PreviewDetails = $"Compiled Lua {version} (decompiled) • {FormatSize(data.Length)}";
                        PreviewStatus = $"Lua preview • {CountLines(luaText)} line(s)";
                        return;
                    }

                    ResetPreview($"Lua decompilation failed: {luaError}");
                    return;
                }
            }

            if (!TryDecodeTextPreview(data, out string text, out string encodingName, out string? error))
            {
                ResetPreview($"Text preview unavailable: {error}");
                return;
            }

            ClearImagePreview();
            HasImagePreview = false;
            HasTextPreview = true;
            TextPreviewContent = text;
            PreviewDetails = $"{encodingName} • {FormatSize(data.Length)}";
            PreviewStatus = $"Text preview • {CountLines(text)} line(s)";
        }
        catch (Exception ex)
        {
            ResetPreview($"Text preview unavailable: {ex.Message}");
        }
    }

    private void UpdateCsvPreview(IIPSArchiveTreeNodeViewModel node)
    {
        byte[]? data = _currentPreviewData;
        if (data == null)
        {
            ResetPreview("Preview unavailable.");
            return;
        }

        if (data.Length > MaxTextPreviewBytes)
        {
            ResetPreview($"Preview skipped for files larger than {FormatSize(MaxTextPreviewBytes)}.");
            return;
        }

        try
        {
            if (!TryDecodeTextPreview(data, out string text, out _, out string? decodeError))
            {
                ResetPreview($"CSV preview unavailable: {decodeError}");
                return;
            }

            char separator = text.Contains('\t') ? '\t' : ',';
            string[] lines = text.Split('\n');
            List<string> nonEmptyLines = [];
            foreach (string line in lines)
            {
                string trimmed = line.TrimEnd('\r');
                if (trimmed.Length > 0)
                {
                    nonEmptyLines.Add(trimmed);
                }
            }

            if (nonEmptyLines.Count == 0)
            {
                ResetPreview("CSV file is empty.");
                return;
            }

            string[] headers = nonEmptyLines[0].Split(separator);
            List<string[]> rows = new(nonEmptyLines.Count - 1);
            for (int i = 1; i < nonEmptyLines.Count; i++)
            {
                rows.Add(nonEmptyLines[i].Split(separator));
            }

            ClearImagePreview();
            HasImagePreview = false;
            HasTextPreview = false;
            HasTablePreview = true;
            TablePreviewHeaders = headers;
            TablePreviewRows = rows;
            PreviewDetails = $"{headers.Length} col(s) • {rows.Count} row(s) • {FormatSize(data.Length)}";
            PreviewStatus = $"CSV preview • {headers.Length} column(s), {rows.Count} row(s)";
        }
        catch (Exception ex)
        {
            ResetPreview($"CSV preview unavailable: {ex.Message}");
        }
    }

    private void UpdateDatPreview(IIPSArchiveTreeNodeViewModel node)
    {
        byte[]? data = _currentPreviewData;
        if (data == null)
        {
            ResetPreview("Preview unavailable.");
            return;
        }

        try
        {
            if (!DatFile.IsDatFile(data))
            {
                ShowHexPreview(data, data.Length);
                return;
            }

            DatFile dat = new DatFile();
            dat.Open(data);

            if (dat.ContentType != DatFile.DatContentType.TSV || dat.Sheets.Count == 0)
            {
                ShowHexPreview(data, data.Length);
                return;
            }

            List<DatSheetViewModel> sheets = new(dat.Sheets.Count);
            int totalRows = 0;
            foreach (TsvSheet sheet in dat.Sheets)
            {
                string[] headers = sheet.TableHead ?? Array.Empty<string>();
                List<string[]> rows = [];
                if (sheet.Table != null)
                {
                    foreach (string[] row in sheet.Table)
                    {
                        rows.Add(row);
                    }
                }

                totalRows += rows.Count;
                sheets.Add(new DatSheetViewModel
                {
                    Name = sheet.Name ?? $"Sheet {sheets.Count + 1}",
                    Headers = headers,
                    Rows = rows,
                });
            }

            ClearImagePreview();
            HasImagePreview = false;
            HasTextPreview = false;
            HasTablePreview = false;
            HasDatPreview = true;
            DatPreviewSheets = sheets;
            DatSelectedSheetIndex = 0;
            PreviewDetails = $"{sheets.Count} sheet(s) • {totalRows} row(s) • {FormatSize(data.Length)}";
            PreviewStatus = $"DAT preview • {sheets.Count} sheet(s)";
        }
        catch (Exception ex)
        {
            ResetPreview($"DAT preview unavailable: {ex.Message}");
        }
    }

    private const int MaxHexPreviewBytes = 4096;

    private void UpdateHexPreview(IIPSArchiveTreeNodeViewModel node)
    {
        byte[]? data = _currentPreviewData;
        if (data == null)
        {
            ResetPreview("Preview unavailable.");
            return;
        }

        ShowHexPreview(data, _currentPreviewLength);
    }

    private void ShowHexPreview(byte[] data, long totalLength)
    {
        int limit = (int)Math.Min(data.Length, MaxHexPreviewBytes);
        string hex = FormatHexDump(data, limit);
        bool truncated = data.Length > MaxHexPreviewBytes;

        ClearImagePreview();
        HasImagePreview = false;
        HasTextPreview = true;
        HasTablePreview = false;
        HasDatPreview = false;
        DatPreviewSheets = [];
        TextPreviewContent = hex;
        PreviewDetails = $"Hex • {FormatSize(totalLength)}{(truncated ? $" (first {FormatSize(MaxHexPreviewBytes)})" : "")}";
        PreviewStatus = $"Hex preview • {FormatSize(totalLength)}";
    }

    private static string FormatHexDump(byte[] data, int length)
    {
        StringBuilder sb = new(length * 5);
        for (int offset = 0; offset < length; offset += 16)
        {
            sb.Append(offset.ToString("X8"));
            sb.Append("  ");

            int rowEnd = Math.Min(offset + 16, length);
            for (int i = offset; i < offset + 16; i++)
            {
                if (i == offset + 8) sb.Append(' ');
                if (i < rowEnd)
                {
                    sb.Append(data[i].ToString("X2"));
                    sb.Append(' ');
                }
                else
                {
                    sb.Append("   ");
                }
            }

            sb.Append(" |");
            for (int i = offset; i < rowEnd; i++)
            {
                byte b = data[i];
                sb.Append(b is >= 0x20 and <= 0x7E ? (char)b : '.');
            }
            sb.Append('|');
            sb.Append('\n');
        }

        return sb.ToString();
    }

    private void ResetPreview(string status)
    {
        ClearImagePreview();
        HasImagePreview = false;
        HasTextPreview = false;
        HasTablePreview = false;
        HasDatPreview = false;
        DatPreviewSheets = [];
        TablePreviewHeaders = Array.Empty<string>();
        TablePreviewRows = [];
        TextPreviewContent = string.Empty;
        _currentPreviewData = null;
        PreviewStatus = status;
        PreviewDetails = "-";
    }

    private void ReplacePreviewBitmap(Bitmap bitmap)
    {
        Bitmap? previous = PreviewBitmap;
        PreviewBitmap = bitmap;
        previous?.Dispose();
    }

    private void ClearImagePreview()
    {
        Bitmap? previous = PreviewBitmap;
        PreviewBitmap = null;
        previous?.Dispose();
    }

    private static bool TryDecodeTextPreview(byte[] data,
        out string text,
        out string encodingName,
        out string? error)
    {
        foreach ((Encoding encoding, string name, int bomLength) in GetTextEncodingCandidates(data))
        {
            try
            {
                string decoded = encoding.GetString(data, bomLength, data.Length - bomLength);
                if (!LooksLikeText(decoded))
                {
                    continue;
                }

                text = decoded.Replace("\r\n", "\n").Replace('\r', '\n');
                encodingName = name;
                error = null;
                return true;
            }
            catch (DecoderFallbackException)
            {
            }
            catch (ArgumentException)
            {
            }
        }

        text = string.Empty;
        encodingName = string.Empty;
        error = "The file does not look like readable text.";
        return false;
    }

    private static string DescribeXmlFormat(IIPSArchiveEntry entry)
    {
        string extension = Path.GetExtension(entry.ArchivePath ?? string.Empty);
        if (!string.Equals(extension, ".xml", StringComparison.OrdinalIgnoreCase))
        {
            return "-";
        }

        try
        {
            byte[] data = entry.ReadAllBytes();
            return FormatXmlFormat(MhoCryXmlCodec.DetectFormat(data));
        }
        catch (Exception ex)
        {
            return $"Inspection failed: {ex.Message}";
        }
    }

    private static string FormatXmlFormat(MhoCryXmlFormat format)
    {
        return format switch
        {
            MhoCryXmlFormat.PlainXml => "Plain XML",
            MhoCryXmlFormat.CryXmlBinary => "CryXmlB",
            MhoCryXmlFormat.EncryptedXml => "Encrypted XML",
            MhoCryXmlFormat.EncryptedCryXmlBinary => "Encrypted CryXmlB",
            _ => "Not XML",
        };
    }

    private static string DescribeLuaFormat(IIPSArchiveEntry entry)
    {
        string extension = Path.GetExtension(entry.ArchivePath ?? string.Empty);
        if (!string.Equals(extension, ".lua", StringComparison.OrdinalIgnoreCase))
        {
            return "-";
        }

        try
        {
            byte[] data = entry.ReadAllBytes();
            LuaFileInfo info = LuaFile.Identify(data);
            return info.Type switch
            {
                LuaFileType.LuaSource => "Source",
                LuaFileType.LuaCompiled => $"Compiled (Lua {info.GetVersionString() ?? "?"})",
                _ => "Unknown",
            };
        }
        catch (Exception ex)
        {
            return $"Inspection failed: {ex.Message}";
        }
    }

    private static bool TryDecompileLua(byte[] data, out string text, out string? error)
    {
        try
        {
            LuaByteBuffer buffer = new LuaByteBuffer(data);
            buffer.Order(LuaByteBuffer.LITTLE_ENDIAN);
            buffer.Position(0);
            Configuration config = new Configuration();
            BHeader header = new BHeader(buffer, config);
            LFunction lmain = header.main;

            Decompiler d = new Decompiler(lmain);
            Decompiler.State result = d.Decompile();

            using MemoryStream ms = new MemoryStream();
            Output output = new Output(new FileOutputProvider(ms));
            d.Print(result, output);
            output.Finish();

            ms.Position = 0;
            text = Encoding.UTF8.GetString(ms.ToArray()).Replace("\r\n", "\n").Replace('\r', '\n');
            error = null;
            return true;
        }
        catch (Exception ex)
        {
            text = string.Empty;
            error = ex.Message;
            return false;
        }
    }

    private static IEnumerable<(Encoding Encoding, string Name, int BomLength)> GetTextEncodingCandidates(byte[] data)
    {
        if (HasPrefix(data, 0xEF, 0xBB, 0xBF))
        {
            yield return (Utf8Strict, "UTF-8", 3);
            yield break;
        }

        if (HasPrefix(data, 0xFF, 0xFE, 0x00, 0x00))
        {
            yield return (Utf32LeStrict, "UTF-32 LE", 4);
            yield break;
        }

        if (HasPrefix(data, 0x00, 0x00, 0xFE, 0xFF))
        {
            yield return (Utf32BeStrict, "UTF-32 BE", 4);
            yield break;
        }

        if (HasPrefix(data, 0xFF, 0xFE))
        {
            yield return (Utf16LeStrict, "UTF-16 LE", 2);
            yield break;
        }

        if (HasPrefix(data, 0xFE, 0xFF))
        {
            yield return (Utf16BeStrict, "UTF-16 BE", 2);
            yield break;
        }

        yield return (Utf8Strict, "UTF-8", 0);

        if (LooksLikeUtf16(data, false))
        {
            yield return (Utf16LeStrict, "UTF-16 LE", 0);
        }

        if (LooksLikeUtf16(data, true))
        {
            yield return (Utf16BeStrict, "UTF-16 BE", 0);
        }

        yield return (GbkEncoding, "GBK", 0);
    }

    private static bool HasPrefix(byte[] data, params byte[] prefix)
    {
        if (data.Length < prefix.Length)
        {
            return false;
        }

        for (int i = 0; i < prefix.Length; i++)
        {
            if (data[i] != prefix[i])
            {
                return false;
            }
        }

        return true;
    }

    private static bool LooksLikeUtf16(byte[] data, bool bigEndian)
    {
        if (data.Length < 4 || (data.Length & 1) != 0)
        {
            return false;
        }

        int nullCount = 0;
        int inspectedPairs = Math.Min(data.Length / 2, 64);
        for (int i = 0; i < inspectedPairs; i++)
        {
            int offset = i * 2;
            byte first = data[offset];
            byte second = data[offset + 1];
            if (bigEndian ? first == 0 && second != 0 : second == 0 && first != 0)
            {
                nullCount++;
            }
        }

        return nullCount >= Math.Max(2, inspectedPairs / 4);
    }

    private static bool LooksLikeText(string text)
    {
        if (text.IndexOf('\0') >= 0)
        {
            return false;
        }

        int controlCount = 0;
        foreach (char character in text)
        {
            if (character == '\r' || character == '\n' || character == '\t')
            {
                continue;
            }

            if (char.IsControl(character))
            {
                controlCount++;
            }
        }

        return controlCount <= Math.Max(4, text.Length / 100);
    }

    private static int CountLines(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        return text.Count(static character => character == '\n') + 1;
    }

    private static string FormatSize(long value)
    {
        string[] units = ["B", "KB", "MB", "GB", "TB"];
        double size = value;
        int unit = 0;

        while (size >= 1024 && unit < units.Length - 1)
        {
            size /= 1024;
            unit++;
        }

        return unit == 0 ? $"{size:0} {units[unit]}" : $"{size:0.##} {units[unit]}";
    }
}
