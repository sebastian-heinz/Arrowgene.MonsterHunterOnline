using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;
using Arrowgene.MonsterHunterOnline.Service.Tdr;
using Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class DataLoadHandler : CsProtoStructureHandler<RemoteDataLoadReq>
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(DataLoadHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_DATA_LOAD_REQ;

    private readonly CharacterManager _characterManager;

    public DataLoadHandler(CharacterManager characterManager)
    {
        _characterManager = characterManager;
    }

    public override void Handle(Client client, RemoteDataLoadReq req)
    {
        CSICsRemoteDataInfo remoteData;
        switch (req.RemoteDataType)
        {
            case ROMTE_DATA_TYPE.ITEMMGR_DATA_TYPE:
                remoteData = new ItemListRsp();
                // TODO null check
                client.Inventory.PopulateItemListProperties((ItemListRsp)remoteData);
                break;
            case ROMTE_DATA_TYPE.LEVELINFO_DATA_TYPE:
                remoteData = new PlayerLevelInitInfo();
                ((PlayerLevelInitInfo)remoteData).UnlockHubStarData.Add(new PlayerUnLockHubStarInfo()
                {
                    HubID = 1,
                    PageIndex = 1,
                    StarLv = 6,
                });
                ((PlayerLevelInitInfo)remoteData).UnlockHubData.Add(new PlayerUnLockHubInfo()
                {
                    HubID = 1,
                    PageIndex = 1,
                });
                ((PlayerLevelInitInfo)remoteData).UnLockLevelData.Add(new PlayerUnlockLevelInfo()
                {
                    LevelID = 100101,
                });


                break;
            case ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE:
                remoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE);
                TlvHunterStar hs = new TlvHunterStar();
                //hs.LevelInfo = 1;
                hs.StarScore = 1000;
                hs.StatInfo = 1;
                
                StreamBuffer b = new StreamBuffer();
                b.WriteByte((byte)TlvMagic.NoVariant);
                hs.WriteTlv(b);
                ((RemoteDataInitInfo)remoteData).DataPacket.AddRange(b.GetAllBytes());
                break;
            case ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE:
                remoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE);
                // TODO unsure when this gets called
                //TlvTaskSys taskSys = new TlvTaskSys();
                //((RemoteDataInitInfo)remoteData).DataPacket.AddRange(taskSys.ToByteArray());
                break;
            case ROMTE_DATA_TYPE.NORMAL_LIMIT_DATATYPE:
                remoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.NORMAL_LIMIT_DATATYPE);
                break;
            case ROMTE_DATA_TYPE.SUPPLY_PLAN_DATA_TYPE:
                remoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.SUPPLY_PLAN_DATA_TYPE);
                break;
            default:
                Logger.Error(client, $"failed to build response for remoteDataType:{req.RemoteDataType}");
                return;
        }

        CsCsProtoStructurePacket dataLoadRsp = CsProtoResponse.DataLoadRsp(new RemoteDataLoadRsp(remoteData));
        client.SendCsProtoStructurePacket(dataLoadRsp);

        if (req.RemoteDataType == ROMTE_DATA_TYPE.LEVELINFO_DATA_TYPE)
        {
            CsCsProtoStructurePacket<TownInstanceVerifyRsp> townServerInitNtf = CsProtoResponse.TownServerInitNtf;
            TownInstanceVerifyRsp verifyRsp = townServerInitNtf.Structure;
            verifyRsp.ErrNo = 0;
            verifyRsp.LineId = 0;
            verifyRsp.LevelEnterType = 0;

            InstanceInitInfo instanceInitInfo = verifyRsp.InstanceInitInfo;
            instanceInitInfo.BattleGroundId = 0;
            instanceInitInfo.LevelId = 150301;

            // TODO hack
            instanceInitInfo.LevelId = client.State.InitLevelId;

            instanceInitInfo.CreateMaxPlayerCount = 4;
            instanceInitInfo.GameMode = GameMode.Town;
            instanceInitInfo.TimeType = TimeType.Noon;
            instanceInitInfo.WeatherType = WeatherType.Sunny;
            instanceInitInfo.Time = 1;
            instanceInitInfo.LevelRandSeed = 1;
            instanceInitInfo.WarningFlag = 0;
            instanceInitInfo.CreatePlayerMaxLv = 99;

            client.SendCsProtoStructurePacket(townServerInitNtf);
            client.State.prevLevelId = client.State.levelId;
            client.State.levelId = instanceInitInfo.LevelId;
        }
    }
}