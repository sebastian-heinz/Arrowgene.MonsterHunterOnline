using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    public class RemoteDataLoadReq : Structure
    {
        public RemoteDataLoadReq()
        {
            RemoteDataType = 0;
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        public ROMTE_DATA_TYPE RemoteDataType { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteUInt16(buffer, (ushort)RemoteDataType);
        }

        public override void Read(IBuffer buffer)
        {
            RemoteDataType = (ROMTE_DATA_TYPE)ReadUInt16(buffer);
        }
    }
}