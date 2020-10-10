using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using PylonC.NET;
using HalconDotNet;

namespace AutoMachineDAL
{
    public  class Balser_AreaCamera_Gige:CameraAbstractClass
    {

       /**********************************************软触发模式开始***************************************************************/

        private PYLON_DEVICE_HANDLE hDev = new PYLON_DEVICE_HANDLE();   /* Handle for the pylon device. */
        private uint numDevices;                                        /* Number of available devices. */
        private const int numGrabs = 1000;                              /* Number of images to grab. */
        private PylonBuffer<Byte> imgBuf = null;                        /* Buffer used for grabbing. */

        public Balser_AreaCamera_Gige()
        {
            


        }

        //打开相机
        public override void OpenCameraSoftTrigger(string DCF_Name)
        {
            bool isAvail;
           
            Pylon.Initialize();

            /* Enumerate all camera devices. You must call PylonEnumerateDevices() before creating a device. */
            numDevices = Pylon.EnumerateDevices();

            if (0 == numDevices)
            {
                MessageBox.Show("没有找到相机");
                return;
            }
            else
            {
                /* Get a handle for the first device found.  */
                hDev = Pylon.CreateDeviceByIndex(0);
            }

            /* Before using the device, it must be opened. Open it for configuring parameters and for grabbing images. */
            Pylon.DeviceOpen(hDev, Pylon.cPylonAccessModeControl | Pylon.cPylonAccessModeStream);

            /* Set the pixel format to Mono8, where gray values will be output as 8 bit values for each pixel. */
            /* ... Check first to see if the device supports the Mono8 format. */
            isAvail = Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_PixelFormat_Mono8");

            if (!isAvail)
            {
                /* Feature is not available. */
                MessageBox.Show("设备不支持8位灰度图像");
            }

            /* ... Set the pixel format to Mono8. */
            Pylon.DeviceFeatureFromString(hDev, "PixelFormat", "Mono8");

            /* Disable acquisition start trigger if available. */
            isAvail = Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_TriggerSelector_AcquisitionStart");
            if (isAvail)
            {
                Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "AcquisitionStart");
                Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "Off");
            }


            /* Disable frame burst start trigger if available */
            isAvail = Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_TriggerSelector_FrameBurstStart");
            if (isAvail)
            {
                Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "FrameBurstStart");
                Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "Off");
            }

            /* Disable frame start trigger if available */
            isAvail = Pylon.DeviceFeatureIsAvailable(hDev, "EnumEntry_TriggerSelector_FrameStart");
            if (isAvail)
            {
                Pylon.DeviceFeatureFromString(hDev, "TriggerSelector", "FrameStart");
                Pylon.DeviceFeatureFromString(hDev, "TriggerMode", "Off");
            }

            /* For GigE cameras, we recommend increasing the packet size for better
            performance. If the network adapter supports jumbo frames, set the packet
            size to a value > 1500, e.g., to 8192. In this sample, we only set the packet size
            to 1500. */
            /* ... Check first to see if the GigE camera packet size parameter is supported and if it is writable. */
            isAvail = Pylon.DeviceFeatureIsWritable(hDev, "GevSCPSPacketSize");

            if (isAvail)
            {
                /* ... The device supports the packet size feature. Set a value. */
                Pylon.DeviceSetIntegerFeature(hDev, "GevSCPSPacketSize", 1500);
            }

        }


        //连续采集图像
        public override void ContinuousAcquisitonSoftTrigger()
        {

        }


        //单张采集图像
        public override void SnapAcquisitionSoftTrigger(ref byte[] ImageBufferPtr)
        {

                PylonGrabResult_t grabResult;

                /* Grab one single frame from stream channel 0. The
                camera is set to "single frame" acquisition mode.
                Wait up to 500 ms for the image to be grabbed.
                If imgBuf is null a buffer is automatically created with the right size.*/
                Pylon.DeviceGrabSingleFrame(hDev, 0, ref imgBuf, out grabResult, 500);
                IntPtr dataAddress = imgBuf.Pointer;

                /* Check to see if the image was grabbed successfully. */
                if (grabResult.Status == EPylonGrabStatus.Grabbed)
                {
                    Marshal.Copy(dataAddress, ImageBufferPtr, 0, 1280*1024-1);

                }
                else
                {
                    Console.WriteLine("图像抓取失败!n");
                }


        }

         //RGB转HObject
         public HObject RgbToHObject(int ImageWidth, int ImageHeight, ref byte[] ImageInPutBuffer, ref byte[] ImageOutBuffer)
        {

            HObject Image;
            byte[] R_OutBuffer = new byte[ImageWidth * ImageHeight];
            byte[] G_OutBuffer = new byte[ImageWidth * ImageHeight];
            byte[] B_OutBuffer = new byte[ImageWidth * ImageHeight];
            for (int i = 1; i < ImageHeight - 1; i++)
            {
                for (int j = 1; j < ImageWidth - 1; j++)
                {
                    int Index = (i * ImageWidth + j) * 3;


                    byte RGB_B = ImageInPutBuffer[Index + 0];
                    byte RGB_G = ImageInPutBuffer[Index + 1];
                    byte RGB_R = ImageInPutBuffer[Index + 2];


                    Index = (i * ImageWidth + j);
		    B_OutBuffer[Index+0 ]=RGB_B;
		    G_OutBuffer[Index+1 ]=RGB_G;
                    R_OutBuffer[Index+2 ]=RGB_R;

                }
            }

            //byte数组转IntPtr
            unsafe
            {
                IntPtr R,G,B;
                fixed (byte* R_Outptr = R_OutBuffer,G_Outptr = G_OutBuffer, B_Outptr = B_OutBuffer)
                {
                    HOperatorSet.GenImage3(out Image, "byte", ImageWidth, ImageHeight, new IntPtr(R_Outptr), new IntPtr(G_Outptr), new IntPtr(B_Outptr));
                }



            }


            return Image;

        }

        //停止采集图像
        public override void StopCameraSoftTrigger()
        {
            Pylon.DeviceClose(hDev);
            Pylon.DestroyDevice(hDev);

        }


        //关闭相机
        public override void CloseCameraSoftTrigger()
        {
            imgBuf.Dispose();
            imgBuf = null;
            Pylon.Terminate();
        }


        /**********************************************软触发模式结束***************************************************************/



        /**********************************************硬触发模式开始***************************************************************/

        //打开相机
        public override void OpenCameraHardTrigger(string DCF_Name)
        {

        

        }


        //信号触发采集图像
        public override void ContinuousAcquisitonHardTrigger()
        {


        }


        //停止采集图像
        public override void StopCameraHardTrigger()
        {

        }


        //关闭相机
        public override void CloseCameraHardTrigger()
        {


        }


        //允许硬件触发
        public override void EnableHardTrigger()
        {

        }


        //禁止硬件触发
        public override void DisableHardTrigger()
        {

        }


       /**********************************************硬触发模式结束***************************************************************/
    }
}
