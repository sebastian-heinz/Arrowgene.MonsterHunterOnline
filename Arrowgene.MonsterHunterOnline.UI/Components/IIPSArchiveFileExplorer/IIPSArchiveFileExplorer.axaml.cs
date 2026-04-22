using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Arrowgene.MonsterHunterOnline.UI.Infrastructure;

namespace Arrowgene.MonsterHunterOnline.UI.Components;

public sealed class EditFileRequestEventArgs : EventArgs
{
    public EditFileRequestEventArgs(EditableFile file)
    {
        File = file;
    }

    public EditableFile File { get; }
}

public partial class IIPSArchiveFileExplorer : UserControl
{
    public IIPSArchiveFileExplorer()
    {
        InitializeComponent();
        IIPSArchiveFileExplorerViewModel vm = new IIPSArchiveFileExplorerViewModel();
        DataContext = vm;
        vm.PropertyChanged += OnViewModelPropertyChanged;
        AddHandler(Avalonia.Input.InputElement.KeyDownEvent, OnSwfKeyDown, Avalonia.Interactivity.RoutingStrategies.Tunnel);
    }

    private IIPSArchiveFileExplorerViewModel ViewModel => (IIPSArchiveFileExplorerViewModel)DataContext!;

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IIPSArchiveFileExplorerViewModel.TablePreviewHeaders))
        {
            RebuildCsvDataGridColumns();
        }

        if (e.PropertyName == nameof(IIPSArchiveFileExplorerViewModel.DatPreviewSheets) ||
            e.PropertyName == nameof(IIPSArchiveFileExplorerViewModel.DatSelectedSheetIndex))
        {
            RebuildDatDataGrid();
        }
    }

    private void ToggleHexModeClick(object? sender, RoutedEventArgs e)
    {
        ViewModel.ToggleHexMode();
    }

    private void RebuildCsvDataGridColumns()
    {
        DataGrid grid = CsvPreviewDataGrid;
        grid.Columns.Clear();

        string[] headers = ViewModel.TablePreviewHeaders;
        for (int i = 0; i < headers.Length; i++)
        {
            grid.Columns.Add(new DataGridTextColumn
            {
                Header = headers[i],
                Binding = new Binding($"[{i}]"),
                MaxWidth = 200,
            });
        }

        grid.ItemsSource = ViewModel.TablePreviewRows;
    }

    private void RebuildDatDataGrid()
    {
        List<DatSheetViewModel> sheets = ViewModel.DatPreviewSheets;
        int index = ViewModel.DatSelectedSheetIndex;
        if (sheets.Count == 0 || index < 0 || index >= sheets.Count)
        {
            return;
        }

        // The DataGrid lives inside the TabControl's content template.
        // After the TabControl updates, walk the visual tree to find it.
        DatPreviewTabControl.UpdateLayout();
        DataGrid? grid = FindDatSheetDataGrid(DatPreviewTabControl);
        if (grid == null)
        {
            return;
        }

        DatSheetViewModel sheet = sheets[index];
        grid.Columns.Clear();
        for (int i = 0; i < sheet.Headers.Length; i++)
        {
            grid.Columns.Add(new DataGridTextColumn
            {
                Header = sheet.Headers[i],
                Binding = new Binding($"[{i}]"),
                MaxWidth = 200,
            });
        }

        grid.ItemsSource = sheet.Rows;
    }

    private static DataGrid? FindDatSheetDataGrid(Control root)
    {
        if (root is DataGrid dg && dg.Name == "DatSheetDataGrid")
        {
            return dg;
        }

        if (root is Avalonia.Visual visual)
        {
            int count = Avalonia.VisualTree.VisualExtensions.GetVisualChildren(visual).Count();
            foreach (Avalonia.Visual child in Avalonia.VisualTree.VisualExtensions.GetVisualChildren(visual))
            {
                if (child is Control childControl)
                {
                    DataGrid? found = FindDatSheetDataGrid(childControl);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }
        }

        return null;
    }

    private void SaveArchiveClick(object? sender, RoutedEventArgs e)
    {
        ViewModel.TrySaveArchive();
    }

    private void NavigateBackClick(object? sender, RoutedEventArgs e)
    {
        ViewModel.NavigateBackFromSwf();
    }

    private void BrowseSwfClick(object? sender, RoutedEventArgs e)
    {
        ViewModel.TryNavigateIntoSwf(ViewModel.SelectedNode);
    }

    private void EditFileClick(object? sender, RoutedEventArgs e)
    {
        if (ViewModel.SelectedNode == null)
        {
            return;
        }

        byte[]? data = ViewModel.ReadSelectedNodeBytes();
        if (data == null)
        {
            return;
        }

        string filePath = ViewModel.SelectedNode.DisplayPath ?? ViewModel.SelectedNode.Name;
        string ext = System.IO.Path.GetExtension(filePath);

        ClientTools.MhoCryXmlFormat? xmlFormat = null;
        if (string.Equals(ext, ".xml", System.StringComparison.OrdinalIgnoreCase) &&
            ClientTools.MhoCryXmlCodec.IsCryXmlCodex(data))
        {
            xmlFormat = ClientTools.MhoCryXmlCodec.DetectFormat(data);
        }

        EditableFile file = new()
        {
            FileName = System.IO.Path.GetFileName(filePath),
            ArchivePath = ViewModel.SelectedNode.SwfArchivePath ?? filePath,
            Data = data,
            XmlFormat = xmlFormat,
            SwfTagIndex = ViewModel.SelectedNode.SwfTagIndex,
        };

        EditFileRequested?.Invoke(this, new EditFileRequestEventArgs(file));
    }

    public event EventHandler<EditFileRequestEventArgs>? EditFileRequested;

    private void FileListDoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        ViewModel.TryNavigateIntoSwf(ViewModel.SelectedNode);
    }

    private void OnSwfKeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Escape && ViewModel.IsInsideSwf)
        {
            ViewModel.NavigateBackFromSwf();
            e.Handled = true;
        }
    }

    private void HelpClick(object? sender, RoutedEventArgs e)
    {
        Window? owner = TopLevel.GetTopLevel(this) as Window;
        if (owner == null)
        {
            return;
        }

        const string help = """
            IIPS Archive Explorer
            =====================

            Opening Archives
            -----------------
            Use the toolbar to open a single .ifs archive, an IIPSFileList.lst
            (merges all referenced archives), or a loose directory.

            Browsing Files
            ---------------
            The file list shows all entries sorted by archive path.
            Use the filter box to search by file name or path.
            Select a file to view its metadata and preview.

            Preview
            --------
            The preview panel shows content based on file type:
              - Images: PNG, JPG, GIF, BMP, DDS, TIFF
              - Text: TXT, JSON, XML (including CryXmlB), INI, CFG
              - Lua: compiled Lua is decompiled automatically
              - CSV/TSV: shown as a table
              - DAT: MHO data tables with sheet tabs
              - Hex: toggle with the hex button for any file

            SWF Files
            ----------
            Select a .swf file and click "Browse SWF Contents" to
            navigate inside. The SWF's assets are listed as files:

              - First item is a rendered preview of the first frame
              - Image tags (JPEG, lossless bitmaps) preview as images
              - ABC bytecode, shapes, sprites show as hex
              - All assets can be extracted and edited

            Navigation:
              - Click "Files" in the breadcrumb to go back
              - Press Escape to go back
              - Double-click a .swf to open it directly

            File Editor
            ------------
            Select a text file and click "Edit File" to open it in the
            File Editor tab. Supported file types:
              - Text: TXT, JSON, INI, CFG, CSV, AS (ActionScript)
              - XML: plain, CryXmlB, and encrypted XML are decoded
                     automatically and re-encrypted on save
              - Lua: source .lua files

            The editor opens files in tabs. Each tab shows:
              - The full archive path
              - A monospace text editor
              - A Save button (enabled when modified)
              - Tab header shows * when unsaved changes exist

            Saving edits:
              1. Edit the file content in the editor
              2. Click Save on the editor tab
              3. Changes are written to the archive in memory
              4. Return to the Archive Explorer and click Save
                 (toolbar) to write the .ifs file to disk

            Files inside SWF archives are also editable. The save
            flow automatically rebuilds the SWF container with the
            modified tag and writes it back to the IIPS archive.

            Encrypted XML files are re-encrypted in the original
            format when saved. CryXmlB files are saved as plain XML.

            Extracting
            -----------
            "Extract" saves the selected file or folder to disk.
            "Extract All" saves every entry in the archive.

            Editing Archive (single archive mode)
            --------------------------------------
            Add:     insert a new file into the archive
            Modify:  replace the selected file's data
            Remove:  delete entries from the archive
            Save:    write changes back to the .ifs file

            Changes are local until you press Save.
            """;

        HelpDialog dialog = new HelpDialog("Archive Explorer Help", help);
        dialog.ShowDialog(owner);
    }

    private void ClearFilterClick(object? sender, RoutedEventArgs e)
    {
        ViewModel.FilterText = string.Empty;
    }

    private async void AddFileClick(object? sender, RoutedEventArgs e)
    {
        string? sourcePath = await PickFileAsync("Choose a local file to add");
        if (string.IsNullOrEmpty(sourcePath))
        {
            return;
        }

        Window? owner = TopLevel.GetTopLevel(this) as Window;
        if (owner == null)
        {
            return;
        }

        TextInputDialog dialog = new TextInputDialog(
            "Add File",
            "Archive path",
            ViewModel.SuggestArchivePath(sourcePath),
            "Add");

        string? archivePath = await dialog.ShowDialog<string?>(owner);
        if (!string.IsNullOrEmpty(archivePath))
        {
            ViewModel.TryAddFile(sourcePath, archivePath);
        }
    }

    private async void ModifySelectionClick(object? sender, RoutedEventArgs e)
    {
        string? sourcePath = await PickFileAsync("Choose replacement file");
        if (!string.IsNullOrEmpty(sourcePath))
        {
            ViewModel.TryModifySelection(sourcePath);
        }
    }

    private async void RemoveSelectionClick(object? sender, RoutedEventArgs e)
    {
        Window? owner = TopLevel.GetTopLevel(this) as Window;
        if (owner == null || ViewModel.SelectedNode == null)
        {
            return;
        }

        int selectedCount = ViewModel.SelectedNode.EnumerateFileNodes().Count(static node => node.Entry != null);
        if (selectedCount == 0)
        {
            return;
        }

        string subject = selectedCount == 1 ? "1 archive entry" : $"{selectedCount} archive entries";
        ConfirmationDialog dialog = new ConfirmationDialog(
            "Remove Selection",
            $"Remove {subject} from the archive? Changes stay local until you save.",
            "Remove");

        bool confirmed = await dialog.ShowDialog<bool>(owner);
        if (confirmed)
        {
            ViewModel.TryRemoveSelection();
        }
    }

    private async void ExtractSelectionClick(object? sender, RoutedEventArgs e)
    {
        string? path = await PickFolderAsync("Extract selected archive entry");
        if (!string.IsNullOrEmpty(path))
        {
            ViewModel.TryExtractSelection(path);
        }
    }

    private async void ExtractAllClick(object? sender, RoutedEventArgs e)
    {
        string? path = await PickFolderAsync("Extract archive");
        if (!string.IsNullOrEmpty(path))
        {
            ViewModel.TryExtractAll(path);
        }
    }

    private async System.Threading.Tasks.Task<string?> PickFileAsync(string title, IReadOnlyList<FilePickerFileType>? fileTypes = null)
    {
        if (OperatingSystem.IsMacOS())
        {
            return await MacNativePicker.PickFileAsync(title);
        }

        TopLevel? topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider == null)
        {
            return null;
        }

        FilePickerOpenOptions options = new FilePickerOpenOptions
        {
            Title = title,
            AllowMultiple = false
        };

        if (fileTypes is { Count: > 0 })
        {
            options.FileTypeFilter = fileTypes;
        }

        IReadOnlyList<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(options);

        return files.Count == 0 ? null : files[0].TryGetLocalPath();
    }

    private async System.Threading.Tasks.Task<string?> PickFolderAsync(string title)
    {
        if (OperatingSystem.IsMacOS())
        {
            return await MacNativePicker.PickFolderAsync(title);
        }

        TopLevel? topLevel = TopLevel.GetTopLevel(this);
        if (topLevel?.StorageProvider == null)
        {
            return null;
        }

        IReadOnlyList<IStorageFolder> folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = title,
            AllowMultiple = false
        });

        return folders.Count == 0 ? null : folders[0].TryGetLocalPath();
    }

}
