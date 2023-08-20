using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// Create Server Entities List
    /// </summary>
    public class SpawnSrvEntList : Structure
    {
        public SpawnSrvEntList()
        {
            InitMode = 0;
            EntList = new List<SpawnSrvEnt>();
        }

        /// <summary>
        /// Check in initialize mode
        /// </summary>
        public byte InitMode { get; set; }

        /// <summary>
        /// Spawn Server Entities List
        /// </summary>
        public List<SpawnSrvEnt> EntList { get; }

        public override void Write(IBuffer buffer)
        {
            WriteByte(buffer, InitMode);
            WriteList(buffer, EntList, (uint)CsProtoConstant.CS_MAX_ENT_NUM, WriteUInt32, WriteStructure);
        }

        public override void Read(IBuffer buffer)
        {
            InitMode = ReadByte(buffer);
            ReadList(buffer, EntList, (uint)CsProtoConstant.CS_MAX_ENT_NUM, ReadUInt32, ReadStructure<SpawnSrvEnt>);
        }
    }
}