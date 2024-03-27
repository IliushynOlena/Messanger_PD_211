using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    public class ChatServer
    {
        const short port = 4041;
        const string JOIN_CMD = "$<join>";
        List<IPEndPoint> members ;
        UdpClient server ;
        IPEndPoint clientEndPoint = null;
        public ChatServer()
        {
            server = new TcpListener(new IPEndPoint(IPAddress.Parse(serverAddress), port));
        }   
        public void Start()
        {       
            server.Start();
            Console.WriteLine("Waiting for connection......");

            TcpClient client =  server.AcceptTcpClient();
            Console.WriteLine("Connected!!!");
            NetworkStream ns =  client.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            while (true)
            {

                string message = sr.ReadLine();
                Console.WriteLine($" Got message {message}  from : {client.Client.LocalEndPoint} " +
                    $"at {DateTime.Now.ToShortTimeString()}");

                sw.WriteLine("Thanks.....");
                sw.Flush(); 
              
            }

        }
    }
    internal class Program
    {       
        static void Main(string[] args)
        {
            ChatServer server = new ChatServer();   
            server.Start();
              
            
        }
       
    }
}
