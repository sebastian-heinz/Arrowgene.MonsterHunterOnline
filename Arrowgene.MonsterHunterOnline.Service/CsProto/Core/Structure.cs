using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public abstract class Structure : IStructure
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true, // TODO for now cause of old types not using get/set
        };

        public abstract void Write(IBuffer buffer);
        public abstract void Read(IBuffer buffer);

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

        protected void WriteStructure(IBuffer buffer, IStructure val)
        {
            val.Write(buffer);
        }

        protected TStructure ReadStructure<TStructure>(IBuffer buffer) where TStructure : IStructure, new()
        {
            TStructure val = new TStructure();
            val.Read(buffer);
            return val;
        }

        protected TStructure ReadStructure<TStructure>(IBuffer buffer, TStructure val) where TStructure : IStructure
        {
            val.Read(buffer);
            return val;
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
    }
}