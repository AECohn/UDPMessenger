using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Dns = System.Net.Dns;
using IPAddress = System.Net.IPAddress;
using IPHostEntry = System.Net.IPHostEntry;

namespace SimpleUDP
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public UdpReceiveResult UdpResult { get; set; }
    }

    internal class Messenger
    {
        private IPAddress _localAddress = LocalIpAddress();

        public void StopListening()
        {
            _isListening = false;
        }


        private bool _isListening;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public async Task Send(string address, int port, byte[] message)
        {
            try
            {
                using (UdpClient udp = new UdpClient())
                {
                    if(address == "224.0.0.1")
                    {
                        udp.JoinMulticastGroup(IPAddress.Parse(address));
                    }
                    await udp.SendAsync(message, message.Length, address, port);
                }
            }
            catch (Exception e)
            {
                //ErrorLog.Error(e.Message);
            }
        }

        public async Task Listen(int port)
        {
            using (UdpClient receiveClient = new UdpClient(port))
            {
                _isListening = true;
                try
                {
                    while (_isListening)
                    {
                        UdpReceiveResult result = await receiveClient.ReceiveAsync();
                        if (result.Buffer.Length > 0)
                            if (result.RemoteEndPoint.Address.ToString() != _localAddress.ToString())
                                MessageReceived?.Invoke(this, new MessageReceivedEventArgs { UdpResult = result });
                    }
                }
                catch (Exception e)
                {
                    //ErrorLog.Error(e.Message);
                }
            }
        }

        private static IPAddress LocalIpAddress()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry hostEntry = Dns.GetHostEntry(hostName);

            foreach (IPAddress ipAddress in hostEntry.AddressList)
            {
                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ipAddress;
                }
            }

            return null;
        }
    }
}