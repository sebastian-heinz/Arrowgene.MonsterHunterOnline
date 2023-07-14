using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class ListRoleRsp : Structure
{
    public ListRoleRsp()
    {
        ErrNo = 0;
        BanTime = 0;
        LastLoinRoleIndex = 0;
        RoleList = new List<RoleBaseInfo>();
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

    public List<RoleBaseInfo> RoleList { get; }

    public override void Write(IBuffer buffer)
    {
        WriteUInt32(buffer, ErrNo);
        WriteUInt32(buffer, BanTime);
        WriteUInt32(buffer, LastLoinRoleIndex);
        WriteList(buffer, RoleList, (short)CsProtoConstant.CS_MAX_ROLE_NUM, WriteInt16, WriteStructure);
    }

    public override void Read(IBuffer buffer)
    {
        ErrNo = ReadUInt16(buffer);
        BanTime = ReadUInt16(buffer);
        LastLoinRoleIndex = ReadUInt16(buffer);
        ReadList(buffer, RoleList, (short)CsProtoConstant.CS_MAX_ROLE_NUM, ReadInt16, ReadStructure<RoleBaseInfo>);
    }
}