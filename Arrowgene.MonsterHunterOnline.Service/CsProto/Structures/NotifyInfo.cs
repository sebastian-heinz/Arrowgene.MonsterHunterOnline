using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class NotifyInfo : Structure, ICsStructure
{
    public NotifyInfo()
    {
        Info = "";
    }

    public string Info { get; set; }

    public  void WriteCs(IBuffer buffer)
    {
        WriteString(buffer, Info);
    }

    public void ReadCs(IBuffer buffer)
    {
        Info = ReadString(buffer);
    }
}