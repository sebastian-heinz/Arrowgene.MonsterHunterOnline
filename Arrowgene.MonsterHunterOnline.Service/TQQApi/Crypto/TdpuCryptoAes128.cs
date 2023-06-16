using System.Security.Cryptography;

namespace Arrowgene.MonsterHunterOnline.Service.TQQApi.Crypto;

public class TdpuCryptoAes128 : TdpuCrypto
{
    public static byte[] Key = new byte[16]
    {
        0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01
    };

    public static byte[] IV = new byte[16]
    {
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F
    };


    private readonly Aes _aes;
    private TConnSecEnc _tConnSecEnc;
    private byte[] _key;
    private byte[] _iv;

    public TdpuCryptoAes128() : this(Key)
    {
    }

    public TdpuCryptoAes128(byte[] key) : this(key, IV)
    {
    }

    public TdpuCryptoAes128(byte[] key, byte[] iv)
    {
        _key = key;
        _iv = iv;
        _aes = Aes.Create();
        _aes.Mode = CipherMode.CBC;
        _aes.Padding = PaddingMode.None;
    }

    public override TConnSecEnc TConnSecEnc => TConnSecEnc.TCONN_SEC_AES;

    public override TdpuCrypto GetSafeInstance()
    {
        return new TdpuCryptoAes128(_key, _iv);
    }

    public override byte[] Encrypt(byte[] plain)
    {
        if (plain == null || plain.Length <= 0)
        {
            return plain;
        }

        plain = Tsf4gPad(plain, 16);
        ICryptoTransform encryptor = _aes.CreateEncryptor(_key, _iv);
        return encryptor.TransformFinalBlock(plain, 0, plain.Length);
    }

    public override byte[] Decrypt(byte[] cipher)
    {
        if (cipher == null || cipher.Length <= 0)
        {
            return cipher;
        }
        ICryptoTransform decryptor = _aes.CreateDecryptor(_key, _iv);
        byte[] decrypted = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
        return Tsf4gUnPad(decrypted);
    }
}