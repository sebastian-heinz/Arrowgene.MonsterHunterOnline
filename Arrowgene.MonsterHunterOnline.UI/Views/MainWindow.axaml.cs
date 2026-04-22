using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.ClientTools.FileProvider;
using Arrowgene.MonsterHunterOnline.ClientTools.IIPS;
using Arrowgene.MonsterHunterOnline.UI.Components;
using Arrowgene.MonsterHunterOnline.UI.Infrastructure;
using Arrowgene.MonsterHunterOnline.UI.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Microsoft.Extensions.Logging;

namespace Arrowgene.MonsterHunterOnline.UI.Views;

public partial class MainWindow : Window
{
    private IIPSUnifiedArchive? _unifiedArchive;

    public MainWindow()
    {
        InitializeComponent();
        IIPSExplorer.EditFileRequested += OnEditFileRequested;
        FileEditorControl.SaveFileRequested += OnSaveFileRequested;
    }

    private void OnEditFileRequested(object? sender, EditFileRequestEventArgs e)
    {
        FileEditorControl.OpenFile(e.File);
        MainTabs.SelectedIndex = 1; // File Editor tab
    }

    private void OnSaveFileRequested(object? sender, SaveFileRequestEventArgs e)
    {
        if (IIPSExplorer.DataContext is not IIPSArchiveFileExplorerViewModel explorerVm)
        {
            return;
        }

        bool success = e.File.IsInsideSwf
            ? explorerVm.TryUpdateSwfTag(e.File.ArchivePath, e.File.SwfTagIndex!.Value, e.Data)
            : explorerVm.TryUpdateFileByPath(e.File.ArchivePath, e.Data);

        if (!success)
        {
            return;
        }

        foreach (EditorTabViewModel tab in FileEditorControl.ViewModel.Tabs)
        {
            if (tab.File == e.File)
            {
                FileEditorControl.MarkTabSaved(tab);
                break;
            }
        }
    }

    private MainWindowViewModel Vm => (MainWindowViewModel)DataContext!;

    // ── Toolbar buttons ──

    private async void OpenArchiveClick(object? sender, RoutedEventArgs e)
    {
        string? path = await PickFileAsync("Select IIPS archive", [("IIPS Archive", ["*.ifs"])]);
        if (!string.IsNullOrEmpty(path))
            await LoadFromSingleArchiveAsync(path);
    }

    private async void FileListClick(object? sender, RoutedEventArgs e)
    {
        string? path = await PickFileAsync("Select IIPS file list", [("IIPS File List", ["*.lst"])]);
        if (!string.IsNullOrEmpty(path))
            await LoadFromIIPSListAsync(path);
    }

    private async void FolderClick(object? sender, RoutedEventArgs e)
    {
        string? path = await PickFolderAsync("Select MHO client files directory");
        if (!string.IsNullOrEmpty(path))
            await LoadFromDirectoryAsync(path);
    }

    // ── Directory mode ──

    private async Task LoadFromDirectoryAsync(string dirPath)
    {
        CleanupPreviousSource();
        IFileProvider provider = new DirectoryFileProvider(dirPath);
        Vm.DataSourceLabel = $"Directory: {dirPath}";

        if (IIPSExplorer.DataContext is IIPSArchiveFileExplorerViewModel explorerVm)
            explorerVm.TryOpenFileProvider(provider, dirPath);

        await LoadAllViewersAsync(provider);
        RecentFilesStore.Add(dirPath, RecentEntryKind.Directory);
    }

    // ── IIPS list mode ──

    private async Task LoadFromIIPSListAsync(string lstPath)
    {
        CleanupPreviousSource();
        Vm.IsLoading = true;
        Vm.HasDataSource = false;
        Vm.StatusText = "Opening IIPS file list...";

        try
        {
            _unifiedArchive = await Task.Run(() => IIPSUnifiedArchive.Open(lstPath));

            // Feed to IIPS explorer
            if (IIPSExplorer.DataContext is IIPSArchiveFileExplorerViewModel explorerVm)
                explorerVm.TryOpenFileList(lstPath);

            IFileProvider provider = new IIPSFileProvider(_unifiedArchive);
            Vm.DataSourceLabel = $"IIPS List: {Path.GetFileName(lstPath)} ({_unifiedArchive.LoadedArchives.Count} archives, {_unifiedArchive.MergedEntries.Count} entries)";

            await LoadAllViewersAsync(provider);
            RecentFilesStore.Add(lstPath, RecentEntryKind.FileList);
        }
        catch (Exception ex)
        {
            Vm.StatusText = $"Error: {ex.Message}";
            Vm.IsLoading = false;
        }
    }

