using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using AutoMachineDAL;
using DevExpress.Data.Camera;
using HaierViewTest.Devices;

namespace HaierViewTest.Model
{
   public class TestModel
   {

       public CommDriver CommDriver { get; set; }

     //  public CameraAbstractClass CameraAbstractClass;

        public TestModel()
        {
            

            InitDevices();
        }

        public void InitDevices()
        {
            CommDriver = new CommDriver(new SerialPort());

          //  CameraAbstractClass=new Balser_AreaCamera_Gige();

        }

        /// <summary>
        /// 开始进行测试
        /// </summary>
        public void StartAction()
        {




        }

    }
}
