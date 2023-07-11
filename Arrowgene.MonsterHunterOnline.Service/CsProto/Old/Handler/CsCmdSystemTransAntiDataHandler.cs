using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Handler;

public class CsCmdSystemTransAntiDataHandler : ICsProtoHandler
{
    public CS_CMD_ID Cmd => CS_CMD_ID.CS_CMD_SYSTEM_TRANS_ANTI_DATA;

    public void Handle(Client client, CsProtoPacket packet)
    {
        //TEST_RESPONSES.send(client);


        for (int i = 1200; i < 2000; i++)
        {
            CsProtoPacket resp = new CsProtoPacket();
            resp.Body = new byte[1024];
            resp.Cmd = (CS_CMD_ID)i;
      //      client.SendCsProto(resp);
        }

        CsProtoPacket resp17 = new CsProtoPacket();
        resp17.Body = new byte[1024];
        resp17.Cmd = CS_CMD_ID.CS_CMD_LOADLEVEL_NTF;
        //client.SendCsProto(resp17);

        int sd = 1;
    }
}