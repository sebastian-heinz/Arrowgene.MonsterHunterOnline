using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// Monster active state
    /// </summary>
    public class MonsterActiveState : Structure
    {
        public MonsterActiveState()
        {
            SyncTime = 0;
            ActiveState = 0;
            MonsterId = 0;
            Position = new XYZPosition();
            Rotation = new Quaternion();
        }

        /// <summary>
        /// 同步时间
        /// </summary>
        public long SyncTime { get; set; }

        /// <summary>
        /// Monster active state
        /// </summary>
        public uint ActiveState { get; set; }

        /// <summary>
        /// Monster Identity
        /// </summary>
        public uint MonsterId { get; set; }

        /// <summary>
        /// Position
        /// </summary>
        public XYZPosition Position { get; set; }

        /// <summary>
        /// Rotation
        /// </summary>
        public Quaternion Rotation { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteInt64(buffer, SyncTime);
            WriteUInt32(buffer, ActiveState);
            WriteUInt32(buffer, MonsterId);
            WriteStructure(buffer, Position);
            WriteStructure(buffer, Rotation);
        }

        public override void Read(IBuffer buffer)
        {
            SyncTime = ReadInt64(buffer);
            ActiveState = ReadUInt32(buffer);
            MonsterId = ReadUInt32(buffer);
            Position = ReadStructure(buffer, Position);
            Rotation = ReadStructure(buffer, Rotation);
        }
    }
}