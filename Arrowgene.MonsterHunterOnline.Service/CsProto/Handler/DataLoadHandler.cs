using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Constant;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;

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
        IRemoteDataInfo remoteData;
        switch (req.RemoteDataType)
        {
            case ROMTE_DATA_TYPE.ITEMMGR_DATA_TYPE:
                remoteData = new CSItemListRsp();
                break;
            case ROMTE_DATA_TYPE.LEVELINFO_DATA_TYPE:
                remoteData = new PlayerLevelInitInfo();
                break;
            case ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE:
                remoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE);
                break;
            case ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE:
                remoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE);
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

        CsProtoStructurePacket dataLoadRsp = CsProtoResponse.DataLoadRsp(new RemoteDataLoadRsp(remoteData));
        client.SendCsProtoStructurePacket(dataLoadRsp);

        if (req.RemoteDataType == ROMTE_DATA_TYPE.LEVELINFO_DATA_TYPE)
        {
            CsProtoStructurePacket<TownInstanceVerifyRsp> townServerInitNtf = CsProtoResponse.TownServerInitNtf;
            TownInstanceVerifyRsp verifyRsp = townServerInitNtf.Structure;
            verifyRsp.ErrNo = 0;
            verifyRsp.LineId = 0;
            verifyRsp.LevelEnterType = 0;

            InstanceInitInfo instanceInitInfo = verifyRsp.InstanceInitInfo;
            instanceInitInfo.BattleGroundId = 0;
            instanceInitInfo.LevelId = 150301;
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