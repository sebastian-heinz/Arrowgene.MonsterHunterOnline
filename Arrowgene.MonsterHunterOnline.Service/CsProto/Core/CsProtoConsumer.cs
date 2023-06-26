using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
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

        public Action<Client> ClientDisconnected;
        public Action<Client> ClientConnected;
        private Dictionary<CS_CMD_ID, ICsProtoHandler> _handlerLookup;

        public CsProtoConsumer(Setting setting) : base(setting.SocketSettings, setting.Name)
        {
            _setting = setting;
            _lock = new object();
            _clients = new Dictionary<ITcpSocket, Client>();
            _handlerLookup = new Dictionary<CS_CMD_ID, ICsProtoHandler>();
        }

        public void AddHandler(ICsProtoHandler packetHandler)
        {
            if (_handlerLookup.ContainsKey(packetHandler.Cmd))
            {
                Logger.Error($"CsProtoHandler: {packetHandler.Cmd} already exists");
            }
            else
            {
                _handlerLookup.Add(packetHandler.Cmd, packetHandler);
            }
        }

        public void HandleCustom(ITcpSocket socket, byte[] data)
        {
            HandleReceived(socket, data);
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
                HandlePacket(client, packet);
            }
        }

        private void HandlePacket(Client client, CsProtoPacket packet)
        {
            if (!_handlerLookup.ContainsKey(packet.Cmd))
            {
                Logger.LogUnhandledPacket(client, packet);
                return;
            }

            ICsProtoHandler packetHandler = _handlerLookup[packet.Cmd];
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