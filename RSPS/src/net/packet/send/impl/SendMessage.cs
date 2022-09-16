using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.packet.send.impl
{
    public class SendMessage : ISendPacket
    {

        private string Message;

        public SendMessage(string message)
        {
            Message = message;
        }

        public byte[] SendPacket(ISAACCipher encryptor)
        {
            //Console.WriteLine("preparing to send message: {0}", Message.Length + 3);
            PacketWriter packetWriter = Packet.CreatePacketWriter(Message.Length + 3);
            //Console.WriteLine("Payload size: {0}", packetWriter.Payload.Capacity);
            packetWriter.WriteVariableHeader(encryptor, 253);
            //System.Environment.Exit(0);
            packetWriter.WriteString(Message);
            packetWriter.FinishVariableHeader();
            //foreach (var byte1 in packetWriter.Payload.ToArray())
            //{
            //    Console.WriteLine(byte1);
            //}
            return packetWriter.Payload;
        }
    }
}
