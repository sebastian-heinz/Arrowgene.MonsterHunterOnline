using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Arrowgene.MonsterHunterOnline.ClientTools.IIPS;

namespace Arrowgene.MonsterHunterOnline.UI.Components;

public sealed class IIPSArchiveTreeNodeViewModel
{
    private readonly IIPSArchiveTreeNodeViewModel _sourceNode;

    private IIPSArchiveTreeNodeViewModel(string name,
        string? archivePath,
        string outputRelativePath,
        bool isDirectory,
        IIPSArchiveEntry? entry,
        IIPSArchiveTreeNodeViewModel? sourceNode = null)
    {
        Name = name;
        ArchivePath = archivePath;
        OutputRelativePath = outputRelativePath;
        IsDirectory = isDirectory;
        Entry = entry;
        _sourceNode = sourceNode ?? this;
        Children = [];
    }

    public string Name { get; }
    public string? ArchivePath { get; }
    public string OutputRelativePath { get; }
    public bool IsDirectory { get; }
    public bool IsFile => !IsDirectory;
    public IIPSArchiveEntry? Entry { get; }
    public long? FileSize { get; private init; }
    public byte[]? InlineData { get; private init; }
    public int? SwfTagIndex { get; private init; }
    public string? SwfArchivePath { get; private init; }
    public ObservableCollection<IIPSArchiveTreeNodeViewModel> Children { get; }
    public string KindLabel => IsDirectory ? "DIR" : "FILE";
    public string DisplayPath => ArchivePath ?? OutputRelativePath;
    public string ParentPath => GetParentPath(DisplayPath);
    public string SecondaryText => IsDirectory
        ? string.IsNullOrEmpty(ArchivePath) ? "Virtual archive folder" : ArchivePath
        : string.IsNullOrEmpty(ArchivePath) ? "Unnamed archive entry" : ArchivePath;
    public string SummaryText => IsDirectory ? $"{Children.Count} items" : FormatSize(Entry?.Length ?? FileSize ?? 0);

    public static IIPSArchiveTreeNodeViewModel CreateDirectory(string name, string? archivePath, string outputRelativePath)
    {
        return new IIPSArchiveTreeNodeViewModel(name, archivePath, outputRelativePath, true, null);
    }

    public static IIPSArchiveTreeNodeViewModel CreateFile(string name, string? archivePath, string outputRelativePath, IIPSArchiveEntry entry)
    {
        return new IIPSArchiveTreeNodeViewModel(name, archivePath, outputRelativePath, false, entry);
    }

    public static IIPSArchiveTreeNodeViewModel CreateFile(string name, string? archivePath, string outputRelativePath, long fileSize)
    {
        return new IIPSArchiveTreeNodeViewModel(name, archivePath, outputRelativePath, false, null) { FileSize = fileSize };
    }

    public static IIPSArchiveTreeNodeViewModel CreateFile(string name, string? archivePath, string outputRelativePath, byte[] inlineData)
    {
        return new IIPSArchiveTreeNodeViewModel(name, archivePath, outputRelativePath, false, null) { InlineData = inlineData, FileSize = inlineData.Length };
    }

    public static IIPSArchiveTreeNodeViewModel CreateSwfChild(string name, string? archivePath, string outputRelativePath, byte[] inlineData, string swfArchivePath, int swfTagIndex)
    {
        return new IIPSArchiveTreeNodeViewModel(name, archivePath, outputRelativePath, false, null) { InlineData = inlineData, FileSize = inlineData.Length, SwfArchivePath = swfArchivePath, SwfTagIndex = swfTagIndex };
    }

    public IIPSArchiveTreeNodeViewModel CloneSubtree()
    {
        IIPSArchiveTreeNodeViewModel clone = CloneShallow();
        foreach (IIPSArchiveTreeNodeViewModel child in Children)
        {
            clone.Children.Add(child.CloneSubtree());
        }

        return clone;
    }

    public IIPSArchiveTreeNodeViewModel CloneWithChildren(IEnumerable<IIPSArchiveTreeNodeViewModel> children)
    {
        IIPSArchiveTreeNodeViewModel clone = CloneShallow();
        foreach (IIPSArchiveTreeNodeViewModel child in children)
        {
            clone.Children.Add(child);
        }

        return clone;
    }

    public IEnumerable<IIPSArchiveTreeNodeViewModel> EnumerateFileNodes()
    {
        IIPSArchiveTreeNodeViewModel sourceNode = _sourceNode;
        if (!sourceNode.IsDirectory)
        {
            yield return sourceNode;
            yield break;
        }

        foreach (IIPSArchiveTreeNodeViewModel child in sourceNode.Children)
        {
            foreach (IIPSArchiveTreeNodeViewModel descendant in child.EnumerateFileNodes())
            {
                yield return descendant;
            }
        }
    }

    public void SortRecursive()
    {
        List<IIPSArchiveTreeNodeViewModel> sortedChildren = Children
            .OrderByDescending(child => child.IsDirectory)
            .ThenBy(child => child.Name, System.StringComparer.OrdinalIgnoreCase)
            .ToList();

        Children.Clear();
        foreach (IIPSArchiveTreeNodeViewModel child in sortedChildren)
        {
            child.SortRecursive();
            Children.Add(child);
        }
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

    private static string GetParentPath(string path)
    {
        int separatorIndex = path.LastIndexOf('\\');
        if (separatorIndex < 0)
        {
            return "/";
        }

        return separatorIndex == 0 ? "\\" : path[..separatorIndex];
    }

    private IIPSArchiveTreeNodeViewModel CloneShallow()
    {
        return new IIPSArchiveTreeNodeViewModel(Name, ArchivePath, OutputRelativePath, IsDirectory, Entry, _sourceNode);
    }
}
