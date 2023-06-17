using Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

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
    private Client _client;
    private CsRoleBaseInfo _roleBaseInfo;
    private CsPlayerInitInfo _playerInitInfo;
    private CsInstanceInitInfo _instanceInitInfo;
    private CsPlayerLevelInitInfo _playerLevelInitInfo;


    public PlayerState(Client client)
    {
        _client = client;
        _roleBaseInfo = new CsRoleBaseInfo();
        _roleBaseInfo.RoleId = 1;
        _roleBaseInfo.RoleIndex = 1;
        _roleBaseInfo.Name = "Star";
        _roleBaseInfo.Gender = 1;
        _roleBaseInfo.Level = 1;

        _playerInitInfo = new CsPlayerInitInfo();
        _playerInitInfo.AccountId = 1;
        _playerInitInfo.NetId = 1;
        _playerInitInfo.DbId = 1;
        _playerInitInfo.SessionId = 1;
        _playerInitInfo.WorldId = 1;
        _playerInitInfo.ServerId = 1;
        _playerInitInfo.WorldSvrId = 1;
        _playerInitInfo.Name = _roleBaseInfo.Name;
        _playerInitInfo.Gender = _roleBaseInfo.Gender;
        _playerInitInfo.IsGM = 1;

        _instanceInitInfo = new CsInstanceInitInfo();
        _instanceInitInfo.BattleGroundId = 1;
        _instanceInitInfo.LevelId = 1;
        _instanceInitInfo.CreateMaxPlayerCount = 1;
        _instanceInitInfo.GameMode = 1;
        _instanceInitInfo.TimeType = 1;
        _instanceInitInfo.WeatherType = 1;
        _instanceInitInfo.Time = 0;
        _instanceInitInfo.LevelRandSeed = 1;
        _instanceInitInfo.WarningFlag = 0;
        _instanceInitInfo.CreatePlayerMaxLv = 99;

        _playerLevelInitInfo = new CsPlayerLevelInitInfo();
    }


    /// <summary>
    /// This is called when the client finished all checks.
    /// At the moment it looks like we need to manually send the Roles,
    /// this will cause characters to show up.
    /// if the role list is empty, you will be put into character creation screen.
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
        SendTownSessionStart();
        SendPlayerInitNtf();

        //  SendPlayerLevelInitNtf();
        //   SendInstanceInitNtf();
    }


    public void SendListRoleRsp()
    {
        ListRoleRsp roleRsp = new ListRoleRsp();
        // if role list is empty, client will auto move to char creation
        roleRsp.CsListRoleRsp.Roles.Add(_roleBaseInfo);
        _client.SendCsPacket(roleRsp);
    }

    /// <summary>
    /// Changes the light settings of current scene
    /// </summary>
    public void SendTimeOfDayNtf()
    {
        TimeOfDayNtf timeOfDayNtf = new TimeOfDayNtf();
        timeOfDayNtf.Time = 50;
        _client.SendCsPacket(timeOfDayNtf);
    }

    public void SendTownSessionStart()
    {
        TownSessionStart townSessionStart = new TownSessionStart();
        _client.SendCsPacket(townSessionStart);
    }

    public void SendPlayerInitNtf()
    {
        PlayerInitNtf playerInitNtf = new PlayerInitNtf();
        playerInitNtf.CSPlayerInitInfo = _playerInitInfo;
        _client.SendCsPacket(playerInitNtf);
    }

    public void SendInstanceInitNtf()
    {
        InstanceInitNtf instanceInitNtf = new InstanceInitNtf();
        instanceInitNtf.CsInstanceInitInfo = _instanceInitInfo;
        _client.SendCsPacket(instanceInitNtf);
    }

    public void SendPlayerLevelInitNtf()
    {
        PlayerLevelInitNtf playerLevelInitNtf = new PlayerLevelInitNtf();
        playerLevelInitNtf.CsPlayerLevelInitInfo = _playerLevelInitInfo;
        _client.SendCsPacket(playerLevelInitNtf);
    }
}