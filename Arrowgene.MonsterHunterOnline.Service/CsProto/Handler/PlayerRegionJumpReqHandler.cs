using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class PlayerRegionJumpReqHandler : CsProtoStructureHandler<PlayerRegionJumpReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(PlayerRegionJumpReq));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_PLAYER_REGION_JUMP_REQ;


    public override void Handle(Client client, PlayerRegionJumpReq req)
    {
        CSVec3 coords = req.PlayerPos;
        string triggerName = (req.TriggerName).Trim(' ', '\t', '\u00A0', '\x00');
        Logger.Info($"Teleport Info: ({triggerName})");

        triggerName = triggerName.Replace("MainArea", "MainArea".Insert("MainArea".IndexOf("Main") + 4, "_"));
        //client.
        //string instanceLevelId = _instanceInitInfo.LevelID.ToString();
        string csvFile = "RegionJump.csv";
        string desiredDirectory = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName);
        string filePath = Path.Combine(desiredDirectory, csvFile);

        using (TextFieldParser parser = new TextFieldParser(filePath))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // Skip the header line
            parser.ReadLine();
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                //string levelId = fields[0];
                //bool isMatch = !string.IsNullOrEmpty(levelId) &&
                //    !string.IsNullOrEmpty(instanceLevelId) &&
                //    (instanceLevelId.Contains(levelId) || levelId.Contains(instanceLevelId));
                bool isMatch = true;
                if (isMatch)
                {
                    string filename = fields[1];
                    string areaName = fields[2];
                    string name = fields[3].Trim();
                    string pos = fields[4];
                    string rotate = fields[5];
                    //Logger.Error($"warp names: ({triggerName}) ({name}), {name.Contains(triggerName) || triggerName.Contains(name)}");
                    //Logger.Error($"stage match found: ({levelId})({filename})({areaName})({name})");
                    if (name.Contains(triggerName) || triggerName.Contains(name))
                    {
                        // Process the position (Pos) and rotation (Rotate) values
                        string[] posValues = pos.Split(',');
                        string[] rotateValues = rotate.Split(',');

                        float posX = float.Parse(posValues[0]);
                        float posY = float.Parse(posValues[1]);
                        float posZ = float.Parse(posValues[2]);

                        float rotateX = float.Parse(rotateValues[0]);
                        float rotateY = float.Parse(rotateValues[1]);
                        float rotateZ = float.Parse(rotateValues[2]);
                        float rotateW = float.Parse(rotateValues[3]);

                        CSQuatT TargetPos = new CSQuatT()
                        {

                            q = new CSQuat()
                            {
                                v = new CSVec3() { x = rotateX, y = rotateY, z = rotateZ },
                                w = rotateW
                            },
                            t = new CSVec3() { x = posX, y = posY, z = posZ }
                        };
                        client.SendCsPacket(NewCsPacket.PlayerRegionJumpRsp(new CSPlayerRegionJumpRsp()
                        {
                            ErrorCode = 0,
                            RegionId = 0,
                            Transform = TargetPos

                        }));
                    }
                }
            }
        }
    }
}