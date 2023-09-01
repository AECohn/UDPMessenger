// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text;
using CrestronDeviceDiscovery;


Messenger messenger = new Messenger();
messenger.MessageReceived += (sender, eventArgs) => Console.WriteLine(Encoding.Latin1.GetString(eventArgs.Message));

await messenger.Send("255.255.255.255", 41794, Data.DiscoveryMessage);
await messenger.Listen(41794);

Console.ReadKey();
messenger.StopListening();



