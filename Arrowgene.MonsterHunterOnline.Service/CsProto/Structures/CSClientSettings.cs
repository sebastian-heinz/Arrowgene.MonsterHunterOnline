using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class CSClientSettings : IStructure
{
    public CSClientSettings()
    {
        Data = new List<byte>();
    }

    public List<byte> Data { get; }


    public void Write(IBuffer buffer)
    {
        ushort dataSize = (ushort)Data.Count;
        buffer.WriteUInt16(dataSize, Endianness.Big);
        for (int i = 0; i < dataSize; i++)
        {
            buffer.WriteByte(Data[i]);
        }
    }
}