using RSPS.src.entity;
using RSPS.src.entity.npc;
using RSPS.src.entity.player;
using RSPS.src.net;
using RSPS.src.net.packet;
using RSPS.src.net.packet.send.impl;
using RSPS.src.readfiles;
using RSPS.src.task;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Numerics;

namespace RSPS.src
{

    //TODO: Implement player walking
    public class Program
    {

        public static readonly World world = new(1337, new ConnectionListener("0.0.0.0", 43594)); //Development world

        private static Stopwatch idleTimer = new Stopwatch();
        
        private static readonly Stopwatch cycleTimer = new Stopwatch();

        public static readonly List<Entity> RemoveableConnections = new List<Entity>(4000);

        private static ParallelOptions MainParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 25 };
        public static ParallelOptions TaskParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 2 };

        public static void Main()
        {
            // Initialize the world
            if (!world.Initialize()) {
                return;
            }
            // Start processing the world
            //System.Threading.Tasks.Task.WaitAll(new System.Threading.Tasks.Task[] { world.Process() }); //when multiple worlds
            world.Process().Wait();//is this not waiting? We could always ca
        }

        public static void SendGlobalByes(Connection connection, byte[] data)
        {
            if (connection.clientSocket == null)
            {
                return;
            }

            // Begin sending the data to the remote device.
            connection.clientSocket.BeginSend(data, 0, data.Length, 0,
                new AsyncCallback(SendCallback), connection);
        }

        public static void SendGlobalByes(Connection connection, byte[] data, int amount)
        {

            if (connection.clientSocket == null)
            {
                Console.WriteLine("I'm a null man");
                return;
            }

            // Begin sending the data to the remote device.  
            connection.clientSocket.BeginSend(data, 0, amount, 0,
                new AsyncCallback(SendCallback), connection);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            Connection connection = new Connection();
            try
            {
                // Retrieve the socket from the state object.  
                connection = (Connection)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = connection.clientSocket.EndSend(ar);
                //Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            }
            catch (Exception e)
            {
                connection.Disconnect();
            }
        }

    }

}