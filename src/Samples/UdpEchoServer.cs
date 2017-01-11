using System;
using System.Net;
using System.Net.Sockets;
namespace TCPIPSocketsInNetCore
{
    public class UdpEchoServer
    {
        public static void TestUdpServer(string[] args)
        {
            if (args.Length > 1)
            {
                throw new ArgumentException("Parameters: <Port>");
            }
            int servPort = (args.Length == 1) ? Int32.Parse(args[0]) : 7;
            UdpClient client = null;
            try
            {
                client = new UdpClient(servPort);
            }
            catch(SocketException ex)
            {
                Console.WriteLine(ex.SocketErrorCode + ": " + ex.Message);
                Environment.Exit((int)ex.SocketErrorCode);
            }
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), servPort);
            while(true)
            {
                try
                {
                    var udpReceiveTask = client.ReceiveAsync();
                    if(udpReceiveTask != null)
                    {
                        byte[] byteBuffer = udpReceiveTask.Result.Buffer;
                        Console.Write("Handling client at " + remoteIPEndPoint + " - ");
                        client.SendAsync(byteBuffer, byteBuffer.Length, remoteIPEndPoint);
                        Console.WriteLine("echoed {0} bytes.", byteBuffer.Length);
                    }
                    
                }
                catch(SocketException ex)
                {
                    Console.WriteLine(ex.SocketErrorCode + ": " + ex.Message);
                }
            }
        }
    }
}