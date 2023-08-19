using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System.Chat;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 客户端发送的广播聊天请求
    /// </summary>
    public class ChatBroadcastReq : Structure
    {
        public ChatBroadcastReq()
        {
            ChannelType = 0;
            Content = "";
            Items = new ChatItemsPkg();
        }

        /// <summary>
        /// 频道类型ID
        /// </summary>
        public ChannelType ChannelType { get; set; }

        /// <summary>
        /// 聊天内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 聊天道具数据
        /// </summary>
        public ChatItemsPkg Items { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteInt32(buffer, (int)ChannelType);
            WriteString(buffer, Content);
            WriteStructure(buffer, Items);
        }

        public override void Read(IBuffer buffer)
        {
            ChannelType = (ChannelType)ReadInt32(buffer);
            Content = ReadString(buffer);
            Items = ReadStructure(buffer, Items);
        }
    }
}