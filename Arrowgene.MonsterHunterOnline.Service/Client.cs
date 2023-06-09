using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto;
using Arrowgene.MonsterHunterOnline.Service.TQQApi;
using Arrowgene.MonsterHunterOnline.Service.TQQApi.Crypto;
using Arrowgene.Networking.Tcp;

namespace Arrowgene.MonsterHunterOnline.Service
{
    public class Client
    {
        private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(Client));

        private readonly ITcpSocket _socket;
        private readonly TpduPacketFactory _tpduPacketFactory;
        private readonly CsProtoPacketFactory _csProtoPacketFactory;

        private TdpuCrypto _tdpuCrypto;

        public Client(ITcpSocket socket, Setting setting)
        {
            _socket = socket;
            Identity = socket.Identity;
            _tpduPacketFactory = new TpduPacketFactory(setting);
            _csProtoPacketFactory = new CsProtoPacketFactory(setting);
            _tdpuCrypto = null;
        }

        public string Identity { get; protected set; }

        public DateTime PingTime { get; set; }

        public TConnSecEnc TConnSecEnc => _tdpuCrypto?.TConnSecEnc ?? TConnSecEnc.TCONN_SEC_NONE;

        public TdpuCrypto GetTdpuCrypto()
        {
            return _tdpuCrypto?.GetSafeInstance();
        }

        public void SetTdpuCrypto(TdpuCrypto tdpuCrypto)
        {
            _tdpuCrypto = tdpuCrypto;
            _tpduPacketFactory.SetTdpuCrypto(tdpuCrypto);
        }

        public void Close()
        {
            _socket.Close();
        }

        /// <summary>
        /// Sends raw bytes to the client, without any further processing
        /// </summary>
        public void SendRaw(byte[] data)
        {
            _socket.Send(data);
        }

        public List<TpduPacket> ReceiveTpdu(byte[] data)
        {
            List<TpduPacket> packets;
            try
            {
                packets = _tpduPacketFactory.Read(data);
            }
            catch (Exception ex)
            {
                Logger.Exception(this, ex);
                packets = new List<TpduPacket>();
            }

            foreach (TpduPacket packet in packets)
            {
            //    Logger.LogPacket(this, packet);
            }

            return packets;
        }

        public void SendTpdu(TpduPacket packet)
        {
            byte[] data;
            try
            {
                data = _tpduPacketFactory.Write(packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(this, ex);
                return;
            }

            SendRaw(data);
           // Logger.LogPacket(this, packet);
        }

        public List<CsProtoPacket> ReceiveCsProto(byte[] data)
        {
            List<CsProtoPacket> packets;
            try
            {
                packets = _csProtoPacketFactory.Read(data);
            }
            catch (Exception ex)
            {
                Logger.Exception(this, ex);
                packets = new List<CsProtoPacket>();
            }

            foreach (CsProtoPacket packet in packets)
            {
                Logger.LogPacket(this, packet);
            }

            return packets;
        }

        public void SendCsProto(CsProtoPacket packet)
        {
            byte[] csProtoData;
            try
            {
                csProtoData = _csProtoPacketFactory.Write(packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(this, ex);
                return;
            }

            // CsProto is transferred over TPDU
            TpduPacket tpduPacket = new TpduPacket();
            tpduPacket.Cmd = TpduCmd.TPDU_CMD_NONE;
            tpduPacket.Body = csProtoData;
            SendTpdu(tpduPacket);

            Logger.LogPacket(this, packet);
        }
    }
}