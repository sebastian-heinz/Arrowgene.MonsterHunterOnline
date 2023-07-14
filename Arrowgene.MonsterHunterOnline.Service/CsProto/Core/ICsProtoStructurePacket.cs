using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public interface ICsProtoStructurePacket : IStructure
    {
        public CS_CMD_ID Cmd { get; }
    }
}