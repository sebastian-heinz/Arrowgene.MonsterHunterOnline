using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 副本进入同意响应
    /// </summary>
    public class MainInstanceAgreeOptRsp : Structure, ICsStructure
    {
        public MainInstanceAgreeOptRsp()
        {
            Reason = 0;
            LevelId = 0;
            NetId = 0;
            Agree = 0;
            RoleName = "";
        }

        public int Reason { get; set; }

        /// <summary>
        /// 副本ID
        /// </summary>
        public int LevelId { get; set; }

        /// <summary>
        /// 玩家的NETID
        /// </summary>
        public int NetId { get; set; }

        /// <summary>
        /// 是否同意
        /// </summary>
        public int Agree { get; set; }

        public string RoleName { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, Reason);
            WriteInt32(buffer, LevelId);
            WriteInt32(buffer, NetId);
            WriteInt32(buffer, Agree);
            WriteString(buffer, RoleName);
        }

        public void ReadCs(IBuffer buffer)
        {
            Reason = ReadInt32(buffer);
            LevelId = ReadInt32(buffer);
            NetId = ReadInt32(buffer);
            Agree = ReadInt32(buffer);
            RoleName = ReadString(buffer);
        }
    }
}