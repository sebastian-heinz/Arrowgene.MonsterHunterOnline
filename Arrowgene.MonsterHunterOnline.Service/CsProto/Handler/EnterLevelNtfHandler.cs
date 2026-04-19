using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Constant;
using Arrowgene.MonsterHunterOnline.Protocol.Old.Structures;
using Arrowgene.MonsterHunterOnline.Protocol.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;
using Microsoft.VisualBasic.FileIO;
using System.Globalization;
using System.IO;
using System.Threading;
using Arrowgene.MonsterHunterOnline.Protocol.Old;
using Arrowgene.MonsterHunterOnline.Protocol.Old.Structures;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class EnterLevelNtfHandler : CsProtoStructureHandler<EnterLevelNtf>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(EnterLevelNtfHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_ENTER_LEVEL_NTF;

    private readonly CharacterManager _characterManager;

    public EnterLevelNtfHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, EnterLevelNtf req)
    {
        // CMD 516 (SpawnPlayer) — create the player entity in the battle instance.
        // Must be sent BEFORE NPC/monster spawns. Without this, the game logic
        // context doesn't fully initialize and CMD 662 callbacks may not register.
        //
        // Client handler chain:
        //   CGameLogic+0x3C0 callback list → CGameRules__Lua_SpawnPlayer (0x1103ED50)
        //   Validates: player context non-null, calls vtable[0x17C/4] to spawn
        //   TDR format: all fields Big-Endian (matching CSSpawnPlayer.WriteCs)
        if (client.Character != null)
        {
            CSSpawnPlayer spawnPlayer = new CSSpawnPlayer();
            spawnPlayer.PlayerId = client.Character.Id;
            spawnPlayer.NetObjId = client.Character.Id;
            spawnPlayer.Name = client.Character.Name;
            spawnPlayer.Gender = (byte)client.Character.Gender;
            spawnPlayer.Position = new XYZPosition()
            {
                x = client.State.Position?.x ?? 0,
                y = client.State.Position?.y ?? 0,
                z = client.State.Position?.z ?? 0
            };
            spawnPlayer.Rotation = new Quaternion() { x = 0, y = 0, z = 0, w = 1 };
            spawnPlayer.Scale = 1.0f;
            spawnPlayer.NewConnect = 1;
            spawnPlayer.SendSrvId = 0;
            spawnPlayer.EquipmentPack = "";
            spawnPlayer.AvatarSetID = 0;
            client.SendCsPacket(NewCsPacket.SpawnPlayer(spawnPlayer));
            Logger.Info(client, $"Sent CMD 516 SpawnPlayer for {client.Character.Name} (ID={client.Character.Id}) NetObjId={spawnPlayer.NetObjId} Pos=({spawnPlayer.Position.x:F3}, {spawnPlayer.Position.y:F3}, {spawnPlayer.Position.z:F3})");
        }

        string staticFolder = Path.Combine(Util.ExecutingDirectory(), "Files/Static");
        string npcFilePath = Path.Combine(staticFolder, "LevelDataNPCs.csv");
        using (TextFieldParser parser = new TextFieldParser(npcFilePath))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // Skip the header line
            parser.ReadLine();
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                string levelId = fields[0];
                // Remove the ";" character
                if (levelId.Length > 0)
                    levelId = levelId.Remove(levelId.Length - 1);

                bool isMatch = (client.State.levelId.ToString() == levelId);
                if (isMatch)
                {
                    //TODO: HACK because it doesnt seems to work with a full list of the zone
                    CsCsProtoStructurePacket<MonsterAppearNtfList> monsterAppearNtfList =
                        CsProtoResponse.MonsterAppearNtfList;

                    string npcID = fields[9];

                    string[] posValues = fields[4].Split(",");
                    string[] rotValues = fields[5].Split(",");

                    float posX = float.Parse(posValues[0], CultureInfo.InvariantCulture);
                    float posY = float.Parse(posValues[1], CultureInfo.InvariantCulture);
                    float posZ = float.Parse(posValues[2], CultureInfo.InvariantCulture);

                    //tricky thing, W is first here, not the same as ChangeTown.csv
                    float rotateW = float.Parse(rotValues[0], CultureInfo.InvariantCulture);
                    float rotateX = float.Parse(rotValues[1], CultureInfo.InvariantCulture);
                    float rotateY = float.Parse(rotValues[2], CultureInfo.InvariantCulture);
                    float rotateZ = float.Parse(rotValues[3], CultureInfo.InvariantCulture);

                    CSVec3 npcPosVec = new CSVec3(posX, posY, posZ);
                    CSQuat npcRotQuat = new CSQuat(rotateW, rotateX, rotateY, rotateZ);

                    monsterAppearNtfList.Structure.Appear.Add(new MonsterAppearNtf()
                    {
                        NetId = 0,
                        SpawnType = 1,
                        MonsterInfoId = int.Parse(npcID),
                        Pose = new CSQuatT(npcPosVec, npcRotQuat),
                    });

                    //TODO: HACK because it doesnt seems to take a full list of the zone
                    client.SendCsProtoStructurePacket(monsterAppearNtfList);
                    Thread.Sleep(25);
                }
            }
            //TODO: should work like a full list
            //client.SendCsProtoStructurePacket(monsterAppearNtfList);
        }

        // Send CSEnterLevelRsp to confirm level entry and complete battle ground initialization
        client.SendCsPacket(NewCsPacket.EnterLevelRsp(new CSEnterLevelRsp()));
        Logger.Info(client, $"Sent CMD 1291 EnterLevelRsp LevelId={client.State.levelId}");
    }
}