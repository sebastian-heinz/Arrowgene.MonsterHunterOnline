using System;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.IIPS;

public class IIPSFile
{
    private static readonly ILogger Logger = LogProvider.Logger(typeof(IIPSFile));

    public const uint IIPSHeaderLength = 0xAC;
    public const uint IIPSMagic = 0x7366696E;
    public uint Magic { get; set; }
    public uint HeaderLength { get; set; }
    public ulong OffsetUnk { get; set; }
    public ulong OffsetA { get; set; }
    public ulong BetOffset { get; set; }
    public ulong HetOffset { get; set; }
    public ulong OffsetE { get; set; }
    public ulong OffsetALength { get; set; }
    public ulong BetLength { get; set; }
    public ulong HetLength { get; set; }
    public ulong OffsetELength { get; set; }
    public string BetMd5 { get; set; }
    public string HetMd5 { get; set; }
    public string HeaderMd5 { get; set; }

    public void Open(string path)
    {
        StreamBuffer b = new StreamBuffer(path);
        b.SetPositionStart();
        Magic = b.ReadUInt32();
        if (IIPSMagic != Magic)
        {
            Logger.Info($"MAGIC miss match");
        }

        HeaderLength = b.ReadUInt32();
        if (IIPSHeaderLength != HeaderLength)
        {
            Logger.Info($"header length miss match");
        }

        uint unk0 = b.ReadUInt32();
        OffsetUnk = b.ReadUInt64();
        BetOffset = b.ReadUInt64();
        HetOffset = b.ReadUInt64();
        OffsetA = b.ReadUInt64();
        OffsetE = b.ReadUInt64();
        HetLength = b.ReadUInt64();
        BetLength = b.ReadUInt64();
        OffsetALength = b.ReadUInt64();
        OffsetELength = b.ReadUInt64();
        uint unk1 = b.ReadUInt32();
        uint unk2 = b.ReadUInt32();
        b.ReadUInt32();
        b.ReadUInt32();
        b.ReadUInt32();
        b.ReadUInt32();
        b.ReadUInt32();
        b.ReadUInt32();
        b.ReadUInt32();
        b.ReadUInt32();
        byte[] md5Bet = b.ReadBytes(16);
        byte[] md5Het = b.ReadBytes(16);
        byte[] md5Unk = b.ReadBytes(16);
        BetMd5 = BitConverter.ToString(md5Bet).Replace("-", "").ToLowerInvariant();
        HetMd5 = BitConverter.ToString(md5Het).Replace("-", "").ToLowerInvariant();
        HeaderMd5 = BitConverter.ToString(md5Unk).Replace("-", "").ToLowerInvariant();

        if (BetOffset + BetLength > (ulong)b.Size)
        {
            Logger.Info($"BET overflow");
        }

        byte[] md5BetTest = b.GetBytes((int)BetOffset, (int)BetLength);
        string BetMd5Test = IIPSCrypto.Md5(md5BetTest);

        if (BetMd5 != BetMd5Test)
        {
            Logger.Info($"BET MD5 miss match");
        }

        if (HetOffset + HetLength > (ulong)b.Size)
        {
            Logger.Info($"HET overflow");
        }

        byte[] md5hetTest = b.GetBytes((int)HetOffset, (int)HetLength);
        string hetMd5Test = IIPSCrypto.Md5(md5hetTest);
        if (HetMd5 != hetMd5Test)
        {
            Logger.Info($"HET MD5 miss match");
        }

        byte[] md5headerTest = b.GetBytes(0, (int)HeaderLength - 16);
        string headerMd5Test = IIPSCrypto.Md5(md5headerTest);
        if (HeaderMd5 != headerMd5Test)
        {
            Logger.Info($"Header MD5 miss match");
        }

        b.Position = (int)HetOffset;
        uint hetMagic = b.ReadUInt32();
        uint hetUnk = b.ReadUInt32();
        uint hetDataLength = b.ReadUInt32();
        byte[] hetData = b.ReadBytes((int)hetDataLength);
        IIPSCrypto.Crypt(hetData);
        Logger.Info(Environment.NewLine + Util.HexDump(hetData));

        b.Position = (int)BetOffset;
        uint betMagic = b.ReadUInt32();
        uint betUnk = b.ReadUInt32();
        uint betDataLength = b.ReadUInt32();
        byte[] betData = b.ReadBytes((int)betDataLength);
        IIPSCrypto.Crypt(betData);
        Logger.Info(Environment.NewLine + Util.HexDump(betData));


        // 6E 69 66 73 - magic
        // AC 00 00 00 - header size
        // 00 00 05 00 -> 5 in cl
        // 00 40 C1 1B - A off o:465649664 l:454752
        // 00 00 00 00

        // AB 46 BE 1B - B off o:465454763 l:184297 BET
        // 00 00 00 00
        // 08 0B BE 1B - C off o:465439496 l:15251 HET
        // 00 00 00 00
        // 00 40 C1 1B - A off o:465649664 l:454752
        // 00 00 00 00
        // 60 30 C8 1B - E off o:466104416 l:28421
        // 00 00 00 00
        // 93 3B 00 00 - C len o:465439496 l:15251
        // 00 00 00 00
        // E9 CF 02 00 - B len o:465454763 l:184297
        // 00 00 00 00
        // 60 F0 06 00 - A len o:465649664 l:454752
        // 00 00 00 00
        // 05 6F 00 00 - E len o:466104416 l:28421
        // 00 00 00 00

        // 00 40 00 00
        // 00 40 00 00

        // 00 00 00 00
        // 00 00 00 00
        // 00 00 00 00
        // 00 00 00 00
        // 00 00 00 00
        // 00 00 00 00
        // 00 00 00 00
        // 00 00 00 00
        // A1 77 4D DC 89 FA F0 E6 51 F3 8A 51 8F EB 5E 9B -md5b
        // 5B CC D5 10 CD E4 92 4D D1 CF 25 E0 85 1B 48 0E -md5C
        // 79 1E B2 3E F8 DF AD 89 89 4E A9 D8 4A 81 51 D3 -md5header
    }
}