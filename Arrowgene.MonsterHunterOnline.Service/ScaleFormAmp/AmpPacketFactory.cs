using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;

namespace Arrowgene.MonsterHunterOnline.Service.ScaleformAmp
{
    public class AmpPacketFactory
    {
        private static readonly ServiceLogger
            Logger = LogProvider.Logger<ServiceLogger>(typeof(AmpPacketFactory));


        private IBuffer _buffer;
        private int _position;
        private bool _readHeader;
        private int _dataSize;


        public AmpPacketFactory()
        {
       
        }

        public byte[] Write(AmpPacket packet)
        {
            IBuffer buffer = new StreamBuffer();
            return buffer.GetAllBytes();
        }

        public List<AmpPacket> Read(byte[] data)
        {
            List<AmpPacket> packets = new List<AmpPacket>();
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
                if (!_readHeader && _buffer.Size - _buffer.Position >= AmpPacket.HeaderLen)
                {
                    int packetLen = _buffer.ReadInt32(Endianness.Big);
                    _dataSize = packetLen - AmpPacket.HeaderLen;
                    _readHeader = true;
                }

                if (_readHeader && _buffer.Size - _buffer.Position >= _dataSize)
                {
                    byte[] body = _buffer.ReadBytes(_dataSize);

                    AmpPacket packet = new AmpPacket();

                    packets.Add(packet);
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
            _readHeader = false;
            _dataSize = 0;
            _position = 0;
            _buffer = null;
        }
    }
}