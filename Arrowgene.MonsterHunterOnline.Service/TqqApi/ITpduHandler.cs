namespace Arrowgene.MonsterHunterOnline.Service.TQQApi;

public interface ITpduHandler
{
    TpduCmd Cmd { get; }
    void Handle(Client client, TpduPacket packet);
}