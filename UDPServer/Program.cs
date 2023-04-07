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
            Messenger messenger = new Messenger();
            Console.WriteLine("type send to send or listen to listen");
            messenger.MessageReceived += (sender, eventArgs) => Console.WriteLine(eventArgs.Message);

            if (Console.ReadLine() == "send")
            {
                messenger.Send(IPAddress.Loopback, 50000, Console.ReadLine() );
            }
            else
            {
                messenger.StartListening(IPAddress.Loopback, 50000);
                messenger.StopListening();
            }

            
        }
    }
}