using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Protocol.Constant;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

public class CsProtoPacketHandler
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CsProtoPacketHandler));


    private readonly Dictionary<CS_CMD_ID, ICsProtoHandler> _handlerLookup;

    public CsProtoPacketHandler()
    {
        _handlerLookup = new Dictionary<CS_CMD_ID, ICsProtoHandler>();
    }

    public void AddHandler(ICsProtoHandler packetHandler)
    {
        if (!_handlerLookup.TryAdd(packetHandler.Cmd, packetHandler))
        {
            Logger.Error($"CsProtoHandler: {packetHandler.Cmd} already exists");
        }
    }

    public void HandleReceived(Client client, byte[] data)
    {
        List<CsProtoPacket> packets = client.ReceiveCsProto(data);
        foreach (CsProtoPacket packet in packets)
        {
            HandlePacket(client, packet);
        }
    }

    public void HandleReceived(Client client, CsProtoPacket packet)
    {
        HandlePacket(client, packet);
    }

    private void HandlePacket(Client client, CsProtoPacket packet)
    {
        if (!_handlerLookup.TryGetValue(packet.Cmd, out ICsProtoHandler packetHandler))
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
}