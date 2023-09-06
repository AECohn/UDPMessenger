﻿// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text;
using CrestronDeviceDiscovery;


Messenger messenger = new Messenger();
messenger.MessageReceived += (sender, eventArgs) =>
{
    var Info = Utilities.CreateDeviceData(eventArgs.UDPResult);
    Console.WriteLine($"Device ModelName: {Info.ModelName}");
    Console.WriteLine($"Device Firmware: {Info.Firmware}");
    Console.WriteLine($"Device Hostname: {Info.DeviceName}");
    Console.WriteLine($"Device Mac: {Info.MacAddress}");
    Console.WriteLine($"Device IP: {Info.DeviceAddress}");
    Console.WriteLine($"Sent from Port: {Info.DevicePort}");

    Console.WriteLine();



};
    //Console.WriteLine(Encoding.ASCII.GetString(eventArgs.Message));

await messenger.Send("255.255.255.255", 41794, Utilities.DiscoveryMessage);
await messenger.Listen(41794);

Console.ReadKey();
messenger.StopListening();



