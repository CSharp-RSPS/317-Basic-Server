/**
 * @Author blakeman8192
 **/
namespace RSPS.src.net.packet
{
    public abstract class Packet
    {

        /// <summary>
        /// The opcode of the packet when present
        /// </summary>
       // public int Opcode { get; set; }

        /// <summary>
        /// The payload size of the packet when known
        /// </summary>
       // public int PayloadSize { get; set; }

        /// <summary>
        /// The packet data
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// The payload reader position
        /// </summary>
        public int Pointer { get; set; }

        /// <summary>
        /// Retrieves the data length of the packet
        /// </summary>
        public int Length => Buffer.Length;


        /// <summary>
        /// Creates a new packet wrapper
        /// </summary>
        /// <param name="data">The packet data</param>
        protected Packet(byte[] data)
        {
            Buffer = data;
          //  Opcode = -1;
         //   PayloadSize = -1;
        }

        /// <summary>
        /// Creates a new packet wrapper
        /// </summary>
        /// <param name="length">The data length</param>
        public Packet(int length) : this(new byte[length]) { }

        public static PacketReader CreatePacketReader(byte[] stream)
        {
            return new PacketReader(stream);
        }

        public static PacketWriter CreatePacketWriter(int length)
        {
            return new PacketWriter(length);
        }

        public enum ByteOrder
        {
            LittleEndian, BigEndian, MiddleEndian, InverseMiddleEndian
        }

        public enum ValueType
        {
            Standard, Additional, Negated, Subtrahend
        }

        public enum AccessType
        {
            ByteAccess, BitAccess
        }

        protected void EnsureCapacity(int length)
        { 
            //Console.WriteLine("Payload Position: {0}, Payload Length: {1}, Length of new byte{2}", PayloadPosition, Payload.Length, length);
            //Console.WriteLine((PayloadPosition + length) >= Payload.Length);
            if ((Pointer + length) > Buffer.Length)
            {
                byte[] oldBuffer = Buffer;
                int newLength = (int)Math.Round((Buffer.Length + length) * 1.5);//* 1.5 so we can reduce calls to make new array. Since making a new array is expensive
                //TODO LLN: testing, was => int newLength = ((int)(Data.Length * 1.5));
                Buffer = new byte[newLength];
                Array.Copy(oldBuffer, 0, Buffer, 0, oldBuffer.Length);
            }
        }

    }

}
