using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace GornHost
{
    class Gorner
    {
        TcpClient client;
        NetworkStream clientStream;
        public Dictionary<int, string> messagesToForward = new Dictionary<int, string>();
        public int clientID;

        public Gorner(TcpClient clnt)
        {
            this.client = clnt;
            clientID = GornHost.NextClientID();
            clientStream = client.GetStream();
        }


        public void Process()
        {
            StreamWriter sw = new StreamWriter(clientStream);
            StreamReader sr = new StreamReader(sw.BaseStream);

            sw.WriteLine("hello"); // Tell the client we can hear them.
            sw.Flush();

            string data;
            try
            {
                while((data = sr.ReadLine()) != "end")
                {
                    foreach(KeyValuePair<int, Gorner> gorn in GornHost.gorners)
                    {
                        if (gorn.Key == clientID)
                            continue;
                        gorn.Value.messagesToForward[clientID] = data;
                    }

                    foreach(KeyValuePair<int, string> message in messagesToForward)
                    {
                        sw.WriteLine("{0}:{1}", message.Key, message.Value);
                    }
                    messagesToForward.Clear();
                    sw.WriteLine("waiting");
                    
                }
            }
            finally
            {
                sw.Close();
                GornHost.gorners.Remove(clientID);
            }
        }
    }

   
}
