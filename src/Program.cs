using System;

namespace TCPIPSocketsInNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IPAddressExample.PrintHostInfo();
            if(args[0] == "s" && args.Length == 2)
            {
                TcpEchoServer.TestServer(new string[]{args[1]});
            }
            else if(args[0] == "c" && args.Length==2)
            {
                TcpEchoClient.TestClient(new string[]{args[1]});
            }
            else
            {
                foreach (String arg in args) 
                {
                    Console.WriteLine(arg + ":");
                    IPAddressExample.PrintHostInfo(arg);
                }
            }
            
        }
    }
}
