using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class NotifyInfo : Structure
{
    public NotifyInfo()
    {
        Info = "";
    }

    public string Info { get; set; }

    public override void Write(IBuffer buffer)
    {
        WriteString(buffer, Info);
    }

    public override void Read(IBuffer buffer)
    {
        Info = ReadString(buffer);
    }
}