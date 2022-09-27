/**
 * @Author blakeman8192
 **/
namespace RSPS.src.net.packet
{
    /// <summary>
    /// Represents a network packet
    /// </summary>
    public abstract class Packet
    {

        /// <summary>
        /// The packet' byte buffer
        /// </summary>
        public byte[] Buffer { get; set; }

        /// <summary>
        /// The buffer reader/writer position
        /// </summary>
        public int Pointer { get; set; }

        /// <summary>
        /// Retrieves the current size of the packet
        /// </summary>
        public int Length => Buffer.Length;


        /// <summary>
        /// Creates a new packet with a predefined byte buffer
        /// </summary>
        /// <param name="buffer">The byte buffer</param>
        protected Packet(byte[] buffer)
        {
            Buffer = buffer;
        }

        /// <summary>
        /// The supported byte orders
        /// </summary>
        public enum ByteOrder
        {
            LittleEndian, BigEndian, MiddleEndian, InverseMiddleEndian
        }

        /// <summary>
        /// The supported value types
        /// </summary>
        public enum ValueType
        {
            Standard, Additional, Negated, Subtrahend
        }

        /// <summary>
        /// The possible access types
        /// </summary>
        public enum AccessType
        {
            ByteAccess, BitAccess
        }

        /// <summary>
        /// Ensures the buffer capacity is large enough to add a given size of bytes
        /// </summary>
        /// <param name="length">The sizes of bytes</param>
        protected void EnsureCapacity(int length)
        { 
            if ((Pointer + length) <= Buffer.Length)
            {
                return;
            }
            int newLength = Buffer.Length + length;
            byte[] newBuffer = new byte[newLength < 4 ? 4 : (newLength * 2)];
            Array.Copy(Buffer, 0, newBuffer, 0, Buffer.Length);
            Buffer = newBuffer;
        }

    }

}
