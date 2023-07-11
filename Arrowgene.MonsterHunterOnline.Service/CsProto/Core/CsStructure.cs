using System;
using System.Collections.Generic;
using System.Numerics;
using Arrowgene.Buffers;

namespace Arrowgene.MonsterHunterOnline.Service.CsProto.Core
{
    public abstract class CsStructure : IStructure
    {
        public abstract void Write(IBuffer buffer);
        public abstract void Read(IBuffer buffer);


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

        protected int ReadInt32(IBuffer buffer)
        {
            return buffer.ReadInt32(Endianness.Big);
        }

        protected void WriteInt32(IBuffer buffer, int val)
        {
            buffer.WriteInt32(val, Endianness.Big);
        }

        protected byte ReadByte(IBuffer buffer)
        {
            return buffer.ReadByte();
        }

        protected void WriteByte(IBuffer buffer, byte val)
        {
            buffer.WriteByte(val);
        }

        protected void ReadArray<T>(IBuffer buffer, int count, T[] dst, Func<IBuffer, T> readFn)
        {
            if (count >= dst.Length)
            {
                // Err
            }

            for (int i = 0; i < count; i++)
            {
                dst[i] = readFn(buffer);
            }
        }

        protected void WriteArray<T>(IBuffer buffer, T[] src, Action<IBuffer, T> writeFn)
        {
            for (int i = 0; i < src.Length; i++)
            {
                writeFn(buffer, src[i]);
            }
        }

        protected void ReadList<T>(IBuffer buffer, int count, List<T> dst, Func<IBuffer, T> readFn)
        {
            dst.Clear();
            for (int i = 0; i < count; i++)
            {
                T val = readFn(buffer);
                dst.Add(val);
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

        protected void ReadList<TSize, TVal>(IBuffer buffer, List<TVal> dst, Func<IBuffer, TSize> readSizeFn,
            Func<IBuffer, TVal> readValFn) where TSize : IBinaryInteger<TSize>
        {
            TSize size = readSizeFn(buffer);
            dst.Clear();
            for (TSize i = TSize.Zero; i < size; i++)
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

        protected void WriteList<TSize, TVal>(IBuffer buffer,
            List<TVal> src,
            Action<IBuffer, TSize> writeSizeFn,
            Action<IBuffer, TVal> writeValFn
        ) where TSize : IBinaryInteger<TSize>
        {
            TSize size = TSize.CreateChecked(src.Count);
            writeSizeFn(buffer, size);
            for (int i = 0; i < src.Count; i++)
            {
                TVal val = src[i];
                writeValFn(buffer, val);
            }
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