using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 创建服务器Entity消息
    /// </summary>
    public class SpawnSrvEnt : Structure
    {
        public SpawnSrvEnt()
        {
            Name = "";
            NetObjId = 0;
            Position = new XYZPosition();
            Rotation = new Quaternion();
            Scale = 0.0f;
        }

        /// <summary>
        /// Entity Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务器Object ID号
        /// </summary>
        public uint NetObjId { get; set; }

        /// <summary>
        /// 创建位置
        /// </summary>
        public XYZPosition Position { get; set; }

        /// <summary>
        /// 创建朝向
        /// </summary>
        public Quaternion Rotation { get; set; }

        /// <summary>
        /// 缩放值
        /// </summary>
        public float Scale { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteString(buffer, Name);
            WriteUInt32(buffer, NetObjId);
            WriteStructure(buffer, Position);
            WriteStructure(buffer, Rotation);
            WriteFloat(buffer, Scale);
        }

        public override void Read(IBuffer buffer)
        {
            Name = ReadString(buffer);
            NetObjId = ReadUInt32(buffer);
            Position = ReadStructure(buffer, Position);
            Rotation = ReadStructure(buffer, Rotation);
            Scale = ReadFloat(buffer);
        }
        
    }
}