using Arrowgene.MonsterHunterOnline.ClientTools;

namespace Arrowgene.MonsterHunterOnline.UI.Components;

/// <summary>
/// Self-contained file container that carries all metadata needed for editing
/// and saving back to the source archive. Decouples the editor from the archive explorer.
/// </summary>
public sealed class EditableFile
{
    public required string FileName { get; init; }
    public required string ArchivePath { get; init; }
    public required byte[] Data { get; init; }
    public MhoCryXmlFormat? XmlFormat { get; init; }

    /// <summary>
    /// When set, this file lives inside a SWF container at <see cref="ArchivePath"/>.
    /// The value is the tag index within the SWF.
    /// </summary>
    public int? SwfTagIndex { get; init; }

    public bool IsInsideSwf => SwfTagIndex.HasValue;
}
