using Arrowgene.MonsterHunterOnline.Service.TQQApi.Crypto;
using Xunit;

namespace Arrowgene.MonsterHunterOnline.Test.Service.TQQApi.Crypto;

public class TestTdpuCryptoAes128
{
    [Fact]
    public void TestEncrypt()
    {
        TdpuCryptoAes128 crypto = new TdpuCryptoAes128();
        byte[] plain = new byte[17];
        byte[] cipher = crypto.Encrypt(plain);
    }
}