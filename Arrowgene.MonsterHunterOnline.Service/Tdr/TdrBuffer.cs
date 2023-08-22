using System;
using System.Net;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr;

/**
 * Tencent Data Representation
 */
public static class TdrBuffer
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(TdrBuffer));

    public static void WriteTlvTag(this IBuffer buffer, int id, TlvType type)
    {
        buffer.WriteVarUInt32(Tlv.MakeTag(id, type));
    }

    public static void WriteVarUInt32(this IBuffer buffer, uint val)
    {
        if (!BitConverter.IsLittleEndian)
        {
            val = (uint)IPAddress.HostToNetworkOrder((int)val);
        }

        int c = 0;
        while (true)
        {
            byte[] bytes = BitConverter.GetBytes(val);
            byte b = (byte)(bytes[0] & (byte)127);
            val >>= 7;
            if (val != 0u)
            {
                b |= 128;
            }

            buffer.WriteByte(b);
            if (val == 0u)
            {
                return;
            }

            c++;
            if (c >= 5)
            {
                Logger.Error("WriteVarUInt32 Error");
                break;
            }
        }
    }
}