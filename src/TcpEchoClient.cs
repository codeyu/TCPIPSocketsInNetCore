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
            if (args.Length != 2) 
            {
                throw new ArgumentException("Parameters: <Server>[:<Port>] <Word>");
            }
            string server = (args[0].Contains(":")) ? args[0].Split(':')[0] : args[0];
            byte[] byteBuffer = Encoding.ASCII.GetBytes(args[1]);
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
                    Console.WriteLine("Connected to server... sending echo string");
                    netStream = client.GetStream();
                    netStream.Write(byteBuffer, 0, byteBuffer.Length);
                    Console.WriteLine("Sent {0} bytes to server...", byteBuffer.Length);
                    int totalBytesRcvd = 0;
                    int bytesRcvd = 0;
                    while (totalBytesRcvd < byteBuffer.Length) 
                    {
                        if ((bytesRcvd = netStream.Read(byteBuffer, totalBytesRcvd,
                            byteBuffer.Length - totalBytesRcvd)) == 0)
                        {
                                Console.WriteLine("Connection closed prematurely.");
                                break;
                        }
                        totalBytesRcvd += bytesRcvd;
                    }
                    Console.WriteLine("Received {0} bytes from server: {1}", totalBytesRcvd,
                                    Encoding.ASCII.GetString(byteBuffer, 0, totalBytesRcvd));
                }
                Console.WriteLine($"Can't connect to server {server}:{servPort}. Try again...");
                
                
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