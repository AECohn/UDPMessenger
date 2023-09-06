using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrestronDeviceDiscovery
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public UdpReceiveResult UDPResult { get; set; }
    }

    internal class Messenger
    {
        

        public void StopListening()
        {
            IsListening = false;
        }

        private bool IsListening = false;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public async Task Send(string Address, int Port, byte[] Message)
        {

            try
            {
                using (UdpClient udp = new UdpClient())
                {
                    await udp.SendAsync(Message, Message.Length, Address, Port);
                }
            }

            catch (Exception e)
            {
               Console.WriteLine(e.Message);
            }
        }

        public async Task Listen(int port)
        {
            using (UdpClient receiveClient = new UdpClient(port))
            {
                IsListening = true;
                try
                {
                    while (IsListening)
                    {
                        UdpReceiveResult result = await receiveClient.ReceiveAsync();
                        //if(result.RemoteEndPoint.Address.ToString() != Utilities.LocalIPAddress().ToString())
                            MessageReceived.Invoke(this, new MessageReceivedEventArgs { UDPResult = result });
                    }
                }
                catch (Exception e)
                {
                   Console.WriteLine(e.Message);

                }
            }

        }
    }
}