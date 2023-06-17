using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class RemoteDataInitInfo : IRemoteDataInfo
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(RemoteDataInitInfo));

    /// <summary>
    /// 数据长度
    /// </summary>
    public const uint CS_MAX_ROMTE_DATA_LEN = 61440;

    public RemoteDataInitInfo(RemoteDataType remoteDataType)
    {
        DataType = remoteDataType;
        DataPacket = new byte[0];
    }

    /// <summary>
    /// 数据内容
    /// </summary>
    public byte[] DataPacket { get; set; }


    public void Write(IBuffer buffer)
    {
        if (DataPacket.Length > CS_MAX_ROMTE_DATA_LEN)
        {
            buffer.WriteInt32(0, Endianness.Big);
            Logger.Error(
                $"DataPacket length, larger than maximum allowed. (len:{DataPacket.Length} > max:{CS_MAX_ROMTE_DATA_LEN}) ");
            return;
        }

        buffer.WriteInt32(DataPacket.Length, Endianness.Big);
        buffer.WriteBytes(DataPacket);
    }

    public RemoteDataType DataType { get; }
}