using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class AttrSync : Structure, ICsStructure
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

    public  void WriteCs(IBuffer buffer)
    {
        WriteUInt32(buffer, EntityId);
        WriteUInt32(buffer, AttrId);
        WriteInt16(buffer, BonusId);
        WriteCsStructure(buffer, Data);
    }

    public void ReadCs(IBuffer buffer)
    {
        EntityId = ReadUInt32(buffer);
        AttrId = ReadUInt32(buffer);
        BonusId = ReadInt16(buffer);
        Data = ReadCsStructure(buffer, Data);
    }
}