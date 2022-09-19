/**
 * @Author blakeman8192
 **/
namespace RSPS.src.net.packet
{
    public abstract class Packet
    {

        public byte[] Payload { get; set; }

        public int PayloadPosition = 0;

        public int Length => Payload.Length;

        protected Packet(byte[] payload)
        {
            Payload = payload;
        }

        public Packet(int length)
        {
            Payload = new byte[length];
        }

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
            LITTLE, BIG, MIDDLE, INVERSE_MIDDLE
        }

        public enum ValueType
        {
            STANDARD, A, C, S
        }

        public enum AccessType
        {
            BYTE_ACCESS, BIT_ACCESS
        }

        protected void EnsureCapacity(int length)
        {
            //Console.WriteLine("Payload Position: {0}, Payload Length: {1}, Length of new byte{2}", PayloadPosition, Payload.Length, length);
            //Console.WriteLine((PayloadPosition + length) >= Payload.Length);
            if ((PayloadPosition + length) > Payload.Length)
            {
                byte[] oldBuffer = Payload;
                int newLength = ((int)(Payload.Length * 1.5));
                //Console.WriteLine("old buffer length: " + Payload.Length);
                Payload = new byte[newLength];
                Array.Copy(oldBuffer, 0, Payload, 0, oldBuffer.Length);
                //Console.WriteLine("new buffer length: " + Payload.Length);
            }
        }

    }

}