    // ── Single archive mode ──

    private async Task LoadFromSingleArchiveAsync(string ifsPath)
    {
        CleanupPreviousSource();
        Vm.IsLoading = true;
        Vm.StatusText = "Opening archive...";

        try
        {
            IIPSArchive archive = await Task.Run(() => IIPSArchive.Open(ifsPath));

            // Feed to IIPS explorer
            if (IIPSExplorer.DataContext is IIPSArchiveFileExplorerViewModel explorerVm)
                explorerVm.TryOpenArchive(ifsPath);

            // Try to load data from this single archive too
            // Create a mini unified archive wrapper for the file provider
            _unifiedArchive = null; // single archive, managed by explorer
            IFileProvider provider = CreateSingleArchiveProvider(archive);

            // Check if this archive has useful data
            bool hasStaticData = provider.Exists("common/staticdata/itemdata.dat") ||
                                 provider.Exists("common/staticdata/monsterdata.dat");

            Vm.DataSourceLabel = $"Archive: {Path.GetFileName(ifsPath)} ({archive.Entries.Count} entries)";

            if (hasStaticData)
            {
                await LoadAllViewersAsync(provider);
            }
            else
            {
                Vm.HasDataSource = false;
                Vm.StatusText = "Single archive mode — data viewer tabs disabled (no static data found).";
                Vm.IsLoading = false;
                MainTabs.SelectedIndex = 0;
            }

            RecentFilesStore.Add(ifsPath, RecentEntryKind.Archive);
        }
        catch (Exception ex)
        {
            Vm.StatusText = $"Error: {ex.Message}";
            Vm.IsLoading = false;
        }
    }

    private static IFileProvider CreateSingleArchiveProvider(IIPSArchive archive)
    {
        return new SingleArchiveFileProvider(archive);
    }

    // ── Load all viewers ──

    private async Task LoadAllViewersAsync(IFileProvider provider)
    {
        Vm.IsLoading = true;
        Vm.HasDataSource = false;
        Vm.StatusText = "Loading all data...";

        try
        {
            List<Task> tasks = [];

            if (ItemControl.DataContext is ItemViewerViewModel itemVm)
                tasks.Add(itemVm.LoadAsync(provider));
            if (EntityControl.DataContext is EntityViewerViewModel entityVm)
                tasks.Add(entityVm.LoadAsync(provider));
            if (CraftControl.DataContext is CraftViewerViewModel craftVm)
                tasks.Add(craftVm.LoadAsync(provider));
            if (QuestControl.DataContext is QuestViewerViewModel questVm)
                tasks.Add(questVm.LoadAsync(provider));
            if (LevelMapControl.DataContext is LevelMapViewerViewModel levelVm)
                tasks.Add(levelVm.LoadClientFilesAsync(provider));

            await Task.WhenAll(tasks);

            Vm.HasDataSource = true;
            Vm.StatusText = "All data loaded.";
        }
        catch (Exception ex)
        {
            Vm.StatusText = $"Error loading data: {ex.Message}";
        }
        finally
        {
            Vm.IsLoading = false;
        }
    }

    // ── Cleanup ──

    private void CleanupPreviousSource()
    {
        _unifiedArchive?.Dispose();
        _unifiedArchive = null;
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        CleanupPreviousSource();
        if (IIPSExplorer.DataContext is IIPSArchiveFileExplorerViewModel explorerVm)
            explorerVm.Dispose();
        base.OnClosing(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.Shutdown();
        LogProvider.Stop();
    }

    // ── Pickers ──

    private async Task<string?> PickFileAsync(string title, List<(string Name, List<string> Patterns)> filters)
    {
        if (OperatingSystem.IsMacOS())
        {
            List<string> extensions = filters
                .SelectMany(f => f.Patterns)
                .Select(p => p.TrimStart('*', '.'))
                .ToList();
            return await MacNativePicker.PickFileAsync(title, extensions);
        }

        TopLevel? topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider == null) return null;

        List<FilePickerFileType> fileTypes = filters
            .Select(f => new FilePickerFileType(f.Name) { Patterns = f.Patterns })
            .ToList();

        IReadOnlyList<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(
            new FilePickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
                FileTypeFilter = fileTypes,
            });

