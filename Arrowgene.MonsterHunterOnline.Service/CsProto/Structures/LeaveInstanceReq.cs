using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 离开战斗副本请求
    /// </summary>
    public class LeaveInstanceReq : Structure, ICsStructure
    {
        public LeaveInstanceReq()
        {
            Type = 0;
            Reason = 0;
            Reserved = 0;
        }

        /// <summary>
        /// 离开方式，0：退出游戏，1：回到城镇的副本进入点 2:回到城镇的主城
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 离开理由
        /// </summary>
        public int Reason { get; set; }

        public int Reserved { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, Type);
            WriteInt32(buffer, Reason);
            WriteInt32(buffer, Reserved);
        }

        public void ReadCs(IBuffer buffer)
        {
            Type = ReadInt32(buffer);
            Reason = ReadInt32(buffer);
            Reserved = ReadInt32(buffer);
        }
    }
}