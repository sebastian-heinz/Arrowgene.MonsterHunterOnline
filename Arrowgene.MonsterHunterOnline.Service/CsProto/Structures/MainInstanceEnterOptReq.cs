using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 副本主UI操作进入请求
    /// </summary>
    public class MainInstanceEnterOptReq : Structure, ICsStructure
    {
        public MainInstanceEnterOptReq()
        {
            LevelId = 0;
            Param = 0;
            UseEmploye = 0;
            WeaponTrial = 0;
            GuildWarTargetId = 0;
        }

        /// <summary>
        /// level id
        /// </summary>
        public int LevelId { get; set; }

        /// <summary>
        /// NPC参数 或 Trigger参数
        /// </summary>
        public int Param { get; set; }

        /// <summary>
        /// 雇佣兵
        /// </summary>
        public int UseEmploye { get; set; }

        /// <summary>
        /// 武器试炼层数
        /// </summary>
        public int WeaponTrial { get; set; }

        /// <summary>
        /// 抢夺目标id
        /// </summary>
        public ulong GuildWarTargetId { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, LevelId);
            WriteInt32(buffer, Param);
            WriteInt32(buffer, UseEmploye);
            WriteInt32(buffer, WeaponTrial);
            WriteUInt64(buffer, GuildWarTargetId);
        }

        public void ReadCs(IBuffer buffer)
        {
            LevelId = ReadInt32(buffer);
            Param = ReadInt32(buffer);
            UseEmploye = ReadInt32(buffer);
            WeaponTrial = ReadInt32(buffer);
            GuildWarTargetId = ReadUInt64(buffer);
        }
    }
}