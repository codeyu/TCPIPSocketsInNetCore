using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
namespace TCPIPSocketsInNetCore
{
    public class IPAddressExample
    {
        public static void Run(string[] args) 
        {
            var host = Dns.GetHostName();
            Console.WriteLine($"{host}\n");
            try
            {
                
                if(args.Length >0  && !string.IsNullOrEmpty(args[0]))
                {
                    Console.WriteLine($"{args[0]}\n");
                    host = args[0];
                }
                    
                //var ipPattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
                IPHostEntry hostInfo;
                
                hostInfo = Dns.GetHostEntryAsync(host).Result;
            
                Console.WriteLine("\tCanonical Name: " + hostInfo.HostName);
                Console.Write("\tIP Addresses: \n");
                foreach (IPAddress ipaddr in hostInfo.AddressList)
                {
                    Console.Write($"\t{ipaddr.ToString()}\n");
                }
                Console.WriteLine();
                Console.Write("\tAliases: ");
                foreach (String alias in hostInfo.Aliases)
                {
                    Console.Write(alias + " ");
                }
                Console.WriteLine("\n");
                

            }
            catch(Exception ex)
            {
                Console.WriteLine("\tUnable to resolve host: " + host + "\n");
                Console.WriteLine("\tException: " + ex.Message + "\n");
            }
        }
    }
}