using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;

/**
 * @Author blakeman8192
 **/
namespace RSPS.src.net.packet
{
    /// <summary>
    /// Handles reading packet data
    /// </summary>
    public sealed class PacketReader : Packet
    {

        /// <summary>
        /// The packet opcode when applicable
        /// </summary>
        public int Opcode { get; set; }

        /// <summary>
        /// The payload size of the packet when applicable
        /// </summary>
        public int PayloadSize { get; set; }

        /// <summary>
        /// The readable bytes left in the reader
        /// </summary>
        public int ReadableBytes => Buffer.Length - Pointer;

        /// <summary>
        /// Retrieves whether any undread bytes are left in the buffer
        /// </summary>
        public bool HasReadableBytes => ReadableBytes > 0;


        public PacketReader(byte[] stream) : base(stream)
        {
            
        }

        /// <summary>
        /// Reads a byte
        /// </summary>
        /// <param name="signed">Whether the byte is signed</param>
        /// <param name="valueType">The value type</param>
        /// <returns>The value</returns>
        public int ReadByte(bool signed, ValueType valueType)
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
        /// Reads a standard unsigned byte
        /// </summary>
        /// <returns>The value</returns>
        public int ReadUnsignedByte()
        {
            return ReadByte(false);
        }

        /// <summary>
        /// Reads a signed byte
        /// </summary>
        /// <param name="type">The value type</param>
        /// <returns>The value</returns>
        public int ReadByte(ValueType type)
        {
            return ReadByte(true, type);
        }

        /// <summary>
        /// Reads an unsigned byte
        /// </summary>
        /// <param name="type">The value type</param>
        /// <returns>The value</returns>
        public int ReadUnsignedByte(ValueType type)
        {
            return ReadByte(false, type);
        }

        /// <summary>
        /// Reads an additional signed byte
        /// </summary>
        /// <param name="signed">Whether </param>
        /// <returns></returns>
        public int ReadAdditionalByte(bool signed = true)
        {
            return ReadByte(signed, ValueType.Additional);
        }

        /// <summary>
        /// Reads a short
        /// </summary>
        /// <param name="signed">Whether the value is signed</param>
        /// <param name="type">The type</param>
        /// <param name="order">The byte order</param>
        /// <returns>The value</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public int ReadShort(bool signed, ValueType type, ByteOrder order)
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

        /**
        * Reads a standard signed big-endian short.
        *
        * @return the value
        */
        public int ReadShort()
        {
            return ReadShort(true, ValueType.Standard, ByteOrder.BigEndian);
        }

        /**
         * Reads a standard big-endian short.
         *
         * @param signed the signedness
         * @return the value
         */
        public int ReadShort(bool signed)
        {
            return ReadShort(signed, ValueType.Standard, ByteOrder.BigEndian);
        }

        /**
         * Reads a signed big-endian short.
         *
         * @param type the value type
         * @return the value
         */
        public int ReadShort(ValueType type)
        {
            return ReadShort(true, type, ByteOrder.BigEndian);
        }

        /**
         * Reads a big-endian short.
         *
         * @param signed the signedness
         * @param type   the value type
         * @return the value
         */
        public int ReadShort(bool signed, ValueType type)
        {
            return ReadShort(signed, type, ByteOrder.BigEndian);
        }

        /**
         * Reads a signed standard short.
         *
         * @param order the byte order
         * @return the value
         */
        public int ReadShort(ByteOrder order)
        {
            return ReadShort(true, ValueType.Standard, order);
        }

        /**
         * Reads a standard short.
         *
         * @param signed the signedness
         * @param order  the byte order
         * @return the value
         */
        public int ReadShort(bool signed, ByteOrder order)
        {
            return ReadShort(signed, ValueType.Standard, order);
        }

        /**
         * Reads a signed short.
         *
         * @param type  the value type
         * @param order the byte order
         * @return the value
         */
        public int ReadShort(ValueType type, ByteOrder order)
        {
            return ReadShort(true, type, order);
        }

        /**
         * Reads an integer.
         *
         * @param signed the signedness
         * @param type   the value type
         * @param order  the byte order
         * @return the value
         */
        public long ReadInt(bool signed, ValueType type, ByteOrder order)
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
            return signed ? value & 0xffffffffL : value;
        }

