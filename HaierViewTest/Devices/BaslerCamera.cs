using Basler.Pylon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using HalconDotNet;
using Camera = Basler.Pylon.Camera;
using MessageBox = System.Windows.Forms.MessageBox;

namespace HaierViewTest.Devices
{
   public class BaslerCamera
    {
        public Basler.Pylon.Camera camera = null;
        private PixelDataConverter converter = new PixelDataConverter();
        private string strUserID = null;

        public long imageWidth = 0;         // 图像宽
        public long imageHeight = 0;        // 图像高
        public long minExposureTime = 0;    // 最小曝光时间
        public long maxExposureTime = 0;    // 最大曝光时间
        public long minGain = 0;            // 最小增益
        public long maxGain = 0;            // 最大增益
        private long grabTime = 0;          // 采集图像时间

        private HObject hPylonImage = null;
        private IntPtr latestFrameAddress = IntPtr.Zero;
        private Stopwatch stopWatch = new Stopwatch();

        /// <summary>
        /// 计算采集图像时间自定义委托
        /// </summary>
        /// <param name="time">采集图像时间</param>
        public delegate void delegateComputeGrabTime(long time);

        /// <summary>
        /// 计算采集图像时间委托事件
        /// </summary>
        public event delegateComputeGrabTime eventComputeGrabTime;

