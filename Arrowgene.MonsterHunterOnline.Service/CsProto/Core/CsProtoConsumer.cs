using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public class CsProtoConsumer : ThreadedBlockingQueueConsumer
    {
        private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CsProtoConsumer));

        private readonly Dictionary<ITcpSocket, Client> _clients;
        private readonly object _lock;
        private readonly Setting _setting;
        private readonly CsProtoPacketHandler _packetHandler;

        public Action<Client> ClientDisconnected;
        public Action<Client> ClientConnected;

        public CsProtoConsumer(Setting setting, CsProtoPacketHandler packetHandler) : base(setting.SocketSettings,
            setting.Name)
        {
            _setting = setting;
            _packetHandler = packetHandler;
            _lock = new object();
            _clients = new Dictionary<ITcpSocket, Client>();
        }

        protected override void HandleReceived(ITcpSocket socket, byte[] data)
        {
            if (!socket.IsAlive)
            {
                return;
            }

            Client client;
            lock (_lock)
            {
                if (!_clients.ContainsKey(socket))
                {
                    Logger.Error(socket, "Client does not exist in lookup");
                    return;
                }

                client = _clients[socket];
            }

            List<CsProtoPacket> packets = client.ReceiveCsProto(data);
            foreach (CsProtoPacket packet in packets)
            {
                _packetHandler.HandleReceived(client, packet);
            }
        }

        protected override void HandleDisconnected(ITcpSocket socket)
        {
            Client client;
            lock (_lock)
            {
                if (!_clients.ContainsKey(socket))
                {
                    Logger.Error(socket, $"Disconnected client does not exist in lookup");
                    return;
                }

                client = _clients[socket];
                _clients.Remove(socket);
            }

            Action<Client> onClientDisconnected = ClientDisconnected;
            if (onClientDisconnected != null)
            {
                try
                {
                    onClientDisconnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    Logger.Exception(client, ex);
                }
            }

            Logger.Info($"Disconnected: {client.Identity}");
        }

        protected override void HandleConnected(ITcpSocket socket)
        {
            Client client = new Client(socket, _setting);
            client.SystemEncryptData = true;
            lock (_lock)
            {
                _clients.Add(socket, client);
            }

            Logger.Info($"Connected: {client.Identity}");

            Action<Client> onClientConnected = ClientConnected;
            if (onClientConnected != null)
            {
                try
                {
                    onClientConnected.Invoke(client);
                }
                catch (Exception ex)
                {
                    Logger.Exception(client, ex);
                }
            }
        }
    }
}