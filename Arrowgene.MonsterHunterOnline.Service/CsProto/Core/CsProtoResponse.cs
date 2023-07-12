using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    /// <summary>
    /// Maps CsProtoStructures to CsCmdId, names are based on `csproto.xml`
    /// </summary>
    public static class CsProtoResponse
    {
        public static CsProtoStructurePacket<ListRoleRsp> ListRoleRsp =>
            new(CS_CMD_ID.CS_CMD_LIST_ROLE_RSP);

        public static CsProtoStructurePacket<ListRoleRsp> CreateRoleRsp =>
            new(CS_CMD_ID.CS_CMD_LIST_ROLE_RSP);

        public static CsProtoStructurePacket<MultiIspSequenceNtf> MultiIspSequenceNtf =>
            new(CS_CMD_ID.CS_CMD_MULTI_ISP_SEQUENCE_NTF);
    }
}