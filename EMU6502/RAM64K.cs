﻿// MIT License
// 
// Copyright (c) 2016 Yve Verstrepen
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.IO;

namespace EMU6502
{
    public class RAM64K
    {
        readonly byte[] _ram;

        //TODO: Buffer objects, shadowing etc, accept byte[] for pieces of memory and make them toggleable
        
        public RAM64K()
        {
            _ram = new byte[0x10000];
            for (int i = 0; i < _ram.Length; i++)
                _ram[i] = 0xFF;
        }

        public byte this[ushort address]
        {
            get { return Read(address); }
            set { Write(address, value); }
        }

        public virtual byte Read(ushort address)
        {
            return _ram[address];
        }
        public ushort Read16(ushort address)
        {
            byte a = Read(address);
            byte b = Read(++address);
            return (ushort)((b << 8) | a);
            // Let's keep things readable shall we
            //return (ushort)((Read(++address) << 8) | Read(--address));
        }
        public virtual void Read(ushort address, byte[] buffer, int size = 0)
        {
            if (size == 0)
                size = buffer.Length;
            size = Math.Min(size, 65536 - address);
            Buffer.BlockCopy(_ram, address, buffer, 0, size);
        }

        public virtual void Write(ushort address, byte value)
        {
            _ram[address] = value;
        }
        public void Write16(ushort address, ushort value)
        {
            Write(address, (byte)(value & 0xFF));
            Write(++address, (byte)(value >> 8));
        }

        public void Load(Stream stream, ushort address = 0x0000, int size = 0)
        {
            if (size <= 0)
                size = (int)(stream.Length - stream.Position);
            size = Math.Min(size, 65536 - address);
            stream.Read(_ram, address, size);
        }
        public void Load(byte[] data, ushort address = 0x0000, int size = 0)
        {
            if (size <= 0)
                size = data.Length;
            size = Math.Min(size, 65536 - address);
            Load(new MemoryStream(data), address, size);
        }

        public void Save(Stream stream, ushort address = 0x0000, int size = 0)
        {
            throw new NotImplementedException();
        }

    }
}
