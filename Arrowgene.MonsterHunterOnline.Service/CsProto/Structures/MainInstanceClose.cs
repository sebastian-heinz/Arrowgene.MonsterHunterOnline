using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 关闭房间
    /// </summary>
    public class MainInstanceClose : Structure, ICsStructure
    {
        public MainInstanceClose()
        {
            LevelId = 0;
            RoomId = 0;
            Reason = 0;
            TriggerNetId = 0;
            RoleName = "";
        }

        /// <summary>
        /// 副本ID
        /// </summary>
        public int LevelId;

        /// <summary>
        /// 房间号
        /// </summary>
        public uint RoomId;

        public int Reason;

        /// <summary>
        /// 玩家的NETID
        /// </summary>
        public uint TriggerNetId;

        public string RoleName;

        public void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, LevelId);
            WriteUInt32(buffer, RoomId);
            WriteInt32(buffer, Reason);
            WriteUInt32(buffer, TriggerNetId);
            WriteString(buffer, RoleName);
        }

        public void ReadCs(IBuffer buffer)
        {
            LevelId = ReadInt32(buffer);
            RoomId = ReadUInt32(buffer);
            Reason = ReadInt32(buffer);
            TriggerNetId = ReadUInt32(buffer);
            RoleName = ReadString(buffer);
        }
    }
}