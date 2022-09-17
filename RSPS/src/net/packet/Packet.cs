/**
 * @Author blakeman8192
 **/
namespace RSPS.src.net.packet
{
    public abstract class Packet
    {

        public byte[] Payload { get; set; }

        public int PayloadPosition = 0;

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

        /**
        * Lengths for the various packets.
        */
        public static readonly int[] PACKET_LENGTHS = {
            0, 0, 0, 1, -1, 0, 0, 0, 0, 0, // 0
            0, 0, 0, 0, 8, 0, 6, 2, 2, 0, // 10
            0, 2, 0, 6, 0, 12, 0, 0, 0, 0, // 20
            0, 0, 0, 0, 0, 8, 4, 0, 0, 2, // 30
            2, 6, 0, 6, 0, -1, 0, 0, 0, 0, // 40
            0, 0, 0, 12, 0, 0, 0, 0, 8, 0, // 50
            0, 8, 0, 0, 0, 0, 0, 0, 0, 0, // 60
            6, 0, 2, 2, 8, 6, 0, -1, 0, 6, // 70
            0, 0, 0, 0, 0, 1, 4, 6, 0, 0, // 80
            0, 0, 0, 0, 0, 3, 0, 0, -1, 0, // 90
            0, 13, 0, -1, 0, 0, 0, 0, 0, 0,// 100
            0, 0, 0, 0, 0, 0, 0, 6, 0, 0, // 110
            1, 0, 6, 0, 0, 0, -1, 0, 2, 6, // 120
            0, 4, 6, 8, 0, 6, 0, 0, 0, 2, // 130
            0, 0, 0, 0, 0, 6, 0, 0, 0, 0, // 140
            0, 0, 1, 2, 0, 2, 6, 0, 0, 0, // 150
            0, 0, 0, 0, -1, -1, 0, 0, 0, 0,// 160
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, // 170
            0, 8, 0, 3, 0, 2, 0, 0, 8, 1, // 180
            0, 0, 12, 0, 0, 0, 0, 0, 0, 0, // 190
            2, 0, 0, 0, 0, 0, 0, 0, 4, 0, // 200
            4, 0, 0, 0, 7, 8, 0, 0, 10, 0, // 210
            0, 0, 0, 0, 0, 0, -1, 0, 6, 0, // 220
            1, 0, 0, 0, 6, 0, 6, 8, 1, 0, // 230
            0, 4, 0, 0, 0, 0, -1, 0, -1, 4,// 240
            0, 0, 6, 6, 0, 0, 0 // 250
        };


    }

}
