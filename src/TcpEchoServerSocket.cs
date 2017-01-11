using System;             // For Console, Int32, ArgumentException, Environment
using System.Net;         // For IPAddress
using System.Net.Sockets; // For TcpListener, TcpClient

namespace TCPIPSocketsInNetCore
{
    public class TcpEchoServerSocket
    {
        private const int BUFSIZE= 32; // Size of receive buffer
        private const int BACKLOG= 5;  // Outstanding connection queue max size
        static void SocketServerTest(string[] args) 
        {

                if (args.Length > 1) //
                throw new ArgumentException("Parameters: [<Port>]");
                int servPort = (args.Length == 1) ? Int32.Parse(args[0]): 7;
                Socket server = null;
                try
                {
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                ProtocolType.Tcp);
                    server.Bind(new IPEndPoint(IPAddress.Any, servPort));
                    server.Listen(BACKLOG);
                }
                catch(SocketException ex)
                {
                    Console.WriteLine(ex.SocketErrorCode + ": " + ex.Message);
                    Environment.Exit((int)ex.SocketErrorCode);
                }
                byte[] rcvBuffer = new byte[BUFSIZE]; // Receive buffer
                int bytesRcvd;
                while(true) 
                { // Run forever, accepting and servicing connections
                    Socket client = null;
                    try 
                    {
                        client = server.Accept(); // Get client connection
                        Console.Write("Handling client at " + client.RemoteEndPoint + " - ");
                        // Receive until client closes connection, indicated by 0 return value
                        int totalBytesEchoed = 0;
                        while ((bytesRcvd = client.Receive(rcvBuffer, 0, rcvBuffer.Length,
                                                        SocketFlags.None)) > 0) {
                        client.Send(rcvBuffer, 0, bytesRcvd, SocketFlags.None);
                        totalBytesEchoed += bytesRcvd;
                        }
                        Console.WriteLine("echoed {0} bytes.", totalBytesEchoed);
                        client.Dispose();   // Close the socket. We are done with this client!
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        client.Dispose();
                    }

                }
            }
        }
}