using System;
using System.Net;
using System.Net.Sockets;
namespace TCPIPSocketsInNetCore
{
    public class IPAddressExample
    {
        public static void PrintHostInfo()
        {
            try
            {
                Console.WriteLine("Local Host:");
                String localHostName = Dns.GetHostName();
                Console.WriteLine("\tHost Name: " + localHostName);
                PrintHostInfo(localHostName);
            }
            catch(Exception)
            {
                Console.WriteLine("Unable to resolve local host\n");
            }
            

        }
        public static void PrintHostInfo(String host) 
        {
            try
            {
                IPHostEntry hostInfo;
                hostInfo = Dns.GetHostEntryAsync(host).Result;
                Console.WriteLine("\tCanonical Name: " + hostInfo.HostName);
                Console.Write("\tIP Addresses: ");
                foreach (IPAddress ipaddr in hostInfo.AddressList)
                {
                    Console.Write(ipaddr.ToString() + " ");
                }
                Console.WriteLine();
                Console.Write("\tAliases: ");
                foreach (String alias in hostInfo.Aliases)
                {
                    Console.Write(alias + " ");
                }
                Console.WriteLine("\n");
            }
            catch(Exception)
            {
                Console.WriteLine("\tUnable to resolve host: " + host + "\n");
            }
        }
    }
}