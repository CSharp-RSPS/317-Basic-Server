﻿using System.ComponentModel;
using System.Text;

/**
 * @Author blakeman8192
 **/
namespace RSPS.src.net.packet
{
    public class PacketWriter : Packet
    {

        private int lengthPosition = 0;

        /**
         * The current bit position.
         */
        private int bitPosition = 0;

        /**
         * The current AccessType of the buffer.
         */
        public AccessType ByteAccessType { get; private set; } = AccessType.ByteAccess;

        /**
         * Bit masks.
         */
        public static readonly int[] BitMasks = {
            0, 0x1, 0x3, 0x7, 0xf, 0x1f, 0x3f, 0x7f, 0xff, 0x1ff, 0x3ff, 0x7ff, 0xfff, 0x1fff, 0x3fff, 0x7fff, 0xffff,
            0x1ffff, 0x3ffff, 0x7ffff, 0xfffff, 0x1fffff, 0x3fffff, 0x7fffff, 0xffffff, 0x1ffffff, 0x3ffffff, 0x7ffffff,
            0xfffffff, 0x1fffffff, 0x3fffffff, 0x7fffffff, -1
        };

        public PacketWriter(int length) : base(length)
        {}

        /// <summary>
        /// Writes a packet header
        /// </summary>
        /// <param name="headerType">The header type</param>
        /// <param name="cipher">The encrypion cipher</param>
        /// <param name="opcode">The packet opcode</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteHeader(PacketHeaderType headerType, ISAACCipher cipher, int opcode)
        {
            WriteByte((opcode + cipher.getNextValue()) & 0xff);

            if (headerType != PacketHeaderType.Fixed)
            {
                lengthPosition = Pointer;
            }
            switch (headerType)
            {
                case PacketHeaderType.VariableByte:
                    WriteByte(0);
                    break;

                case PacketHeaderType.VariableShort:
                    WriteShort(0);
                    break;
            }
            return this;
        }

        /// <summary>
        /// Finishes a packet header
        /// </summary>
        /// <param name="headerType">The header type</param>
        /// <returns>The writer</returns>
        public PacketWriter FinishHeader(PacketHeaderType headerType)
        {
            if (headerType == PacketHeaderType.Fixed)
            { // Static headers don't require anything to finish
                return this;
            }
            int oldPosition = Pointer;
            Pointer = lengthPosition;

            switch (headerType)
            {
                case PacketHeaderType.VariableByte:
                    WriteByte((byte)(oldPosition - lengthPosition - 1));
                    break;

                case PacketHeaderType.VariableShort:
                    short value = (short)(oldPosition - lengthPosition - 2);
                    WriteByte((byte)(value >> 8));
                    WriteByte((byte)(value));
                    break;
            }
            Pointer = oldPosition;
            return this;
        }

        /**
         * Writes a value as a byte.
         *
         * @param value the value
         * @param type  the value type
         */
        public void WriteByte(int value, ValueType type)
        {
            //if (getAccessType() != AccessType.BYTE_ACCESS)
            //{
            //    throw new IllegalStateException("Illegal access type.");
            //}
            EnsureCapacity(1);
            switch (type)
            {
                case ValueType.Additional:
                    value += 128;
                    break;
                case ValueType.Negated:
                    value = -value;
                    break;
                case ValueType.Subtrahend:
                    value = 128 - value;
                    break;
            }
            Buffer[Pointer++] = ((byte)value);
            //Console.WriteLine(Payload.ReadByte());
        }

        /**
         * Writes a value as a normal byte.
         *
         * @param value the value
         */
        public void WriteByte(int value)
        {
            WriteByte(value, ValueType.Standard);
        }

        /**
         * Writes a value as a short.
         *
         * @param value the value
         * @param type  the value type
         * @param order the byte order
         */
        public void WriteShort(int value, ValueType type, ByteOrder order)
        {
            switch (order)
            {
                case ByteOrder.BigEndian:
                    WriteByte(value >> 8);
                    WriteByte(value, type);
                    break;
                case ByteOrder.MiddleEndian:
                    throw new InvalidOperationException("Middle-endian short is impossible!");
                case ByteOrder.InverseMiddleEndian:
                    throw new InvalidOperationException("Inverse-middle-endian short is impossible!");
                case ByteOrder.LittleEndian:
                    WriteByte(value, type);
                    WriteByte(value >> 8);
                    break;
            }
        }

        /**
        * Writes a value as a normal big-endian short.
        *
        * @param value the value.
        */
        public void WriteShort(int value)
        {
            WriteShort(value, ValueType.Standard, ByteOrder.BigEndian);
        }

