using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class AttrSync : Structure
{
    public AttrSync()
    {
        EntityId = 0;
        AttrId = 0;
        BonusId = 0;
        Data = new AttrBaseData();
    }

    public uint EntityId { get; set; }
    public uint AttrId { get; set; }
    public short BonusId { get; set; }
    public AttrBaseData Data { get; set; }

    public override void Write(IBuffer buffer)
    {
        WriteUInt32(buffer, EntityId);
        WriteUInt32(buffer, AttrId);
        WriteInt16(buffer, BonusId);
        WriteStructure(buffer, Data);
    }

    public override void Read(IBuffer buffer)
    {
        EntityId = ReadUInt32(buffer);
        AttrId = ReadUInt32(buffer);
        BonusId = ReadInt16(buffer);
        Data = ReadStructure(buffer, Data);
    }
}