using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Arrowgene.MonsterHunterOnline.Service.Tdr;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public abstract class Structure
    {
        private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(Structure));

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = false,
        };

        public string JsonDump()
        {
            return JsonSerializer.Serialize(this, GetType(), JsonSerializerOptions);
        }

        protected void WriteString(IBuffer buffer, string val)
        {
            buffer.WriteInt32(val.Length + 1, Endianness.Big);
            buffer.WriteCString(val);
        }

        protected string ReadString(IBuffer buffer)
        {
            int valLength = buffer.ReadInt32(Endianness.Big);
            string val = buffer.ReadCString();
            return val;
        }

        protected long ReadInt64(IBuffer buffer)
        {
            return buffer.ReadInt64(Endianness.Big);
        }

        protected void WriteInt64(IBuffer buffer, long val)
        {
            buffer.WriteInt64(val, Endianness.Big);
        }

        protected ulong ReadUInt64(IBuffer buffer)
        {
            return buffer.ReadUInt64(Endianness.Big);
        }

        protected void WriteUInt64(IBuffer buffer, ulong val)
        {
            buffer.WriteUInt64(val, Endianness.Big);
        }

        protected int ReadInt32(IBuffer buffer)
        {
            return buffer.ReadInt32(Endianness.Big);
        }

        protected void WriteInt32(IBuffer buffer, int val)
        {
            buffer.WriteInt32(val, Endianness.Big);
        }

        protected uint ReadUInt32(IBuffer buffer)
        {
            return buffer.ReadUInt32(Endianness.Big);
        }

        protected void WriteUInt32(IBuffer buffer, uint val)
        {
            buffer.WriteUInt32(val, Endianness.Big);
        }

        protected ushort ReadUInt16(IBuffer buffer)
        {
            return buffer.ReadUInt16(Endianness.Big);
        }

        protected void WriteUInt16(IBuffer buffer, ushort val)
        {
            buffer.WriteUInt16(val, Endianness.Big);
        }

        protected short ReadInt16(IBuffer buffer)
        {
            return buffer.ReadInt16(Endianness.Big);
        }

        protected void WriteInt16(IBuffer buffer, short val)
        {
            buffer.WriteInt16(val, Endianness.Big);
        }

        protected byte ReadByte(IBuffer buffer)
        {
            return buffer.ReadByte();
        }

        protected void WriteByte(IBuffer buffer, byte val)
        {
            buffer.WriteByte(val);
        }

        protected void WriteBool(IBuffer buffer, bool val)
        {
            buffer.WriteByte(val ? (byte)1 : (byte)0);
        }

        protected bool ReadBool(IBuffer buffer)
        {
            byte b = buffer.ReadByte();
            return b != 0;
        }

        protected void WriteFloat(IBuffer buffer, float val)
        {
            buffer.WriteFloat(val, Endianness.Big);
        }

        protected float ReadFloat(IBuffer buffer)
        {
            return buffer.ReadFloat(Endianness.Big);
        }

        protected void ReadArray<T>(IBuffer buffer, T[] dst, int limit, Func<IBuffer, T> readFn)
        {
            if (limit >= dst.Length)
            {
                // Err
            }

            for (int i = 0; i < limit; i++)
            {
                dst[i] = readFn(buffer);
            }
        }

        protected void WriteArray<T>(IBuffer buffer, T[] src, int limit, Action<IBuffer, T> writeFn)
        {
            if (limit >= src.Length)
            {
                // Err
            }

            for (int i = 0; i < src.Length; i++)
            {
                writeFn(buffer, src[i]);
            }
        }

        protected void ReadList<TSize, TVal>(
            IBuffer buffer,
            List<TVal> dst,
            TSize limit,
            Func<IBuffer, TSize> readSizeFn,
            Func<IBuffer, TVal> readValFn
        ) where TSize : IBinaryInteger<TSize>
        {
            TSize size = readSizeFn(buffer);
            if (size > limit)
            {
                size = limit;
            }

            dst.Clear();
            for (TSize i = TSize.Zero; i < size; i++)
            {
                TVal val = readValFn(buffer);
                dst.Add(val);
            }
        }

        protected void ReadList<TVal>(
            IBuffer buffer,
            List<TVal> dst,
            int count,
            int limit,
            Func<IBuffer, TVal> readValFn
        )
        {
            if (count > limit)
            {
                count = limit;
            }

            dst.Clear();
            for (int i = 0; i < count; i++)
            {
                TVal val = readValFn(buffer);
                dst.Add(val);
            }
        }

        protected void WriteList<TSize, TVal>(IBuffer buffer,
            List<TVal> src,
            TSize limit,
            Action<IBuffer, TSize> writeSizeFn,
            Action<IBuffer, TVal> writeValFn
        ) where TSize : IBinaryInteger<TSize>
        {
            TSize size = TSize.CreateChecked(src.Count);
            if (size > limit)
            {
                size = limit;
            }

            writeSizeFn(buffer, size);
            for (int i = 0; i < src.Count; i++)
            {
                TVal val = src[i];
                writeValFn(buffer, val);
            }
        }

        protected void WriteList<TVal>(IBuffer buffer,
            List<TVal> src,
            int count,
            int limit,
            Action<IBuffer, TVal> writeValFn
        )
        {
            int size = src.Count;
            if (count > size)
            {
                count = size;
            }

            if (count > limit)
            {
                count = limit;
            }

            for (int i = 0; i < size; i++)
            {
                TVal val = src[i];
                writeValFn(buffer, val);
            }
        }

        protected void WriteCsStructure(IBuffer buffer, ICsStructure val)
        {
            val.WriteCs(buffer);
        }

        protected TStructure ReadCsStructure<TStructure>(IBuffer buffer) where TStructure : ICsStructure, new()
        {
            TStructure val = new TStructure();
            val.ReadCs(buffer);
            return val;
        }

        protected TStructure ReadCsStructure<TStructure>(IBuffer buffer, TStructure val) where TStructure : ICsStructure
        {
            val.ReadCs(buffer);
            return val;
        }

        /// <summary>
        /// Writes a TLV structures, by prefixing its size and adding the magic byte 0x99 (TlvMagic.NoVariant)
        /// </summary>
        protected void WriteTlvStructure<TSize>(
            IBuffer buffer,
            ITlvStructure val,
            TSize limit,
            Action<IBuffer, TSize> writeSizeFn
        ) where TSize : IBinaryInteger<TSize>
        {
            // TODO performance
            // need to write the TLV length first, so client knows if there is tlv data
            // also need to check length against the limit before writing
            // at the moment using a tmp buffer, however it should be able to work with existing
            // buffer by adjusting position and length to delete in case of error.
            StreamBuffer tmp = new StreamBuffer();
            WriteByte(tmp, (byte)TlvMagic.NoVariant);
            val.WriteTlv(tmp);
            byte[] tlv = tmp.GetAllBytes();

            TSize size = TSize.CreateChecked(tlv.Length);
            if (size > limit)
            {
                writeSizeFn(buffer, TSize.Zero);
                Logger.Error($"WriteTlvStructure: (size[{size}] > limit[{limit}])");
                return;
            }

            writeSizeFn(buffer, size);
            buffer.WriteBytes(tlv);
        }

        protected void ReadTlvStructure<TSize>(
            IBuffer buffer,
            ITlvStructure val,
            TSize limit,
            Func<IBuffer, TSize> readSizeFn) where TSize : IBinaryInteger<TSize>
        {
            TSize size = readSizeFn(buffer);
            if (size > limit)
            {
                int intSize = int.CreateChecked(size);
                buffer.Position += intSize;
                Logger.Error($"ReadTlvStructure: (size[{size}] > limit[{limit}]) skipping:{intSize}");
                return;
            }

            val.ReadTlv(buffer);
        }


        protected T[] InitArray<T>(int count) where T : new()
        {
            T[] val = new T[count];
            for (int i = 0; i < count; i++)
            {
                val[i] = new T();
            }

            return val;
        }

        protected T[] InitArray<T>(int count, Func<T> factory)
        {
            T[] val = new T[count];
            for (int i = 0; i < count; i++)
            {
                val[i] = factory();
            }

            return val;
        }

        /// <summary>
        /// Writes a TLV sub structure, does not prefix the data with 0x99 (TlvMagic.NoVariant)
        /// </summary>
        protected void WriteTlvSubStructure(IBuffer buffer, int id, ITlvStructure val)
        {
            WriteTlvTag(buffer, id, TlvType.ID_4_BYTE);
            int subStartPos = buffer.Position;
            WriteInt32(buffer, 0);

            val.WriteTlv(buffer);

            int subEndPos = buffer.Position;
            int subSize = buffer.Position - subStartPos - 4;
            buffer.Position = subStartPos;
            buffer.WriteInt32(subSize, Endianness.Big);
            buffer.Position = subEndPos;
        }

        protected void WriteTlvSubStructureList<TITlvStructure>(
            IBuffer buffer,
            int id,
            int limit,
            List<TITlvStructure> val)
            where TITlvStructure : ITlvStructure
        {
            WriteTlvTag(buffer, id, TlvType.ID_4_BYTE);
            int subStartPos = buffer.Position;
            WriteInt32(buffer, 0);
            for (int i = 0; i < limit; i++)
            {
                val[i].WriteTlv(buffer);
            }

            int subEndPos = buffer.Position;
            int subSize = buffer.Position - subStartPos - 4;
            buffer.Position = subStartPos;
            buffer.WriteInt32(subSize, Endianness.Big);
            buffer.Position = subEndPos;
        }

        protected void WriteTlvTag(IBuffer buffer, int id, TlvType type)
        {
            buffer.WriteTlvTag(id, type);
        }

        protected void WriteTlvBool(IBuffer buffer, int id, bool val)
        {
            WriteTlvTag(buffer, id, TlvType.ID_1_BYTE);
            WriteBool(buffer, val);
        }

        protected void WriteTlvByte(IBuffer buffer, int id, byte val)
        {
            WriteTlvTag(buffer, id, TlvType.ID_1_BYTE);
            WriteByte(buffer, val);
        }

        protected void WriteTlvInt16(IBuffer buffer, int id, short val)
        {
            WriteTlvTag(buffer, id, TlvType.ID_2_BYTE);
            WriteInt16(buffer, val);
        }

        protected void WriteTlvUInt16(IBuffer buffer, int id, ushort val)
        {
            WriteTlvTag(buffer, id, TlvType.ID_2_BYTE);
            WriteUInt16(buffer, val);
        }

        protected void WriteTlvInt32(IBuffer buffer, int id, int val)
        {
            WriteTlvTag(buffer, id, TlvType.ID_4_BYTE);
            WriteInt32(buffer, val);
        }

        protected void WriteTlvUInt32(IBuffer buffer, int id, uint val)
        {
            WriteTlvTag(buffer, id, TlvType.ID_4_BYTE);
            WriteUInt32(buffer, val);
        }

        protected void WriteTlvInt64(IBuffer buffer, int id, long val)
        {
            WriteTlvTag(buffer, id, TlvType.ID_8_BYTE);
            WriteInt64(buffer, val);
        }

        protected void WriteTlvUInt64(IBuffer buffer, int id, ulong val)
        {
            WriteTlvTag(buffer, id, TlvType.ID_8_BYTE);
            WriteUInt64(buffer, val);
        }

        protected void WriteTlvInt32Arr(IBuffer buffer, int id, int[] val)
        {
            WriteTlvTag(buffer, id, TlvType.ID_4_BYTE);
            int count = val.Length;
            WriteInt32(buffer, count * 4);
            for (int i = 0; i < count; i++)
            {
                WriteInt32(buffer, val[i]);
            }
        }
    }
}