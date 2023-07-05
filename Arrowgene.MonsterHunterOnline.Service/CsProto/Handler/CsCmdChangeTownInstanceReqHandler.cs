using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdChangeTownInstanceReqHandler : ICsProtoHandler
{
    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_CHANGE_TOWN_INSTANCE_REQ;

    public void Handle(Client client, CsProtoPacket packet)
    {
        CSChangeTownInstanceReq req = new CSChangeTownInstanceReq();
        req.Read(packet.NewBuffer());

        client.State.OnChangeTownInstance(req);
    }
}