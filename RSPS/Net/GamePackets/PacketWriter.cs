using RSPS.Net.Ciphers;
using RSPS.Net.GamePackets;
using System.ComponentModel;
using System.Text;

/**
 * @Author blakeman8192
 **/
namespace RSPS.Net.GamePackets
{
    /// <summary>
    /// Represents a packet writer
    /// </summary>
    public sealed class PacketWriter : Packet
    {

        /// <summary>
        /// The current length position for variable packet headers
        /// </summary>
        private int lengthPosition = 0;

        /// <summary>
        /// The current bit position for bit writing
        /// </summary>
        private int bitPosition = 0;

        /// <summary>
        /// The current byte access type of the buffer, what kind of data we're currently writing
        /// </summary>
        public AccessType ByteAccessType { get; private set; } = AccessType.ByteAccess;

        /// <summary>
        /// The type of packet header
        /// </summary>
        public PacketHeaderType HeaderType { get; private set; } = PacketHeaderType.Fixed;

        /// <summary>
        /// The possible bit masks for bit writing
        /// </summary>
        public static readonly int[] BitMasks = {
            0, 0x1, 0x3, 0x7, 0xf, 0x1f, 0x3f, 0x7f, 0xff, 0x1ff, 0x3ff, 0x7ff, 0xfff, 0x1fff, 0x3fff, 0x7fff, 0xffff,
            0x1ffff, 0x3ffff, 0x7ffff, 0xfffff, 0x1fffff, 0x3fffff, 0x7fffff, 0xffffff, 0x1ffffff, 0x3ffffff, 0x7ffffff,
            0xfffffff, 0x1fffffff, 0x3fffffff, 0x7fffffff, -1
        };


        /// <summary>
        /// Creates a new packet writer with a predefined byte buffer size
        /// </summary>
        /// <param name="size">The byte buffer size</param>
        public PacketWriter(int size) : base(new byte[size]) { }

