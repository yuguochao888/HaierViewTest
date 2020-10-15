using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HaierViewTest.Devices
{
   
   public class CommDriver
    {
        /// <summary>
        /// 串口对象
        /// </summary>
        private SerialPort _serialPort;


        public CommDriver(SerialPort serialPort)
        {
            _serialPort = serialPort;

        }

        public CommDriver()
        {
            _serialPort=new SerialPort();
            _serialPort.BaudRate = 9600;
            _serialPort.PortName = "Com1";


        }

        public bool Write(string value)
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.WriteLine(value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
               // MessageBox.Show("串口发送失败!!" + e.Message);
                return false;
            }

            return true;

        }


        public bool Open()
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    _serialPort.Open();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
             //   MessageBox.Show("串口打开失败!!" + e.Message);
                return false;
            }

            return true;
        }
        protected string Read()
        {
            string strRead = string.Empty;
            try
            {
                strRead = _serialPort.ReadExisting();
            }
            catch (System.Exception ex)
            {
             //   MessageBox.Show("串口接收数据失败!!" + ex.Message);
                strRead = "ERROR";
            }

            return strRead;

        }

    }
}
