using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SuperNovaSerialPortWebSocket.ArdunioSerialPort;

namespace SuperNovaSerialPortWebSocket.Hub
{
    public class SerialPortHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine("Connected --> Connection Opened" + Context.ConnectionId);
            Clients.Client(Context.ConnectionId).SendAsync("ReceiveConnID", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("Disconnected --> Connection Closed" + Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SerialPortDataRequest(string serialPort)
        {

            var serial = new ArduinoSerialPort(serialPort);
            if (serial.SerialPortIsOpen())
            {
                while (true)
                {
                    var data = serial.SerialPortReadLine();
                    if (data != null)
                    {
                        await SendData(data);
                    }
                }
            }
            else
            {
                await SendData("Connection is closed");
            }
        }

        public async Task GetAllComAsync(string connId)
        {
            var serialPorts = SerialPort.GetPortNames();
            Console.WriteLine("Ports Listed {0}",serialPorts.Length);
            
            await Clients.All.SendAsync("GetAllComAsync", serialPorts);
        }
        

        public async Task SendData(string data)
        {
            Console.WriteLine("Data Received --> " + data);
            await Clients.All.SendAsync("ReceiveData", data);
        }
    }
}
