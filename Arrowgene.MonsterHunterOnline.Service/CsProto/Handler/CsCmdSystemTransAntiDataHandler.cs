namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdSystemTransAntiDataHandler : ICsProtoHandler
{
    public CsProtoCmd Cmd => CsProtoCmd.CS_CMD_SYSTEM_TRANS_ANTI_DATA;

    public void Handle(Client client, CsProtoPacket packet)
    {
        //TEST_RESPONSES.send(client);


        for (int i = 1200; i < 2000; i++)
        {
            CsProtoPacket resp = new CsProtoPacket();
            resp.Body = new byte[1024];
            resp.Cmd = (CsProtoCmd)i;
      //      client.SendCsProto(resp);
        }

        CsProtoPacket resp17 = new CsProtoPacket();
        resp17.Body = new byte[1024];
        resp17.Cmd = CsProtoCmd.CS_CMD_LOADLEVEL_NTF;
        //client.SendCsProto(resp17);

        int sd = 1;
    }
}