using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System.ChatSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 客户端发送的密语和私聊聊天请求
    /// </summary>
    public class ChatPrivateReq : Structure, ICsStructure
    {
        public ChatPrivateReq()
        {
            ChannelType = ChannelType.None;
            Content = "";
            TargetName = "";
            Items = new ChatItemsPkg();
            TargetSvrId = 0;
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
        /// 目标玩家角色名字
        /// </summary>
        public string TargetName { get; set; }

        /// <summary>
        /// 聊天道具数据
        /// </summary>
        public ChatItemsPkg Items { get; set; }

        /// <summary>
        /// 目标玩家服ID
        /// </summary>
        public uint TargetSvrId { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, (int)ChannelType);
            WriteString(buffer, Content);
            WriteString(buffer, TargetName);
            WriteCsStructure(buffer, Items);
            WriteUInt32(buffer, TargetSvrId);
        }

        public void ReadCs(IBuffer buffer)
        {
            ChannelType = (ChannelType)ReadInt32(buffer);
            Content = ReadString(buffer);
            TargetName = ReadString(buffer);
            Items = ReadCsStructure(buffer, Items);
            TargetSvrId = ReadUInt32(buffer);
        }
    }
}