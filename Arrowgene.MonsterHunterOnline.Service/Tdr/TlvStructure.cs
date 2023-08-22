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

    protected void WriteTdrTlvTag(IBuffer buffer, int id, TlvType type)
    {
        buffer.WriteTlvTag(id, type);
    }

    protected void WriteVarUInt32(IBuffer buffer, uint val)
    {
        buffer.WriteVarUInt32(val);
    }

    protected void WriteString(IBuffer buffer, string val)
    {
        buffer.WriteInt32(val.Length + 1, Endianness.Big);
        buffer.WriteCString(val);
    }

    protected string ReadString(IBuffer buffer)
    {
        int valLength = buffer.ReadInt32(Endianness.Big);
        string val = buffer.ReadCString();
        return val;
    }

    protected long ReadInt64(IBuffer buffer)
    {
        return buffer.ReadInt64(Endianness.Big);
    }

    protected void WriteInt64(IBuffer buffer, long val)
    {
        buffer.WriteInt64(val, Endianness.Big);
    }

    protected ulong ReadUInt64(IBuffer buffer)
    {
        return buffer.ReadUInt64(Endianness.Big);
    }

    protected void WriteUInt64(IBuffer buffer, ulong val)
    {
        buffer.WriteUInt64(val, Endianness.Big);
    }

    protected int ReadInt32(IBuffer buffer)
    {
        return buffer.ReadInt32(Endianness.Big);
    }

    protected void WriteInt32(IBuffer buffer, int val)
    {
        buffer.WriteInt32(val, Endianness.Big);
    }

    protected uint ReadUInt32(IBuffer buffer)
    {
        return buffer.ReadUInt32(Endianness.Big);
    }

    protected void WriteUInt32(IBuffer buffer, uint val)
    {
        buffer.WriteUInt32(val, Endianness.Big);
    }

    protected ushort ReadUInt16(IBuffer buffer)
    {
        return buffer.ReadUInt16(Endianness.Big);
    }

    protected void WriteUInt16(IBuffer buffer, ushort val)
    {
        buffer.WriteUInt16(val, Endianness.Big);
    }

    protected short ReadInt16(IBuffer buffer)
    {
        return buffer.ReadInt16(Endianness.Big);
    }

    protected void WriteInt16(IBuffer buffer, short val)
    {
        buffer.WriteInt16(val, Endianness.Big);
    }

    protected byte ReadByte(IBuffer buffer)
    {
        return buffer.ReadByte();
    }

    protected void WriteByte(IBuffer buffer, byte val)
    {
        buffer.WriteByte(val);
    }

    protected void WriteBool(IBuffer buffer, bool val)
    {
        buffer.WriteByte(val ? (byte)1 : (byte)0);
    }

    protected bool ReadBool(IBuffer buffer)
    {
        byte b = buffer.ReadByte();
        return b != 0;
    }
}