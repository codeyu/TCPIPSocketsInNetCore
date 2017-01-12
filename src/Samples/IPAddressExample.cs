using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
namespace TCPIPSocketsInNetCore
{
    public class IPAddressExample
    {
        public static void Run(String host) 
        {
            try
            {
                if(string.IsNullOrEmpty(host))
                {
                    host = Dns.GetHostName();
                }
                else
                {
                    var ipPattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
                    IPHostEntry hostInfo;
                    if(Regex.IsMatch(host,ipPattern))
                    {
                        
                    }
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
                
            }
            catch(Exception)
            {
                Console.WriteLine("\tUnable to resolve host: " + host + "\n");
            }
        }
    }
}