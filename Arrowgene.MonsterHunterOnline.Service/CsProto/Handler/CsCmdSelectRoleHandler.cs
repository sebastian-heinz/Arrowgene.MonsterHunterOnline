using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdSelectRoleHandler : ICsProtoHandler
{
    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_SELECT_ROLE_REQ;

    public void Handle(Client client, CsProtoPacket packet)
    {
        IBuffer req = new StreamBuffer(packet.Body);
        req.SetPositionStart();
        //     <struct name="CSSelectRoleReq" version="1" desc="选择角色请求">
        //         <entry name="RoleIndex" type="int" desc="角色Index"/>
        //         <entry name="MacAddress" type="string" size="CS_MAC_ADDRESS_LEN" desc="客户端mac地址" sizeinfo="int"/>
        //         </struct>
        int RoleIndex = req.ReadInt32(Endianness.Big);
        int MacAddressLen = req.ReadInt32(Endianness.Big);
        string MacAddress = req.ReadString(MacAddressLen - 1);


        //     <struct name="CSSelectRoleRsp" version="1" desc="选择角色响应">
        //         <entry name="ErrNo" type="int" desc="0为成功，-3表示没有可用的TownServer服务器, 其他是错误码"/>
        //         <entry name="RoleIndex" type="int" desc="角色Index"/>
        //         </struct>
        IBuffer res = new StreamBuffer();
        res.WriteInt32(0, Endianness.Big);
        res.WriteInt32(RoleIndex, Endianness.Big);

        CsProtoPacket resp = new CsProtoPacket();
        resp.Body = res.GetAllBytes();
        resp.Cmd = CS_CMD_ID.CS_CMD_SELECT_ROLE_RSP;
        client.SendCsProto(resp);

        client.State.OnRoleSelected();
    }
}