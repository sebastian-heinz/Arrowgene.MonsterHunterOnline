using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    public class RemoteDataInitInfo : Structure, CSICsRemoteDataInfo
    {
        public RemoteDataInitInfo(ITlvStructure remoteData, ROMTE_DATA_TYPE dataType)
        {
            DataPacket = remoteData;
            DataType = dataType;
        }

        public ROMTE_DATA_TYPE DataType { get; }

        /// <summary>
        /// 数据内容
        /// </summary>
        public ITlvStructure DataPacket { get; }

        public void WriteCs(IBuffer buffer)
        {
            WriteTlvStructure(buffer, DataPacket, CsProtoConstant.CS_MAX_ROMTE_DATA_LEN, WriteInt32);
        }

        public void ReadCs(IBuffer buffer)
        {
            // todo switch on type and create instance
            ReadTlvStructure(buffer, DataPacket, CsProtoConstant.CS_MAX_ROMTE_DATA_LEN, ReadInt32);
        }
    }
}