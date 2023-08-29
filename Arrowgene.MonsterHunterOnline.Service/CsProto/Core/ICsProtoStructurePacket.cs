using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public interface CSICsCsProtoStructurePacket : ICsStructure
    {
        public CS_CMD_ID Cmd { get; }
    }
}