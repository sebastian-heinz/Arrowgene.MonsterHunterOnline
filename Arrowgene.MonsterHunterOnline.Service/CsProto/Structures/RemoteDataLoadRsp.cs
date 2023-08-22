using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    public class RemoteDataLoadRsp : Structure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(RemoteDataLoadRsp));

        public RemoteDataLoadRsp(IRemoteDataInfo remoteData)
        {
            RemoteData = remoteData;
        }

        public IRemoteDataInfo RemoteData { get; set; }

        public override void Write(IBuffer buffer)
        {
            WriteUInt16(buffer, (ushort)RemoteData.DataType);
            WriteStructure(buffer, RemoteData);
        }

        public override void Read(IBuffer buffer)
        {
            ROMTE_DATA_TYPE remoteDataType = (ROMTE_DATA_TYPE)ReadUInt16(buffer);
            switch (remoteDataType)
            {
                case ROMTE_DATA_TYPE.ITEMMGR_DATA_TYPE:
                    RemoteData = ReadStructure<ItemListRsp>(buffer);
                    break;
                case ROMTE_DATA_TYPE.LEVELINFO_DATA_TYPE:
                    RemoteData = ReadStructure<PlayerLevelInitInfo>(buffer);
                    break;
                case ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE:
                    RemoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE);
                    RemoteData = ReadStructure(buffer, (RemoteDataInitInfo)RemoteData);
                    break;
                case ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE:
                    RemoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE);
                    RemoteData = ReadStructure(buffer, (RemoteDataInitInfo)RemoteData);
                    break;
                case ROMTE_DATA_TYPE.NORMAL_LIMIT_DATATYPE:
                    RemoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.NORMAL_LIMIT_DATATYPE);
                    RemoteData = ReadStructure(buffer, (RemoteDataInitInfo)RemoteData);
                    break;
                case ROMTE_DATA_TYPE.SUPPLY_PLAN_DATA_TYPE:
                    RemoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.SUPPLY_PLAN_DATA_TYPE);
                    RemoteData = ReadStructure(buffer, (RemoteDataInitInfo)RemoteData);
                    break;
                default:
                    Logger.Error("Failed to create 'RemoteData' instance of type 'RemoteDataInfo'");
                    break;
            }
        }
    }
}