using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleUDP
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Messager messager = new Messager();
            Console.WriteLine("type send to send or listen to listen");
            messager.MessageReceived += (sender, eventArgs) => Console.WriteLine(eventArgs.Message);

            if (Console.ReadLine() == "send")
            {
                messager.Send("192.168.1.20", 50000, Console.ReadLine());
            }
            else
            {
                messager.StartListening();
            }

            
        }
    }
}