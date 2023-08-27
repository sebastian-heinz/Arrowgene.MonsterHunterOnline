using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.CsProto;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Core;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Enums;
using Arrowgene.MonsterHunterOnline.Service.CsProto.Structures;
using Arrowgene.MonsterHunterOnline.Service.System;
using Arrowgene.MonsterHunterOnline.Service.System.CharacterSystem;
using Arrowgene.MonsterHunterOnline.Service.System.ItemSystem;
using Arrowgene.MonsterHunterOnline.Service.TqqApi;
using Arrowgene.MonsterHunterOnline.Service.TqqApi.Crypto;
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

        public bool SystemEncryptData { get; set; }

        public Client(ITcpSocket socket, Setting setting)
        {
            _socket = socket;
            Identity = socket.Identity;
            _tpduPacketFactory = new TpduPacketFactory(setting);
            _csProtoPacketFactory = new CsProtoPacketFactory(setting);
            _tdpuCrypto = null;
            State = new PlayerState(this);
        }

        public string Identity { get; protected set; }

        public DateTime PingTime { get; set; }

        public PlayerState State { get; set; }

        public TConnSecEnc TConnSecEnc => _tdpuCrypto?.TConnSecEnc ?? TConnSecEnc.TCONN_SEC_NONE;
        public Account Account { get; set; }
        public Character Character { get; set; }
        public Inventory Inventory { get; set; }

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

        // TODO remove this, CSPacket is obsolete
        public void SendCsPacket(CsPacket packet)
        {
            SendCsProtoPacket(packet.BuildPacket());
        }

        public void SendCsProtoStructurePacket(ICsProtoStructurePacket packet)
        {
            SendCsProtoStructure(packet.Cmd, packet);
        }

        public void SendCsProtoStructure(CS_CMD_ID cmd, IStructure structure)
        {
            IBuffer buffer = new StreamBuffer();
            structure.Write(buffer);
            CsProtoPacket packet = new CsProtoPacket();
            packet.Body = buffer.GetAllBytes();
            packet.Cmd = cmd;
            SendCsProtoPacket(packet);
        }

        public void SendCsProtoPacket(CsProtoPacket packet)
        {
            byte[] csProtoData;
            try
            {
                packet.Source = PacketSource.Server;
                csProtoData = _csProtoPacketFactory.Write(packet);
            }
            catch (Exception ex)
            {
                Logger.Exception(this, ex);
                return;
            }

            if (SystemEncryptData)
            {
                // TODO this should  probably be a setting on packet level
                SendRaw(csProtoData);
                Logger.LogPacket(this, packet);
                return;

                //string key = "MultiByteToWideC";
                string key = "GetSystemDirecto";
                byte[] encData = new byte[csProtoData.Length];
                for (int i = 0; i < csProtoData.Length; i++)
                {
                    encData[i] = (byte)(key[i % key.Length] ^ csProtoData[i]);
                }

                CsTPacket<CSPkgEncryptData> resp = NewCsPacket.PkgEncryptData(new CSPkgEncryptData()
                {
                    EncryptData = new List<byte>(encData),
                });

                CsProtoPacket csProtoSystemEncryptPacket = resp.BuildPacket();
                csProtoSystemEncryptPacket.Source = PacketSource.Server;
                byte[] respData = _csProtoPacketFactory.Write(csProtoSystemEncryptPacket);

                SendRaw(respData);
                Logger.LogPacket(this, packet);
                Logger.LogPacket(this, csProtoSystemEncryptPacket);
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