using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;

/**
 * @Author blakeman8192
 **/
namespace RSPS.Net.GamePackets
{
    /// <summary>
    /// Handles reading packet data
    /// </summary>
    public sealed class PacketReader : Packet
    {

        /// <summary>
        /// The packet opcode when applicable
        /// </summary>
        public int Opcode { get; private set; }

        /// <summary>
        /// The payload size of the packet when applicable
        /// </summary>
        public int PayloadSize { get; private set; }

        /// <summary>
        /// The readable bytes left in the reader
        /// </summary>
        public int ReadableBytes => Buffer.Length - Pointer;

        /// <summary>
        /// Retrieves whether any undread bytes are left in the buffer
        /// </summary>
        public bool HasReadableBytes => ReadableBytes > 0;


        /// <summary>
        /// Creates a new packet reader
        /// </summary>
        /// <param name="buffer">The byte buffer to read from</param>
        public PacketReader(byte[] buffer) : this(-1, buffer.Length, buffer) { }

        /// <summary>
        /// Creates a new packet reader
        /// </summary>
        /// <param name="opcode">The packet opcode</param>
        /// <param name="payloadSize">The packet payload size</param>
        /// <param name="buffer">The byte buffer to read from</param>
        public PacketReader(int opcode, int payloadSize, byte[] buffer) : base(buffer) {
            Opcode = opcode;
            PayloadSize = payloadSize;
        }

        /// <summary>
        /// Reads a byte
        /// </summary>
        /// <param name="signed">Whether the byte is signed</param>
        /// <param name="valueType">The value type</param>
        /// <returns>The value</returns>
        private int ReadByte(bool signed, ValueType valueType)
        {
            int value = Buffer[Pointer++];

            switch (valueType)
            {
                case ValueType.Additional:
                    value -= 128;
                    break;
                case ValueType.Negated:
                    value = -value;
                    break;
                case ValueType.Subtrahend:
                    value = 128 - value;
                    break;
            }
            return signed ? value & 0xff : value;
        }

        /// <summary>
        /// Reads a byte
        /// </summary>
        /// <param name="signed">Whether the byte is signed</param>
        /// <returns></returns>
        public int ReadByte(bool signed = true)
        {
            return ReadByte(signed, ValueType.Standard);
        }

        /// <summary>
        /// Reads an additional byte
        /// </summary>
        /// <param name="signed">Whether the byte is signed</param>
        /// <returns>The value</returns>
        public int ReadByteAdditional(bool signed = true)
        {
            return ReadByte(signed, ValueType.Additional);
        }

        /// <summary>
        /// Reads a negated byte
        /// </summary>
        /// <param name="signed">Whether the byte is signed</param>
        /// <returns>The value</returns>
        public int ReadByteNegated(bool signed = true)
        {
            return ReadByte(signed, ValueType.Negated);
        }

        /// <summary>
        /// Reads a subtrahend byte
        /// </summary>
        /// <param name="signed">Whether the byte is signed</param>
        /// <returns>The value</returns>
        public int ReadByteSubtrahend(bool signed = true)
        {
            return ReadByte(signed, ValueType.Subtrahend);
        }

        /// <summary>
        /// Reads a short
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <param name="type">The type</param>
        /// <param name="order">The byte order</param>
        /// <returns>The value</returns>
        /// <exception cref="InvalidOperationException"></exception>
        private int ReadShort(bool signed, ValueType type, ByteOrder order)
        {
            int value = 0;

            switch (order)
            {
                case ByteOrder.BigEndian:
                    value |= ReadByte(signed) << 8;
                    value |= ReadByte(signed, type);
                    break;

                case ByteOrder.LittleEndian:
                    value |= ReadByte(signed, type);
                    value |= ReadByte(signed) << 8;
                    break;

                case ByteOrder.InverseMiddleEndian:
                    throw new InvalidOperationException("Inverse Middle Endian short is impossible");

                case ByteOrder.MiddleEndian:
                    throw new InvalidOperationException("Middle-endian short is impossible!");
            }
            return value;
        }

