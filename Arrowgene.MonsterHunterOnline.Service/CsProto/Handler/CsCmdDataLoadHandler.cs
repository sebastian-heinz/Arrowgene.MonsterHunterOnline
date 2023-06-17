using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Packets;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdDataLoadHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CsCmdDataLoadHandler));


    public CsProtoCmd Cmd => CsProtoCmd.CS_CMD_DATA_LOAD_REQ;

    public void Handle(Client client, CsProtoPacket packet)
    {
        IBuffer req = new StreamBuffer(packet.Body);
        req.SetPositionStart();
        RemoteDataType remoteDataType = (RemoteDataType)req.ReadUInt16(Endianness.Big);
        DataLoadRsp rsp = null;
        switch (remoteDataType)
        {
            case RemoteDataType.ITEMMGR_DATA_TYPE:
                rsp = new DataLoadRsp(new CsItemListRsp());
                break;
            case RemoteDataType.LEVELINFO_DATA_TYPE:
                rsp = new DataLoadRsp(new CsPlayerLevelInitInfo());
                break;
            case RemoteDataType.HUNTERSTAR_DATA_TYPE:
                rsp = new DataLoadRsp(new RemoteDataInitInfo(RemoteDataType.HUNTERSTAR_DATA_TYPE));
                break;
            case RemoteDataType.TASKSYS_DATA_TYPE:
                rsp = new DataLoadRsp(new RemoteDataInitInfo(RemoteDataType.TASKSYS_DATA_TYPE));
                break;
            case RemoteDataType.NORMAL_LIMIT_DATATYPE:
                rsp = new DataLoadRsp(new RemoteDataInitInfo(RemoteDataType.NORMAL_LIMIT_DATATYPE));
                break;
            case RemoteDataType.SUPPLY_PLAN_DATA_TYPE:
                rsp = new DataLoadRsp(new RemoteDataInitInfo(RemoteDataType.SUPPLY_PLAN_DATA_TYPE));
                break;
        }

        if (rsp == null)
        {
            Logger.Error(client, $"failed to build response for remoteDataType:{remoteDataType}");
            return;
        }

        client.SendCsProto(rsp.BuildPacket());
    }
}