        /// <summary>
        /// Writes a packet header
        /// </summary>
        /// <param name="headerType">The header type</param>
        /// <param name="cipher">The encrypion cipher</param>
        /// <param name="opcode">The packet opcode</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteHeader(PacketHeaderType headerType, ISAACCipher cipher, int opcode)
        {
            // Set the header type
            HeaderType = headerType;
            // Write the packet opcode
            WriteByte((opcode + cipher.GetNextValue()) & 0xff);

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
        /// <returns>The writer</returns>
        public PacketWriter FinishHeader()
        {
            if (HeaderType == PacketHeaderType.Fixed)
            { // Static headers don't require anything to finish
                return this;
            }
            int oldPosition = Pointer;
            Pointer = lengthPosition;

            switch (HeaderType)
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

        /// <summary>
        /// Writes a byte
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="type">The value type</param>
        /// <returns>The writer</returns>
        private PacketWriter WriteByte(int value, ValueType type)
        {
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
            return this;
        }

        /// <summary>
        /// Writes a boolean as a byte
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteByte(bool value)
        {
            return WriteByte(value ? 1 : 0);
        }

        /// <summary>
        /// Writes a standard byte
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteByte(int value)
        {
            return WriteByte(value, ValueType.Standard);
        }

        /// <summary>
        /// Writes an additional byte
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteByteAdditional(int value)
        {
            return WriteByte(value, ValueType.Additional);
        }

        /// <summary>
        /// Writes a negated byte
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteByteNegated(int value)
        {
            return WriteByte(value, ValueType.Negated);
        }

        /// <summary>
        /// Writes a subtrahend byte
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteByteSubtrahend(int value)
        {
            return WriteByte(value, ValueType.Subtrahend);
        }

        /// <summary>
        /// Writes a short
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="type">The value type</param>
        /// <param name="order">The byte order</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns>The writer</returns>
        private PacketWriter WriteShort(int value, ValueType type, ByteOrder order)
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
            return this;
        }

        /// <summary>
        /// Writes a standard big-endian short
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteShort(int value)
        {
            return WriteShort(value, ValueType.Standard, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes a standard little-endian short
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteShortLittleEndian(int value)
        {
            return WriteShort(value, ValueType.Standard, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Writes an additional big-endian short
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteShortAdditional(int value)
        {
            return WriteShort(value, ValueType.Additional, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes an additional little-endian short
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteShortAdditionalLittleEndian(int value)
        {
            return WriteShort(value, ValueType.Additional, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Writes a negated big-endian short
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteShortNegated(int value)
        {
            return WriteShort(value, ValueType.Negated, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes a negated little-endian short
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteShortNegatedLittleEndian(int value)
        {
            return WriteShort(value, ValueType.Negated, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Writes a subtrahend big-endian short
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteShortSubtrahend(int value)
        {
            return WriteShort(value, ValueType.Subtrahend, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes a subtrahend little-endian short
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteShortSubtrahendLittleEndian(int value)
        {
            return WriteShort(value, ValueType.Subtrahend, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Writes an integer
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="type">The value type</param>
        /// <param name="order">The byte order</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns>The writer</returns>
        private PacketWriter WriteInt(int value, ValueType type, ByteOrder order)
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
            return this;
        }

        /// <summary>
        /// Writes a standard big endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteInt(int value)
        {
            return WriteInt(value, ValueType.Standard, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes a standard little endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntLittleEndian(int value)
        {
            return WriteInt(value, ValueType.Standard, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Writes a standard middle endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntMiddleEndian(int value)
        {
            return WriteInt(value, ValueType.Standard, ByteOrder.MiddleEndian);
        }

        /// <summary>
        /// Writes a standard inverse middle endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntInverseMiddleEndian(int value)
        {
            return WriteInt(value, ValueType.Standard, ByteOrder.InverseMiddleEndian);
        }

        /// <summary>
        /// Writes an additional big endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntAdditional(int value)
        {
            return WriteInt(value, ValueType.Additional, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes an additional little endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntAdditionalLittleEndian(int value)
        {
            return WriteInt(value, ValueType.Additional, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Writes a additional middle endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntAdditionalMiddleEndian(int value)
        {
            return WriteInt(value, ValueType.Additional, ByteOrder.MiddleEndian);
        }

        /// <summary>
        /// Writes an additional inverse middle endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntAdditionalInverseMiddleEndian(int value)
        {
            return WriteInt(value, ValueType.Additional, ByteOrder.InverseMiddleEndian);
        }

        /// <summary>
        /// Writes a negated big endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntNegated(int value)
        {
            return WriteInt(value, ValueType.Negated, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes a negated little endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntNegatedLittleEndian(int value)
        {
            return WriteInt(value, ValueType.Negated, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Writes a negated middle endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntNegatedMiddleEndian(int value)
        {
            return WriteInt(value, ValueType.Negated, ByteOrder.MiddleEndian);
        }

        /// <summary>
        /// Writes a negated inverse middle endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntNegatedInverseMiddleEndian(int value)
        {
            return WriteInt(value, ValueType.Negated, ByteOrder.InverseMiddleEndian);
        }

        /// <summary>
        /// Writes a subtrahend big endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntSubtrahend(int value)
        {
            return WriteInt(value, ValueType.Subtrahend, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes a subtrahend little endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntSubtrahendLittleEndian(int value)
        {
            return WriteInt(value, ValueType.Subtrahend, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Writes a subtrahend middle endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntSubtrahendMiddleEndian(int value)
        {
            return WriteInt(value, ValueType.Subtrahend, ByteOrder.MiddleEndian);
        }

        /// <summary>
        /// Writes a subtrahend inverse middle endian integer.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteIntSubtrahendInverseMiddleEndian(int value)
        {
            return WriteInt(value, ValueType.Subtrahend, ByteOrder.InverseMiddleEndian);
        }

        /// <summary>
        /// Writes a long
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="type">The value type</param>
        /// <param name="order">The byte order</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <returns>The writer</returns>
        private PacketWriter WriteLong(long value, ValueType type, ByteOrder order)
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
            return this;
        }

        /// <summary>
        /// Writes a standard big-endian long.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteLong(long value)
        {
            return WriteLong(value, ValueType.Standard, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes an additional big-endian long.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteLongAdditional(long value)
        {
            return WriteLong(value, ValueType.Additional, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes an additional little-endian long.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteLongAdditionalLittleEndian(long value)
        {
            return WriteLong(value, ValueType.Additional, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Writes a negated big-endian long.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteLongNegated(long value)
        {
            return WriteLong(value, ValueType.Negated, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes an negated little-endian long.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteLongNegatedLittleEndian(long value)
        {
            return WriteLong(value, ValueType.Negated, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Writes a subtrahend big-endian long.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteLongSubtrahend(long value)
        {
            return WriteLong(value, ValueType.Subtrahend, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Writes an subtrahend little-endian long.
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteLongSubtrahendLittleEndian(long value)
        {
            return WriteLong(value, ValueType.Subtrahend, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Writes a string trailed by 10 bytes to indicate the end
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteRS2String(string value)
        {
            foreach (sbyte b in Encoding.ASCII.GetBytes(value).Select(v => (sbyte)v))
            {
                WriteByte(b);
            }
            return WriteByte(10);
        }

        /// <summary>
        /// Writes the bytes from the argued buffer into this buffer. 
        /// This method does not modify the argued buffer,and please do not flip() the buffer before hand.
        /// </summary>
        /// <param name="data">The data to write</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteBytes(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                WriteByte(data[i]);
            }
            return this;
        }

        /// <summary>
        /// Writes the specified amount of bytes from the argued buffer into this buffer. 
        /// This method does not modify the argued buffer,and please do not flip() the buffer before hand.
        /// </summary>
        /// <param name="data">The data buffer to write from</param>
        /// <param name="amount">The amount of data to write</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteBytes(byte[] data, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                WriteByte(data[i]);
            }
            return this;
        }

        /// <summary>
        /// Writes the value as a variable amount of bits.
        /// </summary>
        /// <param name="amount">The amount of bits</param>
        /// <param name="value">The value</param>
        /// <remarks>
        /// Byte Position: 3
        /// Bit Offset: 8
        /// Bit position: 25
        /// </remarks>
        /// <returns>The writer</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public PacketWriter WriteBits(int amount, int value)
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
            }
            long OldPayloadPosition = Pointer;

            for (; amount > bitOffset; bitOffset = 8)
            {
                byte tmp = (byte)Buffer[bytePos];
                tmp &= (byte)~BitMasks[bitOffset];
                tmp |= (byte)((value >> (amount - bitOffset)) & BitMasks[bitOffset]);

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
            }
            else
            { 
                byte tmp = (byte)Buffer[bytePos];
                tmp &= (byte)~(BitMasks[amount] << (bitOffset - amount));
                tmp |= (byte)((value & BitMasks[amount]) << (bitOffset - amount));
                Buffer[bytePos] = tmp;
            }
            return this;
        }

        /// <summary>
        /// Writes a boolean bit flag.
        /// </summary>
        /// <param name="flag">The flag</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteBit(bool flag)
        {
            return WriteBits(1, flag ? 1 : 0);
        }

        /// <summary>
        /// Writes the bytes from the argued byte array into this buffer, in reverse
        /// </summary>
        /// <param name="data">The data to write</param>
        /// <returns>The writer</returns>
        public PacketWriter WriteBytesReverse(byte[] data)
        {
            for (int i = data.Length - 1; i >= 0; i--)
            {
                WriteByte(data[i]);
            }
            return this;
        }

        /// <summary>
        /// Modifies the byte access type
        /// </summary>
        /// <param name="accessType">The new access type</param>
        /// <returns>The writer</returns>
        public PacketWriter SetAccessType(AccessType accessType)
        {
            ByteAccessType = accessType;
            return SwitchAccessType(accessType);
        }

        /// <summary>
        /// Switches the byte access type
        /// </summary>
        /// <param name="type">The new access type</param>
        /// <returns>The writer</returns>
        public PacketWriter SwitchAccessType(AccessType type)
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
            return this;
        }

    }
}
