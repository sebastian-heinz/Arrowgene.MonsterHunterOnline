using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdTeamInfoGetReqHandler : ICsProtoHandler
{
    private static readonly ServiceLogger Logger =
        LogProvider.Logger<ServiceLogger>(typeof(CsCmdTeamInfoGetReqHandler));


    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_TEAM_INFO_GET_REQ;


    public void Handle(Client client, CsProtoPacket packet)
    {
        client.SendCsPacket(NewCsPacket.TeamInfoNtf(new CSTeamInfoNtf()));
    }
}