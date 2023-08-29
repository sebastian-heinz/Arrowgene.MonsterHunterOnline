using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using System.Globalization;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class ChangeTownInstanceReqHandler : CsProtoStructureHandler<ChangeTownInstanceReq>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(ChangeTownInstanceReq));
    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CHANGE_TOWN_INSTANCE_REQ;

    public override void Handle(Client client, ChangeTownInstanceReq req)
    {
        string staticFolder = Path.Combine(Util.ExecutingDirectory(), "Files\\Static");
        string csvPath = Path.Combine(staticFolder, "ChangeTown.csv");
        //Logger.Info($"staticfolder:{staticFolder}\n csvpath:{csvPath}");
        int level = req.LevelId;
        if (level < 100)
        {
            level = client.State.levelId;
        }
        string triggerName = (req.TriggerName).Trim(' ', '\t', '\u00A0', '\x00');
        if (triggerName.Length < 5)
        {
            triggerName = req.DstPoint;
        }
        CsCsProtoStructurePacket<TownInstanceVerifyRsp> townServerInitNtf = CsProtoResponse.TownServerInitNtf;
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
                            
                            if (("FarmToVillageTrigger".Contains(triggerName) || triggerName.Contains("FarmToVillageTrigger")) || ("FromGuildCampToVillage".Contains(triggerName) || triggerName.Contains("FromGuildCampToVillage")))
                            {
                                level = client.State.prevLevelId;
                                Logger.Info($"tp from guild or farm");
                            }
                            else if (dstPoint.Length > 4)
                            {
                                level = int.Parse(dstPoint);
                                Logger.Info($"tp not from guild or farm ");
                            }
                            instanceInitInfo.LevelId = level;
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


                            CSQuatT TargetPosition = new CSQuatT()
                            {

                                q = new CSQuat()
                                {
                                    v = new CSVec3() { x = rotateX, y = rotateY, z = rotateZ },
                                    w = rotateW
                                },
                                t = new CSVec3() { x = (float)posX, y = (float)posY, z = (float)posZ }
                            };

                            CsCsProtoStructurePacket<ChangeTownInstanceRsp> ChangeTownInstance = CsProtoResponse.ChangeTownInstanceRsp;
                            ChangeTownInstance.Structure.ErrCode = 0;
                            ChangeTownInstance.Structure.LevelId = level;

                            client.SendCsProtoStructurePacket(ChangeTownInstance);


                            CsCsProtoStructurePacket<PlayerTeleport> PlayerTeleport = CsProtoResponse.PlayerTeleport;
                            PlayerTeleport.Structure.SyncTime = 1;
                            //One day netobjid would not be the character id ?
                            PlayerTeleport.Structure.NetObjId = client.Character.Id;
                            PlayerTeleport.Structure.Region = level;
                            PlayerTeleport.Structure.TargetPos = TargetPosition;
                            PlayerTeleport.Structure.ParentGuid = 1;
                            PlayerTeleport.Structure.InitState = 1;
                            client.SendCsProtoStructurePacket(PlayerTeleport);

                            client.SendCsProtoStructurePacket(townServerInitNtf);
                            client.State.prevLevelId = client.State.levelId;
                            client.State.levelId = instanceInitInfo.LevelId;
                            return;
                        }
                    }
                }
                Logger.Error($"Warp point not found {req.TriggerName}, {req.LevelId}, {req.DstPoint}");
            }

        }
        else
        {

            CsCsProtoStructurePacket<ChangeTownInstanceRsp> ChangeTownInstance = CsProtoResponse.ChangeTownInstanceRsp;
            ChangeTownInstance.Structure.ErrCode = 0;
            ChangeTownInstance.Structure.LevelId = level;
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
            client.State.prevLevelId = client.State.levelId;
            client.State.levelId = instanceInitInfo.LevelId;
        }
    }
}