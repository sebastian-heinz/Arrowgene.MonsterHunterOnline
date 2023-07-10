using System;
using System.Net;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.Tdr;

public class TdrBuffer
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(TdrBuffer));


    public static void WriteVarUInt32(IBuffer buffer, uint src)
    {
        if (!BitConverter.IsLittleEndian)
        {
            src = (uint)IPAddress.HostToNetworkOrder((int)src);
        }

        int c = 0;
        while (true)
        {
            byte[] bytes = BitConverter.GetBytes(src);
            byte b = (byte)(bytes[0] & (byte)127);
            src >>= 7;
            if (src != 0u)
            {
                b |= 128;
            }

            buffer.WriteByte(b);
            if (src == 0u)
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