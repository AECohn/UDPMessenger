using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CrestronDeviceDiscovery
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public UdpReceiveResult Message { get; set; }
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
                //not yet implemented
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
                        MessageReceived.Invoke(this, new MessageReceivedEventArgs { Message = result });
                    }
                }
                catch (Exception e)
                {
                    //not yet implemented
                }
            }

        }
    }
}