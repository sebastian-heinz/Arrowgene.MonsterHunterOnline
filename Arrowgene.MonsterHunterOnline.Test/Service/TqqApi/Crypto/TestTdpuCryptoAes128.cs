using Arrowgene.MonsterHunterOnline.Service.TqqApi.Crypto;
using Xunit;

namespace Arrowgene.MonsterHunterOnline.Test.Service.TqqApi.Crypto;

public class TestTdpuCryptoAes128
{
    [Fact]
    public void TestEncrypt()
    {
        TdpuCryptoAes128 crypto = new TdpuCryptoAes128();
        byte[] plain = new byte[17];
        byte[] cipher = crypto.Encrypt(plain);
        byte[] decrypted = crypto.Decrypt(cipher);
        Assert.Equal(plain, decrypted);
    }
}