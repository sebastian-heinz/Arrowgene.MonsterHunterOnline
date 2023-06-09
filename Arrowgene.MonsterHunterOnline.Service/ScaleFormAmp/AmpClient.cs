using System;
using System.Collections.Generic;
using System.Net;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Client.SyncReceive;
using Arrowgene.Networking.Tcp.Consumer.EventConsumption;
using Arrowgene.Networking.Udp;

namespace Arrowgene.MonsterHunterOnline.Service.ScaleformAmp;

public class AmpClient
{
    private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(AmpClient));

    private const int DefaultMaxPayloadSizeBytes = 1024;

    private readonly UdpSocket _udp;
    private readonly SyncReceiveTcpClient _tcp;
    private readonly AmpPacketFactory _packetFactory;

    public AmpClient()
    {
        _packetFactory = new AmpPacketFactory();

        _udp = new UdpSocket(DefaultMaxPayloadSizeBytes);
        _udp.ReceivedPacket += UdpOnReceivedPacket;

        EventConsumer consumer = new EventConsumer();
        consumer.ClientConnected += ConsumerOnClientConnected;
        consumer.ClientDisconnected += ConsumerOnClientDisconnected;
        consumer.ReceivedPacket += ConsumerOnReceivedPacket;
        _tcp = new SyncReceiveTcpClient(consumer);
        _tcp.Name = "HeartBeatClient";
    }

    public void Start()
    {
        IPEndPoint broadcastIpEndPoint = new IPEndPoint(IPAddress.Any, 7533);
        _udp.StartListen(broadcastIpEndPoint);
    }

    public void Send(AmpPacket packet)
    {
        byte[] data = _packetFactory.Write(packet);
        _tcp.Send(data);
    }

    private void UdpOnReceivedPacket(object sender, ReceivedUdpPacketEventArgs e)
    {
        //Logger.Debug("BC " + e.RemoteIpEndPoint + Environment.NewLine + Util.HexDump(e.Received));
        // TODO parse TCP port

        if (_tcp.IsAlive)
        {
            return;
        }

        _tcp.Connect("127.0.0.1", 7534, TimeSpan.FromMinutes(1));
    }

    private void ConsumerOnReceivedPacket(object sender, ReceivedPacketEventArgs e)
    {
        Logger.Debug("ConsumerOnReceivedPacket " + Environment.NewLine + Util.HexDump(e.Data));
        List<AmpPacket> packets = _packetFactory.Read(e.Data);
        byte[] resp = new byte[] { 0x0a, 0, 0, 0, 09, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0xA, 0xB };
        _tcp.Send(resp);
    }

    private void ConsumerOnClientDisconnected(object sender, DisconnectedEventArgs e)
    {
        Logger.Debug("HeartBeat Client Disconnected");
    }

    private void ConsumerOnClientConnected(object sender, ConnectedEventArgs e)
    {
        Logger.Debug("HeartBeat Client Connected");
    }
}