using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.TqqApi.Crypto;

namespace Arrowgene.MonsterHunterOnline.Service.TqqApi
{
    public class TpduPacketFactory
    {
        private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(TpduPacketFactory));


        private readonly Setting _setting;

        private IBuffer _buffer;
        private int _position;
        private bool _readHeader;
        private TpduPacket _currentPacket;
        private int _dataSize;

        private TdpuCrypto _tdpuCrypto;


        public TpduPacketFactory(Setting setting)
        {
            _setting = setting;
            _tdpuCrypto = null;
        }

        public void SetTdpuCrypto(TdpuCrypto tdpuCrypto)
        {
            _tdpuCrypto = tdpuCrypto;
        }

        public byte[] Write(TpduPacket packet)
        {
            byte[] headerExt = packet.HeaderExt;
            byte[] body = packet.Body;
            if (_tdpuCrypto != null)
            {
                // TODO! skip x bytes of header! wich is len() or other non crypted fields, 
                // crypto for header need to be applied on a per field basis
                // #headerExt = _tdpuCrypto.Encrypt(headerExt);
                // Okay we only encrpt body here, header is managed by application layer
                body = _tdpuCrypto.Encrypt(body);
            }

            packet.HeadLen = headerExt.Length + TpduPacket.HeaderLen;
            packet.EncHeadLen = (byte)packet.EncHead.Length;
            if (packet.EncHeadLen != packet.EncHead.Length)
            {
                // err
            }

            packet.BodyLen = body.Length + packet.EncHeadLen;

            IBuffer buffer = new StreamBuffer();
            buffer.WriteByte(packet.Magic);
            buffer.WriteByte(packet.Version);
            buffer.WriteByte((byte)packet.Cmd);
            buffer.WriteByte(packet.EncHeadLen);
            buffer.WriteInt32(packet.HeadLen, Endianness.Big);
            buffer.WriteInt32(packet.BodyLen, Endianness.Big);

            packet.Header = buffer.GetAllBytes();

            buffer.WriteBytes(headerExt);
            buffer.WriteBytes(packet.EncHead);
            buffer.WriteBytes(body);
            return buffer.GetAllBytes();
        }

        public List<TpduPacket> Read(byte[] data)
        {
            List<TpduPacket> packets = new List<TpduPacket>();
            if (_buffer == null)
            {
                _buffer = new StreamBuffer(data);
            }
            else
            {
                _buffer.SetPositionEnd();
                _buffer.WriteBytes(data);
            }

            _buffer.Position = _position;

            bool read = true;
            while (read)
            {
                read = false;
                if (!_readHeader && _buffer.Size - _buffer.Position >= TpduPacket.HeaderLen)
                {
                    _currentPacket = new TpduPacket();
                    _currentPacket.Header = _buffer.GetBytes(_buffer.Position, TpduPacket.HeaderLen);
                    _currentPacket.Magic = _buffer.ReadByte();
                    _currentPacket.Version = _buffer.ReadByte();
                    _currentPacket.Cmd = (TpduCmd)_buffer.ReadByte();
                    _currentPacket.EncHeadLen = _buffer.ReadByte();
                    _currentPacket.HeadLen = _buffer.ReadInt32(Endianness.Big);
                    _currentPacket.BodyLen = _buffer.ReadInt32(Endianness.Big);
                    _readHeader = true;
                    _dataSize = (_currentPacket.HeadLen - TpduPacket.HeaderLen) + _currentPacket.BodyLen;
                }

                if (_readHeader && _buffer.Size - _buffer.Position >= _dataSize)
                {
                    byte[] headerExt = _buffer.ReadBytes(_currentPacket.HeadLen - TpduPacket.HeaderLen);
                    byte[] encHead = Array.Empty<byte>();
                    byte[] body = _buffer.ReadBytes(_currentPacket.BodyLen);

                    if (_tdpuCrypto != null)
                    {
                        body = _tdpuCrypto.Decrypt(body);
                        _currentPacket.BodyLen = body.Length;
                    }

                    if (_currentPacket.Cmd == TpduCmd.TPDU_CMD_NONE)
                    {
                        Span<byte> bodySpan = body.AsSpan();
                        encHead = bodySpan.Slice(0, _currentPacket.EncHeadLen).ToArray();
                        body = bodySpan.Slice(
                            _currentPacket.EncHeadLen,
                            _currentPacket.BodyLen - _currentPacket.EncHeadLen
                        ).ToArray();
                    }

                    _currentPacket.HeaderExt = headerExt;
                    _currentPacket.EncHead = encHead;
                    _currentPacket.Body = body;
                    packets.Add(_currentPacket);
                    _currentPacket = null;
                    _readHeader = false;
                    read = _buffer.Position != _buffer.Size;
                }
            }

            if (_buffer.Position == _buffer.Size)
            {
                Reset();
            }
            else
            {
                _position = _buffer.Position;
            }

            return packets;
        }

        private void Reset()
        {
            _currentPacket = null;
            _readHeader = false;
            _dataSize = 0;
            _position = 0;
            _buffer = null;
        }
    }
}