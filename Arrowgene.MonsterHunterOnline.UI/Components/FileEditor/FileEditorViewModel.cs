using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Arrowgene.MonsterHunterOnline.ClientTools;
using Arrowgene.MonsterHunterOnline.UI.ViewModels;

namespace Arrowgene.MonsterHunterOnline.UI.Components;

public sealed class FileEditorViewModel : ViewModelBase
{
    private int _selectedTabIndex;
    private string _statusText = "No files open.";

    public ObservableCollection<EditorTabViewModel> Tabs { get; } = [];

    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetProperty(ref _selectedTabIndex, value);
    }

    public bool HasTabs => Tabs.Count > 0;

    public string StatusText
    {
        get => _statusText;
        set => SetProperty(ref _statusText, value);
    }

    public void OpenFile(EditableFile file)
    {
        EditorTabViewModel? existing = Tabs.FirstOrDefault(
            t => string.Equals(t.File.ArchivePath, file.ArchivePath, StringComparison.OrdinalIgnoreCase));

        if (existing != null)
        {
            SelectedTabIndex = Tabs.IndexOf(existing);
            return;
        }

        string content;
        try
        {
            if (file.XmlFormat is not null and not MhoCryXmlFormat.NotXml)
            {
                content = MhoCryXmlCodec.ReadXml(file.Data);
            }
            else
            {
                content = Encoding.UTF8.GetString(file.Data);
            }
        }
        catch (Exception ex)
        {
            StatusText = $"Cannot open {file.FileName}: {ex.Message}";
            return;
        }

        EditorTabViewModel tab = new(file, content);
        Tabs.Add(tab);
        OnPropertyChanged(nameof(HasTabs));
        SelectedTabIndex = Tabs.Count - 1;
        StatusText = $"Opened {file.FileName}.";
    }

    public void CloseTab(EditorTabViewModel tab)
    {
        int index = Tabs.IndexOf(tab);
        if (index < 0)
        {
            return;
        }

        Tabs.RemoveAt(index);
        OnPropertyChanged(nameof(HasTabs));

        if (Tabs.Count == 0)
        {
            SelectedTabIndex = 0;
            StatusText = "No files open.";
        }
        else if (SelectedTabIndex >= Tabs.Count)
        {
            SelectedTabIndex = Tabs.Count - 1;
        }
    }
}
