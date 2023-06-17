using System.Collections.Generic;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

public class CsListRoleRsp : IStructure
{
    public CsListRoleRsp()
    {
        ErrNo = 0;
        BanTime = 0;
        LastLoinRoleIndex = 0;
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

    public List<CsRoleBaseInfo> Roles { get; }


    public void Write(IBuffer buffer)
    {
        buffer.WriteUInt32(ErrNo, Endianness.Big);
        buffer.WriteUInt32(BanTime, Endianness.Big);
        buffer.WriteUInt32(LastLoinRoleIndex, Endianness.Big);
        short rolesSize = (short)Roles.Count;
        buffer.WriteInt16(rolesSize, Endianness.Big);
        for (int i = 0; i < rolesSize; i++)
        {
            Roles[i].Write(buffer);
        }
    }
}