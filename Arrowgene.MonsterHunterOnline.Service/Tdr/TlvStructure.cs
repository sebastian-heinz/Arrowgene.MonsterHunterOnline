using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr;

public abstract class TlvStructure
{
    public abstract void Write(IBuffer buffer);
    public abstract void Read(IBuffer buffer);

    protected void WriteTlvStructure(IBuffer buffer, TlvStructure val)
    {
        val.Write(buffer);
    }

    protected void WriteTlvTag(IBuffer buffer, int id, TlvType type)
    {
        buffer.WriteTlvTag(id, type);
    }

    protected void WriteTlvByte(IBuffer buffer, int id, byte val)
    {
        WriteTlvTag(buffer, id, TlvType.ID_1_BYTE);
        WriteByte(buffer, val);
    }

    protected void WriteTlvInt16(IBuffer buffer, int id, short val)
    {
        WriteTlvTag(buffer, id, TlvType.ID_2_BYTE);
        WriteInt16(buffer, val);
    }

    protected void WriteTlvInt32(IBuffer buffer, int id, int val)
    {
        WriteTlvTag(buffer, id, TlvType.ID_4_BYTE);
        WriteInt32(buffer, val);
    }

    protected void WriteTlvInt64(IBuffer buffer, int id, long val)
    {
        WriteTlvTag(buffer, id, TlvType.ID_8_BYTE);
        WriteInt64(buffer, val);
    }

    protected void WriteTlvInt32Arr(IBuffer buffer, int id, int[] val)
    {
        WriteTlvTag(buffer, id, TlvType.ID_4_BYTE);
        int count = val.Length;
        WriteInt32(buffer, count * 4);
        for (int i = 0; i < count; i++)
        {
            WriteInt32(buffer, val[i]);
        }
    }

    protected void WriteByte(IBuffer buffer, byte val)
    {
        buffer.WriteByte(val);
    }

    protected void WriteInt16(IBuffer buffer, int val)
    {
        buffer.WriteInt32(val, Endianness.Big);
    }

    protected int ReadInt32(IBuffer buffer)
    {
        return buffer.ReadInt32(Endianness.Big);
    }

    protected void WriteInt32(IBuffer buffer, int val)
    {
        buffer.WriteInt32(val, Endianness.Big);
    }

    protected void WriteInt64(IBuffer buffer, long val)
    {
        buffer.WriteInt64(val, Endianness.Big);
    }
}