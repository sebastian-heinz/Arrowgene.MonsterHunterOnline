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

        public static CsProtoStructurePacket<RoleDataErrorRsp> RoleDataErrorRsp =>
            new(CS_CMD_ID.CS_CMD_ROLEDATA_ERR_RSP);

        public static CsProtoStructurePacket<NotifyInfo> NotifyInfo =>
            new(CS_CMD_ID.CS_CMD_SYSTEM_NOTIFY_INFO);

        public static CsProtoStructurePacket<DeleteRoleRsp> DeleteRoleRsp =>
            new(CS_CMD_ID.CS_CMD_DELETE_ROLE_RSP);

        public static CsProtoStructurePacket<AccountRsp> AccountRsp =>
            new(CS_CMD_ID.SC_CMD_WORLD_ACCOUNT_RSP);

        public static CsProtoStructurePacket<ModifyFaceRsp> ModifyFaceRsp =>
            new(CS_CMD_ID.CS_CMD_MODIFY_FACE_RSP);

        public static CsProtoStructurePacket<SelectRoleRsp> SelectRoleRsp =>
            new(CS_CMD_ID.CS_CMD_SELECT_ROLE_RSP);

        public static CsProtoStructurePacket<TownSessionStart> TownSessionStart =>
            new(CS_CMD_ID.CS_CMD_TOWN_SESSION_START);

        public static CsProtoStructurePacket<PlayerInitInfo> PlayerInitInfo =>
            new(CS_CMD_ID.CS_CMD_PLAYER_INIT_NTF);

        public static CsProtoStructurePacket DataLoadRsp(RemoteDataLoadRsp structure) =>
            new(CS_CMD_ID.CS_CMD_DATA_LOAD_RSP, structure);

        public static CsProtoStructurePacket<TownInstanceVerifyRsp> TownServerInitNtf =>
            new(CS_CMD_ID.CS_CMD_TOWN_SERVER_INIT_NTF);

        public static CsProtoStructurePacket<AttrSyncList> AttrSyncList =>
            new(CS_CMD_ID.CS_CMD_ATTR_SYNC_LIST_NTF);

        public static CsProtoStructurePacket<AttrSync> AttrSync =>
            new(CS_CMD_ID.CS_CMD_ATTR_SYNC_NTF);
    }
}