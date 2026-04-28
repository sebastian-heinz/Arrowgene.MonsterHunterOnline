using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.Networking.SAEAServer;
using Arrowgene.Networking.SAEAServer.Consumer.BlockingQueueConsumption;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi;

public class TpduConsumer : ThreadedBlockingQueue
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(TpduConsumer));

    private readonly Dictionary<long, Client> _clients;
    private readonly object _lock;
    private readonly Dictionary<TpduCmd, ITpduHandler> _handlerLookup;


    public TpduConsumer(
        int orderingLaneCount,
        int queueCapacityPerLane,
        string identity
    ) : base(orderingLaneCount, queueCapacityPerLane, identity)
    {
        _lock = new object();
        _clients = new Dictionary<long, Client>();
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

        List<TpduPacket> packets = client.ReceiveTpdu(data);
        foreach (TpduPacket packet in packets)
        {
            HandlePacket(client, packet);
        }
    }

    private void HandlePacket(Client client, TpduPacket packet)
    {
        if (!_handlerLookup.TryGetValue(packet.Cmd, out ITpduHandler packetHandler))
        {
            Logger.LogUnhandledPacket(client, packet);
            return;
        }

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