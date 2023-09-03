using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 队员小地图点击点通知
    /// </summary>
    public class TeamPushVecNtf : Structure, ICsStructure
    {
        public TeamPushVecNtf()
        {
            TeamId = 0;
            Vec3 = new CSVec3();
        }

        public uint TeamId { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public CSVec3 Vec3 { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteUInt32(buffer, TeamId);
            WriteCsStructure(buffer, Vec3);
        }

        public void ReadCs(IBuffer buffer)
        {
            TeamId = ReadUInt32(buffer);
            Vec3 = ReadCsStructure(buffer, Vec3);
        }
    }
}