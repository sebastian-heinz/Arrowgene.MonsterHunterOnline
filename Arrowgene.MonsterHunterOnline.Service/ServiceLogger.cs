using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto;
using Arrowgene.MonsterHunterOnline.Service.TQQApi;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.MonsterHunterOnline.Service
{
    public class ServiceLogger : Logger
    {
        private Setting _setting;

        private static List<CsProtoCmd> _ignore = new List<CsProtoCmd>()
        {
            CsProtoCmd.CS_CMD_SYSTEM_TRANS_ANTI_DATA,
            CsProtoCmd.CS_CMD_SYSTEM_PKG_TIMER_RECORD
        };

        public override void Initialize(string identity, string name, Action<Log> write)
        {
            base.Initialize(identity, name, write);
        }

        public override void Configure(object loggerTypeConfig, object identityConfig)
        {
            base.Configure(loggerTypeConfig, identityConfig);
        }

        public void Hex(byte[] data)
        {
            Info($"{Util.HexDump(data)}");
        }

        public void Info(Client client, string message)
        {
            Info($"{client.Identity} {message}");
        }

        public void Debug(Client client, string message)
        {
            Debug($"{client.Identity} {message}");
        }

        public void Error(Client client, string message)
        {
            Error($"{client.Identity} {message}");
        }

        public void Exception(Client client, Exception exception)
        {
            if (exception == null)
            {
                Write(LogLevel.Error, $"{client.Identity} Exception was null.", null);
            }
            else
            {
                Write(LogLevel.Error, $"{client.Identity} {exception}", exception);
            }
        }

        public void Info(ITcpSocket socket, string message)
        {
            Info($"[{socket.Identity}] {message}");
        }

        public void Debug(ITcpSocket socket, string message)
        {
            Debug($"[{socket.Identity}] {message}");
        }

        public void Error(ITcpSocket socket, string message)
        {
            Error($"[{socket.Identity}] {message}");
        }

        public void Exception(ITcpSocket socket, Exception exception)
        {
            if (exception == null)
            {
                Write(LogLevel.Error, $"{socket.Identity} Exception was null.", null);
            }
            else
            {
                Write(LogLevel.Error, $"{socket.Identity} {exception}", exception);
            }
        }

        public void LogPacket(Client client, TpduPacket packet)
        {
            Write(LogLevel.Info,
                $"PACKET:{client.Identity}{Environment.NewLine}{packet}",
                packet
            );
        }

        public void LogPacket(Client client, CsProtoPacket packet)
        {
            if (_ignore.Contains(packet.Cmd))
            {
                return;
            }

            Write(LogLevel.Info,
                $"PACKET:{client.Identity}{Environment.NewLine}{packet}",
                packet
            );
        }

        public void LogPacketError(Client client, TpduPacket packet)
        {
            Write(LogLevel.Error,
                $"PACKET ERROR:{client.Identity}{Environment.NewLine}{packet}",
                packet
            );
        }

        public void LogPacketError(Client client, CsProtoPacket packet)
        {
            Write(LogLevel.Error,
                $"PACKET ERROR:{client.Identity}{Environment.NewLine}{packet}",
                packet
            );
        }

        public void LogUnhandledPacket(Client client, TpduPacket packet)
        {
            Write(LogLevel.Error,
                $"PACKET UNHANDLED:{Environment.NewLine}{client.Identity}{Environment.NewLine}{packet}",
                packet
            );
        }

        public void LogUnhandledPacket(Client client, CsProtoPacket packet)
        {
            Write(LogLevel.Error,
                $"PACKET UNHANDLED:{Environment.NewLine}{client.Identity}{Environment.NewLine}{packet}",
                packet
            );
        }
    }
}