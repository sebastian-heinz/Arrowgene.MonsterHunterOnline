using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

public class DataLoadRsp : CsPacket
{
    public override CsProtoCmd Cmd => CsProtoCmd.CS_CMD_DATA_LOAD_RSP;

    public DataLoadRsp(IRemoteDataInfo remoteDataInfo)
    {
        RemoteDataInfo = remoteDataInfo;
        DataType = remoteDataInfo.DataType;
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public RemoteDataType DataType { get; }

    /// <summary>
    /// 数据
    /// </summary>
    public IRemoteDataInfo RemoteDataInfo { get; }

    public override void Write(IBuffer buffer)
    {
        buffer.WriteUInt16((ushort)DataType, Endianness.Big);
        RemoteDataInfo.Write(buffer);
    }
}