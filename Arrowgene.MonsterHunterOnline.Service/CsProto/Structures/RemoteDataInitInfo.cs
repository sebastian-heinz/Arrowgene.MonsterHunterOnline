using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    public class RemoteDataInitInfo : Structure, ICsStructure, CSICsRemoteDataInfo
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

        public  void WriteCs(IBuffer buffer)
        {
            WriteList(buffer, DataPacket, CsProtoConstant.CS_MAX_ROMTE_DATA_LEN, WriteInt32, WriteByte);
        }

        public void ReadCs(IBuffer buffer)
        {
            ReadList(buffer, DataPacket, CsProtoConstant.CS_MAX_ROMTE_DATA_LEN, ReadInt32, ReadByte);
        }
    }
}