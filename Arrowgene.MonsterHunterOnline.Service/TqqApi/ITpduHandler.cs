namespace Arrowgene.MonsterHunterOnline.Service.TqqApi;

public interface ITpduHandler
{
    TpduCmd Cmd { get; }
    void Handle(Client client, TpduPacket packet);
}