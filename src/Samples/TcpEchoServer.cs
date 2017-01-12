using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
namespace TCPIPSocketsInNetCore
{
    public class TcpEchoServer
    {
        private const int BUFSIZE = 32;
        public static void Run(string[] args)
        {
            if (args.Length > 1)
                throw new ArgumentException("Parameters: [<Port>]");
            int servPort = (args.Length == 1) ? Int32.Parse(args[0]): 7;
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(IPAddress.Any, servPort);
                listener.Start();
                Console.WriteLine($"Server started. Listening to TCP clients at 127.0.0.1:{servPort}"); 

            }
            catch(SocketException e)
            {
                Console.WriteLine(e.SocketErrorCode + ": " + e.Message);
                Environment.Exit(-1);
            }
            byte[] rcvBuffer = new byte[BUFSIZE];
            while(true)
            {
                try
                {
                    Console.WriteLine("Waiting for client...");  
                    var clientTask = listener.AcceptTcpClientAsync(); // Get the client  
   
                    if(clientTask.Result != null)
                    {  
                        Console.WriteLine("Client connected. Waiting for data.");  
                        var client = clientTask.Result;  
                        string message = "";  
   
                        while (message != null && !message.StartsWith("quit"))
                         {  
                            byte[] data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");  
                            client.GetStream().Write(data, 0, data.Length);  
   
                            byte[] buffer = new byte[1024];  
                            client.GetStream().Read(buffer, 0, buffer.Length);  
   
                            message = Encoding.ASCII.GetString(buffer);  
                            Console.WriteLine(message);  
                        }  
                        Console.WriteLine("Closing connection.");  
                        client.GetStream().Dispose();  
                    }  
                }  
                catch(Exception e) 
                {
                    Console.WriteLine(e.Message);
                } 
            }
        }
    }
}