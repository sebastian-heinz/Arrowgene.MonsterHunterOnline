using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// FIFO同步信息
    /// </summary>
    public class FifoSyncInfo : Structure, ICsStructure
    {
        public FifoSyncInfo()
        {
            SyncTime = 0;
            Pos = new CSVec3();
            Rot = new CSQuat();
            Rot1 = new CSQuat();
            Rot2 = new CSQuat();
            Type = 0;
            Extension = 0;
            State1 = 0;
            State2 = 0;
            State3 = 0;
            State4 = 0;
            AgState = 0;
            SkillId = 0;
            SkillLevel = 0;
            Param = 0;
            ParamF = 0.0f;
            AttackRotation = 0.0f;
            Sp = 0;
            Sta = 0;
        }

        /// <summary>
        /// 同步时间
        /// </summary>
        public long SyncTime { get; set; }

        /// <summary>
        /// 当前位置
        /// current location
        /// </summary>
        public CSVec3 Pos { get; set; }

        /// <summary>
        /// 当前方向
        /// </summary>
        public CSQuat Rot { get; set; }

        /// <summary>
        /// 方向1
        /// </summary>
        public CSQuat Rot1 { get; set; }

        /// <summary>
        /// 方向2
        /// </summary>
        public CSQuat Rot2 { get; set; }

        /// <summary>
        /// 队列种类
        /// queue type
        /// </summary>
        public byte Type { get; set; }

        /// <summary>
        /// 额外信息
        /// </summary>
        public byte Extension { get; set; }

        /// <summary>
        /// 玩家Stance或者FSM状态
        /// </summary>
        public uint State1 { get; set; }

        /// <summary>
        /// 玩家Stance或者FSM状态
        /// </summary>
        public uint State2 { get; set; }

        /// <summary>
        /// 玩家Stance或者FSM状态
        /// </summary>
        public uint State3 { get; set; }

        /// <summary>
        /// 玩家Stance或者FSM状态
        /// </summary>
        public uint State4 { get; set; }

        /// <summary>
        /// 玩家AG状态
        /// </summary>
        public uint AgState { get; set; }

        /// <summary>
        /// 玩家SkillID
        /// </summary>
        public uint SkillId { get; set; }

        /// <summary>
        /// 玩家SkillLevel
        /// </summary>
        public uint SkillLevel { get; set; }

        /// <summary>
        /// 参数信息
        /// </summary>
        public int Param { get; set; }

        /// <summary>
        /// 浮点参数信息
        /// </summary>
        public float ParamF { get; set; }

        /// <summary>
        /// 攻击转向
        /// </summary>
        public float AttackRotation { get; set; }

        /// <summary>
        /// 玩家属性
        /// Player attributes
        /// </summary>
        public ushort Sp { get; set; }

        /// <summary>
        /// 玩家属性
        /// Player attributes
        /// </summary>
        public ushort Sta { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt64(buffer, SyncTime);
            WriteCsStructure(buffer, Pos);
            WriteCsStructure(buffer, Rot);
            WriteCsStructure(buffer, Rot1);
            WriteCsStructure(buffer, Rot2);
            WriteByte(buffer, Type);
            WriteByte(buffer, Extension);
            WriteUInt32(buffer, State1);
            WriteUInt32(buffer, State2);
            WriteUInt32(buffer, State3);
            WriteUInt32(buffer, State4);
            WriteUInt32(buffer, AgState);
            WriteUInt32(buffer, SkillId);
            WriteUInt32(buffer, SkillLevel);
            WriteInt32(buffer, Param);
            WriteFloat(buffer, ParamF);
            WriteFloat(buffer, AttackRotation);
            WriteUInt16(buffer, Sp);
            WriteUInt16(buffer, Sta);
        }

        public void ReadCs(IBuffer buffer)
        {
            SyncTime = ReadInt64(buffer);
            Pos = ReadCsStructure(buffer, Pos);
            Rot = ReadCsStructure(buffer, Rot);
            Rot1 = ReadCsStructure(buffer, Rot1);
            Rot2 = ReadCsStructure(buffer, Rot2);
            Type = ReadByte(buffer);
            Extension = ReadByte(buffer);
            State1 = ReadUInt32(buffer);
            State2 = ReadUInt32(buffer);
            State3 = ReadUInt32(buffer);
            State4 = ReadUInt32(buffer);
            AgState = ReadUInt32(buffer);
            SkillId = ReadUInt32(buffer);
            SkillLevel = ReadUInt32(buffer);
            Param = ReadInt32(buffer);
            ParamF = ReadFloat(buffer);
            AttackRotation = ReadFloat(buffer);
            Sp = ReadUInt16(buffer);
            Sta = ReadUInt16(buffer);
        }
    }
}