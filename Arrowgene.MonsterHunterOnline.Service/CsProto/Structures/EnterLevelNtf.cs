using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class EnterLevelNtf : Structure
{
    public EnterLevelNtf()
    {
        Reserver = 0;
    }

    public int Reserver { get; set; }

    public override void Write(IBuffer buffer)
    {
        WriteInt32(buffer, Reserver);
    }

    public override void Read(IBuffer buffer)
    {
        Reserver = ReadInt32(buffer);
    }
}