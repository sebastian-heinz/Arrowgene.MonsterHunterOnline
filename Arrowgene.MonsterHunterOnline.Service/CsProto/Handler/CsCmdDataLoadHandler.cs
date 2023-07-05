using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdDataLoadHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CsCmdDataLoadHandler));


    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_DATA_LOAD_REQ;

    public void Handle(Client client, CsProtoPacket packet)
    {
        IBuffer req = new StreamBuffer(packet.Body);
        req.SetPositionStart();
        ROMTE_DATA_TYPE remoteDataType = (ROMTE_DATA_TYPE)req.ReadUInt16(Endianness.Big);
        CSRemoteDataLoadRsp rsp = null;
        switch (remoteDataType)
        {
            case ROMTE_DATA_TYPE.ITEMMGR_DATA_TYPE:
                rsp = new CSRemoteDataLoadRsp(client.State._ItemListRsp);
                break;
            case ROMTE_DATA_TYPE.LEVELINFO_DATA_TYPE:
                rsp = new CSRemoteDataLoadRsp(client.State._playerLevelInitInfo);
                break;
            case ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE:
                rsp = new CSRemoteDataLoadRsp(new RemoteDataInitInfo(ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE));
                break;
            case ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE:
                rsp = new CSRemoteDataLoadRsp(new RemoteDataInitInfo(ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE));
                break;
            case ROMTE_DATA_TYPE.NORMAL_LIMIT_DATATYPE:
                rsp = new CSRemoteDataLoadRsp(new RemoteDataInitInfo(ROMTE_DATA_TYPE.NORMAL_LIMIT_DATATYPE));
                break;
            case ROMTE_DATA_TYPE.SUPPLY_PLAN_DATA_TYPE:
                rsp = new CSRemoteDataLoadRsp(new RemoteDataInitInfo(ROMTE_DATA_TYPE.SUPPLY_PLAN_DATA_TYPE));
                break;
        }

        if (rsp == null)
        {
            Logger.Error(client, $"failed to build response for remoteDataType:{remoteDataType}");
            return;
        }

        client.SendCsPacket(NewCsPacket.DataLoadRsp(rsp));
    }
}