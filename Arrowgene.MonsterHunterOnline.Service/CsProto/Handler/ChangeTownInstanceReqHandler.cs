using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using System;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ChangeTownInstanceReqHandler : CsProtoStructureHandler<ChangeTownInstanceReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ChangeTownInstanceReq));
    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CHANGE_TOWN_INSTANCE_REQ;

    public override void Handle(Client client, ChangeTownInstanceReq req)
    {
        string csvFile = "ChangeTown.csv";
        string desiredDirectory = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName);
        string csvPath = Path.Combine(desiredDirectory, csvFile);
        int level = req.LevelId;
        string triggerName = (req.trigger_name).Trim(' ', '\t', '\u00A0', '\x00');
        if (triggerName.Length < 5)
        {
            triggerName = req.dstpoint;
        }
        CsProtoStructurePacket<TownInstanceVerifyRsp> townServerInitNtf = CsProtoResponse.TownServerInitNtf;
        TownInstanceVerifyRsp verifyRsp = townServerInitNtf.Structure;
        verifyRsp.ErrNo = 0;
        verifyRsp.LineId = 0;
        verifyRsp.LevelEnterType = 0;

        InstanceInitInfo instanceInitInfo = verifyRsp.InstanceInitInfo;
        instanceInitInfo.BattleGroundId = 0;
        
        instanceInitInfo.CreateMaxPlayerCount = 4;
        instanceInitInfo.GameMode = GameMode.Town;
        instanceInitInfo.TimeType = TimeType.Noon;
        instanceInitInfo.WeatherType = WeatherType.Sunny;
        instanceInitInfo.Time = 1;
        instanceInitInfo.LevelRandSeed = 1;
        instanceInitInfo.WarningFlag = 0;
        instanceInitInfo.CreatePlayerMaxLv = 99;

 
        if (triggerName.Length > 5)
        {
            using (TextFieldParser parser = new TextFieldParser(csvPath))
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
                    //bool isMatch = !string.IsNullOrEmpty(levelId) &&
                    //    !string.IsNullOrEmpty(level_comp) &&
                    //    (level_comp.Contains(levelId) || levelId.Contains(level_comp));
                    bool isMatch = (level_comp.Contains(levelId) || levelId.Contains(level_comp));
                    if (isMatch)
                    {
                        string filename = fields[1];
                        string areaName = fields[2];
                        string name = fields[3].Trim();
                        string pos = fields[5];
                        string rotate = fields[6];
                        string dstPoint = fields[7];

                        if (name.Contains(triggerName) || triggerName.Contains(name))
                        {
                            if (dstPoint.Length > 4)
                            {
                                level = int.Parse(dstPoint);
                            }
                            instanceInitInfo.LevelId = level;
                            //Logger.Info($"warp point match found: ({levelId})({filename})({areaName})({name})");
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


                            CSQuatT TargetPosition = new CSQuatT()
                            {

                                q = new CSQuat()
                                {
                                    v = new CSVec3() { x = rotateX, y = rotateY, z = rotateZ },
                                    w = rotateW
                                },
                                t = new CSVec3() { x = posX, y = posY, z = posZ }
                            };

                            CsProtoStructurePacket<ChangeTownInstanceRsp> ChangeTownInstance = CsProtoResponse.ChangeTownInstanceRsp;
                            ChangeTownInstance.Structure.ErrCode = 0;
                            ChangeTownInstance.Structure.LevelID = level;

                            client.SendCsProtoStructurePacket(ChangeTownInstance);


                            CsProtoStructurePacket<PlayerTeleport> PlayerTeleport = CsProtoResponse.PlayerTeleport;
                            PlayerTeleport.Structure.SyncTime = 1;
                            PlayerTeleport.Structure.NetObjId = 1;
                            PlayerTeleport.Structure.Region = 1;
                            PlayerTeleport.Structure.TargetPos = TargetPosition;
                            PlayerTeleport.Structure.ParentGUID = 1;
                            PlayerTeleport.Structure.InitState = 1;
                            client.SendCsProtoStructurePacket(PlayerTeleport);

                            client.SendCsProtoStructurePacket(townServerInitNtf);
                            return;
                        }
                    }
                }
                Logger.Error($"Warp point not found {req.trigger_name}, {req.LevelId}, {req.dstpoint}");
            }

        }
        else
        {

            CsProtoStructurePacket<ChangeTownInstanceRsp> ChangeTownInstance = CsProtoResponse.ChangeTownInstanceRsp;
            ChangeTownInstance.Structure.ErrCode = 0;
            ChangeTownInstance.Structure.LevelID = level;
            client.SendCsProtoStructurePacket(ChangeTownInstance);

            client.SendCsPacket(NewCsPacket.PlayerTeleport(new CSPlayerTeleport()
            {
                SyncTime = 1,
                NetObjId = 1,
                Region = 1,
                TargetPos = new CSQuatT()
                {
                    q = new CSQuat()
                    {
                        v = new CSVec3() { x = 200.0f, y = 200.0f, z = 200.0f },
                        w = 0.0f
                    },
                    t = new CSVec3() { x = 400f, y = 400f, z = 400f }
                },
                ParentGUID = 1,
                InitState = 1
            }));

            client.SendCsProtoStructurePacket(townServerInitNtf);
        }
    }
}