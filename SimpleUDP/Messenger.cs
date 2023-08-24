using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleUDP
{
    public class Messenger
    {
        public class MessageReceivedEventArgs : EventArgs
        {
            public string Message { get; set; }
        }

        private bool _listening;

        /// <summary>
        /// Event that is raised when a message is received by the Messenger object.
        /// </summary>
        /// <remarks>
        /// This event is raised when a message is received by the Messenger object. The event handler should
        /// be used to handle the received message in some way (e.g., displaying it on the console, saving it
        /// to a file, etc.).
        ///
        /// To handle the event, you should attach an event handler to this event using the += operator. The
        /// event handler should take two parameters: a reference to the object that raised the event (usually
        /// referred to as "sender"), and a <see cref="MessageReceivedEventArgs"/> object that contains the
        /// message that was received.
        ///
        /// </remarks>
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        /// <summary>
        /// Sends a message to a specified network address and port.
        /// </summary>
        /// <param name="Address">The network address to which the message will be sent. Use IpAddress.Parse to pass a string.</param>
        /// <param name="Port">The port number on which the message will be sent.</param>
        /// <param name="Message">The message to be sent.</param>
        /// <remarks>
        /// This method establishes a connection to the specified network address and port, and sends
        /// the specified message over the connection. The connection is then closed.
        /// </remarks>
        public async Task Start()
        {
            int port = 12345;
            try
            {
                UdpClient udp = new UdpClient();
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, port);

                udp.Client.Bind(RemoteIpEndPoint);

                
                //Byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);
                var taskResult = await udp.ReceiveAsync();
                var receiveBytes = taskResult.Buffer;
                //string returnData = Encoding.GetString(receiveBytes);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                //MessageReceived.Invoke(this, new MessageReceivedEventArgs { Message = returnData });
                Console.WriteLine(returnData);

                //udp.Dispose();
            }
            catch (Exception e)
            {
                //not yet implemented
            }
        }

        /// <summary>
        /// Starts listening for incoming UDP packets on the specified IP address and port.
        /// </summary>
        /// <param name="address">The IP address to listen on. Use IpAddress.Parse to pass a string.</param>
        /// <param name="port">The port to listen on.</param>
        /// <remarks>
        /// This method creates a new UDP client and listens for incoming packets on the specified IP address
        /// and port. When a packet is received, the message is decoded from a byte array to a UTF-8 encoded
        /// string and the <see cref="MessageReceived"/> event is raised.
        /// </remarks>
        /*public void StartListening(IPAddress address, int port)
        {
            try
            {
                _listening = true;
                var ipEndPoint = new IPEndPoint(address, port);

                UdpClient listener = new UdpClient(ipEndPoint);
                {
                    while (_listening)
                    {
                        var buffer = listener.ReceiveAsync();

                        // Convert the byte array to a string
                        string message = Encoding.UTF8.GetString(buffer.Result.Buffer);

                        //Raise an event containing the message
                        MessageReceived?.Invoke(this, new MessageReceivedEventArgs { Message = message });
                    }

                    listener.Dispose();
                }
            }
            catch (Exception e)
            {
                //not yet implemented
            }
        }
        /// <summary>
        /// Stops the Messenger object from listening for incoming messages.
        /// </summary>
        /// <remarks>
        /// This method stops the Messenger object from listening for incoming messages.
        ///
        /// To use this method, simply call it on a Messenger object when you want to stop listening for incoming
        /// messages.
        ///
        /// </remarks>
        public void StopListening()
        {
            _listening = false;
        }*/
    }
}