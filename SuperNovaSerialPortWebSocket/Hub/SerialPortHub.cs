using System;
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

        public async Task SerialPortDataRequest(string connId)
        {
            var serialPort = new ArduinoSerialPort();
            if (serialPort.SerialPortIsOpen())
            {
                while (true)
                {
                    var data = serialPort.SerialPortReadLine();
                    if (data != null)
                    {
                        await SendData(data, connId);
                    }
                }
            }
            else
            {
                await SendData("Connection is closed", connId);
            }
        }

        public async Task SendData(string data, string connId)
        {
            Console.WriteLine("Data Received --> " + data);
            await Clients.Client(connId).SendAsync("ReceiveData", data);
        }
    }
}
