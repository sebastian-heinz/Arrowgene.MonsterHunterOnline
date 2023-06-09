using Arrowgene.Buffers;
using Arrowgene.MonsterHunterOnline.Service.TQQApi.Crypto;

namespace Arrowgene.MonsterHunterOnline.Service.TQQApi.Handler;

public class TpduCmdSynAckHandler : ITpduHandler
{
    public TpduCmd Cmd => TpduCmd.TPDU_CMD_SYNACK;

    public void Handle(Client client, TpduPacket packet)
    {
        TpduPacket ident = new TpduPacket();
        ident.Cmd = TpduCmd.TPDU_CMD_IDENT;
        IBuffer identHeadExt = new StreamBuffer();


        byte[] encryptIdent = new byte[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        TdpuCrypto crypto = client.GetTdpuCrypto();
        if (crypto != null)
        {
            encryptIdent = crypto.Encrypt(encryptIdent);
        }

        identHeadExt.WriteInt32(encryptIdent.Length, Endianness.Big); //  "Len" / Int32sb,
        // "EncryptIdent" / Bytes(this.Len),
        identHeadExt.WriteBytes(encryptIdent);
        ident.HeaderExt = identHeadExt.GetAllBytes();
        client.SendTpdu(ident);


        TpduPacket chgskey = new TpduPacket();
        chgskey.Cmd = TpduCmd.TPDU_CMD_CHGSKEY;
        IBuffer chgskeyHeadExt = new StreamBuffer();
        //    "Type" / Int16sb,
        chgskeyHeadExt.WriteInt16(0, Endianness.Big);
        //    "Len" / Int16sb,

        //  "EncryptSkey" / Bytes(this.Len),
        byte[] sKey = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        // TODO copy skey, and keep it to update client
        byte[] encryptSkey = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        crypto = client.GetTdpuCrypto();
        if (crypto != null)
        {
            encryptSkey = crypto.Encrypt(encryptSkey);
        }

        chgskeyHeadExt.WriteInt16((short)encryptSkey.Length, Endianness.Big);
        chgskeyHeadExt.WriteBytes(encryptSkey);
        chgskey.HeaderExt = chgskeyHeadExt.GetAllBytes();
        client.SendTpdu(chgskey);

        client.SetTdpuCrypto(new TdpuCryptoAes128(sKey));
    }
}