        return files.Count > 0 ? files[0].TryGetLocalPath() : null;
    }

    private async Task<string?> PickFolderAsync(string title)
    {
        if (OperatingSystem.IsMacOS())
            return await MacNativePicker.PickFolderAsync(title);

        TopLevel? topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider == null) return null;

        IReadOnlyList<IStorageFolder> folders = await topLevel.StorageProvider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions { Title = title, AllowMultiple = false });

        return folders.Count > 0 ? folders[0].TryGetLocalPath() : null;
    }

    // ── Recent files ──

    private void RecentClick(object? sender, RoutedEventArgs e)
    {
        MenuFlyout flyout = new() { Placement = PlacementMode.BottomEdgeAlignedLeft };

        List<RecentEntry> entries = RecentFilesStore.Load();

        if (entries.Count == 0)
        {
            flyout.Items.Add(new MenuItem { Header = "(no recent items)", IsEnabled = false });
        }
        else
        {
            foreach (RecentEntry entry in entries)
            {
                MenuItem item = new()
                {
                    Header = $"[{entry.KindLabel}]  {entry.DisplayName}",
                    Tag = entry
                };
                ToolTip.SetTip(item, entry.Path);
                item.Click += RecentItemClick;
                flyout.Items.Add(item);
            }

            flyout.Items.Add(new Separator());

            MenuItem clearItem = new() { Header = "Clear Recent" };
            clearItem.Click += ClearRecentClick;
            flyout.Items.Add(clearItem);
        }

        flyout.ShowAt(RecentButton);
    }

    private async void RecentItemClick(object? sender, RoutedEventArgs e)
    {
        if (sender is not MenuItem { Tag: RecentEntry entry }) return;

        switch (entry.Kind)
        {
            case RecentEntryKind.Archive:
                await LoadFromSingleArchiveAsync(entry.Path);
                break;
            case RecentEntryKind.FileList:
                await LoadFromIIPSListAsync(entry.Path);
                break;
            case RecentEntryKind.Directory:
                await LoadFromDirectoryAsync(entry.Path);
                break;
        }
    }

    private void ClearRecentClick(object? sender, RoutedEventArgs e)
    {
        RecentFilesStore.Clear();
    }
}

/// <summary>
/// IFileProvider backed by a single IIPSArchive (not unified).
/// </summary>
file sealed class SingleArchiveFileProvider : IFileProvider
{
    private readonly IIPSArchive _archive;
    private readonly Dictionary<string, IIPSArchiveEntry> _lookup;

    public SingleArchiveFileProvider(IIPSArchive archive)
    {
        _archive = archive;
        _lookup = new Dictionary<string, IIPSArchiveEntry>(StringComparer.OrdinalIgnoreCase);
        foreach (IIPSArchiveEntry entry in archive.Entries)
        {
            if (entry.Exists && !string.IsNullOrEmpty(entry.ArchivePath))
                _lookup[Normalize(entry.ArchivePath!)] = entry;
        }
    }

    public bool Exists(string relativePath) => _lookup.ContainsKey(Normalize(relativePath));

    public byte[] ReadAllBytes(string relativePath)
    {
        if (_lookup.TryGetValue(Normalize(relativePath), out var entry))
            return entry.ReadAllBytes();
        throw new FileNotFoundException($"Entry not found: {relativePath}");
    }

    public Stream OpenRead(string relativePath) => new MemoryStream(ReadAllBytes(relativePath), writable: false);

    public IEnumerable<string> EnumerateFiles(string relativeDir, string pattern)
    {
        string prefix = Normalize(relativeDir);
        if (!prefix.EndsWith('/')) prefix += '/';
        string ext = Path.GetExtension(pattern);
        string namePrefix = pattern.Contains('*') ? pattern[..pattern.IndexOf('*')] : pattern;

        return _lookup.Keys.Where(k =>
        {
            if (!k.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) return false;
            string remainder = k[prefix.Length..];
            if (remainder.Contains('/')) return false;
            if (!string.IsNullOrEmpty(namePrefix) && !remainder.StartsWith(namePrefix, StringComparison.OrdinalIgnoreCase)) return false;
            if (!string.IsNullOrEmpty(ext) && !remainder.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) return false;
            return true;
        });
    }

    public IEnumerable<string> EnumerateDirectories(string relativeDir)
    {
        string prefix = Normalize(relativeDir);
        if (!prefix.EndsWith('/')) prefix += '/';
        HashSet<string> dirs = new(StringComparer.OrdinalIgnoreCase);
        foreach (string key in _lookup.Keys)
        {
            if (!key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) continue;
            string remainder = key[prefix.Length..];
            int sep = remainder.IndexOf('/');
            if (sep > 0) dirs.Add(prefix + remainder[..sep]);
        }
        return dirs;
    }

    private static string Normalize(string path) => path.Replace('\\', '/');
}
