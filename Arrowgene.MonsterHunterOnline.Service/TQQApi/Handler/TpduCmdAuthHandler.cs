using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.TQQApi.Crypto;
using Arrowgene.MonsterHunterOnline.Service.TQQApi.Model;

namespace Arrowgene.MonsterHunterOnline.Service.TQQApi.Handler;

public class TpduCmdAuthHandler : ITpduHandler
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(TpduCmdAuthHandler));

    public TpduCmd Cmd => TpduCmd.TPDU_CMD_AUTH;

    public void Handle(Client client, TpduPacket packet)
    {
        // "EncMethod" / Enum(Int32sb, TCONN_SEC_ENC),
        // "ServiceID" / Int32sb,
        // "AuthType" / Enum(Int32sb, TCONN_SEC_AUTH),
        // "AuthData" / TPDUExtAuthData(selector=lambda this: int(this.AuthType)),

        IBuffer req = new StreamBuffer(packet.HeaderExt);
        req.SetPositionStart();
        TConnSecEnc EncMethod = (TConnSecEnc)req.ReadInt32(Endianness.Big);
        int ServiceID = req.ReadInt32();
        TconnSecAuth AuthType = (TconnSecAuth)req.ReadInt32(Endianness.Big);
        switch (AuthType)
        {
            case TconnSecAuth.TCONN_SEC_AUTH_QQV1:
            case TconnSecAuth.TCONN_SEC_AUTH_QQV2:
                TQQAuthInfo authInfo = new TQQAuthInfo();
                authInfo.Uin = req.ReadUInt32(Endianness.Big);
                authInfo.SignLen = req.ReadByte();
                authInfo.SignData = req.ReadBytes(authInfo.SignLen);
                authInfo.Sign2Len = req.ReadByte();
                authInfo.Sign2Data = req.ReadBytes(authInfo.Sign2Len);
                break;
            case TconnSecAuth.TCONN_SEC_AUTH_QQUNIFIED:
                TQQUnifiedSig unifiedSig = new TQQUnifiedSig();
                unifiedSig.Version = req.ReadInt16(Endianness.Big);
                unifiedSig.Time = req.ReadUInt32(Endianness.Big);
                unifiedSig.EncryptSignLen = req.ReadInt16(Endianness.Big);
                unifiedSig.EncryptSignData = req.ReadBytes(unifiedSig.EncryptSignLen);
                break;
        }

        switch (EncMethod)
        {
            case TConnSecEnc.TCONN_SEC_AES:
                client.SetTdpuCrypto(new TdpuCryptoAes128());
                break;
            case TConnSecEnc.TCONN_SEC_NONE:
                client.SetTdpuCrypto(null);
                break;
            default:
                Logger.Error($"requested EncMethod:{EncMethod} not supported");
                break;
        }


        Logger.Info(client, $"EncMethod:{client.TConnSecEnc}");

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