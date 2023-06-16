using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto
{
    public class CsProtoPacketFactory
    {
        private static readonly ServiceLogger Logger = LogProvider.Logger<ServiceLogger>(typeof(CsProtoPacketFactory));


        private readonly Setting _setting;

        private IBuffer _buffer;
        private int _position;
        private bool _readHeader;
        private CsProtoPacket _currentPacket;
        private int _dataSize;
        private uint _seqId;

        public CsProtoPacketFactory(Setting setting)
        {
            _setting = setting;
            _seqId = 0;
        }

        public byte[] Write(CsProtoPacket packet)
        {
            _seqId++;
            packet.SeqId = _seqId;
            packet.HeadLen = CsProtoPacket.HeaderLen;
            packet.BodyLen = (uint)packet.Body.Length;
            IBuffer buffer = new StreamBuffer();
            buffer.WriteUInt16((ushort)packet.Cmd, Endianness.Big);
            buffer.WriteUInt16(packet.HeadLen, Endianness.Big);
            buffer.WriteUInt32(packet.BodyLen, Endianness.Big);
            buffer.WriteUInt32(packet.SeqId, Endianness.Big);
            buffer.WriteUInt32(packet.NoUse, Endianness.Big);

            packet.Header = buffer.GetAllBytes();

            buffer.WriteBytes(packet.Body);
            return buffer.GetAllBytes();
        }

        public List<CsProtoPacket> Read(byte[] data)
        {
            List<CsProtoPacket> packets = new List<CsProtoPacket>();
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
                if (!_readHeader && _buffer.Size - _buffer.Position >= CsProtoPacket.HeaderLen)
                {
                    _currentPacket = new CsProtoPacket();
                    _currentPacket.Source = PacketSource.Client;
                    _currentPacket.Header = _buffer.GetBytes(_buffer.Position, CsProtoPacket.HeaderLen);
                    _currentPacket.Cmd = (CsProtoCmd)_buffer.ReadUInt16(Endianness.Big);
                    _currentPacket.HeadLen = _buffer.ReadUInt16(Endianness.Big);
                    _currentPacket.BodyLen = _buffer.ReadUInt32(Endianness.Big);
                    _currentPacket.SeqId = _buffer.ReadUInt32(Endianness.Big);
                    _currentPacket.NoUse = _buffer.ReadUInt32(Endianness.Big);
                    _readHeader = true;
                    _dataSize = (int)_currentPacket.BodyLen;
                }

                if (_readHeader && _buffer.Size - _buffer.Position >= _dataSize)
                {
                    _currentPacket.Body = _buffer.ReadBytes(_dataSize);
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