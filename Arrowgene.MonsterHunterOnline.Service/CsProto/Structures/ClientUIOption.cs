using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

/// <summary>
/// 客户端UI自定义信息
/// </summary>
public class ClientUIOption : IStructure
{
    public ClientUIOption()
    {
        OptionData = new List<byte>();
    }

    /// <summary>
    /// 实际数据
    /// </summary>
    public List<byte> OptionData { get; }


    public void Write(IBuffer buffer)
    {
        int optionDataSize = OptionData.Count;
        buffer.WriteInt32(optionDataSize, Endianness.Big);
        for (int i = 0; i < optionDataSize; i++)
        {
            buffer.WriteByte(OptionData[i]);
        }
    }
}