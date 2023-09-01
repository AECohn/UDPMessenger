using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using Timer = System.Timers.Timer;

namespace CrestronDeviceDiscovery;

public class DeviceDiscovery
{
    static bool IsListening;
    
    //public static Dictionary<string, DeviceData> Devices = new Dictionary<string, DeviceData>();
    
    /*public async Task <Dictionary<string, DeviceData>> GetDevices()
    {
        Send(IPAddress.Broadcast.ToString(), 41794, Data.DiscoveryMessage);
        Task<Dictionary<string, DeviceData>> task = Listen(41794);
        task.Wait();
        return task.Result;
    }*/

    public static void Send(string Address, int Port, byte[] Message)
    {
            

        var messageBytes = Message;

        try
        {
            UdpClient udp = new UdpClient();

            udp.Send(messageBytes, messageBytes.Length, Address, Port);


        }

        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    public static async Task<Dictionary<string, DeviceData>> Listen(int port) //have to give this the ability to know when it's complete
    {
        IsListening = true;
        Dictionary<string, DeviceData> tempDictionary = new Dictionary<string, DeviceData>();
        UdpReceiveResult result;
        TimeSpan timeoutDuration = TimeSpan.FromSeconds(1);
        CancellationTokenSource cts = new CancellationTokenSource(timeoutDuration);
        using (UdpClient receiveClient = new UdpClient(port))
        {
            try
            {
                


                while (!cts.Token.IsCancellationRequested)
                {
                    result = await receiveClient.ReceiveAsync();
                    
                   

                    if (result.Buffer.Length > 0)
                    {
                        if (!result.RemoteEndPoint.Address.Equals(Data.LocalIPAddress()))
                        {
                            var stringResult = Encoding.Default.GetString(result.Buffer);
                            var DeviceInfo = stringResult.Split('\0').Where(x => !string.IsNullOrEmpty(x))
                                .Where(str => Regex.IsMatch(str, @"[a-zA-Z0-9]")).ToList();
                            DeviceInfo.Add(result.RemoteEndPoint.Address.ToString());
                            DeviceInfo.Add(result.RemoteEndPoint.Port.ToString());
                            DeviceData temp = new DeviceData();
                            temp.DeviceName = DeviceInfo[0];
                            temp.DeviceAddress = DeviceInfo[2];
                            temp.DevicePort = Convert.ToInt32(DeviceInfo[3]);
                            temp.DeviceInfo = DeviceInfo[1];
                            //Devices.Add(DeviceInfo[0], temp);
                            
                            //placeholder for testing
                            /*
                            Console.WriteLine(temp.DeviceName);
                            Console.WriteLine(temp.DeviceAddress);
                            Console.WriteLine(temp.DevicePort);
                            Console.WriteLine(temp.DeviceInfo);
                            
                            */
                           

                            Console.WriteLine();
                            
                            //Task.Delay(timeoutDuration, cts.Token);
                        }

                        //Console.WriteLine($"Device: {Encoding.Default.GetString(result.Buffer)} {"\n"} Address: {result.RemoteEndPoint.Address} {"\n"} Port: {result.RemoteEndPoint.Port}");
                    }
                    else
                    {
                        
                    }
                    
                }
               
            }
            catch (Exception e)
            {
                //not yet implemented
            }
            finally
            {
                receiveClient.Close();
                cts.Dispose();
            }
            
        }
        return new Dictionary<string, DeviceData>();
    }
}