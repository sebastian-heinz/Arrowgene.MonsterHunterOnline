using System;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.Database;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.TqqApi;
using Arrowgene.MonsterHunterOnline.Service.TQQApi.Crypto;
using Arrowgene.MonsterHunterOnline.Service.TQQApi.Structure;

namespace Arrowgene.MonsterHunterOnline.Service.TQQApi.Handler;

public class TpduCmdAuthHandler : TdpuStructureHandler<TqqExtAuthInfo>
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(TpduCmdAuthHandler));

    public override TpduCmd Cmd => TpduCmd.TPDU_CMD_AUTH;

    private IDatabase _database;

    public TpduCmdAuthHandler(IDatabase database)
    {
        _database = database;
    }

    public override void Handle(Client client, TqqExtAuthInfo req)
    {
        uint uin = req.AuthData.Uin;

        Logger.Info(client, $"Authentication Request (Uin:{uin})");

        switch (req.EncMethod)
        {
            case TConnSecEnc.TCONN_SEC_AES:
                client.SetTdpuCrypto(new TdpuCryptoAes128());
                break;
            case TConnSecEnc.TCONN_SEC_NONE:
                client.SetTdpuCrypto(null);
                break;
            default:
                Logger.Error(client, $"requested EncMethod:{req.EncMethod} not supported");
                // TODO send error rsp
                break;
        }

        Logger.Info(client, $"EncMethod:{client.TConnSecEnc}");

        string passwordHash = null;
        if (req.AuthData is TqqUnifiedAuthInfo unifiedAuth)
        {
            passwordHash = BitConverter.ToString(unifiedAuth.SigInfo.ToArray());
        }
        else if (req.AuthData is TqqAuthInfo auth)
        {
            Logger.Error(client, $"TqqAuthInfo not supported");
            // TODO send error rsp
            return;
        }
        else
        {
            Logger.Error(client, $"Requested AuthData not supported");
            // TODO send error rsp
            return;
        }

        Account account = _database.SelectAccountByUin(uin);
        if (account == null)
        {
            // create new account on fly
            account = _database.CreateAccount(uin, passwordHash);

            if (account == null)
            {
                Logger.Error(client, $"Failed to create account");
                // TODO send error rsp
                return;
            }
        }

        if (account.PasswordHash != passwordHash)
        {
            Logger.Error(client, $"Invalid Password");
            // TODO send error rsp
            return;
        }

        client.Account = account;
        Logger.Info(client, $"Authentication Success (Uin:{uin})");

        // TODO clean up after this point
        TpduPacket syn = new TpduPacket();
        syn.Cmd = TpduCmd.TPDU_CMD_SYN;
        IBuffer headExt = new StreamBuffer();
        byte[] encryptSynInfo = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        TdpuCrypto crypto = client.GetTdpuCrypto();
        if (crypto != null)
        {
            encryptSynInfo = crypto.Encrypt(encryptSynInfo);
        }

        //    "Len" / Int8ub,
        headExt.WriteByte((byte)encryptSynInfo.Length);
        // "EncryptSynInfo" / Bytes(this.Len),
        headExt.WriteBytes(encryptSynInfo);
        syn.HeaderExt = headExt.GetAllBytes();
        client.SendTpdu(syn);
    }
}