using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;

public class CsListRoleRsp
{
    public CsListRoleRsp()
    {
        Roles = new List<CsRoleBaseInfo>();
    }

    /// <summary>
    /// 0为成功，其他是错误码
    /// </summary>
    public uint ErrNo { get; set; }

    /// <summary>
    /// 封号时间
    /// </summary>
    public uint BanTime { get; set; }

    /// <summary>
    /// 最后登陆角色的Index
    /// </summary>
    public uint LastLoinRoleIndex { get; set; }

    public List<CsRoleBaseInfo> Roles { get; set; }

    public CsProtoPacket BuildPacket()
    {
        IBuffer buffer = new StreamBuffer();
        buffer.WriteUInt32(ErrNo, Endianness.Big);
        buffer.WriteUInt32(BanTime, Endianness.Big);
        buffer.WriteUInt32(LastLoinRoleIndex, Endianness.Big);
        buffer.WriteInt16((short)Roles.Count, Endianness.Big);
        foreach (CsRoleBaseInfo role in Roles)
        {
            role.Write(buffer);
        }

        CsProtoPacket packet = new CsProtoPacket();
        packet.Body = buffer.GetAllBytes();
        packet.Cmd = CsProtoCmd.CS_CMD_LIST_ROLE_RSP;
        return packet;
    }
}