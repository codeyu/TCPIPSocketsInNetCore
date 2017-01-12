using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TCPIPSocketsInNetCore
{
    public class UdpEchoClient
    {
        public static void Run(string[] args)
        {
            if (args.Length != 2)
            {
                throw new System.ArgumentException("Parameters: <Server>[:<Port>] <Word>");
            }
            string server = (args[0].Contains(":")) ? args[0].Split(':')[0] : args[0];
            int servPort = (args[0].Contains(":")) ? Int32.Parse(args[0].Split(':')[1]) : 7;
            byte[] sendPacket = Encoding.ASCII.GetBytes(args[1]);
            UdpClient client = new UdpClient();
            try
            {
                client.SendAsync(sendPacket, sendPacket.Length, server, servPort);
                Console.WriteLine("Sent {0} bytes to the server...", sendPacket.Length);
                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Parse(server), servPort);
                var udpReceiveTask = client.ReceiveAsync();
                if(udpReceiveTask != null)
                {
                    byte[] rcvPacket = udpReceiveTask.Result.Buffer;
                    Console.WriteLine("Received {0} bytes from {1}: {2}",
                                    rcvPacket.Length, remoteIPEndPoint,
                                    Encoding.ASCII.GetString(rcvPacket, 0, rcvPacket.Length));
                }
                
            }
            catch(SocketException se)
            {
                Console.WriteLine(se.SocketErrorCode + ": " + se.Message);
            }
            client.Dispose();
        }
    }
}