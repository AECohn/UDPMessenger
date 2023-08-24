using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUDP
{
    internal class Program
    {

        static void OnUdpData(IAsyncResult result)
        {
            // this is what had been passed into BeginReceive as the second parameter:
            UdpClient socket = result.AsyncState as UdpClient;
            // points towards whoever had sent the message:
            IPEndPoint source = new IPEndPoint(0, 0);
            // get the actual message and fill out the source:
            byte[] message = socket.EndReceive(result, ref source);
            // do what you'd like with `message` here:
            Console.WriteLine("Got " + message.Length + " bytes from " + source);
            Console.WriteLine(Encoding.GetEncoding("ISO-8859-1").GetString(message));
            // schedule the next receive operation once reading is done:
            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);
        }

        static void Main(string[] args)
        {
            UdpClient socket = new UdpClient(5394); // `new UdpClient()` to auto-pick port
            // schedule the first receive operation:
            socket.BeginReceive(new AsyncCallback(OnUdpData), socket);
            // sending data (for the sake of simplicity, back to ourselves):
            IPEndPoint target = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5394);
            // send a couple of sample messages:
            for (int num = 1; num <= 3; num++)
            {
                byte[] message = new byte[num];
                socket.Send(message, message.Length, target);
            }

            Console.ReadKey();
        }
    }
}

