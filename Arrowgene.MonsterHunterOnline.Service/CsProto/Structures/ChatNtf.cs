using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.System.ChatSystem;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    /// <summary>
    /// 发送给客户端的其他玩家聊天信息通知
    /// </summary>
    public class ChatNtf : Structure, ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(ChatNtf));

        public ChatNtf()
        {
            SourceId = 0;
            SrcUin = 0;
            SrcDbId = 0;
            SrcLevelGrpId = 0;
            SourceName = "";
            SrcVipLevel = 0;
            SrcVipCanUse = 0;
            QQMask = 0;
            ChannelType = 0;
            LineId = 0;
            WorldSvrId = 0;
            ShowTime = 0;
            Head = new List<byte>();
            Content = "";
            SendByMe = false;
            ContainBanWords = false;
            Items = new ChatItemsPkg();
            SrcLevel = 0;
            SrcGuildName = "";
            SrcHunterStar = "";
            SrcHrLevel = 0;
        }

        /// <summary>
        /// 源玩家ID
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// 源玩家QQ号
        /// </summary>
        public uint SrcUin { get; set; }

        /// <summary>
        /// 源玩家DBID
        /// </summary>
        public ulong SrcDbId { get; set; }

        /// <summary>
        /// 源角色所在关卡的逻辑关卡组ID
        /// </summary>
        public int SrcLevelGrpId { get; set; }

        /// <summary>
        /// 源玩家角色名字
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// 源VIP等级
        /// </summary>
        public byte SrcVipLevel { get; set; }

        /// <summary>
        /// 源VIP是否可用
        /// </summary>
        public byte SrcVipCanUse { get; set; }

        /// <summary>
        /// 其他合作福利标记
        /// </summary>
        public int QQMask { get; set; }

        /// <summary>
        /// 频道类型ID
        /// </summary>
        public ChannelType ChannelType { get; set; }

        /// <summary>
        /// 消息发起者的线ID
        /// </summary>
        public int LineId { get; set; }

        /// <summary>
        /// 消息发送者的WorldSvrID
        /// </summary>
        public int WorldSvrId { get; set; }

        /// <summary>
        /// 消息展示持续时间秒
        /// </summary>
        public int ShowTime { get; set; }

        /// <summary>
        /// 头部信息
        /// </summary>
        public List<byte> Head { get; }

        /// <summary>
        /// 聊天内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 是否为自己对单个别人说（1=true或0=false），只有密语频道有用，用于区分是别人对自己说还是自己对别人说。如果是自己对别人说，则SourceID和SourceName分别为目标玩家ID和目标玩家角色名（而非源玩家ID和源玩家角色名）。
        /// </summary>
        public bool SendByMe { get; set; }

        /// <summary>
        /// 是否包括密语敏感词(1表示包含，0表示不包含)
        /// </summary>
        public bool ContainBanWords { get; set; }

        /// <summary>
        /// 聊天道具数据
        /// </summary>
        public ChatItemsPkg Items { get; set; }

        /// <summary>
        /// 源玩家普通等级, 如果满级的话就是HR等级
        /// </summary>
        public int SrcLevel { get; set; }

        /// <summary>
        /// 源玩家猎团名字
        /// </summary>
        public string SrcGuildName { get; set; }

        /// <summary>
        /// 玩家猎人星级
        /// </summary>
        public string SrcHunterStar { get; set; }

        /// <summary>
        /// 源玩家HR等级
        /// </summary>
        public int SrcHrLevel { get; set; }

        public  void WriteCs(IBuffer buffer)
        {
            WriteInt32(buffer, SourceId);
            WriteUInt32(buffer, SrcUin);
            WriteUInt64(buffer, SrcDbId);
            WriteInt32(buffer, SrcLevelGrpId);
            WriteString(buffer, SourceName);
            WriteByte(buffer, SrcVipLevel);
            WriteByte(buffer, SrcVipCanUse);
            WriteInt32(buffer, QQMask);
            WriteInt32(buffer, (int)ChannelType);
            WriteInt32(buffer, LineId);
            WriteInt32(buffer, WorldSvrId);
            WriteInt32(buffer, ShowTime);
            WriteList(buffer, Head, CsProtoConstant.MAX_CHAT_HEAD_LEN, WriteInt32, WriteByte);
            WriteString(buffer, Content);
            WriteBool(buffer, SendByMe);
            WriteBool(buffer, ContainBanWords);
            WriteCsStructure(buffer, Items);
            WriteInt32(buffer, SrcLevel);
            WriteString(buffer, SrcGuildName);
            WriteString(buffer, SrcHunterStar);
            WriteInt32(buffer, SrcHrLevel);
        }

        public void ReadCs(IBuffer buffer)
        {
            SourceId = ReadInt32(buffer);
            SrcUin = ReadUInt32(buffer);
            SrcDbId = ReadUInt64(buffer);
            SrcLevelGrpId = ReadInt32(buffer);
            SourceName = ReadString(buffer);
            SrcVipLevel = ReadByte(buffer);
            SrcVipCanUse = ReadByte(buffer);
            QQMask = ReadInt32(buffer);
            ChannelType = (ChannelType)ReadInt32(buffer);
            LineId = ReadInt32(buffer);
            WorldSvrId = ReadInt32(buffer);
            ShowTime = ReadInt32(buffer);
            ReadList(buffer, Head, CsProtoConstant.MAX_CHAT_HEAD_LEN, ReadInt32, ReadByte);
            Content = ReadString(buffer);
            SendByMe = ReadBool(buffer);
            ContainBanWords = ReadBool(buffer);
            Items = ReadCsStructure(buffer, Items);
            SrcLevel = ReadInt32(buffer);
            SrcGuildName = ReadString(buffer);
            SrcHunterStar = ReadString(buffer);
            SrcHrLevel = ReadInt32(buffer);
        }
    }
}