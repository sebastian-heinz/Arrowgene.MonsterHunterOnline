using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

public interface ICsProtoHandler
{
    CS_CMD_ID Cmd { get; }
    void Handle(Client client, CsProtoPacket packet);
}