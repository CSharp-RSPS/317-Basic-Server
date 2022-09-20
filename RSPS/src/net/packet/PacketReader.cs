using System.Reflection.PortableExecutable;
using System.Text;

/**
 * @Author blakeman8192
 **/
namespace RSPS.src.net.packet
{
    public class PacketReader : Packet
    {

        public PacketReader(byte[] stream) : base(stream)
        {}

        public int ReadableBytes => Length - PayloadPosition;

        public int ReadByte(bool signed, ValueType valueType)
        {
            int value = Payload[PayloadPosition++];
            switch (valueType)
            {
                case ValueType.A:
                    value = value - 128;
                    break;
                case ValueType.C:
                    value = -value;
                    break;
                case ValueType.S:
                    value = 128 - value;
                    break;
            }
            return signed ? value & 0xff : value;
        }

        /**
         * Reads a standard signed byte
         **/
        public int ReadByte()
        {
            return ReadByte(true, ValueType.STANDARD);
        }

        public int ReadByte(bool signed)
        {
            return ReadByte(signed, ValueType.STANDARD);
        }


        public int ReadByte(ValueType type)
        {
            return ReadByte(true, type);
        }

        public int ReadShort(bool signed, ValueType type, ByteOrder order)
        {
            int value = 0;
            switch (order)
            {
                case ByteOrder.BIG:
                    value |= ReadByte(signed) << 8;
                    value |= ReadByte(signed, type);
                    break;

                case ByteOrder.LITTLE:
                    value |= ReadByte(signed, type);
                    value |= ReadByte(signed) << 8;
                    break;

                case ByteOrder.INVERSE_MIDDLE:
                    throw new InvalidOperationException("Inverse Middle Endian short is impossible");

                case ByteOrder.MIDDLE:
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
            return ReadShort(true, ValueType.STANDARD, ByteOrder.BIG);
        }

        /**
         * Reads a standard big-endian short.
         *
         * @param signed the signedness
         * @return the value
         */
        public int ReadShort(bool signed)
        {
            return ReadShort(signed, ValueType.STANDARD, ByteOrder.BIG);
        }

        /**
         * Reads a signed big-endian short.
         *
         * @param type the value type
         * @return the value
         */
        public int ReadShort(ValueType type)
        {
            return ReadShort(true, type, ByteOrder.BIG);
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
            return ReadShort(signed, type, ByteOrder.BIG);
        }

        /**
         * Reads a signed standard short.
         *
         * @param order the byte order
         * @return the value
         */
        public int ReadShort(ByteOrder order)
        {
            return ReadShort(true, ValueType.STANDARD, order);
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
            return ReadShort(signed, ValueType.STANDARD, order);
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
                case ByteOrder.BIG:
                    value = ReadByte(false) << 24 |
                            ReadByte(false) << 16 |
                            ReadByte(false) << 8  |
                            ReadByte(false, type);
                    break;
                case ByteOrder.MIDDLE:
                    value = ReadByte(false) << 8  |
                            ReadByte(false, type) |
                            ReadByte(false) << 24 |
                            ReadByte(false) << 16;
                    break;
                case ByteOrder.INVERSE_MIDDLE:
                    value = ReadByte(false) << 16 |
                            ReadByte(false) << 24 |
                            ReadByte(false, type) |
                            ReadByte(false) << 8;
                    break;
                case ByteOrder.LITTLE:
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
            return (int)ReadInt(true, ValueType.STANDARD, ByteOrder.BIG);
        }

        /**
         * Reads a standard big-endian integer.
         *
         * @param signed the signedness
         * @return the value
         */
        public long ReadInt(bool signed)
        {
            return ReadInt(signed, ValueType.STANDARD, ByteOrder.BIG);
        }

        /**
         * Reads a signed big-endian integer.
         *
         * @param type the value type
         * @return the value
         */
        public int ReadInt(ValueType type)
        {
            return (int)ReadInt(true, type, ByteOrder.BIG);
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
            return ReadInt(signed, type, ByteOrder.BIG);
        }

        /**
         * Reads a signed standard integer.
         *
         * @param order the byte order
         * @return the value
         */
        public int ReadInt(ByteOrder order)
        {
            return (int)ReadInt(true, ValueType.STANDARD, order);
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
            return ReadInt(signed, ValueType.STANDARD, order);
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
                case ByteOrder.BIG:
                    value = (long)ReadByte(false) << 56 |
                            (long)ReadByte(false) << 48 |
                            (long)ReadByte(false) << 40 |
                            (long)ReadByte(false) << 32 |
                            (long)ReadByte(false) << 24 |
                            (long)ReadByte(false) << 16 |
                            (long)ReadByte(false) << 8  |
                            (long)ReadByte(false, type);
                    break;
                case ByteOrder.MIDDLE:
                    throw new InvalidOperationException("middle-endian long is not implemented!");
                case ByteOrder.INVERSE_MIDDLE:
                    throw new InvalidOperationException("inverse-middle-endian long is not implemented!");
                case ByteOrder.LITTLE:
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
            return ReadLong(ValueType.STANDARD, ByteOrder.BIG);
        }

        /**
         * Reads a signed big-endian long
         *
         * @param type the value type
         * @return the value
         */
        public long ReadLong(ValueType type)
        {
            return ReadLong(type, ByteOrder.BIG);
        }

        /**
         * Reads a signed standard long.
         *
         * @param order the byte order
         * @return the value
         */
        public long ReadLong(ByteOrder order)
        {
            return ReadLong(ValueType.STANDARD, order);
        }

        /**
         * Reads a RuneScape string value.
         *
         * @return the string
         */
        public String ReadString()
        {
            byte temp;
            StringBuilder b = new StringBuilder();
            while ((temp = (byte)ReadByte()) != 10)
            {
                b.Append((char)temp);
            }
            return b.ToString();
        }

        public int HexToInt(byte[] data)
        {
            int value = 0;
            int n = 1000;

            foreach (byte b in data)
            {
                int num = (b & 0xFF) * n;
                value += num;

                if (n > 1)
                {
                    n = n / 1000;
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
        public byte[] readBytes(int amount)
        {
            return readBytes(amount, ValueType.STANDARD);
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
        public byte[] readBytes(int amount, ValueType type)
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
            for (int i = PayloadPosition + amount - 1; i >= PayloadPosition; i--)
            {
                int value = Payload[i];
                switch (type)
                {
                    case ValueType.A:
                        value -= 128;
                        break;
                    case ValueType.C:
                        value = -value;
                        break;
                    case ValueType.S:
                        value = 128 - value;
                        break;
                }
                data[dataPosition++] = (byte)value;
            }
            PayloadPosition += dataPosition;
            return data;
        }

    }

}
