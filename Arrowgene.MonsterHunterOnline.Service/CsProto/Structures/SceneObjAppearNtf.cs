using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// sceneobj appear notify
    /// </summary>
    public class SceneObjAppearNtf : Structure, ICsStructure
    {
        public SceneObjAppearNtf()
        {
            NetId = 0;
            EntityName = "";
            ClassName = "";
            Pose = new CSQuatT();
            SubTypeId = 0;
            Sync2CE = 0;
            SpawnType = 0;
            Bone = 0;
            Holder = 0;
            Owner = 0;
            Faction = 0;
            RegionId = 0;
            UsrData = new List<byte>();
            EntGuid = 0;
            PropertityFile = "";
            MHSpawnType = 0;
            BTState = "";
            BBVars = new CSBBVarList();
            Buff = new List<byte>();
            ParentId = 0;
            ParentGuid = 0;
        }

        /// <summary>
        /// logic entity id
        /// </summary>
        public uint NetId { get; set; }

        /// <summary>
        /// sceneobj LL name
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// sceneobj ce class name
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Appear location
        /// </summary>
        public CSQuatT Pose { get; set; }

        /// <summary>
        /// scene obj type id, defined by each scene obj
        /// </summary>
        public int SubTypeId { get; set; }

        /// <summary>
        /// 是否同步生成到CE,0为否,其他为需要
        /// </summary>
        public byte Sync2CE { get; set; }

        /// <summary>
        /// Spawn类型,0绝对位置,1相对骨骼位置
        /// </summary>
        public byte SpawnType { get; set; }

        public int Bone { get; set; }

        /// <summary>
        /// 依附物体NetID
        /// </summary>
        public uint Holder { get; set; }

        /// <summary>
        /// 物体OwnerNetID
        /// </summary>
        public uint Owner { get; set; }

        /// <summary>
        /// 阵营
        /// </summary>
        public int Faction { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public List<byte> UsrData { get; }

        /// <summary>
        /// entity guid
        /// </summary>
        public ulong EntGuid { get; set; }

        /// <summary>
        /// 属性文件的名字
        /// </summary>
        public string PropertityFile { get; set; }

        /// <summary>
        /// SpawnType
        /// </summary>
        public short MHSpawnType { get; set; }

        /// <summary>
        /// BT state
        /// </summary>
        public string BTState { get; set; }

        /// <summary>
        /// Blackboard vars
        /// </summary>
        public CSBBVarList BBVars { get; set; }

        /// <summary>
        /// Buff数据
        /// </summary>
        public List<byte> Buff { get; }

        /// <summary>
        /// parent entityid
        /// </summary>
        public uint ParentId { get; set; }

        /// <summary>
        /// parent entityGUID
        /// </summary>
        public ulong ParentGuid { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteUInt32(buffer, NetId);
            WriteString(buffer, EntityName);
            WriteString(buffer, ClassName);
            WriteCsStructure(buffer, Pose);
            WriteInt32(buffer, SubTypeId);
            WriteByte(buffer, Sync2CE);
            WriteByte(buffer, SpawnType);
            WriteInt32(buffer, Bone);
            WriteUInt32(buffer, Holder);
            WriteUInt32(buffer, Owner);
            WriteInt32(buffer, Faction);
            WriteInt32(buffer, RegionId);
            WriteList(buffer, UsrData, CsProtoConstant.CS_MAX_SO_USER_DATA_LEN, WriteInt32, WriteByte);
            WriteUInt64(buffer, EntGuid);
            WriteString(buffer, PropertityFile);
            WriteInt16(buffer, MHSpawnType);
            WriteString(buffer, BTState);
            WriteCsStructure(buffer, BBVars);
            WriteList(buffer, Buff, (ushort)CsProtoConstant.CS_MAX_BUFF_DATA_LEN, WriteUInt16, WriteByte);
            WriteUInt32(buffer, ParentId);
            WriteUInt64(buffer, ParentGuid);
        }

        public void ReadCs(IBuffer buffer)
        {
            NetId = ReadUInt32(buffer);
            EntityName = ReadString(buffer);
            ClassName = ReadString(buffer);
            Pose = ReadCsStructure(buffer, Pose);
            SubTypeId = ReadInt32(buffer);
            Sync2CE = ReadByte(buffer);
            SpawnType = ReadByte(buffer);
            Bone = ReadInt32(buffer);
            Holder = ReadUInt32(buffer);
            Owner = ReadUInt32(buffer);
            Faction = ReadInt32(buffer);
            RegionId = ReadInt32(buffer);
            ReadList(buffer, UsrData, CsProtoConstant.CS_MAX_SO_USER_DATA_LEN, ReadInt32, ReadByte);
            EntGuid = ReadUInt64(buffer);
            PropertityFile = ReadString(buffer);
            MHSpawnType = ReadInt16(buffer);
            BTState = ReadString(buffer);
            BBVars = ReadCsStructure(buffer, BBVars);
            ReadList(buffer, Buff, (ushort)CsProtoConstant.CS_MAX_BUFF_DATA_LEN, ReadUInt16, ReadByte);
            ParentId = ReadUInt32(buffer);
            ParentGuid = ReadUInt64(buffer);
        }
    }
}