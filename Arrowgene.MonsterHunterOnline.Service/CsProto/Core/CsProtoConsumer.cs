using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.Networking.SAEAServer;
using Arrowgene.Networking.SAEAServer.Consumer.BlockingQueueConsumption;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public class CsProtoConsumer : ThreadedBlockingQueue
    {
        private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CsProtoConsumer));

        private readonly Dictionary<long, Client> _clients;
        private readonly object _lock;
        private readonly CsProtoPacketHandler _packetHandler;


        public CsProtoConsumer(
            CsProtoPacketHandler packetHandler,
            int orderingLaneCount,
            int queueCapacityPerLane,
            string identity
        ) : base(orderingLaneCount, queueCapacityPerLane, identity)
        {
            _packetHandler = packetHandler;
            _lock = new object();
            _clients = new Dictionary<long, Client>();
        }

        protected override void HandleReceived(ClientHandle clientHandle, byte[] data)
        {
            if (!clientHandle.IsAlive)
            {
                return;
            }

            Client client;
            lock (_lock)
            {
                if (!_clients.TryGetValue(clientHandle.UniqueId, out client))
                {
                    Logger.Error(clientHandle, "Client does not exist in lookup");
                    return;
                }
            }

            List<CsProtoPacket> packets = client.ReceiveCsProto(data);
            foreach (CsProtoPacket packet in packets)
            {
                _packetHandler.HandleReceived(client, packet);
            }
        }

        protected override void HandleDisconnected(ClientSnapshot clientSnapshot)
        {
            Client client;
            lock (_lock)
            {
                if (!_clients.Remove(clientSnapshot.UniqueId, out client))
                {
                    Logger.Error(clientSnapshot, "Disconnected client does not exist in lookup");
                    return;
                }
            }

            Logger.Info($"Disconnected: {client.Identity}");
        }

        protected override void HandleConnected(ClientHandle clientHandle)
        {
            Client client = new Client(clientHandle);
            client.SystemEncryptData = true;
            lock (_lock)
            {
                _clients.Add(clientHandle.UniqueId, client);
            }

            Logger.Info($"Connected: {client.Identity}");
        }

        protected override void HandleError(ClientSnapshot clientSnapshot, Exception exception, string message)
        {
            Logger.Exception(clientSnapshot, exception);
        }
    }
}