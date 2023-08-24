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
            // Specify the port to listen on
            int port = 12345;

            // Create a UDP client to listen for incoming messages
            UdpClient udpClient = new UdpClient(12345);

            Console.WriteLine("UDP listener started. Waiting for messages...");

            try
            {
                while (true)
                {
                    // Receive a UDP message and the sender's information
                    IPEndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, port);
                    byte[] receivedBytes = udpClient.Receive(ref senderEndPoint);

                    // Convert the received bytes to a string
                    string receivedMessage = Encoding.UTF8.GetString(receivedBytes);

                    Console.WriteLine($"Received message from {senderEndPoint}: {receivedMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                udpClient.Close();
            }
        }
    }
}

