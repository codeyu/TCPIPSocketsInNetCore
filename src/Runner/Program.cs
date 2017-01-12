using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var port = Console.ReadLine();
            TCPIPSocketsInNetCore.TcpEchoServerSocket.SocketServerTest(new[]{port});
        }
    }
}
