using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 聊天发送的道具信息
    /// </summary>
    public class ChatItemsPkg : Structure
    {
        public ChatItemsPkg()
        {
            Items = new List<byte>();
            SoulStoneArray = new List<int>();
        }

        /// <summary>
        /// 聊天道具数据
        /// </summary>
        public List<byte> Items { get; }

        /// <summary>
        /// 狩魂石属性数据
        /// </summary>
        public List<int> SoulStoneArray { get; }

        public override void Write(IBuffer buffer)
        {
            WriteList(buffer, Items, CsProtoConstant.CS_CHAT_ITEMS_LEN, WriteInt32, WriteByte);
            WriteList(buffer, SoulStoneArray, CsProtoConstant.CS_MAX_SOUL_STONE_ATTR_COUNT, WriteInt32, WriteInt32);
        }

        public override void Read(IBuffer buffer)
        {
            ReadList(buffer, Items, CsProtoConstant.CS_CHAT_ITEMS_LEN, ReadInt32, ReadByte);
            ReadList(buffer, SoulStoneArray, CsProtoConstant.CS_MAX_SOUL_STONE_ATTR_COUNT, ReadInt32, ReadInt32);
        }
    }
}