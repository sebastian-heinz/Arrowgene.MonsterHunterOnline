using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// monster appear notify
    /// </summary>
    public class MonsterAppearNtf : Structure, ICsStructure
    {
        public MonsterAppearNtf()
        {
            NetId = 0;
            SpawnType = 0;
            MonsterInfoId = 0;
            EntGuid = 0;
            Name = "";
            Class = "";
            Pose = new CSQuatT();
            Faction = 0;
            BTState = "";
            BBVars = new CSBBVarList();
            Dead = 0;
            LcmState = new CSMonsterLocomotion();
            AttrInit = new List<CSAttrData>();
            ProjIds = new List<CSAmmoInfo>();
            Buff = new List<byte>();
            ParentGuid = 0;
            LastChildId = 0;
        }

        /// <summary>
        /// logic entity id
        /// </summary>
        public int NetId { get; set; }

        /// <summary>
        /// SpawnType
        /// </summary>
        public short SpawnType { get; set; }

        /// <summary>
        /// monster static data id
        /// </summary>
        public int MonsterInfoId { get; set; }

        /// <summary>
        /// entity guid
        /// </summary>
        public ulong EntGuid { get; set; }

        /// <summary>
        /// monster name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// monster class
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Appear location
        /// </summary>
        public CSQuatT Pose { get; set; }

        /// <summary>
        /// 阵营
        /// </summary>
        public int Faction { get; set; }

        /// <summary>
        /// BT state
        /// </summary>
        public string BTState { get; set; }

        /// <summary>
        /// Blackboard vars
        /// </summary>
        public CSBBVarList BBVars { get; set; }

        /// <summary>
        /// is dead
        /// </summary>
        public byte Dead { get; set; }

        /// <summary>
        /// Locomotion state
        /// </summary>
        public CSMonsterLocomotion LcmState { get; set; }

        public List<CSAttrData> AttrInit { get; }

        public List<CSAmmoInfo> ProjIds { get; }

        /// <summary>
        /// buff数据
        /// </summary>
        public List<byte> Buff { get; }

        /// <summary>
        /// cry parent entity guid
        /// </summary>
        public ulong ParentGuid { get; set; }

        /// <summary>
        /// last child logic entity id
        /// </summary>
        public int LastChildId { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, NetId);
            WriteInt16(buffer, SpawnType);
            WriteInt32(buffer, MonsterInfoId);
            WriteUInt64(buffer, EntGuid);
            WriteString(buffer, Name);
            WriteString(buffer, Class);
            WriteCsStructure(buffer, Pose);
            WriteInt32(buffer, Faction);
            WriteString(buffer, BTState);
            WriteCsStructure(buffer, BBVars);
            WriteByte(buffer, Dead);
            WriteCsStructure(buffer, LcmState);
            WriteList(buffer, AttrInit, (short)CsProtoConstant.CS_ATTR_INIT_MAX, WriteInt16, WriteCsStructure);
            WriteList(buffer, ProjIds, CsProtoConstant.CS_MAX_AMMO_NUM, WriteInt32, WriteCsStructure);
            WriteList(buffer, Buff, (short)CsProtoConstant.CS_ATTR_INIT_MAX, WriteInt16, WriteByte);
            WriteUInt64(buffer, ParentGuid);
            WriteInt32(buffer, LastChildId);
        }

        public void ReadCs(IBuffer buffer)
        {
            NetId = ReadInt32(buffer);
            SpawnType = ReadInt16(buffer);
            MonsterInfoId = ReadInt32(buffer);
            EntGuid = ReadUInt64(buffer);
            Name = ReadString(buffer);
            Class = ReadString(buffer);
            Pose = ReadCsStructure(buffer, Pose);
            Faction = ReadInt32(buffer);
            BTState = ReadString(buffer);
            BBVars.ReadCs(buffer);
            Dead = ReadByte(buffer);
            LcmState = ReadCsStructure(buffer, LcmState);
            ReadList(buffer, AttrInit, (short)CsProtoConstant.CS_ATTR_INIT_MAX, ReadInt16, ReadCsStructure<CSAttrData>);
            ReadList(buffer, ProjIds, CsProtoConstant.CS_MAX_AMMO_NUM, ReadInt32, ReadCsStructure<CSAmmoInfo>);
            ReadList(buffer, Buff, (short)CsProtoConstant.CS_ATTR_INIT_MAX, ReadInt16, ReadByte);
            ParentGuid = ReadUInt64(buffer);
            LastChildId = ReadInt32(buffer);
        }
    }
}