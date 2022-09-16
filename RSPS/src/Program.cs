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
        public static Socket? listener = null;
        private static Stopwatch idleTimer = new Stopwatch();
        public static readonly World world = new World();
        private static readonly Stopwatch cycleTimer = new Stopwatch();

        public static readonly Queue<Entity> connections = new Queue<Entity>();
        public static readonly List<Entity> RemoveableConnections = new List<Entity>(4000);

        private static ParallelOptions MainParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 25 };
        public static ParallelOptions TaskParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 2 };

        public static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(Constants.ENDPOINT), Constants.PORT);
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listener.Bind(endPoint);
                listener.Listen(25);
                listener.Blocking = false;
                listener.DontFragment = true;
                listener.NoDelay = true;
                listener.BeginAccept(new AsyncCallback(NetworkListener.AcceptCallback), listener);
                Console.WriteLine("Sever {0} has started on {1}:{2}", Constants.SERVER_NAME, Constants.ENDPOINT, Constants.PORT);
                cycleTimer.Start();
                //TaskManager.StartTaskManager();
                //ReadItemPrices.ReadPrices();
                //Environment.Exit(0);
                
                while (true)
                {
                    try
                    {
                        //List<Player> playerCopy = World.players.ToList();
                        cycleTimer.Restart();
                        long startTimer = cycleTimer.ElapsedMilliseconds;

                        Parallel.ForEach(World.players, MainParallelOptions, player =>
                        {
                            player.MovementHandler.ProcessMovements();
                        });

                        Parallel.ForEach(World.players, MainParallelOptions, player =>
                        {
                            //NpcUpdating.Update(player);
                            PlayerUpdating.Update(player);
                        });

                        Parallel.ForEach(World.players, MainParallelOptions, player =>
                        {
                            player.ResetFlags();
                        });

                        foreach (Npc npc in World.npcs)
                        {
                            npc.ResetFlags();
                        }

                        for (int i = 0; i < 100; i++)
                        {
                            if (connections.Count <= 0)
                            {
                                break;
                            }

                            var entity = connections.Dequeue();
                            if (entity.GetType() == typeof(Player))
                            {
                                Player player = (Player)entity;
                                if (player == null)
                                {
                                    Console.WriteLine("Player is null");
                                }
                                player?.InitializePlayerSession();
                            }
                        }


                        for (int i = 0; i < (World.players.Count > 0 ? 1 : 0); i++)
                        {
                            //!World.players[i].PlayerConnection.clientSocket.Connected
                            if (!(World.players[i].PlayerConnection.clientSocket.Poll(1, SelectMode.SelectRead)
                                && World.players[i].PlayerConnection.clientSocket.Available == 0))
                            {
                                World.players[i].NeedsPlacement = false;
                                World.players[i].Position = new Position(-1, -1);
                                World.players.RemoveAt(i);
                            }
                        }

                        //foreach (Player player in World.players)
                        //{
                        //    if (player.PlayerConnection == null || !player.PlayerConnection.clientSocket.Connected)
                        //    {
                        //        player.Position.SetNewPosition(new Position(-1, -1));
                        //    }
                        //}

                        /*                        for (int i = 0; i < 45; i++)
                                                {
                                                    if (RemoveableConnections.Count <= 0)
                                                    {
                                                        break;
                                                    }

                                                    var entity = RemoveableConnections.ElementAt(i);
                                                    if (entity == null)
                                                    {
                                                        continue;
                                                    }

                                                    if (entity.GetType() == typeof(Player))
                                                    {
                                                        Player player = (Player)entity;
                                                        player.Position.SetNewPosition(new Position(-1, -1));
                                                        World.players.Remove(player);
                                                    }
                                                }*/


                        long endTimer = cycleTimer.ElapsedMilliseconds;
                        long sleepTime = 600 - (endTimer - startTimer);
                        Console.WriteLine("Server sleeping for: " + sleepTime + ". Current Players: " + World.players.Count);
                        Thread.Sleep((int)sleepTime);
                    }catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex.ToString());
                        Console.Error.WriteLine("Main Game Loop is dead?");
                        Environment.Exit(1);
                    }
                    
                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Socket is dead :(");
                Environment.Exit(1);
            }
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