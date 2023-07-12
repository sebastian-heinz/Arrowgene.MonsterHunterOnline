using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp;
using Arrowgene.Networking.Tcp.Consumer.BlockingQueueConsumption;

namespace Arrowgene.MonsterHunterOnline.Service.TQQApi
{
    public class TpduConsumer : ThreadedBlockingQueueConsumer
    {
        private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(TpduConsumer));

        private readonly Dictionary<ITcpSocket, Client> _clients;
        private readonly object _lock;
        private readonly Setting _setting;

        public Action<Client> ClientDisconnected;
        public Action<Client> ClientConnected;
        private Dictionary<TpduCmd, ITpduHandler> _handlerLookup;

        public TpduConsumer(Setting setting) : base(setting.SocketSettings, setting.Name)
        {
            _setting = setting;
            _lock = new object();
            _clients = new Dictionary<ITcpSocket, Client>();
            _handlerLookup = new Dictionary<TpduCmd, ITpduHandler>();
        }

        public void AddHandler(ITpduHandler packetHandler)
        {
            if (_handlerLookup.ContainsKey(packetHandler.Cmd))
            {
                Logger.Error($"TpduPacketHandlerId: {packetHandler.Cmd} already exists");
            }
            else
            {
                _handlerLookup.Add(packetHandler.Cmd, packetHandler);
            }
        }

        protected override void HandleReceived(ITcpSocket socket, byte[] data)
        {
            // Logger.Error(Environment.NewLine + Util.HexDump(data));
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

            List<TpduPacket> packets = client.ReceiveTpdu(data);
            foreach (TpduPacket packet in packets)
            {
                HandlePacket(client, packet);
            }
        }

        private void HandlePacket(Client client, TpduPacket packet)
        {
            if (!_handlerLookup.ContainsKey(packet.Cmd))
            {
                Logger.LogUnhandledPacket(client, packet);
                return;
            }

            ITpduHandler packetHandler = _handlerLookup[packet.Cmd];
            try
            {
                packetHandler.Handle(client, packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(client, ex);
                Logger.LogPacketError(client, packet);
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