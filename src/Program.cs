using System;

namespace TCPIPSocketsInNetCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IPAddressExample.PrintHostInfo();
            foreach (String arg in args) 
            {
                Console.WriteLine(arg + ":");
                IPAddressExample.PrintHostInfo(arg);
            }
        }
    }
}
