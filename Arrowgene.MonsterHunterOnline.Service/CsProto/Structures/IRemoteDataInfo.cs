using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Structures
{
    public interface CSICsRemoteDataInfo : ICsStructure
    {
        public ROMTE_DATA_TYPE DataType { get; }
    }
}