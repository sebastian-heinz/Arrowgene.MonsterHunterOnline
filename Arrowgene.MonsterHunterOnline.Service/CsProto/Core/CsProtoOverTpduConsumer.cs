using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core;

public class CsProtoOverTpduConsumer
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CsProtoOverTpduConsumer));

    private readonly Setting _setting;
    private readonly Dictionary<CS_CMD_ID, ICsProtoHandler> _handlerLookup;

    public CsProtoOverTpduConsumer(Setting setting)
    {
        _setting = setting;
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

    public void HandleReceived(Client client, byte[] data)
    {
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
}