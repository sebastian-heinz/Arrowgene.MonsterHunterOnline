namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdSystemTransAntiDataHandler : ICsProtoHandler
{
    public CsProtoCmd Cmd => CsProtoCmd.CS_CMD_SYSTEM_TRANS_ANTI_DATA;

    public void Handle(Client client, CsProtoPacket packet)
    {
       //TEST_RESPONSES.send(client);
       int i = 1;
    }
}