        /**
         * Reads a signed standard big-endian integer.
         *
         * @return the value
         */
        public int ReadInt()
        {
            return (int)ReadInt(true, ValueType.Standard, ByteOrder.BigEndian);
        }

        /**
         * Reads a standard big-endian integer.
         *
         * @param signed the signedness
         * @return the value
         */
        public long ReadInt(bool signed)
        {
            return ReadInt(signed, ValueType.Standard, ByteOrder.BigEndian);
        }

        /**
         * Reads a signed big-endian integer.
         *
         * @param type the value type
         * @return the value
         */
        public int ReadInt(ValueType type)
        {
            return (int)ReadInt(true, type, ByteOrder.BigEndian);
        }

        /**
         * Reads a big-endian integer.
         *
         * @param signed the signedness
         * @param type   the value type
         * @return the value
         */
        public long ReadInt(bool signed, ValueType type)
        {
            return ReadInt(signed, type, ByteOrder.BigEndian);
        }

        /**
         * Reads a signed standard integer.
         *
         * @param order the byte order
         * @return the value
         */
        public int ReadInt(ByteOrder order)
        {
            return (int)ReadInt(true, ValueType.Standard, order);
        }

        /**
         * Reads a standard integer.
         *
         * @param signed the signedness
         * @param order  the byte order
         * @return the value
         */
        public long ReadInt(bool signed, ByteOrder order)
        {
            return ReadInt(signed, ValueType.Standard, order);
        }

        /**
         * Reads a signed integer.
         *
         * @param type  the value type
         * @param order the byte order
         * @return the value
         */
        public int ReadInt(ValueType type, ByteOrder order)
        {
            return (int)ReadInt(true, type, order);
        }

        /**
         * Reads a signed long value.
         *
         * @param type  the value type
         * @param order the byte order
         * @return the value
         */
        public long ReadLong(ValueType type, ByteOrder order)
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

        /**
         * Reads a signed standard big-endian long.
         *
         * @return the value
         */
        public long ReadLong()
        {
            return ReadLong(ValueType.Standard, ByteOrder.BigEndian);
        }

        /**
         * Reads a signed big-endian long
         *
         * @param type the value type
         * @return the value
         */
        public long ReadLong(ValueType type)
        {
            return ReadLong(type, ByteOrder.BigEndian);
        }

        /**
         * Reads a signed standard long.
         *
         * @param order the byte order
         * @return the value
         */
        public long ReadLong(ByteOrder order)
        {
            return ReadLong(ValueType.Standard, order);
        }

        /**
         * Reads a RuneScape string value.
         *
         * @return the string
         */
        public String ReadString()
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

        /**
         * Reads the amuont of bytes into the array, starting at the current
         * position.
         * 
         * @param amount
         *            the amount to read
         * @return a buffer filled with the data
         */
        public byte[] ReadBytes(int amount)
        {
            return ReadBytes(amount, ValueType.Standard);
        }

        /**
		 * Reads the amount of bytes into a byte array, starting at the current
		 * position.
		 * 
		 * @param amount
		 *            the amount of bytes
		 * @param type
		 *            the value type of each byte
		 * @return a buffer filled with the data
		 */
        public byte[] ReadBytes(int amount, ValueType type)
        {
            byte[] data = new byte[amount];
            for (int i = 0; i < amount; i++)
            {
                data[i] = (byte)ReadByte(type);
            }
            return data;
        }

        /**
		 * Reads the amount of bytes from the buffer in reverse, starting at
		 * current position + amount and reading in reverse until the current
		 * position.
		 * 
		 * @param amount
		 *            the amount of bytes
		 * @param type
		 *            the value type of each byte
		 * @return a buffer filled with the data
		 */
        public byte[] ReadBytesReverse(int amount, ValueType type)
        {
            byte[] data = new byte[amount];
            int dataPosition = 0;

            for (int i = Pointer + amount - 1; i >= Pointer; i--)
            {
                //int value = Buffer[i];
                int value = ReadByte(type);
                /*
                switch (type)
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
                }*/
                data[dataPosition++] = (byte)value;
            }
            //Pointer += dataPosition;
            return data;
        }

    }

}
