using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 付费重新捏脸响应
    /// </summary>
    public class ModifyFaceRsp : Structure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ModifyFaceRsp));

        public ModifyFaceRsp()
        {
            Result = 0;
        }

        /// <summary>
        /// 结果
        /// </summary>
        public int Result;

        public override void Write(IBuffer buffer)
        {
            WriteInt32(buffer, Result);
        }

        public override void Read(IBuffer buffer)
        {
            Result = ReadInt32(buffer);
        }
    }
}