using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class DataLoadHandler : CsProtoStructureHandler<RemoteDataLoadReq>
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(DataLoadHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_DATA_LOAD_REQ;
    

    public override void Handle(Client client, RemoteDataLoadReq req)
    {
        CsProtoStructurePacket dataLoadRsp;
        switch (req.RemoteDataType)
        {
            case ROMTE_DATA_TYPE.ITEMMGR_DATA_TYPE:
                dataLoadRsp = CsProtoResponse.DataLoadRsp(new RemoteDataLoadRsp(new CSItemListRsp()));
                break;
            case ROMTE_DATA_TYPE.LEVELINFO_DATA_TYPE:
                dataLoadRsp = CsProtoResponse.DataLoadRsp(new RemoteDataLoadRsp(new PlayerLevelInitInfo()));
                break;
            case ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE:
                dataLoadRsp = CsProtoResponse.DataLoadRsp(new RemoteDataInitInfo(ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE));
                break;
            case ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE:
                dataLoadRsp = CsProtoResponse.DataLoadRsp(new RemoteDataInitInfo(ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE));
                break;
            case ROMTE_DATA_TYPE.NORMAL_LIMIT_DATATYPE:
                dataLoadRsp =
                    CsProtoResponse.DataLoadRsp(new RemoteDataInitInfo(ROMTE_DATA_TYPE.NORMAL_LIMIT_DATATYPE));
                break;
            case ROMTE_DATA_TYPE.SUPPLY_PLAN_DATA_TYPE:
                dataLoadRsp =
                    CsProtoResponse.DataLoadRsp(new RemoteDataInitInfo(ROMTE_DATA_TYPE.SUPPLY_PLAN_DATA_TYPE));
                break;
            default:
                Logger.Error(client, $"failed to build response for remoteDataType:{req.RemoteDataType}");
                return;
        }

        client.SendCsProtoStructurePacket(dataLoadRsp);
    }
}