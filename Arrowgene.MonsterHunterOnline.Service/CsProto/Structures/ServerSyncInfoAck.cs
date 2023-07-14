using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 服务器强制状态变化的确认
    /// </summary>
    public class ServerSyncInfoAck : Structure
    {
        public ServerSyncInfoAck()
        {
            SyncTime = 0;
        }

        /// <summary>
        /// 同步时间
        /// </summary>
        public long SyncTime { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteInt64(buffer, SyncTime);
        }

        public override void Read(IBuffer buffer)
        {
            SyncTime = ReadInt64(buffer);
        }
    }
}