using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.Chat;

namespace Arrowgene.MonsterHunterOnline.Service;

/// <summary>
///  this is a temporary holder for central management of packet and information,
/// it can be considered the "playground" for now.
///
/// On*- function are lifecycle hooks
/// Send*- functions are to send specific data that has been consistently populated
/// </summary>
public class PlayerState
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(PlayerState));

    private Client _client;

    public CSRoleBaseInfo _roleBaseInfo;

    //  public CSRoleBaseInfo _roleBaseInfo2;
    public CSPlayerInitInfo _playerInitInfo;
    public CSInstanceInitInfo _instanceInitInfo;
    public CSPlayerLevelInitInfo _playerLevelInitInfo;
    public CSSpawnPlayer _spawnPlayer;
    public CSTownInstanceVerifyRsp _townInstanceVerifyRsp;
    public CSEnterInstanceRsp _enterInstanceRsp;
    public CSInstanceVerifyRsp _verifyRsp;
    public CSReselectRoleRsp _reselectRoleRsp;
    public CSPlayerAppearNtf _playerAppearNtf;
    public CSItemListRsp _ItemListRsp;
    public CSTeamInfoNtf _teamInfoNtf;
    public TeamMemberInfo _TeamMemberInfo;
    public int LevelId;

    public PlayerState(Client client)
    {
        LevelId = 150301;

        _client = client;
        _roleBaseInfo = new CSRoleBaseInfo();
        _roleBaseInfo.RoleID = 1;
        _roleBaseInfo.RoleIndex = 0;
        _roleBaseInfo.Name = "Star";
        _roleBaseInfo.Gender = 1;
        _roleBaseInfo.AvatarSetID = 1;
        _roleBaseInfo.Level = 80;
        _roleBaseInfo.HRLevel = 8;
        _roleBaseInfo.FaceId = 1;
        _roleBaseInfo.HairId = 1;
        _roleBaseInfo.UnderclothesId = 1;
        _roleBaseInfo.SkinColor = 1;
        _roleBaseInfo.HairColor = 1;
        _roleBaseInfo.InnerColor = 1;
        _roleBaseInfo.EyeBall = 1;
        _roleBaseInfo.EyeColor = 1;

        // _roleBaseInfo2 = new CSRoleBaseInfo();
        // _roleBaseInfo2.RoleID = 1;
        // _roleBaseInfo2.RoleIndex = 1;
        // _roleBaseInfo2.Name = "Moon";
        // _roleBaseInfo2.Gender = 1;
        // _roleBaseInfo2.Level = 1;
        // _roleBaseInfo2.FaceId = 1;
        // _roleBaseInfo2.HairId = 1;
        // _roleBaseInfo2.UnderclothesId = 1;
        // _roleBaseInfo2.SkinColor = 1;
        // _roleBaseInfo2.HairColor = 1;
        // _roleBaseInfo2.InnerColor = 1;
        // _roleBaseInfo2.EyeBall = 1;
        // _roleBaseInfo2.EyeColor = 1;

        _playerInitInfo = new CSPlayerInitInfo();
        _playerInitInfo.AccountID = 1;
        _playerInitInfo.NetID = 1;
        _playerInitInfo.DBId = 1;
        _playerInitInfo.SessionID = 1;
        _playerInitInfo.WorldID = 1;
        _playerInitInfo.ServerID = 1;
        _playerInitInfo.WorldSvrID = 1;
        _playerInitInfo.Name = _roleBaseInfo.Name;
        _playerInitInfo.Gender = _roleBaseInfo.Gender;
        _playerInitInfo.IsGM = 1;
        _playerInitInfo.AvatarSetID = _roleBaseInfo.AvatarSetID;
        _playerInitInfo.ParentEntityGUID = 0;
        _playerInitInfo.RandSeed = 1;
        _playerInitInfo.CatCuisineID = 0;
        _playerInitInfo.FirstEnterLevel = 0;
        _playerInitInfo.FirstEnterMap = 0;
        _playerInitInfo.PvpPrepareStageState = 0;
        _playerInitInfo.ServerTime = 100;

        //spawn location
        _playerInitInfo.Pose.t.x = 409.91379f;
        _playerInitInfo.Pose.t.y = 358.74976f;
        _playerInitInfo.Pose.t.z = 100.0f; // height

        _playerInitInfo.Pose.q.v.x = 10;
        _playerInitInfo.Pose.q.v.y = 10;
        _playerInitInfo.Pose.q.v.z = 10;
        _playerInitInfo.Pose.q.w = 10;


        _playerInitInfo.StoreSize = 20;
        _playerInitInfo.NormalSize = 20;
        _playerInitInfo.MaterialStoreSize = 20;

        _playerInitInfo.Weapon = 1;

        //   for (int i = 1; i < 40; i++)
        //   {
        //       _playerInitInfo.EquipItem.Add((byte)i);
        //       _playerInitInfo.Attr.Add((byte)i);
        //       _playerInitInfo.BagItem.Add((byte)i);
        //       _playerInitInfo.StoreItem.Add((byte)i);
        //       _playerInitInfo.Buff.Add((byte)i);
        //       _playerInitInfo.Skill.Add((byte)i);
        //       _playerInitInfo.CD.Add((byte)i);
        //       _playerInitInfo.Star.Add((byte)i);
        //       _playerInitInfo.FacialInfo[i] = (byte)i;
        //       _playerInitInfo.Spoor.Add((byte)i);
        //   }

        StreamBuffer ast = new StreamBuffer();
        ast.WriteUInt32(1, Endianness.Big); //attr id
        ast.WriteUInt16(1, Endianness.Big); //attr type = CS_ATTR_DATA_BASE
        ast.WriteUInt16(1, Endianness.Big); //attr type (CS_PROP_SYNC_TYPE) = CS_PROP_SYNC_INT
        ast.WriteInt32(20, Endianness.Big);

        _playerInitInfo.Attr = new List<byte>(ast.GetAllBytes());
        
      //    <macrosgroup name="CS_PROP_SYNC_TYPE">
      //    <macro name="CS_PROP_SYNC_INT" value="1" desc="符号整数" />
      //    <macro name="CS_PROP_SYNC_FLOAT" value="2" desc="单精度浮点数" />
      //    <macro name="CS_PROP_SYNC_STRING" value="3" desc="字符串" />
      //    <macro name="CS_PROP_SYNC_BOOL" value="4" desc="布尔" />
      //    <macro name="CS_PROP_SYNC_VEC3" value="5" desc="向量" />
      //    <macro name="CS_PROP_SYNC_UINT64" value="6" desc="uint64" />
      //    </macrosgroup>
              //    <struct name="CSAttrBaseData" version="1">
              //    <entry name="Type" type="ushort" desc="类型枚举" bindmacrosgroup="CS_PROP_SYNC_TYPE"/>
              //    <entry name="Value" type="CSAttrValue" desc="变量值" select="Type"/>
              //    </struct>
              //       <struct name="CSAttrData" version="1">
              //       <entry name="AttrID" type="uint"/>
              //       <entry name="Type" type="ushort" desc="类型枚举" bindmacrosgroup="CS_ATTR_DATA_TYPE"/>
              //       <entry name="Value" type="CSAttrDataUnion" desc="变量值" select="Type"/>
              //       </struct>

              //   <struct name="CSAttrInit" version="1">
              //       <entry name="EntityID" type="uint"/>
              //       <entry name="Count" type="short"/>
              //       <entry name="Attr" type="CSAttrData" count="CS_ATTR_INIT_MAX" refer="Count"/>
              //       </struct>
  
// <macrosgroup name="CS_ATTR_DATA_TYPE">
//           <macro name="CS_ATTR_DATA_BASE" value="1" />
//           <macro name="CS_ATTR_DATA_BONUS" value="2" />
//           </macrosgroup>

              _playerInitInfo.FacialInfo[0] = 1;
        _playerInitInfo.FacialInfo[1] = 1;

        _instanceInitInfo = new CSInstanceInitInfo();
        _instanceInitInfo.BattleGroundID = 0;
        _instanceInitInfo.LevelID = LevelId;
        _instanceInitInfo.CreateMaxPlayerCount = 4;
        _instanceInitInfo.GameMode = 99;
        _instanceInitInfo.TimeType = 1;
        _instanceInitInfo.WeatherType = 1;
        _instanceInitInfo.time = 1;
        _instanceInitInfo.LevelRandSeed = 1;
        _instanceInitInfo.WarningFlag = 0;
        _instanceInitInfo.CreatePlayerMaxLv = 99;
        //    <LevelInfo>
        //    <Difficulty>
        //    <Info Name="Easy" ID="1"/>
        //    <Info Name="Normal" ID="2"/>
        //    <Info Name="Hard" ID="3"/>
        //    <Info Name="Insane" ID="4"/>
        //    </Difficulty>
        //    <GameMode>
        //    <Info Name="Standard" ID="1"/>
        //    <Info Name="Adventure" ID="2"/>
        //    <Info Name="Casual" ID="3"/>
        //    <Info Name="Arena" ID="4"/>
        //    <Info Name="ThousandsHunter" ID="5"/>
        //    <Info Name="Raid" ID="6"/>
        //    <Info Name="Survive" ID="7"/>
        //    <Info Name="Story" ID="8"/>
        //    <Info Name="Training" ID="9"/>
        //    <Info Name="Testing" ID="10"/>
        //    <Info Name="WaterFight" ID="11"/>
        //    <Info Name="HunterCraft" ID="12"/>
        //    <Info Name="DuelOfHunter" ID="13"/>
        //    <Info Name="Airship" ID="14"/>
        //    <Info Name="Extreme" ID="15"/>
        //    <Info Name="Bouns" ID="16"/>
        //    <Info Name="PvP" ID="17"/>
        //    <Info Name="Tutorial" ID="23"/>
        //    <Info Name="Elite" ID="26"/>
        //    <Info Name="Extreme" ID="33"/>
        //    <Info Name="SingleElite" ID="34"/>
        //    <Info Name="Town" ID="99"/>
        //    </GameMode>
        //    <Weather>
        //    <Info Name="Sunny" ID="1"/>
        //    <Info Name="SmallRain" ID="2"/>
        //    <Info Name="HeavyRain" ID="4"/>
        //    <Info Name="Snow" ID="8"/>
        //    <Info Name="Blizzard" ID="16"/>
        //    <Info Name="Foggy" ID="32"/>
        //    <Info Name="Cloudy" ID="64"/>
        //    <Info Name="AfterRain" ID="128"/>
        //    <Info Name="SandStorm" ID="256"/>
        //    </Weather>
        //    <Time>
        //    <Info Name="Morning" ID="1"/>
        //    <Info Name="Noon" ID="2"/>
        //    <Info Name="Dusk" ID="4"/>
        //    <Info Name="Night" ID="8"/>
        //    </Time>
        //    <RegionType>
        //    <Info Name="type1" ID="1"/>
        //    <Info Name="type2" ID="2"/>
        //    <Info Name="type3" ID="3"/>
        //    </RegionType>
        //    </LevelInfo>

        _playerLevelInitInfo = new CSPlayerLevelInitInfo();
        _playerLevelInitInfo.UnLockLevelData.Add(new PlayerUnlockLevelInfo()
        {
            LevelID = 1
        });

        _spawnPlayer = new CSSpawnPlayer();
        _spawnPlayer.PlayerId = 1;
        _spawnPlayer.NetObjId = 1;
        _spawnPlayer.Name = _roleBaseInfo.Name;
        _spawnPlayer.Gender = _roleBaseInfo.Gender;
        _spawnPlayer.Scale = 1.0f;
        _spawnPlayer.NewConnect = 1;
        _spawnPlayer.SendSrvId = 1;
        _spawnPlayer.AvatarSetID = _roleBaseInfo.AvatarSetID;
        _spawnPlayer.Position = new XYZPosition()
        {
            x = _playerInitInfo.Pose.t.x,
            y = _playerInitInfo.Pose.t.y,
            z = _playerInitInfo.Pose.t.z
        };

        _townInstanceVerifyRsp = new CSTownInstanceVerifyRsp();
        _townInstanceVerifyRsp.IntanceInitInfo = _instanceInitInfo;
        _townInstanceVerifyRsp.LineID = 0;
        _townInstanceVerifyRsp.LevelEnterType = 0;

        _enterInstanceRsp = new CSEnterInstanceRsp();
        _enterInstanceRsp.InstanceID = 0;
        _enterInstanceRsp.Key = "test";
        _enterInstanceRsp.BattleSvr = "127.0.0.1:8143";
        _enterInstanceRsp.RoleId = 0;
        _enterInstanceRsp.ServiceID = 0;
        _enterInstanceRsp.InstanceInfo = _instanceInitInfo;

        _verifyRsp = new CSInstanceVerifyRsp();
        _verifyRsp.ErrNo = 0;

        _reselectRoleRsp = new CSReselectRoleRsp();


        _playerAppearNtf = new CSPlayerAppearNtf();
        _playerAppearNtf.NetID = 1;
        _playerAppearNtf.SessionID = 1;
        _playerAppearNtf.Name = _roleBaseInfo.Name;
        _playerAppearNtf.Gender = _roleBaseInfo.Gender;
        _playerAppearNtf.Pose = _playerInitInfo.Pose;
        _playerAppearNtf.AvatarSetID = _roleBaseInfo.AvatarSetID;

        _ItemListRsp = new CSItemListRsp();

        _TeamMemberInfo = new TeamMemberInfo();
        _TeamMemberInfo.NetId = (uint)_playerInitInfo.NetID;
        _TeamMemberInfo.DBId = _playerInitInfo.DBId;
        _TeamMemberInfo.Name = _roleBaseInfo.Name;
        _TeamMemberInfo.Level = (uint)_roleBaseInfo.Level;
        _TeamMemberInfo.LevelId = (uint)LevelId;
        _TeamMemberInfo.LevelExtra = 1;
        _TeamMemberInfo.Sex = _roleBaseInfo.Gender;
        _TeamMemberInfo.WeaponType = 1;
        _TeamMemberInfo.Star = 1;
        _TeamMemberInfo.CanBeKicked = 0;
        _TeamMemberInfo.Online = 1;
        _TeamMemberInfo.Vec3.x = 10;
        _TeamMemberInfo.Vec3.y = 10;
        _TeamMemberInfo.Vec3.z = 10;
        _TeamMemberInfo.Dir.x = 10;
        _TeamMemberInfo.Dir.y = 10;
        _TeamMemberInfo.Dir.z = 10;
        _TeamMemberInfo.LineId = 1;
        _TeamMemberInfo.HRLevel = _roleBaseInfo.HRLevel;
        _TeamMemberInfo.HunterStar = _roleBaseInfo.StarLevel;
        _TeamMemberInfo.Zone = 1;
        _TeamMemberInfo.WeaponTitle = 1;

        _teamInfoNtf = new CSTeamInfoNtf();
        _teamInfoNtf.Team.TeamId = 1;
        _teamInfoNtf.Team.TeamName = "StarTeam";
        _teamInfoNtf.Team.MemberMax = 4;
        _teamInfoNtf.Team.FreeJoin = 1;
        _teamInfoNtf.Team.HasPwd = 0;
        _teamInfoNtf.Team.Pwd = "";
        _teamInfoNtf.Team.OpenRecruit = 1;
        _teamInfoNtf.Team.MinLevel = 1;
        _teamInfoNtf.Team.MaxLevel = 1;
        _teamInfoNtf.Team.MinStar = 1;
        _teamInfoNtf.Team.MaxStar = 4;
        _teamInfoNtf.Team.TargetMap = 1;
        _teamInfoNtf.Team.TargetMode = 1;
        _teamInfoNtf.Team.TargetLevelGrp = 1;
        _teamInfoNtf.Team.Difficulty = 1;
        _teamInfoNtf.Team.LeaderDBID = 1;
        _teamInfoNtf.Team.LeaderID = 1;
        _teamInfoNtf.Team.CreateTime = 1;
        _teamInfoNtf.Team.TownSvr = 1;
        _teamInfoNtf.Team.BattleSvr = 1;
        _teamInfoNtf.Team.Members.Add(_TeamMemberInfo);
    }

    public void OnChatMsg(ChatMessage chatMessage)
    {
        if (chatMessage.Message == "test")
        {
            

            StreamBuffer ast = new StreamBuffer();
            ast.WriteUInt32(1, Endianness.Big); //EntityID
            ast.WriteUInt32(73, Endianness.Big); //attr id
            ast.WriteInt16(1, Endianness.Big); //BonusID
            ast.WriteUInt16(1, Endianness.Big); //attr type (CS_PROP_SYNC_TYPE) = CS_PROP_SYNC_INT
            ast.WriteInt32(100, Endianness.Big);
            CsProtoPacket csp = new CsProtoPacket();
            csp.Cmd = CS_CMD_ID.CS_CMD_ATTR_SYNC_NTF;
            csp.Body = ast.GetAllBytes();
            _client.SendCsProto(csp);
                        
    //   ast.SetPositionStart();
    //   ast.WriteUInt32(1, Endianness.Big); //EntityID
    //   ast.WriteUInt32(1, Endianness.Big); //attr id
    //   ast.WriteInt16(1, Endianness.Big); //BonusID
    //   ast.WriteUInt16(1, Endianness.Big); //attr type (CS_PROP_SYNC_TYPE) = CS_PROP_SYNC_INT
    //   ast.WriteInt32(22, Endianness.Big);
    //   csp = new CsProtoPacket();
    //   csp.Cmd = CS_CMD_ID.CS_CMD_ATTR_SYNC_NTF;
    //   csp.Body = ast.GetAllBytes();
    //   _client.SendCsProto(csp);
    //   
    //   ast.SetPositionStart();
    //   ast.WriteUInt32(1, Endianness.Big); //EntityID
    //   ast.WriteUInt32(74, Endianness.Big); //attr id
    //   ast.WriteInt16(1, Endianness.Big); //BonusID
    //   ast.WriteUInt16(1, Endianness.Big); //attr type (CS_PROP_SYNC_TYPE) = CS_PROP_SYNC_INT
    //   ast.WriteInt32(1000, Endianness.Big);
    //   csp = new CsProtoPacket();
    //   csp.Cmd = CS_CMD_ID.CS_CMD_ATTR_SYNC_NTF;
    //   csp.Body = ast.GetAllBytes();
    //   _client.SendCsProto(csp);
    //   
    //   ast.SetPositionStart();
    //   ast.WriteUInt32(1, Endianness.Big); //EntityID
    //   ast.WriteUInt32(20, Endianness.Big); //attr id
    //   ast.WriteInt16(20, Endianness.Big); //BonusID
    //   ast.WriteUInt16(2, Endianness.Big); //attr type (CS_PROP_SYNC_TYPE) = CS_PROP_SYNC_INT
    //   ast.WriteFloat(100, Endianness.Big);
    //   csp = new CsProtoPacket();
    //   csp.Cmd = CS_CMD_ID.CS_CMD_ATTR_SYNC_NTF;
    //   csp.Body = ast.GetAllBytes();
    //   _client.SendCsProto(csp);
    //   
    //   ast.SetPositionStart();
    //   ast.WriteUInt32(1, Endianness.Big); //EntityID
    //   ast.WriteUInt32(21, Endianness.Big); //attr id
    //   ast.WriteInt16(21, Endianness.Big); //BonusID
    //   ast.WriteUInt16(1, Endianness.Big); //attr type (CS_PROP_SYNC_TYPE) = CS_PROP_SYNC_INT
    //   ast.WriteFloat(100, Endianness.Big);
    //   csp = new CsProtoPacket();
    //   csp.Cmd = CS_CMD_ID.CS_CMD_ATTR_SYNC_NTF;
    //   csp.Body = ast.GetAllBytes();
    //   _client.SendCsProto(csp);
    //   
    //   ast.SetPositionStart();
    //   ast.WriteUInt32(1, Endianness.Big); //EntityID
    //   ast.WriteUInt32(2, Endianness.Big); //attr id
    //   ast.WriteInt16(2, Endianness.Big); //BonusID
    //   ast.WriteUInt16(3, Endianness.Big); //attr type (CS_PROP_SYNC_TYPE) = CS_PROP_SYNC_INT
    //   string name = "testa";
    //   ast.WriteInt32(name.Length + 1, Endianness.Big);
    //   ast.WriteCString(name);
    //   csp = new CsProtoPacket();
    //   csp.Cmd = CS_CMD_ID.CS_CMD_ATTR_SYNC_NTF;
    //   csp.Body = ast.GetAllBytes();
    //   _client.SendCsProto(csp);

             //  <entry name="EntityID" type="uint"/>
             //  <entry name="AttrID" type="uint"/>
             //  <entry name="BonusID" type="short"/>
             //  <entry name="Data" type="CSAttrBaseData" desc="变量值"/>
       //   _client.SendCsPacket(NewCsPacket.TokenSync(new CSTokenSync()
       //   {
       //       Entries = new List<CSTokenSyncEntry>()
       //       {
       //           new CSTokenSyncEntry(
       //               new CSTokenSyncVar(CS_TOKEN_SYNC_TYPE.CS_TOKEN_SYNC_ENTITYID)
       //               {
       //                   EntityID = 1
       //               }
       //           )
       //           {
       //               Name = "localplayer"
       //           }
       //       }
       //   }));

       //   _client.SendCsPacket(NewCsPacket.EntityAppearNtfIDList(new CSEntityAppearNtfIDList()
       //   {
       //       InitType = 1,
       //       LogicEntityID = new List<uint>() { 1 },
       //       LogicEntityType = new List<uint>() { 1 }
       //   }));
            return;
            _client.SendCsPacket(NewCsPacket.PlayerQueryInfo(new CSPlayerQueryInfo()
            {
                ErrNo = 0,
                NetID = 1,
                SessionID = 1,
                Name = "FAFA",
                Gender = 0,
                AvatarSetID = 1,
                Weapon = 0,
                WeaponAtkFlag = 0,
                HunterStar = "asdasd"
            }));

            _client.SendCsPacket(NewCsPacket.SpawnSrvEnt(new CSSpawnSrvEnt()
            {
                Name = _roleBaseInfo.Name,
                NetObjID = 1,
                Position = new XYZPosition()
                {
                    x = _playerInitInfo.Pose.t.x,
                    y = _playerInitInfo.Pose.t.y,
                    z = _playerInitInfo.Pose.t.z
                },
                Rotation = new Quaternion(),
                Scale = 2.0f
            }));

            _client.SendCsPacket(NewCsPacket.NewPlayer(new CSSpawnNewPlayer()
            {
                Name = _roleBaseInfo.Name
            }));

            _client.SendCsPacket(NewCsPacket.AssignId(new CSAssignPlayerId()
            {
                PlayerId = 1
            }));

            SendPlayerSpawn();

            _client.SendCsPacket(NewCsPacket.PlayerAppearNtf(_playerAppearNtf));

            CSBattleEntitySpeed spd = new CSBattleEntitySpeed();
            spd.NetObjId = 1;
            spd.IsStart = 1;
            spd.InitSpeed.x = 10;
            spd.InitSpeed.y = 10;
            spd.InitSpeed.z = 10;
            spd.Accelator.x = 10;
            spd.Accelator.y = 10;
            spd.Accelator.z = 10;
            spd.InitAngleSpeed = 10;
            spd.AngleAccelator = 10;
            _client.SendCsPacket(NewCsPacket.EntitySpeed(spd));

            _client.SendCsPacket(NewCsPacket.ObjectAction(new CSObjectActionSyncEntry(
                    new CSTeleportParam()
                    {
                        targetPos = new CSVec3() { x = 1.0f, y = 1.0f, z = 1.0f }
                    })
                {
                    EntityId = 1
                }
            ));

            _client.SendCsPacket(NewCsPacket.ActorLocomotion(new CSActorLocomotion()
            {
                NetObjId = 1,
                SyncTime = 0,
                MoveType = 1,
                UserDataF1 = 10.0f,
                UserDataF2 = 10.0f,
                UserDataF3 = 10.0f,
                UserDataF4 = 10.0f,
                UserDataF5 = 10.0f,
                UserDataV1 = new CSVec3() { x = 10.0f, y = 10.0f, z = 10.0f },
                UserDataU1 = 10,
                UserDataU2 = 10,
                UserDataI1 = 10,
                UserDataS1 = "",
                UserDataS2 = "",
            }));
        }
    }

    /// <summary>
    /// Since FileCheck could occur multiple times during connection,
    /// i chose multi_net_ip info, hoping that its only once on game startup called.
    /// ideally want to only send this packet once when the game loaded.
    /// the problem is via the CsProto over TDPU connection, is that we do not get a tcp connection / disconnect event.
    /// but for future battle server connectivity weg get those, but we still need to be able to manage both the same way.
    /// </summary>
    public void OnReady()
    {
        SendListRoleRsp();
    }

    /// <summary>
    /// client selected a role to play, unsure as of yet what to reply.
    /// Based on logs we are hitting the right spots, but there is still more missing.
    /// </summary>
    public void OnRoleSelected()
    {
        _client.SendCsPacket(NewCsPacket.SelecteRoleRsp(new CSSelectRoleRsp()));
        SendTownSessionStart();
        SendPlayerInitNtf();
    }

    /// <summary>
    /// client used menu from level -> char selection
    /// </summary>
    public void OnRoleReSelected()
    {
        SendListRoleRsp();
    }

    /// <summary>
    /// Need to send after client has processed player init ntf, currently choose a random packet, that occurs after.
    /// Hoping it will not be asked again in the future.
    /// </summary>
    public void OnPlayerInitFinished()
    {
        SendTownServerInitNtf();
    }

    /// <summary>
    /// we have entered level, now need a way to assing a NetID for movement sync.
    /// </summary>
    public void OnEnterLevel()
    {
        // all packets here seem not to be required
        // just testing

        SendPlayerLevelInitNtf();


        //  SendBruteForce();


        // //
    }

    /// <summary>
    /// in game press M-key -> select another town
    /// </summary>
    public void OnChangeTownInstance(CSChangeTownInstanceReq req)
    {
        //  TODO req tells us spawn position name, the coordinates should be in levels/xx/mission .xml 
        _client.SendCsPacket(NewCsPacket.ChangeTownInstanceRsp(new CSChangeTownInstanceRsp()
        {
            ErrCode = 0,
            LevelID = req.LevelId
        }));
        _instanceInitInfo.LevelID = req.LevelId;
        SendTownServerInitNtf();
    }

    public void OnBattleSvr()
    {
        //SendPlayerLevelInitNtf();
        //SendTownServerInitNtf();
        //SendPlayerSpawn();
        //SendLoadLevelNtf();
        //SendInstanceInitNtf();
        //SendPlayerInitNtf();
    }

    public void OnCreateRole(CSRoleCreateInfo info)
    {
        CSListRoleRsp roleRsp = new CSListRoleRsp();
        _roleBaseInfo.Name = info.Name;
        _roleBaseInfo.Gender = info.Gender;
        _roleBaseInfo.FaceId = info.FaceId;
        _roleBaseInfo.HairId = info.HairId;
        _roleBaseInfo.UnderclothesId = info.UnderclothesId;
        _roleBaseInfo.SkinColor = info.SkinColor;
        _roleBaseInfo.HairColor = info.HairColor;
        _roleBaseInfo.InnerColor = info.InnerColor;
        _roleBaseInfo.EyeBall = info.EyeBall;
        _roleBaseInfo.EyeColor = info.EyeColor;
        _roleBaseInfo.FaceTattooIndex = info.FaceTattooIndex;
        _roleBaseInfo.FaceTattooColor = info.FaceTattooColor;
        _roleBaseInfo.FacialInfo = info.FacialInfo;
        roleRsp.RoleList.Role.Add(_roleBaseInfo);
        _client.SendCsPacket(NewCsPacket.CreateRoleRsp(roleRsp));
    }

    public void SendBruteForceT()
    {
        Thread bruteForce = new Thread(SendBruteForce);
        bruteForce.Start();
    }

    public void SendBruteForce()
    {
        Thread.Sleep(3000);

        List<string> exclude = new List<string>()
        {
            "CatTreatureErr",
            "CatTreatureOpen",
            "XHunterResultNtf",
            "CommanderChgNtf",
            "BattlePunishNtf",
            "ExcellentPointNtf",
            "SensitiveVerify",
            "SensitiveVerifyResult",
            "CatCuisineUnlockNtf",
            "C2STalkExec",
            "C2STalkEnd",
            "S2CTalkErr",
            "S2CTalkExec",
            "EquipPlanUnlockNtf",
            "EquipPlanEditNtf",
            "LineUpStateNtf",
            "GuildMatchSignUpReadyNtf",
            "GuildMatchSignUpListNtf",
            "GuildMatchSignUpAdd",
            "GuildMatchSignUpDel",
            "ScheduleError",
            "ObtainTargetListRes",
            "GuildMatchQualifierFirstNtf",
            "GuildMatchStateNtf",
            "GuildMatchPairListNtf",
            "PkgEncryptData",
            "Ping",
            "HeartBeat",
            "GameManagerCmd",
            "GlobalErrNtf",
            "PkgTimerRecord",
            "PkgTransAntiData",
            "PingReply",
            "PkgChatEncryptData",
            "ZipPkg",
            "DelBuff",
            "BuffParamChange",
            "NotifyInfo",
            "DropClientNotifyInfo",
            "PropSync",
            "TimeOfDayNtf",
            "AttrSync",
            "AttrInfo",
            "FarmSetEquipAvatarNtf",
            "FarmWoodIndexIDMappingNtf",
            "CatTreatureInfo",
            "C2SSpeakWord",
            "C2SSpeakSetSelfDef",
            "C2SSpeakSetAuto",
            "S2CSculptureLibSnapshot",
            "S2CGetSculptureLibs",
            "S2CSculptureErr",
            "S2CSpeakErr",
            "SurrenderVoteNtf",
            "C2SGetSculptureLibs",
            "S2CGetSculpture",
            "S2CSculptureAvatarSnapshot",
            "S2CScriptAddSculpture",
            "S2CSpoorFetchPrize",
            "S2CSpoorErr",
            "S2CSpoorAddPoints",
            "ItemMgrMoveSwapItemsNtf",
        };

        Type type = typeof(NewCsPacket);
        List<MethodInfo> collectedMethods = new List<MethodInfo>();
        foreach (MethodInfo mi in type.GetMethods())
        {
            if (exclude.Contains(mi.Name))
            {
                continue;
            }

            if (mi.Name == "Equals"
                || mi.Name == "GetType"
                || mi.Name == "ToString"
                || mi.Name == "GetHashCode")
            {
                continue;
            }

            if (mi.Name.EndsWith("Req"))
            {
                continue;
            }

            if (mi.Name.StartsWith("C2S"))
            {
                continue;
            }

            if (mi.Name.EndsWith("Rsp"))
            {
                continue;
            }

            collectedMethods.Add(mi);
        }


        //LevelIntegrateNtf
//2023-06-25 05:47:10 - Error - PlayerState: Sending S2CScriptActivityTimeUpdateNtf (28/206)

        int start = 80;
        int current = 1;
        int total = collectedMethods.Count;
        foreach (MethodInfo mi in collectedMethods)
        {
            if (current < start)
            {
                current++;
                continue;
            }

            ParameterInfo[] parameters = mi.GetParameters();
            if (parameters.Length != 1)
            {
                Logger.Error($"parameters.Length != 1 ({mi})");
                continue;
            }

            object parameterInstance = CreateInstance(parameters[0].ParameterType);
            object ret = mi.Invoke(null, new object[] { parameterInstance });
            if (ret is CsPacket csPacket)
            {
                Logger.Error($"Sending {mi.Name} ({current}/{total})");
                try
                {
                    _client.SendCsPacket(csPacket);
                }
                catch (Exception ex)
                {
                    Logger.Exception(ex);
                }

                Thread.Sleep(1000);
                current++;
            }
            else
            {
                Logger.Error($"ret is NOT CsPacket csPacket({mi})");
            }
        }
    }

    private object CreateInstance(Type type)
    {
        if (type.IsInterface)
        {
            Type concrete = null;
            foreach (Type t in Assembly.GetExecutingAssembly().GetExportedTypes())
            {
                if (!t.IsInterface && !t.IsAbstract && type.IsAssignableFrom(t))
                {
                    concrete = t;
                    break;
                }
            }

            if (concrete == null)
            {
                Logger.Error($"Interface ({type}) can not be created, no implementation found");
                return null;
            }

            Logger.Info($"Interface ({type}) can not be created, using implementation {concrete}");
            type = concrete;
        }

        ConstructorInfo[] cis = type.GetConstructors();

        if (cis.Length <= 0)
        {
            return Activator.CreateInstance(type);
        }

        ParameterInfo[] pis = cis[0].GetParameters();
        if (pis.Length <= 0)
        {
            return Activator.CreateInstance(type);
        }

        object[] paramInstances = new object[pis.Length];
        for (int i = 0; i < pis.Length; i++)
        {
            paramInstances[i] = CreateInstance(pis[i].ParameterType);
        }

        return Activator.CreateInstance(type, paramInstances);
    }

    public void SendListRoleRsp()
    {
        CSListRoleRsp roleRsp = new CSListRoleRsp();
        // if role list is empty, client will auto move to char creation
        roleRsp.RoleList.Role.Add(_roleBaseInfo);
        // roleRsp.RoleList.Role.Add(_roleBaseInfo2);
        _client.SendCsPacket(NewCsPacket.ListRoleRsp(roleRsp));
    }

    /// <summary>
    /// Changes the light settings of current scene
    /// </summary>
    public void SendTimeOfDayNtf()
    {
        CSTimeOfDayNtf timeOfDayNtf = new CSTimeOfDayNtf();
        timeOfDayNtf.time = 50;
        _client.SendCsPacket(NewCsPacket.TimeOfDayNtf(timeOfDayNtf));
    }

    public void SendTownSessionStart()
    {
        _client.SendCsPacket(NewCsPacket.TownSessionStart(new CSTownSessionStart()));
    }

    public void SendPlayerInitNtf()
    {
        _client.SendCsPacket(NewCsPacket.PlayerInitNtf(_playerInitInfo));
    }

    public void SendInstanceInitNtf()
    {
        _client.SendCsPacket(NewCsPacket.InstanceInitNtf(_instanceInitInfo));
    }

    public void SendPlayerLevelInitNtf()
    {
        _client.SendCsPacket(NewCsPacket.PlayerLevelInitNtf(_playerLevelInitInfo));
    }

    public void SendPlayerSpawn()
    {
        _client.SendCsPacket(NewCsPacket.SpawnPlayer(_spawnPlayer));
    }

    public void SendLoadLevelNtf()
    {
        _client.SendCsPacket(NewCsPacket.LoadLevelNtf(new CSLoadLevelNtf()));
    }

    public void SendTownServerInitNtf()
    {
        _client.SendCsPacket(NewCsPacket.TownServerInitNtf(_townInstanceVerifyRsp));
    }

    public void SendEnterInstanceRsp()
    {
        _client.SendCsPacket(NewCsPacket.EnterInstanceRsp(_enterInstanceRsp));
    }

    public void SendInstanceVerifyRsp()
    {
        _client.SendCsPacket(NewCsPacket.InstanceVerifyRsp(_verifyRsp));
    }

    public void SendReselectRoleRsp()
    {
        _client.SendCsPacket(NewCsPacket.ReselectRoleRsp(_reselectRoleRsp));
    }
}