using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.Tdr.TlvStructures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    public class RemoteDataLoadRsp : Structure, ICsStructure
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(RemoteDataLoadRsp));

        public RemoteDataLoadRsp(CSICsRemoteDataInfo remoteData)
        {
            RemoteData = remoteData;
        }

        public CSICsRemoteDataInfo RemoteData { get; set; }

        public void WriteCs(IBuffer buffer)
        {
            WriteUInt16(buffer, (ushort)RemoteData.DataType);
            WriteCsStructure(buffer, RemoteData);
        }

        public void ReadCs(IBuffer buffer)
        {
            ROMTE_DATA_TYPE remoteDataType = (ROMTE_DATA_TYPE)ReadUInt16(buffer);
            switch (remoteDataType)
            {
                case ROMTE_DATA_TYPE.ITEMMGR_DATA_TYPE:
                    RemoteData = ReadCsStructure<ItemListRsp>(buffer);
                    break;
                case ROMTE_DATA_TYPE.LEVELINFO_DATA_TYPE:
                    RemoteData = ReadCsStructure<PlayerLevelInitInfo>(buffer);
                    break;
                case ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE:
                    TlvHunterStar tlvHunterStar = new TlvHunterStar();
             
                  //  RemoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.HUNTERSTAR_DATA_TYPE);
                  //  RemoteData = ReadCsStructure(buffer, (RemoteDataInitInfo)RemoteData);
                    break;
                case ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE:
                  //  RemoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.TASKSYS_DATA_TYPE);
                  //  RemoteData = ReadCsStructure(buffer, (RemoteDataInitInfo)RemoteData);
                    break;
                case ROMTE_DATA_TYPE.NORMAL_LIMIT_DATATYPE:
                 //   RemoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.NORMAL_LIMIT_DATATYPE);
                  //  RemoteData = ReadCsStructure(buffer, (RemoteDataInitInfo)RemoteData);
                    break;
                case ROMTE_DATA_TYPE.SUPPLY_PLAN_DATA_TYPE:
                  //  RemoteData = new RemoteDataInitInfo(ROMTE_DATA_TYPE.SUPPLY_PLAN_DATA_TYPE);
                  //  RemoteData = ReadCsStructure(buffer, (RemoteDataInitInfo)RemoteData);
                    break;
                default:
                    Logger.Error("Failed to create 'RemoteData' instance of type 'RemoteDataInfo'");
                    break;
            }
        }
    }
}