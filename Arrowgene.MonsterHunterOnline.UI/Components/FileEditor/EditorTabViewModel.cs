using System.Text;
using Arrowgene.MonsterHunterOnline.ClientTools;
using Arrowgene.MonsterHunterOnline.UI.ViewModels;

namespace Arrowgene.MonsterHunterOnline.UI.Components;

public sealed class EditorTabViewModel : ViewModelBase
{
    private string _content = string.Empty;
    private bool _isModified;

    public EditorTabViewModel(EditableFile file, string content)
    {
        File = file;
        _content = content;
    }

    public EditableFile File { get; }

    public string TabHeader => IsModified ? $"{File.FileName} *" : File.FileName;

    public string Content
    {
        get => _content;
        set
        {
            if (SetProperty(ref _content, value))
            {
                IsModified = true;
            }
        }
    }

    public bool IsModified
    {
        get => _isModified;
        set
        {
            if (SetProperty(ref _isModified, value))
            {
                OnPropertyChanged(nameof(TabHeader));
            }
        }
    }

    public byte[] GetSaveBytes()
    {
        byte[] textBytes = Encoding.UTF8.GetBytes(Content);
        if (File.XmlFormat is MhoCryXmlFormat.EncryptedXml or MhoCryXmlFormat.EncryptedCryXmlBinary)
        {
            return MhoCryXmlCodec.Encrypt(textBytes);
        }

        return textBytes;
    }
}
