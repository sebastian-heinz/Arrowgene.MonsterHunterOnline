using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdSelectRoleHandler : ICsProtoHandler
{
    public CsProtoCmd Cmd => CsProtoCmd.CS_CMD_SELECT_ROLE_REQ;

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
        resp.Cmd = CsProtoCmd.CS_CMD_SELECT_ROLE_RSP;
        client.SendCsProto(resp);


        //      <struct name="CSTownSessionStart" version="1" desc="TownSession建立">
        //      <entry name="ErrNo" type="tinyint" desc="0为成功，其他是错误码"/>
        //      </struct>
        // IBuffer res1 = new StreamBuffer();
        // res1.WriteByte(0);

        // CsProtoPacket resp1 = new CsProtoPacket();
        // resp1.Body = res1.GetAllBytes();
        // resp1.Cmd = CsProtoCmd.CS_CMD_TOWN_SESSION_START;
        // client.SendCsProto(resp1);


//
//	<struct name="CSListRoleRsp" version="1" desc="角色列表的响应包">
//		<entry name="ErrNo" type="uint" desc="0为成功，其他是错误码"/>
//		<entry name="BanTime" type="uint" desc="封号时间"/>
//		<entry name="LastLoinRoleIndex" type="uint" desc="最后登陆角色的Index"/>
//		<entry name="RoleList" type="CSRoleList"/>
//	</struct>
        IBuffer res2 = new StreamBuffer();
        res2.WriteUInt32(0, Endianness.Big);
        res2.WriteUInt32(0, Endianness.Big);
        res2.WriteUInt32(0, Endianness.Big);
        //	<struct name="CSRoleList" version="1" desc="角色列表">
//		<entry name="Count" type="smallint" desc="角色数量"/>
//		<entry name="Role" type="CSRoleBaseInfo" count="CS_MAX_ROLE_NUM" desc="角色基本信息" refer="Count"/>
//	</struct>
        res2.WriteInt16(1, Endianness.Big);


//	<struct name="CSRoleBaseInfo" version="1" desc="角色基本信息">
//		<entry name="RoleID" type="biguint" desc="角色ID"/>
//		<entry name="RoleIndex" type="int" desc="角色Index"/>
//		<entry name="Name" type="string" size="CS_MAX_ROLE_NAME" desc="角色名" sizeinfo="int"/>
//		<entry name="Gender" type="tinyuint" desc="角色性别"/>
//		<entry name="Level" type="int" desc="角色等级"/>
//		<entry name="RoleState" type="int" desc="角色状态"/>
//		<entry name="RoleStateEndLeftTime" type="uint" desc="角色状态结束的剩余时间"/>
//		<entry name="AvatarSetID" type="byte" desc="Avatar Set"/>
//		<entry name="FaceId" type="ushort" desc="脸型"/>
//		<entry name="HairId" type="ushort" desc="发型"/>
//		<entry name="UnderclothesId" type="ushort" desc="内衣"/>
//		<entry name="EquipCount" type="ushort" desc="装备数量"/>
//		<entry name="SkinColor" type="int" desc="皮肤颜色"/>
//		<entry name="HairColor" type="int" desc="头发颜色"/>
//		<entry name="InnerColor" type="int" desc="内衣颜色"/>
//		<entry name="EyeBall" type="int" desc="眼睛"/>
//		<entry name="EyeColor" type="int" desc="眼睛颜色"/>
//		<entry name="FaceTattooIndex" type="int" desc="脸部贴花"/>
//		<entry name="FaceTattooColor" type="int" desc="脸部贴花颜色"/>
//		<entry name="Equip" type="CSAvatarItem" count="ROLE_EQUIPED_MAX_NETMESSAGE" desc="装备物品" refer="EquipCount"/>
//		<entry name="HideHelm" type="byte" desc="是否隐藏头盔"/>
//		<entry name="HideFashion" type="byte" desc="是否隐藏时装"/>
//		<entry name="HideSuite" type="byte" desc="是否隐藏套件"/>
//		<entry name="FacialInfo" type="short" count="CS_MAX_FACIALINFO_COUNT" desc="捏脸数据集合"/>
//		<entry name="StarLevel" type="string" size="CS_MAX_HUNTER_STAR_LEVEL_LEN" desc="星级" sizeinfo="int"/>
//		<entry name="HRLevel" type="int" desc="角色HR等级"/>
//		<entry name="SoulStoneLv" type="int" desc="兽魂石等级"/>
//	</struct>
        res2.WriteUInt64(1, Endianness.Big); //RoleID
        res2.WriteInt32(0, Endianness.Big); //RoleIndex

        //Name
        res2.WriteInt32(5, Endianness.Big);
        res2.WriteCString("Test");

        res2.WriteByte(0); // Gender
        res2.WriteInt32(0, Endianness.Big); //Level
        res2.WriteInt32(0, Endianness.Big); //RoleState
        res2.WriteUInt32(0, Endianness.Big); //RoleStateEndLeftTime
        res2.WriteByte(0); // AvatarSetID
        res2.WriteUInt16(0, Endianness.Big); // FaceId
        res2.WriteUInt16(0, Endianness.Big); // HairId
        res2.WriteUInt16(0, Endianness.Big); // UnderclothesId
        res2.WriteUInt16(0, Endianness.Big); // EquipCount
        res2.WriteInt32(0, Endianness.Big); // SkinColor
        res2.WriteInt32(0, Endianness.Big); // HairColor
        res2.WriteInt32(0, Endianness.Big); // InnerColor
        res2.WriteInt32(0, Endianness.Big); // EyeBall
        res2.WriteInt32(0, Endianness.Big); // EyeColor
        res2.WriteInt32(0, Endianness.Big); // FaceTattooIndex
        res2.WriteInt32(0, Endianness.Big); // FaceTattooColor

        //Equip  refer="EquipCount"/
        // <struct name="CSAvatarItem" version="1" desc="角色装备显示物品">
        //     <entry name="ItemType" type="int" desc="物品类型"/>
        //     <entry name="PosIndex" type="uint16" desc="物品位置行"/>
        //     <entry name="Rank" type="uint32" desc="Rank级别"/>
        //     <entry name="EnhanceRule" type="uint8" desc="强化规则"/>
        //     <entry name="EnhanceLevel" type="uint8" desc="强化数据"/>
        //     <entry name="SoltCount" type="uint8" desc="插孔数"/>
        //     <entry name="GemID" type="uint32" desc="插孔宝石物品"/>
        //     <entry name="BreakLevel" type="uint8" desc="突破级别"/>
        //     <entry name="ColorIndex" type="uint8" desc="染色效果"/>
        //     <entry name="FakeShow" type="uint32" desc="幻化效果"/>
        //     <entry name="WakeLevel" type="uint8" desc="觉醒层级"/>
        //     </struct>

        res2.WriteByte(0); // HideHelm
        res2.WriteByte(0); // HideFashion
        res2.WriteByte(0); // HideSuite
        res2.WriteInt16(0, Endianness.Big); // FacialInfo

        //StarLevel
        res2.WriteInt32(5, Endianness.Big);
        res2.WriteCString("Star");

        res2.WriteInt32(0, Endianness.Big); // HRLevel
        res2.WriteInt32(0, Endianness.Big); // SoulStoneLv

        CsProtoPacket resp2 = new CsProtoPacket();
        resp2.Body = res2.GetAllBytes();
        resp2.Cmd = CsProtoCmd.CS_CMD_LIST_ROLE_RSP;
        // client.SendCsProto(resp2);


        //
        //
        // <struct name="CSShakeHand" version="1" desc="连接握手消息">
        //     <entry name="VerifyCode" type="int32" desc="验证码"/>
        //     </struct>
        IBuffer res3 = new StreamBuffer();
        res3.WriteInt32(0, Endianness.Big);

        CsProtoPacket resp3 = new CsProtoPacket();
        resp3.Body = res3.GetAllBytes();
        resp3.Cmd = CsProtoCmd.CS_CMD_SYSTEM_SHAKEHAND;
        //client.SendCsProto(resp3);


        // <struct name="CSAutoStartProcess" version="1" desc="自动发起某些流程">
        //     <entry name="ProcessType" type="int32" desc="流程类型" bindmacrosgroup="CS_AUTO_START_FUNCTION"/>
        //     </struct>
        IBuffer res4 = new StreamBuffer();
        //   <macrosgroup name="CS_AUTO_START_FUNCTION" desc="客户端自动发起的功能">
        //   <macro name="CS_AUTO_START_BACKTOROLELIST" value="0" desc="自动返回角色列表" />
        //   <macro name="CS_AUTO_START_BACKTOTOWN" value="1" desc="自动返回城镇" />
        //   </macrosgroup>
        res4.WriteInt32(0, Endianness.Big);

        CsProtoPacket resp4 = new CsProtoPacket();
        resp4.Body = res4.GetAllBytes();
        resp4.Cmd = CsProtoCmd.CS_CMD_AUTO_START_PROCESS;
        //client.SendCsProto(resp4);


        IBuffer res5 = new StreamBuffer();
        // <struct name="CSMainInstanceOptSynReq" version="1" desc="副本主UI操作同步请求">
        // <entry name="TriggerId" type="int" desc="触发点"/>
        // <entry name="InstancePoint" type="int" desc="副本UI点"/>
        // <entry name="LevelID" type="int" desc="levelId"/>
        // <entry name="LevelDiff" type="int" desc="难度"/>
        // <entry name="LevelMode" type="int" desc="模式"/>
        // </struct>
        res5.WriteInt32(0, Endianness.Big);
        res5.WriteInt32(0, Endianness.Big);
        res5.WriteInt32(0, Endianness.Big);
        res5.WriteInt32(0, Endianness.Big);
        res5.WriteInt32(0, Endianness.Big);
        CsProtoPacket resp5 = new CsProtoPacket();
        resp5.Body = res5.GetAllBytes();
        resp5.Cmd = CsProtoCmd.CS_CMD_MAIN_INSTANCE_OPT_SYN_REQ;
        //   client.SendCsProto(resp5);


        //CS_CMD_SYSTEM_HEARTBEAT
        //CS_CMD_SYSTEM_GAME_MANAGER_CMD
        //

        //<struct name="CSPing" version="1" desc="Ping消息">
        //    <entry name="PingID" type="uint32" desc="ping消息序列号"/>
        //    <entry name="Delay" type="uint16" desc="统计延迟，毫秒"/>
        //    <entry name="CurDelay" type="uint16" desc="最近延迟，毫秒"/>
        //    <entry name="AverageDelay" type="uint16" desc="最近平均延迟，毫秒"/>
        //    <entry name="ServerFps" type="char" desc="最近的服务器fps"/>
        //    <entry name="ServerTm" type="uint32" desc="服务器时间戳"/>
        //    </struct>
        IBuffer res6 = new StreamBuffer();
        res6.WriteUInt32(0, Endianness.Big);
        res6.WriteInt16(0, Endianness.Big);
        res6.WriteInt16(0, Endianness.Big);
        res6.WriteInt16(0, Endianness.Big);
        res6.WriteByte(30);
        res6.WriteUInt32(0, Endianness.Big);


        CsProtoPacket resp6 = new CsProtoPacket();
        resp6.Body = res6.GetAllBytes();
        resp6.Cmd = CsProtoCmd.CS_CMD_SYSTEM_PING;
        //client.SendCsProto(resp6);


        // <struct name="CSRoleDataErrorRsp" version="1" desc="角色数据错误">
        //     <entry name="ErrNo" type="int" desc="0为成功"/>
        //     </struct>
        IBuffer res7 = new StreamBuffer();
        res7.WriteUInt32(0, Endianness.Big);

        CsProtoPacket resp7 = new CsProtoPacket();
        resp7.Body = res7.GetAllBytes();
        resp7.Cmd = CsProtoCmd.CS_CMD_ROLEDATA_ERR_RSP;
        //client.SendCsProto(resp7);
    }
}