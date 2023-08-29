using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 进入TOWN场景验证响应
    /// </summary>
    public class TownInstanceVerifyRsp : Structure, ICsStructure
    {
        public TownInstanceVerifyRsp()
        {
            ErrNo = 0;
            InstanceInitInfo = new InstanceInitInfo();
            LineId = 0;
            LevelEnterType = 0;
        }

        /// <summary>
        /// 响应码, 0为成功
        /// </summary>
        public int ErrNo { get; set; }

        /// <summary>
        /// Instance initialize info
        /// </summary>
        public InstanceInitInfo InstanceInitInfo { get; set; }

        /// <summary>
        /// 所在线ID
        /// </summary>
        public ushort LineId { get; set; }

        /// <summary>
        /// 进入关卡类型，如Townsvr新进，副本返回townsvr，townsvrHub切换等
        /// </summary>
        public int LevelEnterType { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, ErrNo);
            WriteCsStructure(buffer, InstanceInitInfo);
            WriteUInt16(buffer, LineId);
            WriteInt32(buffer, LevelEnterType);
        }

        public void ReadCs(IBuffer buffer)
        {
            ErrNo = ReadInt32(buffer);
            InstanceInitInfo.ReadCs(buffer);
            LineId = ReadUInt16(buffer);
            LevelEnterType = ReadInt32(buffer);
        }
    }
}