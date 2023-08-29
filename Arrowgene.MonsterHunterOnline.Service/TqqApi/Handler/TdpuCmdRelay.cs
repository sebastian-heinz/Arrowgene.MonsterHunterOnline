using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.TqqApi.Crypto;
using Arrowgene.MonsterHunterOnline.Service.TqqApi.Structure;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi.Handler;

public class TdpuCmdRelay : TdpuStructureHandler<TdpuExtRelay>
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(TdpuCmdRelay));

    public override TpduCmd Cmd => TpduCmd.TPDU_CMD_RELAY;

    private IDatabase _database;

    public TdpuCmdRelay(IDatabase database)
    {
        _database = database;
    }

    public override void Handle(Client client, TdpuExtRelay req)
    {
        TdpuCrypto crypto = client.GetTdpuCrypto();
        //  if (crypto == null)
        //  {
        //      return;
        //  }
        //  byte[] identity = crypto.Decrypt(req.EncryptIdent.ToArray());

        // TODO it looks like we need to set ident info in "SynAck" handler
        // then expect it on recon to be here, and lookup the clients account
        // then we can see if we can resume the session, it looks like from this point on we can do cs proto again
        // but have not found out the best way to deal with it yet.


        Account account = _database.SelectAccountByUin(1234);
        if (account == null)
        {
            return;
        }

        client.Account = account;
    }
}