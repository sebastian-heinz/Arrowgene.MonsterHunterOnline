using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    public class RemoteDataInitInfo : Structure, IRemoteDataInfo
    {
        public RemoteDataInitInfo(ROMTE_DATA_TYPE dataType)
        {
            DataPacket = new List<byte>();
            DataType = dataType;
        }

        public ROMTE_DATA_TYPE DataType { get; }

        /// <summary>
        /// 数据内容
        /// </summary>
        public List<byte> DataPacket { get; }

        public override void Write(IBuffer buffer)
        {
            WriteList(buffer, DataPacket, CsProtoConstant.CS_MAX_ROMTE_DATA_LEN, WriteInt32, WriteByte);
        }

        public override void Read(IBuffer buffer)
        {
            ReadList(buffer, DataPacket, CsProtoConstant.CS_MAX_ROMTE_DATA_LEN, ReadInt32, ReadByte);
        }
    }
}