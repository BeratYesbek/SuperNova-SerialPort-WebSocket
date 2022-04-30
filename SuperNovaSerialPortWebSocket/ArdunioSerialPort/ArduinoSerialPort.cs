using System.IO.Ports;

namespace SuperNovaSerialPortWebSocket.ArdunioSerialPort
{
    public class ArduinoSerialPort
    {
        private readonly SerialPort _serialPort;

        public ArduinoSerialPort()
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = "COM4";
            _serialPort.BaudRate = 9600;
            _serialPort.Open();

          
        }


        public bool SerialPortIsOpen()
        {
            return _serialPort.IsOpen;
        }

        public string SerialPortReadLine()
        {
            return _serialPort.ReadLine();
        }

        
    }
}
