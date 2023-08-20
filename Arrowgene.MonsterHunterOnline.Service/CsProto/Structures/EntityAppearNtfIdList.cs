using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 下发EntityID
    /// </summary>
    public class EntityAppearNtfIdList : Structure
    {
        public EntityAppearNtfIdList()
        {
            InitType = 0;
            // TODO perhaps these lists can be combined to a single `LogicEntityId`-type list
            LogicEntityId = new List<uint>();
            LogicEntityType = new List<uint>();
        }

        public int InitType { get; set; }

        public List<uint> LogicEntityId { get; }

        public List<uint> LogicEntityType { get; }

        public override void Write(IBuffer buffer)
        {
            WriteInt32(buffer, InitType);
            int count = Math.Min(LogicEntityId.Count, LogicEntityType.Count);
            WriteInt32(buffer, count);
            WriteList(buffer, LogicEntityId, count, CsProtoConstant.CS_MAX_APPEAR_ID_NTF_NUM, WriteUInt32);
            WriteList(buffer, LogicEntityType, count, CsProtoConstant.CS_MAX_APPEAR_ID_NTF_NUM, WriteUInt32);
        }

        public override void Read(IBuffer buffer)
        {
            InitType = ReadInt32(buffer);
            int count = ReadInt32(buffer);
            ReadList(buffer, LogicEntityId, count, CsProtoConstant.CS_MAX_APPEAR_ID_NTF_NUM, ReadUInt32);
            ReadList(buffer, LogicEntityType, count, CsProtoConstant.CS_MAX_APPEAR_ID_NTF_NUM, ReadUInt32);
        }
    }
}