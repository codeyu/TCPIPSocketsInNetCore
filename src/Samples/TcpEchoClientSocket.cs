using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
namespace TCPIPSocketsInNetCore
{
    public class TcpEchoClientSocket
    {
        public static void Run(string[] args)
        {
            if ((args.Length < 2) || (args.Length > 3))
            { // Test for correct # of args
                throw new ArgumentException("Parameters: <Server> <Word> [<Port>]");
            }
            String server = args[0];     // Server name or IP address
            // Convert input String to bytes
            byte[] byteBuffer = Encoding.ASCII.GetBytes(args[1]);
            // Use port argument if supplied,otherwise default to 7
            int servPort = (args.Length == 3) ? Int32.Parse(args[2]) : 7;
            Socket sock = null;


            try
            { // Create a TCP socket instance
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream,
                                    ProtocolType.Tcp);
                // Creates server IPEndPoint instance. We assume Resolve returns
                // at least one address
                IPEndPoint serverEndPoint = new IPEndPoint(Dns.GetHostAddressesAsync(server).Result[0],
                servPort); // Connect the socket to server on specified port
                sock.Connect(serverEndPoint);
                Console.WriteLine("Connected to server... sending echo string");
                // Send the encoded string to the server
                sock.Send(byteBuffer, 0, byteBuffer.Length, SocketFlags.None);
                Console.WriteLine("Sent {0} bytes to server...", byteBuffer.Length);
                int totalBytesRcvd = 0;   // Total bytes received so far
                int bytesRcvd = 0;        // Bytes received in last read
                // Receive the same string back from the server
                while (totalBytesRcvd < byteBuffer.Length)
                {
                    if ((bytesRcvd = sock.Receive(byteBuffer, totalBytesRcvd,
                        byteBuffer.Length - totalBytesRcvd, SocketFlags.None)) == 0)
                    {
                        Console.WriteLine("Connection closed prematurely.");
                        break;
                    }
                    totalBytesRcvd += bytesRcvd;
                }
                Console.WriteLine("Received {0} bytes from server: {1}", totalBytesRcvd,
                                Encoding.ASCII.GetString(byteBuffer, 0, totalBytesRcvd));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                sock.Dispose();
            }
        }
    }
}