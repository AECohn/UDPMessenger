using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SimpleUDP
{
    public class Messenger
    {
        public class MessageReceivedEventArgs : EventArgs
        {
            public string Message { get; set; }
        }

        private bool Listening;

        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void Send(string Address, int Port, string Message)
        {
            UdpClient sender = new UdpClient();

            sender.Connect(Address, Port);
            sender.Send(System.Text.Encoding.UTF8.GetBytes(Message), Message.Length);
            sender.Dispose();
        }

        public void StartListening()
        {
            Listening = true;
            var ipEndPoint = new IPEndPoint(IPAddress.Any, 50000);

            UdpClient listener = new UdpClient(ipEndPoint);


            while (Listening)
            {
                var buffer = listener.ReceiveAsync();

                // Convert the byte array to a string
                string message = Encoding.UTF8.GetString(buffer.Result.Buffer);

                // Display the message on the console
                MessageReceived?.Invoke(this, new MessageReceivedEventArgs { Message = message });
            }
            
            listener.Dispose();
        }

        private void StopListening()
        {
            Listening = false;
        }
    }
}