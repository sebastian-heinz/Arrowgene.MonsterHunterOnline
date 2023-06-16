using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto;

public static class TEST_RESPONSES
{
    public static void send(Client client)
    {
        //      <struct name="CSTownSessionStart" version="1" desc="TownSession建立">
        //      <entry name="ErrNo" type="tinyint" desc="0为成功，其他是错误码"/>
        //      </struct>

        IBuffer res10 = new StreamBuffer();
        res10.WriteByte(0);
        CsProtoPacket resp10 = new CsProtoPacket();
        resp10.Body = res10.GetAllBytes();
        resp10.Cmd = CsProtoCmd.CS_CMD_TOWN_SESSION_START;
       //   client.SendCsProto(resp10);


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
        //   client.SendCsProto(resp3);


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
        //    client.SendCsProto(resp4);


        IBuffer res5 = new StreamBuffer();
        // <struct name="CSMainInstanceOptSynReq" version="1" desc="副本主UI操作同步请求">
        // <entry name="TriggerId" type="int" desc="触发点"/>
        // <entry name="InstancePoint" type="int" desc="副本UI点"/>
        // <entry name="LevelID" type="int" desc="levelId"/>
        // <entry name="LevelDiff" type="int" desc="难度"/>
        // <entry name="LevelMode" type="int" desc="模式"/>
        // </struct>
        res5.WriteInt32(1, Endianness.Big);
        res5.WriteInt32(2, Endianness.Big);
        res5.WriteInt32(1, Endianness.Big);
        res5.WriteInt32(1, Endianness.Big);
        res5.WriteInt32(1, Endianness.Big);
        CsProtoPacket resp5 = new CsProtoPacket();
        resp5.Body = res5.GetAllBytes();
        resp5.Cmd = CsProtoCmd.CS_CMD_MAIN_INSTANCE_OPT_SYN_REQ;
       //       client.SendCsProto(resp5);


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
        //    client.SendCsProto(resp6);


        // <struct name="CSRoleDataErrorRsp" version="1" desc="角色数据错误">
        //     <entry name="ErrNo" type="int" desc="0为成功"/>
        //     </struct>
        IBuffer res7 = new StreamBuffer();
        res7.WriteUInt32(0, Endianness.Big);
        CsProtoPacket resp7 = new CsProtoPacket();
        resp7.Body = res7.GetAllBytes();
        resp7.Cmd = CsProtoCmd.CS_CMD_ROLEDATA_ERR_RSP;
         //  client.SendCsProto(resp7);


        //   <struct name="CSRoomInitInfo" version="1" desc="玩家进入BS时，重新发送房间必要的信息">
        //       <entry name="RoomSlotNum" type="int"/>
        //       <entry name="RoomSlots" type="uint" count="CS_MAX_BSM_CANDIDATE_COUNT" refer="RoomSlotNum"/>
        //       </struct>

        IBuffer res8 = new StreamBuffer();
        res7.WriteInt32(0, Endianness.Big);
        res7.WriteUInt32(1, Endianness.Big);

        CsProtoPacket resp8 = new CsProtoPacket();
        resp8.Body = res8.GetAllBytes();
        resp8.Cmd = CsProtoCmd.CS_CMD_ROOM_INIT_INFO;
       //   client.SendCsProto(resp7);


        //<struct name="CSBackToTown" version="1" desc="进入副本失败返回town">
        //        <entry name="Uin" type="uint" desc="QQ号"/>
        //        <entry name="RoleID" type="int" desc="角色ID"/>
        //        </struct>
        IBuffer res9 = new StreamBuffer();
        res9.WriteUInt32(1, Endianness.Big);
        res9.WriteInt32(1, Endianness.Big);

        CsProtoPacket resp9 = new CsProtoPacket();
        resp9.Body = res9.GetAllBytes();
        resp9.Cmd = CsProtoCmd.CS_CMD_PLAYER_BACK_TO_TOWN;
        //      client.SendCsProto(resp9);


        //      <struct name="CSTownInstanceVerifyRsp" version="1" desc="进入TOWN场景验证响应">
        //      <entry name="ErrNo" type="int" desc="响应码, 0为成功"/>
        //      <entry name="IntanceInitInfo" type="CSInstanceInitInfo" desc="Instance initialize info"/>
        //      <entry name="LineID" type="uint16" desc="所在线ID"/>
        //      <entry name="LevelEnterType" type="int" desc="进入关卡类型，如Townsvr新进，副本返回townsvr，townsvrHub切换等"/>
        //      </struct>
        //<struct name="CSInstanceInitInfo" version="1" desc="Instance initialize info">
        //        <entry name="BattleGroundID" type="int" desc="BattleGround ID"/>
        //        <entry name="LevelID" type="int" desc="Level id"/>
        //        <entry name="CreateMaxPlayerCount" type="int" desc="创建副本的玩家数量"/>
        //        <entry name="GameMode" type="int"/>
        //        <entry name="TimeType" type="int" desc="关卡时间"/>
        //        <entry name="WeatherType" type="int" desc="关卡天气"/>
        //        <entry name="time" type="float"/>
        //        <entry name="LevelRandSeed" type="int" desc="关卡内数据随机种子，特定系统专用"/>
        //        <entry name="WarningFlag" type="uint8" desc="是否是warning"/>
        //        <entry name="CreatePlayerMaxLv" type="int" desc="副本的玩家最大等级"/>
        //        </struct>
        IBuffer res11 = new StreamBuffer();
        res11.WriteInt32(0, Endianness.Big); //ErrNo
        //CSInstanceInitInfo
        res11.WriteInt32(1, Endianness.Big); //BattleGroundID
        res11.WriteInt32(1, Endianness.Big); //LevelID
        res11.WriteInt32(2, Endianness.Big); //CreateMaxPlayerCount
        res11.WriteInt32(0, Endianness.Big); //GameMode
        res11.WriteInt32(0, Endianness.Big); //TimeType
        res11.WriteInt32(0, Endianness.Big); //WeatherType
        res11.WriteFloat(0, Endianness.Big); //time
        res11.WriteInt32(0, Endianness.Big); //LevelRandSeed
        res11.WriteByte(0); //WarningFlag
        res11.WriteInt32(0, Endianness.Big); //CreatePlayerMaxLv

        //CSInstanceInitInfo
        res11.WriteUInt16(0, Endianness.Big); //LineID
        res11.WriteInt32(0, Endianness.Big); //LevelEnterType


        CsProtoPacket resp11 = new CsProtoPacket();
        resp11.Body = res11.GetAllBytes();
        resp11.Cmd = CsProtoCmd.CS_CMD_TOWN_SERVER_INIT_NTF;
         //    client.SendCsProto(resp11);


        //        <struct name="CSSpawnNewPlayer" version="1" desc="创建客户端玩家消息请求">
        //        <entry name="Name" type="string" size="CS_MAX_ROLE_NAME" desc="角色名" sizeinfo="int"/>
        //        </struct>

        IBuffer res12 = new StreamBuffer();
        res12.WriteCString("ShiBa");
        CsProtoPacket resp12 = new CsProtoPacket();
        resp12.Body = res12.GetAllBytes();
        resp12.Cmd = CsProtoCmd.CS_CMD_SPAWN_NEWPLAYER;
       //    client.SendCsProto(resp12);


        //    <struct name="CSAssignPlayerId" version="1" desc="分配新创建玩家的ID号">
        //        <entry name="PlayerId" type="uint" desc="ID号"/>
        //        </struct>
        IBuffer res13 = new StreamBuffer();
        res13.WriteInt32(1, Endianness.Big);
        CsProtoPacket resp13 = new CsProtoPacket();
        resp13.Body = res13.GetAllBytes();
        resp13.Cmd = CsProtoCmd.CS_CMD_ASSIGN_PLAYERID;
       //   client.SendCsProto(resp13);


        //  <struct name="CSNewLineInfo" version="1" desc="c-->s,请求切换TS">
        //        <entry name="LineID" type="uint16" desc="目标线ID"/>
        //        </struct>
        IBuffer res14 = new StreamBuffer();
        res14.WriteUInt16(1, Endianness.Big);
        CsProtoPacket resp14 = new CsProtoPacket();
        resp14.Body = res14.GetAllBytes();
        resp14.Cmd = CsProtoCmd.CS_CMD_SWITCH_LINE;
        //  client.SendCsProto(resp14);


        //   <struct name="CSPlayCutSceneNtf" version="1" desc="服务器通知客户端播放过场动画">
        //   <entry name="CutSceneID" type="uint32" desc="过场动画ID"/>
        //   <entry name="CutSceneType" type="uint32" desc="过场动画类型"/>
        //   </struct>
        IBuffer res15 = new StreamBuffer();
        res15.WriteUInt32(2, Endianness.Big);
        res15.WriteUInt32(1, Endianness.Big);
        CsProtoPacket resp15 = new CsProtoPacket();
        resp15.Body = res15.GetAllBytes();
        resp15.Cmd = CsProtoCmd.CS_CMD_PLAY_CUTSCENE_NTF;
        //client.SendCsProto(resp15);


        //  <struct name="CSSvrStatusNtf" version="1" desc="下发服务器组状态">
        //      <entry name="OnlinePlayerNum" type="int" desc="GM命令名称"/>
        //      <entry name="PlayerInTown" type="int" desc="GM参数1"/>
        //      <entry name="PlayerInBattle" type="int" desc="GM参数2"/>
        //      <entry name="ActiveInstanceCount" type="int" desc="公告内容"/>
        //      <entry name="PlayerInWaitQueue" type="int" desc="公告内容"/>
        //      <entry name="WorldFPS" type="float" desc="WorldSvr FPS"/>
        //      <entry name="TownFPS" type="float" count="10" desc="TownSvr FPS"/>
        //      <entry name="BattleFPS" type="float" count="10" desc="BattleSvr FPS"/>
        //      </struct>
        IBuffer res16 = new StreamBuffer();
        res16.WriteInt32(2, Endianness.Big);
        res16.WriteInt32(1, Endianness.Big);
        res16.WriteInt32(1, Endianness.Big);
        res16.WriteInt32(1, Endianness.Big);
        res16.WriteInt32(1, Endianness.Big);
        res16.WriteFloat(30, Endianness.Big);
        res16.WriteFloat(30, Endianness.Big);
        res16.WriteFloat(30, Endianness.Big);
        CsProtoPacket resp16 = new CsProtoPacket();
        resp16.Body = res16.GetAllBytes();
        resp16.Cmd = CsProtoCmd.CS_CMD_SVRSTATUS_NTF;
        //client.SendCsProto(resp16);
       // return;
        for (int i = 31; i < 100; i++)
        {
            IBuffer rT = new StreamBuffer();
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            rT.WriteInt32(0, Endianness.Big);
            CsProtoPacket rTs = new CsProtoPacket();
            rTs.Body = rT.GetAllBytes();
            rTs.Cmd = (CsProtoCmd)i;
             client.SendCsProto(rTs);
        }
    }
}