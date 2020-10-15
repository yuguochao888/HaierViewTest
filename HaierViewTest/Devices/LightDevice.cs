using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaierViewTest.Devices
{
   public  class LightDevice
   {
       private CommDriver _comm;
       public LightDevice(string portName) 
       {
           using (SerialPort serialPort = new SerialPort(portName))
           {
               _comm = new CommDriver(serialPort);
               _comm.Open();
           }
        
       }

       /// <summary>
       /// 打开灯
       /// </summary>
       /// <param name="i">灯号</param>
        public void OpenLight(int i)
        {
            _comm.Write("");
        }
        /// <summary>
        /// 关闭灯
        /// </summary>
        /// <param name="i">灯号</param>
        public void CloseLight(int i)
        {
           _comm.Write("");
        }

    }
}
