using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 客户端去拉根据拉Entity信息
    /// </summary>
    public class LoadEntityReq : Structure, ICsStructure
    {
        public LoadEntityReq()
        {
            // TODO perhaps these lists can be combined to a single `LogicEntityId`-type list
            LogicEntityId = new List<uint>();
            LogicEntityType = new List<uint>();
        }

        public List<uint> LogicEntityId { get; }
        public List<uint> LogicEntityType { get; }

        public  void WriteCs(IBuffer buffer)
        {
            int count = Math.Min(LogicEntityId.Count, LogicEntityType.Count);
            WriteInt32(buffer, count);
            WriteList(buffer, LogicEntityId, count, CsProtoConstant.CS_MAX_APPEAR_ID_NTF_NUM, WriteUInt32);
            WriteList(buffer, LogicEntityType, count, CsProtoConstant.CS_MAX_APPEAR_ID_NTF_NUM, WriteUInt32);
        }

        public void ReadCs(IBuffer buffer)
        {
            int count = ReadInt32(buffer);
            ReadList(buffer, LogicEntityId, count, CsProtoConstant.CS_MAX_APPEAR_ID_NTF_NUM, ReadUInt32);
            ReadList(buffer, LogicEntityType, count, CsProtoConstant.CS_MAX_APPEAR_ID_NTF_NUM, ReadUInt32);
        }
    }
}