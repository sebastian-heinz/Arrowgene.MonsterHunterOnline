using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi.Handler;

public class TpduCmdCloseHandler : ITpduHandler
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(TpduConsumer));

    public TpduCmd Cmd => TpduCmd.TPDU_CMD_CLOSE;

    public void Handle(Client client, TpduPacket packet)
    {
        Logger.Info(client, "TPDU_CMD_CLOSE");
        client.Close();
    }
}