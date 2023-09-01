// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text;
using CrestronDeviceDiscovery;


Messenger messenger = new Messenger();
messenger.MessageReceived += (sender, eventArgs) =>
{
    var Info = Utilities.CreateDeviceData(eventArgs.UDPResult);
    /*Console.WriteLine($"Device Hostname: {Info.DeviceName}");
    Console.WriteLine($"Device Info: {Info.DeviceInfo}");
    Console.WriteLine($"Device IP: {Info.DeviceAddress}");
    Console.WriteLine($"Sent from Port: {Info.DevicePort}");*/
    //Console.WriteLine(Info.ModelName);
    //Console.WriteLine(Info.DeviceInfo);
    Console.WriteLine(Info.MacAddress);

    
};
    //Console.WriteLine(Encoding.ASCII.GetString(eventArgs.Message));

await messenger.Send("255.255.255.255", 41794, Utilities.DiscoveryMessage);
await messenger.Listen(41794);

Console.ReadKey();
messenger.StopListening();



