using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdCreateRoleReqHandler : CsProtoStructureHandler<RoleCreateInfo>
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdCreateRoleReqHandler));

    public override CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CREATE_ROLE_REQ;

    public override void Handle(Client client, RoleCreateInfo req)
    {
        Logger.Info(client, $"Creating new Character:{req.Name}");
    }
}