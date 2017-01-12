using System;             // For Console, Int32, ArgumentException, Environment
using System.Net;         // For IPAddress
using System.Net.Sockets; // For TcpListener, TcpClient
using System.Text;

namespace TCPIPSocketsInNetCore
{
    public class TcpEchoServerSocket
    {
        private const int BUFSIZE = 32; // Size of receive buffer
        private const int BACKLOG = 5;  // Outstanding connection queue max size
        public static void SocketServerTest(string[] args)
        {

            if (args.Length > 1) //
                throw new ArgumentException("Parameters: [<Port>]");
            int servPort = (args.Length == 1) ? Int32.Parse(args[0]) : 7;
            Socket server = null;
            try
            {
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                            ProtocolType.Tcp);
                server.Bind(new IPEndPoint(IPAddress.Any, servPort));
                server.Listen(BACKLOG);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.SocketErrorCode + ": " + ex.Message);
                Environment.Exit((int)ex.SocketErrorCode);
            }
            byte[] rcvBuffer = new byte[BUFSIZE]; // Receive buffer
            int bytesRcvd;
            while (true)
            { // Run forever, accepting and servicing connections
                Socket client = null;
                try
                {
                    Console.WriteLine("Waiting for client...");
                    client = server.Accept(); // Get client connection
                    Console.WriteLine($"Server started. Listening to TCP clients at {client.RemoteEndPoint}");  
                    // Receive until client closes connection, indicated by 0 return value
                    int totalBytesEchoed = 0;
                    while ((bytesRcvd = client.Receive(rcvBuffer, 0, rcvBuffer.Length,
                                                    SocketFlags.None)) > 0 && Encoding.ASCII.GetString(rcvBuffer) != "quit")
                    {
                        byte[] data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");
                        client.Send(data, 0, data.Length, SocketFlags.None);
                        totalBytesEchoed += bytesRcvd;
                        Console.WriteLine(Encoding.ASCII.GetString(rcvBuffer));
                    }
                    Console.WriteLine($"Closing connection, echoed {totalBytesEchoed} bytes.");
                    client.Dispose();   // Close the socket. We are done with this client!
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    client.Dispose();
                }

            }
        }
    }
}