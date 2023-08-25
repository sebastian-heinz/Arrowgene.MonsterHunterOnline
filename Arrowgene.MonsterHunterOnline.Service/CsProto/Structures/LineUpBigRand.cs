using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 大随机匹配请求
    /// </summary>
    public class LineUpBigRand : Structure
    {
        public LineUpBigRand()
        {
            Type = 0;
        }

        public int Type { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteInt32(buffer, Type);
        }

        public override void Read(IBuffer buffer)
        {
            Type = ReadInt32(buffer);
        }
    }
}