        /**
        * Writes a value as a big-endian short.
        *
        * @param value the value
        * @param type  the value type
        */
        public void WriteShort(int value, ValueType type)
        {
            WriteShort(value, type, ByteOrder.BigEndian);
        }

        /**
        * Writes a value as a standard short.
        *
        * @param value the value
        * @param order the byte order
        */
        public void WriteShort(int value, ByteOrder order)
        {
            WriteShort(value, ValueType.Standard, order);
        }

        /**
         * Writes a value as an int.
         *
         * @param value the value
         * @param type  the value type
         * @param order the byte order
         */
        public void WriteInt(int value, ValueType type, ByteOrder order)
        {
            switch (order)
            {
                case ByteOrder.BigEndian:
                    WriteByte(value >> 24);
                    WriteByte(value >> 16);
                    WriteByte(value >> 8);
                    WriteByte(value, type);
                    break;
                case ByteOrder.MiddleEndian:
                    WriteByte(value >> 8);
                    WriteByte(value, type);
                    WriteByte(value >> 24);
                    WriteByte(value >> 16);
                    break;
                case ByteOrder.InverseMiddleEndian:
                    WriteByte(value >> 16);
                    WriteByte(value >> 24);
                    WriteByte(value, type);
                    WriteByte(value >> 8);
                    break;
                case ByteOrder.LittleEndian:
                    WriteByte(value, type);
                    WriteByte(value >> 8);
                    WriteByte(value >> 16);
                    WriteByte(value >> 24);
                    break;
            }
        }

        /**
         * Writes a value as a standard big-endian int.
         *
         * @param value the value
         */
        public void WriteInt(int value)
        {
            WriteInt(value, ValueType.Standard, ByteOrder.BigEndian);
        }

        /**
         * Writes a value as a big-endian int.
         *
         * @param value the value
         * @param type  the value type
         */
        public void WriteInt(int value, ValueType type)
        {
            WriteInt(value, type, ByteOrder.BigEndian);
        }

        /**
         * Writes a value as a standard int.
         *
         * @param value the value
         * @param order the byte order
         */
        public void WriteInt(int value, ByteOrder order)
        {
            WriteInt(value, ValueType.Standard, order);
        }

        /**
         * Writes a value as a long.
         *
         * @param value the value
         * @param type  the value type
         * @param order the byte order
         */
        public void WriteLong(long value, ValueType type, ByteOrder order)
        {
            switch (order)
            {
                case ByteOrder.BigEndian:
                    WriteByte((int)(value >> 56));
                    WriteByte((int)(value >> 48));
                    WriteByte((int)(value >> 40));
                    WriteByte((int)(value >> 32));
                    WriteByte((int)(value >> 24));
                    WriteByte((int)(value >> 16));
                    WriteByte((int)(value >> 8));
                    WriteByte((int)value, type);
                    break;
                case ByteOrder.MiddleEndian:
                    throw new InvalidOperationException("Middle-endian long is not implemented!");
                case ByteOrder.InverseMiddleEndian:
                    throw new InvalidOperationException("Inverse-middle-endian long is not implemented!");
                case ByteOrder.LittleEndian:
                    WriteByte((int)value, type);
                    WriteByte((int)(value >> 8));
                    WriteByte((int)(value >> 16));
                    WriteByte((int)(value >> 24));
                    WriteByte((int)(value >> 32));
                    WriteByte((int)(value >> 40));
                    WriteByte((int)(value >> 48));
                    WriteByte((int)(value >> 56));
                    break;
            }
        }

        /**
         * Writes a value as a standard big-endian long.
         *
         * @param value the value
         */
        public void WriteLong(long value)
        {
            WriteLong(value, ValueType.Standard, ByteOrder.BigEndian);
        }

        /**
         * Writes a value as a big-endian long.
         *
         * @param value the value
         * @param type  the value type
         */
        public void WriteLong(long value, ValueType type)
        {
            WriteLong(value, type, ByteOrder.BigEndian);
        }

        /**
         * Writes a value as a standard long.
         *
         * @param value the value
         * @param order the byte order
         */
        public void WriteLong(long value, ByteOrder order)
        {
            WriteLong(value, ValueType.Standard, order);
        }

        /**
         * Writes a RuneScape string value (a null-terminated ASCII string).
         *
         * @param string the string
         */
        public void WriteString(string stringToWrite)
        {
            foreach (sbyte value in Encoding.ASCII.GetBytes(stringToWrite).Select(v => (sbyte)v))
            {
                WriteByte(value);
            }
            WriteByte(10);
        }

