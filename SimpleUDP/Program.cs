using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUDP
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int startPort = 100; // Starting port of the range
            int endPort = 70000;   // Ending port of the range

            Console.WriteLine("Async UDP Port Listener");

            var tasks = new List<Task>();

            for (int port = startPort; port <= endPort; port++)
            {
                //Console.WriteLine($"Listening on port {port}");
                tasks.Add(StartListeningAsync(port));
            }

            await Task.WhenAll(tasks);
        }

        static async Task StartListeningAsync(int port)
        {
            UdpClient udpClient = new UdpClient(port);

            try
            {
                while (true)
                {
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    string message = Encoding.ASCII.GetString(result.Buffer);
                    Console.WriteLine($"Received from {result.RemoteEndPoint} on port {port}: {message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
            finally
            {
                udpClient.Close();
            }
        }
    }
}

