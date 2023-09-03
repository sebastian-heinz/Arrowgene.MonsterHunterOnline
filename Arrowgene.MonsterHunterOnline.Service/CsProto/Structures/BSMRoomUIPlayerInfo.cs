using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    public class BSMRoomUIPlayerInfo : Structure, ICsStructure
    {
        public BSMRoomUIPlayerInfo()
        {
            RoleIndex = 0;
            CharLevel = 0;
            Weapon = 0;
            BoxId = 0;
            RoleName = "";
            StarLevel = "";
            Faction = 0;
            Officer = 0;
            HRLevel = 0;
            BigRand = 0;
        }

        /// <summary>
        /// 角色Index
        /// </summary>
        public uint RoleIndex { get; set; }

        public int CharLevel { get; set; }

        public int Weapon { get; set; }

        /// <summary>
        /// 狩猎包ID
        /// </summary>
        public int BoxId { get; set; }

        public string RoleName { get; set; }

        /// <summary>
        /// 星级
        /// </summary>
        public string StarLevel { get; set; }

        /// <summary>
        /// 阵营
        /// </summary>
        public int Faction { get; set; }

        /// <summary>
        /// 是否教官
        /// </summary>
        public byte Officer { get; set; }

        /// <summary>
        /// HRLevel
        /// </summary>
        public int HRLevel { get; set; }

        /// <summary>
        /// 是否大随机玩家
        /// </summary>
        public byte BigRand { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteUInt32(buffer, RoleIndex);
            WriteInt32(buffer, CharLevel);
            WriteInt32(buffer, Weapon);
            WriteInt32(buffer, BoxId);
            WriteString(buffer, RoleName);
            WriteString(buffer, StarLevel);
            WriteInt32(buffer, Faction);
            WriteByte(buffer, Officer);
            WriteInt32(buffer, HRLevel);
            WriteByte(buffer, BigRand);
        }

        public void ReadCs(IBuffer buffer)
        {
            RoleIndex = ReadUInt32(buffer);
            CharLevel = ReadInt32(buffer);
            Weapon = ReadInt32(buffer);
            BoxId = ReadInt32(buffer);
            RoleName = ReadString(buffer);
            StarLevel = ReadString(buffer);
            Faction = ReadInt32(buffer);
            Officer = ReadByte(buffer);
            HRLevel = ReadInt32(buffer);
            BigRand = ReadByte(buffer);
        }
    }
}