        /// <summary>
        /// 图像处理自定义委托
        /// </summary>
        /// <param name="hImage">halcon图像变量</param>
        public delegate void delegateProcessHImage(HObject hImage);
        /// <summary>
        /// 图像处理委托事件
        /// </summary>
        public event delegateProcessHImage eventProcessImage;
        /// <summary>
        /// if >= Sfnc2_0_0,说明是ＵＳＢ３的相机
        /// </summary>
        static Version Sfnc2_0_0 = new Version(2, 0, 0);
        /******************    实例化相机    ******************/
        /// <summary>
        /// 实例化第一个找到的相机
        /// </summary>
        public BaslerCamera()
        {
            try
            {
                camera = new Camera();
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }

        /// <summary>
        /// 根据相机UserID实例化相机
        /// </summary>
        /// <param name="UserID"></param>
        public BaslerCamera(string UserID)
        {
            try
            {
                strUserID = UserID;
                // 枚举相机列表
                List<ICameraInfo> allCameraInfos = CameraFinder.Enumerate();

                foreach (ICameraInfo cameraInfo in allCameraInfos)
                {
                    if (strUserID == cameraInfo[CameraInfoKey.UserDefinedName])
                    {
                        camera = new Camera(cameraInfo);
                    }
                }

                if (camera == null)
                {
                    MessageBox.Show("未识别到UserID为“" + strUserID + "”的相机！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        //清除图像处理委托事件绑定的方法
        public void Clear_EventProcessImage_Event()
        {
            if (eventProcessImage != null)
            {

                Delegate[] dels = eventProcessImage.GetInvocationList();
                foreach (Delegate d in dels)
                {
                    //得到方法名
                    object delObj = d.GetType().GetProperty("Method").GetValue(d, null);
                    string funcName = (string)delObj.GetType().GetProperty("Name").GetValue(delObj, null);
                    Debug.Print(funcName);
                    eventProcessImage -= d as delegateProcessHImage;
                }
            }
        }
        /*****************************************************/

        /******************    相机操作     ******************/
        /// <summary>
        /// 打开相机
        /// </summary>
        public bool OpenCam()
        {
            try
            {
                camera.Open();
                imageWidth = camera.Parameters[PLCamera.Width].GetValue();               // 获取图像宽 
                imageHeight = camera.Parameters[PLCamera.Height].GetValue();              // 获取图像高
                GetMinMaxExposureTime();
                GetMinMaxGain();
                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;                      // 注册采集回调函数
                camera.ConnectionLost += OnConnectionLost;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 关闭相机,释放相关资源
        /// </summary>
        public void CloseCam()
        {
            try
            {
                camera.Close();
                camera.Dispose();

                if (hPylonImage != null)
                {
                    hPylonImage.Dispose();
                }

                if (latestFrameAddress != null)
                {
                    Marshal.FreeHGlobal(latestFrameAddress);
                    latestFrameAddress = IntPtr.Zero;
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /// <summary>
        /// 相机是否忙碌中
        /// </summary>
        public bool isBusy()
        {
            return camera.StreamGrabber.IsGrabbing;
        }

        /// <summary>
        /// 单张采集
        /// </summary>
        public bool GrabOne()
        {
            try
            {
                if (camera.StreamGrabber.IsGrabbing)
                {
                    MessageBox.Show("Camera is now Continue Grabing Images！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    camera.Parameters[PLCamera.AcquisitionMode].SetValue("SingleFrame");
                    camera.StreamGrabber.Start(1, GrabStrategy.LatestImages, GrabLoop.ProvidedByStreamGrabber);
                    stopWatch.Restart();
                    return true;
                }
            }
            catch (Exception e)
            {
                ShowException(e);
                return false;
            }
        }

        /// <summary>
        /// 开始连续采集
        /// </summary>
        public bool StartGrabbing()
        {
            try
            {
                if (camera.StreamGrabber.IsGrabbing)
                {
                    MessageBox.Show("Camera is now Continue Grabing Images", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                    camera.StreamGrabber.Start(GrabStrategy.LatestImages, GrabLoop.ProvidedByStreamGrabber);
                    stopWatch.Restart();
                    return true;
                }
            }
            catch (Exception e)
            {
                ShowException(e);
                return false;
            }
        }

        /// <summary>
        /// 停止连续采集
        /// </summary>
        public void StopGrabbing()
        {
            try
            {
                if (camera.StreamGrabber.IsGrabbing)
                {
                    camera.StreamGrabber.Stop();
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /*********************************************************/

        /******************    相机参数设置   ********************/
        /// <summary>
        /// 设置Gige相机心跳时间
        /// </summary>
        /// <param name="value"></param>
        public void SetHeartBeatTime(long value)
        {
            try
            {
                // 判断是否是网口相机
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    camera.Parameters[PLGigECamera.GevHeartbeatTimeout].SetValue(value);
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }

        /// <summary>
        /// 设置相机曝光时间
        /// </summary>
        /// <param name="value"></param>
        public void SetExposureTime(long value)
        {
            try
            {
                //要手动设置曝光时间 需要把自动曝光参数设置为false  把相机曝光模式设置为延时曝光模式
                camera.Parameters[PLCamera.ExposureAuto].TrySetValue(PLCamera.ExposureAuto.Off);
                camera.Parameters[PLCamera.ExposureMode].TrySetValue(PLCamera.ExposureMode.Timed);

                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    long min = camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
                    long max = camera.Parameters[PLCamera.ExposureTimeRaw].GetMaximum();
                    long incr = camera.Parameters[PLCamera.ExposureTimeRaw].GetIncrement();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    else
                    {
                        value = min + (((value - min) / incr) * incr);
                    }
                    camera.Parameters[PLCamera.ExposureTimeRaw].SetValue(value);
                }
                else
                {
                    camera.Parameters[PLUsbCamera.ExposureTime].SetValue((double)value);
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }

        /// <summary>
        /// 获取最小最大曝光时间
        /// </summary>
        public void GetMinMaxExposureTime()
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    minExposureTime = camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
                    maxExposureTime = camera.Parameters[PLCamera.ExposureTimeRaw].GetMaximum();
                }
                else
                {
                    minExposureTime = (long)camera.Parameters[PLUsbCamera.ExposureTime].GetMinimum();
                    maxExposureTime = (long)camera.Parameters[PLUsbCamera.ExposureTime].GetMaximum();
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="value"></param>
        public void SetGain(long value)
        {
            try
            {
                camera.Parameters[PLCamera.GainAuto].TrySetValue(PLCamera.GainAuto.Off);

                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    long min = camera.Parameters[PLCamera.GainRaw].GetMinimum();
                    long max = camera.Parameters[PLCamera.GainRaw].GetMaximum();
                    long incr = camera.Parameters[PLCamera.GainRaw].GetIncrement();
                    if (value < min)
                    {
                        value = min;
                    }
                    else if (value > max)
                    {
                        value = max;
                    }
                    else
                    {
                        value = min + (((value - min) / incr) * incr);
                    }
                    camera.Parameters[PLCamera.GainRaw].SetValue(value);
                }
                else
                {
                    camera.Parameters[PLUsbCamera.Gain].SetValue(value);
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }

        /// <summary>
        /// 获取最小最大增益
        /// </summary>
        public void GetMinMaxGain()
        {
            try
            {
                if (camera.GetSfncVersion() < Sfnc2_0_0)
                {
                    minGain = camera.Parameters[PLCamera.GainRaw].GetMinimum();
                    maxGain = camera.Parameters[PLCamera.GainRaw].GetMaximum();
                }
                else
                {
                    minGain = (long)camera.Parameters[PLUsbCamera.Gain].GetMinimum();
                    maxGain = (long)camera.Parameters[PLUsbCamera.Gain].GetMaximum();
                }
            }
            catch (Exception e)
            {
                ShowException(e);
            }
        }
        /****************************************************/


        /****************  图像响应事件函数  ****************/


        // 相机取像回调函数.
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            try
            {
                IGrabResult grabResult = e.GrabResult;
                if (grabResult.GrabSucceeded)
                {
                    grabTime = stopWatch.ElapsedMilliseconds;
                    {
                        if (latestFrameAddress == IntPtr.Zero)
                        {
                            latestFrameAddress = Marshal.AllocHGlobal((Int32)grabResult.PayloadSize);
                        }
                        converter.OutputPixelFormat = PixelType.Mono8;
                        converter.Convert(latestFrameAddress, grabResult.PayloadSize, grabResult);
                        // 转换为Halcon图像显示
                        HOperatorSet.GenImage1(out hPylonImage, "byte", (HTuple)grabResult.Width, (HTuple)grabResult.Height, (HTuple)latestFrameAddress);
                        // 抛出图像处理事件
                        eventProcessImage(hPylonImage);
                        hPylonImage.Dispose();
                    }
                }
                else
                {
                    MessageBox.Show("Grab faild!\n" + grabResult.ErrorDescription, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
            finally
            {
                e.DisposeGrabResultIfClone();
            }
        }

        /****************************************************/


        /// <summary>
        /// 掉线重连回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectionLost(Object sender, EventArgs e)
        {
            try
            {
                camera.Close();

                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                        camera.Open();
                        if (camera.IsOpen)
                        {
                            MessageBox.Show("已重新连接上UserID为“" + strUserID + "”的相机！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                        Thread.Sleep(200);
                    }
                    catch
                    {
                        MessageBox.Show("请重新连接UserID为“" + strUserID + "”的相机！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                if (camera == null)
                {
                    MessageBox.Show("重连超时20s:未识别到UserID为“" + strUserID + "”的相机！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                SetHeartBeatTime(5000);
                imageWidth = camera.Parameters[PLCamera.Width].GetValue();               // 获取图像宽 
                imageHeight = camera.Parameters[PLCamera.Height].GetValue();              // 获取图像高
                GetMinMaxExposureTime();
                GetMinMaxGain();
                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;                      // 注册采集回调函数
                camera.ConnectionLost += OnConnectionLost;

            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }
        private void ShowException(Exception exception)
        {
            MessageBox.Show("Exception caught:\n" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
