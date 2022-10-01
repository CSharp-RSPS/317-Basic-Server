using RSPS.Entities.Mobiles.Players;
using RSPS.Net.Security.Ciphers;
using RSPS.Net.Connections;
using RSPS.Net.GamePackets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.Net.Authentication
{
    internal class RSALoginProtocolDecoder
    {
        private static Random Random = new Random();

        public static readonly BigInteger RSA_MODULUS = BigInteger.Parse("104491769878744214552327916539299463496996457081116392641740420337580247359457531212713234798435269852381858199895582444770363103378419890508986198319599243102737368616946490728678876018327788000439596635223141886089230154991381365099178986572201859664528128354742213167942196819984139030533812106754541601427");
        public static readonly BigInteger RSA_EXPONENT = BigInteger.Parse("60599709927999604905700283074075621535832298407602909067050214324591452792136683802017251754259849192812146824871924219308664736457374966789226657090366163438668361486273661119889727044594679526697477360216141989053223520684400208813776138325305181882593010856241272041142533590779910844335986101437613222337");

        public static void DecodeLogin(Connection connection)
        {
            //Console.WriteLine("Buffer Size: {0}", connection.buffer.Length);
            PacketReader readPacket;
            switch (connection.ConnectionState)
            {
                case ConnectionState.ConnectionRequest://connection state
                    readPacket = new(connection.Buffer);
                    int opcode = readPacket.ReadByte() & 0xff;
                    int nameHash = readPacket.ReadByte() & 0xff;
                    if (opcode == 14)
                    {
                        Console.WriteLine("opcode = game");
                        Console.WriteLine("NameHash: {0}", nameHash);
                        connection.ConnectionState = ConnectionState.Authenticated;
                        MemoryStream stream = new MemoryStream(9);
                        stream.WriteByte(0);
                        stream.Write(BitConverter.GetBytes(new Random().NextInt64()));
                        connection.Send(stream.GetBuffer());
                    }
                    if (opcode == 15)
                    {
                        Console.WriteLine("opcode = update");
                    }
                    break;

                case ConnectionState.Authenticate://login state
                    readPacket = new(connection.Buffer);

                    //Console.WriteLine("We are inside the login state");
                    int loginType = readPacket.ReadByte();
                    Console.WriteLine("LoginType: {0}", loginType);
                    if (loginType != 16 && loginType != 18)
                    {
                        Console.WriteLine("Invalid login Type: {0}", loginType);
                        connection.Dispose();
                    }

                    int blockLength = readPacket.ReadByte();//loginSize = 76

                    Console.WriteLine("Block Length is {0}", blockLength);
                    if (connection.Buffer.Length < blockLength)
                    {
                        Console.WriteLine("State buffer length is less than block length");
                    }

                    int magicId = readPacket.ReadByte();//255
                    Console.WriteLine("Magic ID: {0}", magicId);

                    int clientVersion = readPacket.ReadShort();
                    Console.WriteLine("Client Version {0}", clientVersion);//needs to be 317. Bitconvert works
                    if (clientVersion != 3)//was 317
                    {
                        Console.WriteLine("Incorrect client version");
                    }

                    readPacket.ReadByte();//High/low memory version

                    int length = readPacket.ReadByte();//rsa length
                    Console.WriteLine("RSA Length: " + length);
                    var rsaBytes = readPacket.ReadBytes(length);
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(rsaBytes);

                    var rsaPayload = BigInteger.ModPow(new BigInteger(rsaBytes), RSA_EXPONENT, RSA_MODULUS).ToByteArray();
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(rsaPayload);

                    PacketReader rsaReader = new PacketReader(rsaPayload);

                    //Validate that the RSA block was decoded properly
                    int rsaOpcode = rsaReader.ReadByte();
                    Console.WriteLine("RSA Opcode: {0}", rsaOpcode);

                    if (rsaOpcode != 10)
                    {
                        Console.WriteLine("Unable to decode RSA block properly!");
                        connection.Dispose();
                        return;
                    }

                    // Set up the ISAAC ciphers.
                    long clientHalf = rsaReader.ReadLong();
                    Console.WriteLine("Client Half: {0}", clientHalf);
                    long serverHalf = rsaReader.ReadLong();
                    Console.WriteLine("Client Half: {0}", serverHalf);

                    int[] isaacSeed = { (int)(clientHalf >> 32), (int)clientHalf, (int)(serverHalf >> 32), (int)serverHalf };
                    connection.NetworkDecryptor = new ISAACCipher(isaacSeed);
                    for (int i = 0; i < isaacSeed.Length; i++)
                    {
                        isaacSeed[i] += 50;
                    }
                    connection.NetworkEncryptor = new ISAACCipher(isaacSeed);

                    int uid = rsaReader.ReadInt();
                    Console.WriteLine("UID: {0}", uid);

                    string username = rsaReader.ReadRS2String();
                    string password = rsaReader.ReadRS2String();
                    Console.WriteLine("Username: {0}", username);
                    Console.WriteLine("Password: {0}", password);

                    connection.ConnectionState = ConnectionState.Authenticated;
                    break;
            }
        }

    }
}