        /**
         * Writes the bytes from the argued buffer into this buffer. This method does not modify the argued buffer,
         * and please do not flip() the buffer before hand.
         */
        public void WriteBytes(byte[] src)
        {
            for (int i = 0; i < src.Length; i++)
            {
                WriteByte((int)src[i]);
            }
        }

        /**
         * Writes the bytes from the argued buffer into this buffer. The amount is guarntee it writes only the data you want. 
         * This method does not modify the argued buffer,
         * and please do not flip() the buffer before hand.
         */
        public void WriteBytes(byte[] src, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                WriteByte((int)src[i]);
            }
        }

        /**
         * Writes the value as a variable amount of bits.
         *
         * @param amount the amount of bits
         * @param value  the value
         * 
         *  Byte Position: 3
            Bit Offset: 8
            Bit position: 25
         * 
         */
        public void WriteBits(int amount, int value)
        {
            if (ByteAccessType != AccessType.BitAccess)
            {
                throw new InvalidOperationException("Illegal access type.");
            }
            if (amount < 0 || amount > 32)
            {
                throw new InvalidOperationException("Number of bits must be between 1 and 32 inclusive.");
            }

            int bytePos = bitPosition >> 3;
            int bitOffset = 8 - (bitPosition & 7);
            bitPosition += amount;

            // Re-size the buffer if need be.
            int requiredSpace = (int)(bytePos - Pointer + 1);
            requiredSpace += (amount + 7) / 8;
            if (Buffer.Length < requiredSpace)
            {
                EnsureCapacity(requiredSpace);
                //Payload.Length = Payload.Capacity + requiredSpace;
                //Array.Copy((byte[])Payload, 0, Payload, 0, Payload.Length);
            }

            long OldPayloadPosition = Pointer;

            for (; amount > bitOffset; bitOffset = 8)
            {
                byte tmp = (byte)Buffer[bytePos];
                tmp &= (byte)~BitMasks[bitOffset];
                tmp |= (byte)((value >> (amount - bitOffset)) & BitMasks[bitOffset]);

                //TempPayload[bytePos] &= (byte)~BIT_MASKS[bitOffset];	 // mask out the desired area
                //TempPayload[bytePos++] |= (byte)((value >> (amount - bitOffset)) & BIT_MASKS[bitOffset]);

                Buffer[bytePos++] = tmp;
                OldPayloadPosition++;
                amount -= bitOffset;
            }
            if (amount == bitOffset)
            {
                byte tmp = (byte)Buffer[bytePos];
                tmp &= (byte)~BitMasks[bitOffset];
                tmp |= (byte)((value >> (amount - bitOffset)) & BitMasks[bitOffset]);
                Buffer[bytePos] = tmp;
                //TempPayload[bytePos] &= (byte)~BIT_MASKS[bitOffset];
                //TempPayload[bytePos] |= (byte)(value & BIT_MASKS[bitOffset]);
            }
            else
            { 
                byte tmp = (byte)Buffer[bytePos];
                tmp &= (byte)~(BitMasks[amount] << (bitOffset - amount));
                tmp |= (byte)((value & BitMasks[amount]) << (bitOffset - amount));
                Buffer[bytePos] = tmp;
                //TempPayload[bytePos] &= (byte)~(BIT_MASKS[amount] << (bitOffset - amount));
                //TempPayload[bytePos] |= (byte)((value & BIT_MASKS[amount]) << (bitOffset - amount));
            }
        }


        /**
         * Writes a boolean bit flag.
         *
         * @param flag the flag
         */
        public void WriteBit(bool flag)
        {
            WriteBits(1, flag ? 1 : 0);
        }


        /**
         * Writes the bytes from the argued byte array into this buffer, in
         * reverse.
         *
         * @param data the data to write
         */
        public void WriteBytesReverse(byte[] data)
        {
            for (int i = data.Length - 1; i >= 0; i--)
            {
                WriteByte(data[i]);
            }
        }

        public void SetAccessType(AccessType accessType)
        {
            ByteAccessType = accessType;
            SwitchAccessType(accessType);
        }

        public void SwitchAccessType(AccessType type)
        {
            switch (type)
            {
                case AccessType.BitAccess:
                    bitPosition = (int)(Pointer * 8);
                    break;
                case AccessType.ByteAccess:
                    Pointer = (bitPosition + 7) / 8;
                    break;
            }
        }

    }
}
