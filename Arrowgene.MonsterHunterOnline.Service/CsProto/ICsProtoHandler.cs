namespace Arrowgene.MonsterHunterOnline.Service.CsProto;

public interface ICsProtoHandler
{
    CsProtoCmd Cmd { get; }
    void Handle(Client client, CsProtoPacket packet);
}