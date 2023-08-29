using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 服务器强制状态变化消息
    /// </summary>
    public class ServerSyncInfoNtf : Structure, ICsStructure
    {
        public ServerSyncInfoNtf()
        {
            EntityId = 0;
            SyncInfo = new FifoSyncInfo();
        }

        /// <summary>
        /// 需要同步的Actor的EntityId
        /// </summary>
        public uint EntityId { get; set; }

        /// <summary>
        /// FIFO同步信息
        /// </summary>
        public FifoSyncInfo SyncInfo { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteUInt32(buffer, EntityId);
            WriteCsStructure(buffer, SyncInfo);
        }

        public void ReadCs(IBuffer buffer)
        {
            EntityId = ReadUInt32(buffer);
            SyncInfo = ReadCsStructure(buffer, SyncInfo);
        }
    }
}