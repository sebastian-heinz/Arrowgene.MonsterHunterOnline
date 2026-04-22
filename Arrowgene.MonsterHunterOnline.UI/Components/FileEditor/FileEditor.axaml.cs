using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Arrowgene.MonsterHunterOnline.UI.Components;

public sealed class SaveFileRequestEventArgs : EventArgs
{
    public SaveFileRequestEventArgs(EditableFile file, byte[] data)
    {
        File = file;
        Data = data;
    }

    public EditableFile File { get; }
    public byte[] Data { get; }
}

public partial class FileEditor : UserControl
{
    public FileEditor()
    {
        InitializeComponent();
        DataContext = new FileEditorViewModel();
    }

    public FileEditorViewModel ViewModel => (FileEditorViewModel)DataContext!;

    public event EventHandler<SaveFileRequestEventArgs>? SaveFileRequested;

    public void OpenFile(EditableFile file)
    {
        ViewModel.OpenFile(file);
    }

    public void MarkTabSaved(EditorTabViewModel tab)
    {
        tab.IsModified = false;
        ViewModel.StatusText = $"Saved {tab.File.FileName}.";
    }

    private void CloseTabClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is EditorTabViewModel tab)
        {
            ViewModel.CloseTab(tab);
        }
    }

    private void SaveTabClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is EditorTabViewModel tab)
        {
            byte[] data = tab.GetSaveBytes();
            SaveFileRequested?.Invoke(this, new SaveFileRequestEventArgs(tab.File, data));
        }
    }
}