        /// <summary>
        /// Reads a standard big endian short
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadShort(bool signed = true)
        {
            return ReadShort(signed, ValueType.Standard, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads a standard little endian short
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadShortLittleEndian(bool signed = true)
        {
            return ReadShort(signed, ValueType.Standard, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Reads a negated big endian short
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadShortAdditional(bool signed = true)
        {
            return ReadShort(signed, ValueType.Additional, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads a negated big endian short
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadShortNegated(bool signed = true)
        {
            return ReadShort(signed, ValueType.Negated, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads a negated big endian short
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadShortSubtrahend(bool signed = true)
        {
            return ReadShort(signed, ValueType.Subtrahend, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads a negated little endian short
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadShortAdditionalLittleEndian(bool signed = true)
        {
            return ReadShort(signed, ValueType.Additional, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Reads a negated little endian short
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadShortNegatedLittleEndian(bool signed = true)
        {
            return ReadShort(signed, ValueType.Negated, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Reads a negated little endian short
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadShortSubtrahendLittleEndian(bool signed = true)
        {
            return ReadShort(signed, ValueType.Subtrahend, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Reads an integer
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <param name="type">The type</param>
        /// <param name="order">The byte order</param>
        /// <returns>The value</returns>
        private int ReadInt(bool signed, ValueType type, ByteOrder order)
        {
            int value = 0;

            switch (order)
            {
                case ByteOrder.BigEndian:
                    value = ReadByte(false) << 24 |
                            ReadByte(false) << 16 |
                            ReadByte(false) << 8  |
                            ReadByte(false, type);
                    break;
                case ByteOrder.MiddleEndian:
                    value = ReadByte(false) << 8  |
                            ReadByte(false, type) |
                            ReadByte(false) << 24 |
                            ReadByte(false) << 16;
                    break;
                case ByteOrder.InverseMiddleEndian:
                    value = ReadByte(false) << 16 |
                            ReadByte(false) << 24 |
                            ReadByte(false, type) |
                            ReadByte(false) << 8;
                    break;
                case ByteOrder.LittleEndian:
                    value = ReadByte(false, type) |
                            ReadByte(false) << 8  |
                            ReadByte(false) << 16 |
                            ReadByte(false) << 24;
                    break;
            }
            return Convert.ToInt32(signed ? value & 0xffffffffL : value);
        }

        /// <summary>
        /// Reads a standard big endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadInt(bool signed = true)
        {
            return ReadInt(signed, ValueType.Standard, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads a standard little endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntLittleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Standard, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Reads a standard middle endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntMiddleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Standard, ByteOrder.MiddleEndian);
        }

        /// <summary>
        /// Reads a standard inverse middle endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntInverseMiddleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Standard, ByteOrder.InverseMiddleEndian);
        }

        /// <summary>
        /// Reads an additional big endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntAdditional(bool signed = true)
        {
            return ReadInt(signed, ValueType.Additional, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads an additional little endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntAdditionalLittleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Additional, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Reads a additional middle endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntAdditionalMiddleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Additional, ByteOrder.MiddleEndian);
        }

        /// <summary>
        /// Reads an additional inverse middle endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntAdditionalInverseMiddleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Additional, ByteOrder.InverseMiddleEndian);
        }

        /// <summary>
        /// Reads a negated big endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntNegated(bool signed = true)
        {
            return ReadInt(signed, ValueType.Negated, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads a negated little endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntNegatedLittleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Negated, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Reads a negated middle endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntNegatedMiddleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Negated, ByteOrder.MiddleEndian);
        }

        /// <summary>
        /// Reads a negated inverse middle endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntNegatedInverseMiddleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Negated, ByteOrder.InverseMiddleEndian);
        }

        /// <summary>
        /// Reads a subtrahend big endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntSubtrahend(bool signed = true)
        {
            return ReadInt(signed, ValueType.Subtrahend, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads a subtrahend little endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntSubtrahendLittleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Subtrahend, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Reads a subtrahend middle endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntSubtrahendMiddleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Subtrahend, ByteOrder.MiddleEndian);
        }

        /// <summary>
        /// Reads a subtrahend inverse middle endian integer.
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <returns>The value</returns>
        public int ReadIntSubtrahendInverseMiddleEndian(bool signed = true)
        {
            return ReadInt(signed, ValueType.Subtrahend, ByteOrder.InverseMiddleEndian);
        }

        /// <summary>
        /// Reads a long
        /// </summary>
        /// <param name="type">The type</param>
        /// <param name="order">The byte order</param>
        /// <returns>The value</returns>
        private long ReadLong(ValueType type, ByteOrder order)
        {
            long value = 0;

            switch (order)
            {
                case ByteOrder.BigEndian:
                    value = (long)ReadByte(false) << 56 |
                            (long)ReadByte(false) << 48 |
                            (long)ReadByte(false) << 40 |
                            (long)ReadByte(false) << 32 |
                            (long)ReadByte(false) << 24 |
                            (long)ReadByte(false) << 16 |
                            (long)ReadByte(false) << 8  |
                            (long)ReadByte(false, type);
                    break;
                case ByteOrder.MiddleEndian:
                    throw new InvalidOperationException("middle-endian long is not implemented!");

                case ByteOrder.InverseMiddleEndian:
                    throw new InvalidOperationException("inverse-middle-endian long is not implemented!");

                case ByteOrder.LittleEndian:
                    value = (long)ReadByte(false, type) |
                            (long)ReadByte(false) << 8  |
                            (long)ReadByte(false) << 16 |
                            (long)ReadByte(false) << 24 |
                            (long)ReadByte(false) << 32 |
                            (long)ReadByte(false) << 40 |
                            (long)ReadByte(false) << 48 |
                            (long)ReadByte(false) << 56;
                    break;
            }
            return value;
        }

        /// <summary>
        /// Reads a standard big-endian long.
        /// </summary>
        /// <returns>The value</returns>
        public long ReadLong()
        {
            return ReadLong(ValueType.Standard, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads an additional big-endian long.
        /// </summary>
        /// <returns>The value</returns>
        public long ReadLongAdditional()
        {
            return ReadLong(ValueType.Additional, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads an additional little-endian long.
        /// </summary>
        /// <returns>The value</returns>
        public long ReadLongAdditionalLittleEndian()
        {
            return ReadLong(ValueType.Additional, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Reads a negated big-endian long.
        /// </summary>
        /// <returns>The value</returns>
        public long ReadLongNegated()
        {
            return ReadLong(ValueType.Negated, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads an negated little-endian long.
        /// </summary>
        /// <returns>The value</returns>
        public long ReadLongNegatedLittleEndian()
        {
            return ReadLong(ValueType.Negated, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Reads a subtrahend big-endian long.
        /// </summary>
        /// <returns>The value</returns>
        public long ReadLongSubtrahend()
        {
            return ReadLong(ValueType.Subtrahend, ByteOrder.BigEndian);
        }

        /// <summary>
        /// Reads an subtrahend little-endian long.
        /// </summary>
        /// <returns>The value</returns>
        public long ReadLongSubtrahendLittleEndian()
        {
            return ReadLong(ValueType.Subtrahend, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Reads a string value trailed by 10 bytes to indicate the end of the string
        /// </summary>
        /// <returns>The value</returns>
        public string ReadRS2String()
        {
            byte temp;
            StringBuilder b = new();

            while ((temp = (byte)ReadByte()) != 10)
            {
                b.Append((char)temp);
            }
            return b.ToString();
        }

        public static int HexToInt(byte[] data)
        {
            int value = 0;
            int n = 1000;

            foreach (byte b in data)
            {
                int num = (b & 0xFF) * n;
                value += num;

                if (n > 1)
                {
                    n /= 1000;
                }
            }
            return value;
        }

        /// <summary>
        /// Reads the amuont of bytes into the array, starting at the current position
        /// </summary>
        /// <param name="amount">The amount of bytes to read</param>
        /// <param name="type">The value type</param>
        /// <returns>The read bytes</returns>
        public byte[] ReadBytes(int amount, ValueType type = ValueType.Standard)
        {
            byte[] data = new byte[amount];

            for (int i = 0; i < amount; i++)
            {
                data[i] = (byte)ReadByte(true, type);
            }
            return data;
        }

        /// <summary>
        /// Reads a given amount of bytes and retrieves them in reverse
        /// </summary>
        /// <param name="amount">The amount of bytes to read</param>
        /// <param name="type">The value type</param>
        /// <returns>The read bytes in reverse</returns>
        public byte[] ReadBytesReverse(int amount, ValueType type)
        {
            byte[] data = ReadBytes(amount, type);
            Array.Reverse(data);
            return data;
        }

    }

}
