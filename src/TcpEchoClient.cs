using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
namespace TCPIPSocketsInNetCore
{
    public class TcpEchoClient
    {
        public static void TestClient(string[] args)
        {
            if (args.Length != 1) 
            {
                throw new ArgumentException("Parameters: <Server>[:<Port>]");
            }
            string server = (args[0].Contains(":")) ? args[0].Split(':')[0] : args[0];
            
            int servPort = (args[0].Contains(":")) ? Int32.Parse(args[0].Split(':')[1]) : 7;
            TcpClient client = null;
            NetworkStream netStream = null;
            try
            {
                client = new TcpClient();
                client.ConnectAsync(server, servPort);
                client.NoDelay = true;//!!!
                if(client.Connected)
                {
                    Console.WriteLine("Connected to server...");
                    while(true)
                    {
                        netStream = client.GetStream();
                        if(netStream.CanRead)
                        {
                            byte[] buffer = new byte[1024]; 
                            netStream.Read(buffer, 0, buffer.Length);
                            Console.WriteLine(Encoding.ASCII.GetString(buffer));
                        }
                        var str = Console.ReadLine();
                        
                        if(netStream.CanWrite)
                        {
                            byte[] byteBuffer = Encoding.ASCII.GetBytes(str);
                            netStream.Write(byteBuffer, 0, byteBuffer.Length);
                            Console.WriteLine("Sent {0} bytes to server...", byteBuffer.Length);
                            
                        }
                        if(str == "quit")
                        {
                            Console.WriteLine("Closing connection. goodbye~~"); 
                            netStream.Dispose();
                            client.Dispose();
                            break;
                        }
                    }
                    
                    
                }
                else
                {
                    Console.WriteLine($"Can't connect to server {server}:{servPort}. Try again...");
                }
                
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if(netStream != null)
                    netStream.Dispose();
                if(client != null)
                    client.Dispose();
            }
        }
    }
}