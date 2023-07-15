using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System;
using System.Globalization;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class PlayerRegionJumpReqHandler : CsProtoStructureHandler<PlayerRegionJumpReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(PlayerRegionJumpReq));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_PLAYER_REGION_JUMP_REQ;


    public override void Handle(Client client, PlayerRegionJumpReq req)
    {
        //CSVec3 coords = req.PlayerPos;
        string triggerName = (req.TriggerName).Trim(' ', '\t', '\u00A0', '\x00');
        //Logger.Info($"Teleport Info: ({triggerName})");

        //the warp zones for mezeporta since its handled attrociously 
        if (triggerName == "Teleport_To_Main_Area" && client.State.levelId == 180201)
        {
            triggerName = "Teleport_To_Main_Area_Point";
        }
        if (triggerName == "Teleport_To_MainArea" && client.State.levelId == 180201)
        {
            triggerName = "Teleport_To_Main_AreaPoint";
        }

        string instanceLevelId = client.State.levelId.ToString();
        
        string staticFolder = Path.Combine(Util.ExecutingDirectory(), "Files/Static");
        string filePath = Path.Combine(staticFolder, "RegionJump.csv");

        using (TextFieldParser parser = new TextFieldParser(filePath))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // Skip the header line
            parser.ReadLine();
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                string levelId = fields[0];
                bool isMatch = !string.IsNullOrEmpty(levelId) &&
                    !string.IsNullOrEmpty(instanceLevelId) &&
                    (instanceLevelId.Contains(levelId) || levelId.Contains(instanceLevelId));

                if (isMatch)
                {
                    string filename = fields[1];
                    string areaName = fields[2];
                    string name = fields[3].Trim();
                    string pos = fields[5];
                    string rotate = fields[6];
                    //Logger.Error($"warp names: ({triggerName}) ({name}), {name.Contains(triggerName) || triggerName.Contains(name)}");
                    //Logger.Error($"stage match found: ({levelId})({filename})({areaName})({name})");
                    if (name.Contains(triggerName) || triggerName.Contains(name))
                    {
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

                        CSQuatT TargetPos = new CSQuatT()
                        {

                            q = new CSQuat()
                            {
                                v = new CSVec3() { x = rotateX, y = rotateY, z = rotateZ },
                                w = rotateW
                            },
                            t = new CSVec3() { x = posX, y = posY, z = posZ }
                        };

                        CsProtoStructurePacket<PlayerRegionJumpRsp> PlayerRegionJump = CsProtoResponse.PlayerRegionJumpRsp;
                        PlayerRegionJump.Structure.ErrorCode = 0;
                        PlayerRegionJump.Structure.RegionId = 0;
                        PlayerRegionJump.Structure.Transform = TargetPos;

                        client.SendCsProtoStructurePacket(PlayerRegionJump);
                    }
                }
            }
        }
    }
}