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
        // _characterManager.SyncAllAttr(client);

        //TODO: packet is a list so the game should handle a list of spawns, but it appears that it doesnt want ?
        //      maybe because there is other entities ?
        //CsCsProtoStructurePacket<MonsterAppearNtfList> monsterAppearNtfList = CsProtoResponse.MonsterAppearNtfList;

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
    }
}