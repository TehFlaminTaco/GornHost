using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GornHost
{
    class GornHost
    {
        public static Dictionary<int, Gorner> gorners = new Dictionary<int, Gorner>();
        static void Main(string[] args)
        {
            IPAddress ipAd = IPAddress.Parse("127.0.0.1");
            TcpListener server = new TcpListener(ipAd, 4676);

            server.Start();

            Console.WriteLine("Server running on port 4676");
            Console.WriteLine("Local End point is : {0}", server.LocalEndpoint);
            Console.WriteLine("Awaiting Connections...");
            while (true)
            {
                Gorner gorner = new Gorner(server.AcceptTcpClient());
                gorners[gorner.clientID] = gorner;
                new Thread(new ThreadStart(gorner.Process)).Start();
            }
            //server.Stop();
        }

        public static int NextClientID()
        {
            int id = 0;
            while (gorners.ContainsKey(id)){id++;}
            return id;
        }
    }
}
