using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Constant;
using Arrowgene.MonsterHunterOnline.Protocol.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Protocol.Old;
using Arrowgene.MonsterHunterOnline.Protocol.Old.Structures;
using Arrowgene.MonsterHunterOnline.Protocol.Structures;
using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.IO;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class InstanceVerifyReqHandler : CsProtoStructureHandler<InstanceVerifyReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(InstanceVerifyReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_INSTANCE_VERIFY_REQ;

    private readonly CharacterManager _characterManager;

    private readonly IDatabase _database;

    public InstanceVerifyReqHandler(CharacterManager characterManager, IDatabase database)
    {
        _characterManager = characterManager;
        _database = database;
    }

    public override void Handle(Client client, InstanceVerifyReq req)
    {
        Logger.Trace(client, req.JsonDump());


        Account account = _database.SelectAccountByUin(req.Uin);
        if (account == null)
        {
            Logger.Error(client, $"account == null (Uin:{req.Uin})");
            return;
        }


        Character character = _database.SelectCharacterById((uint)req.RoleId);
        if (character == null)
        {
            Logger.Error(client, $"character == null (RoleId:{req.RoleId})");
            return;
        }

        if (account.Id != character.AccountId)
        {
            Logger.Error(client,
                $"account.Id({account.Id}) != character.AccountId({character.AccountId}) (CharacterId:{character.Id})");
            return;
        }

        client.Account = account;
        client.Character = character;
        client.Inventory = new Inventory(character.Id, _database);

        CsCsProtoStructurePacket<PlayerInitInfo> playerInitInfo = CsProtoResponse.PlayerInitInfo;
        playerInitInfo.Structure.Pose.t.x = 1681.2958f;
        playerInitInfo.Structure.Pose.t.y = 346.80392f;
        playerInitInfo.Structure.Pose.t.z = 205.375f;

        string staticFolder = Path.Combine(Util.ExecutingDirectory(), "Files", "Static");
        string csvSpawnPointsPath = Path.Combine(staticFolder, "SpawnPoints.csv");
        //int level = client.State.levelId;
        int level = req.ServiceId;
        client.State.levelId = level;
        using (TextFieldParser parser = new TextFieldParser(csvSpawnPointsPath))
        {
            string level_comp = level.ToString();
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // Skip the header line
            parser.ReadLine();
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                string levelId = fields[0];
                bool isMatch = (level_comp.Contains(levelId) || levelId.Contains(level_comp));
                if (isMatch)
                {
                    string filename = fields[1];
                    string areaName = fields[2];
                    string pos = fields[3];
                    string rotate = fields[4];

                    //Logger.Info($"warp point match found: ({levelId})({filename})({areaName})({name})");
                    // Process the position (Pos) and rotation (Rotate) values
                    string[] posValues = pos.Split(',');
                    string[] rotateValues = rotate.Split(',');

                    float posX = float.Parse(posValues[0], CultureInfo.InvariantCulture);
                    float posY = float.Parse(posValues[1], CultureInfo.InvariantCulture);
                    float posZ = float.Parse(posValues[2], CultureInfo.InvariantCulture);

                    float rotateX = float.Parse(rotateValues[0], CultureInfo.InvariantCulture);
                    float rotateY = float.Parse(rotateValues[1], CultureInfo.InvariantCulture);
                    float rotateZ = float.Parse(rotateValues[2], CultureInfo.InvariantCulture);
                    float rotateW = float.Parse(rotateValues[3], CultureInfo.InvariantCulture);

                    playerInitInfo.Structure.Pose.t.x = posX;
                    playerInitInfo.Structure.Pose.t.y = posY;
                    playerInitInfo.Structure.Pose.t.z = posZ;
                    playerInitInfo.Structure.Pose.q.v.x = rotateX;
                    playerInitInfo.Structure.Pose.q.v.y = rotateY;
                    playerInitInfo.Structure.Pose.q.v.z = rotateZ;
                    playerInitInfo.Structure.Pose.q.w = rotateW;

                    Logger.Debug($"Warp point found at {posX} {posY} {posZ} for level {level}");
                    break;
                }
            }
        }

        _characterManager.PopulatePlayerInitInfo(client, client.Character, playerInitInfo.Structure);
        client.SendCsProtoStructurePacket(playerInitInfo);

        CsCsProtoStructurePacket<InstanceVerifyRsp> instanceVerifyRsp = CsProtoResponse.InstanceVerifyRsp;
        client.SendCsProtoStructurePacket(instanceVerifyRsp);

        // Send standalone InstanceInitInfo (CMD 668) on battle server to trigger
        // CBattleGround initialization callbacks at CGameLogic+0x6F8.
        CsCsProtoStructurePacket<InstanceInitInfo> instanceInitInfo = CsProtoResponse.InstanceInitInfo;
        instanceInitInfo.Structure.BattleGroundId = 0;
        instanceInitInfo.Structure.LevelId = level;
        instanceInitInfo.Structure.CreateMaxPlayerCount = 4;
        instanceInitInfo.Structure.GameMode = GameMode.Casual;
        instanceInitInfo.Structure.TimeType = TimeType.Noon;
        instanceInitInfo.Structure.WeatherType = WeatherType.Sunny;
        instanceInitInfo.Structure.Time = 1;
        instanceInitInfo.Structure.LevelRandSeed = 1;
        instanceInitInfo.Structure.WarningFlag = 0;
        instanceInitInfo.Structure.CreatePlayerMaxLv = 99;
        client.SendCsProtoStructurePacket(instanceInitInfo);

        // NOTE: CSLoadLevelNtf removed — the client auto-loads the level from
        // InstanceInitInfo.LevelId (received in EnterInstanceRsp from town server).
        // Sending LoadLevelNtf after causes a second load that may interfere with
        // CGameRules initialization from the first load.
    }
}