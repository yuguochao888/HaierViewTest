using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DataModel.Models;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using HaierViewTest.Common;
using HaierViewTest.Devices;
using HaierViewTest.Interface;
using HalconDotNet;

namespace HaierViewTest.ViewModels
{
    [POCOViewModel]
    public class TestViewViewModel
    {
      


        public TestViewViewModel()
        {
            TestCommand = new AsyncCommand<string>(TestAction);

            StartTestCommand = new DelegateCommand(StartTest);

            OpenCameraCommand=new DelegateCommand(OpenCamera);

            CloseCameraCommand=new DelegateCommand(CloseCamera);
            
        }

        /// <summary>
        /// 初始化相机
        /// </summary>
        private void InitCamera()
        {
            _baslerCamera = new BaslerCamera();
            _baslerCamera.eventProcessImage += _baslerCamera_eventProcessImage;
            Timer timer=new Timer();
            timer.Interval = 1000;
            timer.Tick -= Timer_Tick;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CameraState = _baslerCamera.camera.IsConnected;
        }

        #region Property



        /// <summary>
        /// 相机连接状态
        /// </summary>
        public virtual bool CameraState { set; get; } = false;

        public virtual BitmapImage ShowImage { set; get; }

        #endregion

        private byte[] _imageBytes;

        #region Command


        public AsyncCommand<string> TestCommand { set; get; }

        public DelegateCommand StartTestCommand { set; get; }

      
        #endregion
        public virtual SolidColorBrush ConectState { set; get; }

        private async Task TestAction(string actionName)
        {
            BindImage();
            await Task.Run((() =>
            {
                DealAction(actionName);


            }));

        }


        #region Action

        /// <summary>
        /// 开始测试
        /// </summary>
        private void StartTest()
        {

            if (ShowImage == null)
            {
                return;
            }

            BitmapImage image = ShowImage.Clone();

            //ShowImage.StreamSource.Write(byteList,0,(int)ShowImage.StreamSource.Length);


        }

        /// <summary>
        /// 打开本地相机显示图片
        /// </summary>
        private void BindImage()
        {
            var image = Global.OpenImageFile();
            if (image != null)
            {
                ShowImage = image.Clone();
            }

        }


        private void DealAction(string AcitonName)
        {

            Assembly assembly = Assembly.LoadFrom("");

            var type = assembly.GetType();

            if (!(Activator.CreateInstance(type) is IViewTest actionInstance))
            {
                return;
            }

            actionInstance.StartAction(out string resultMessage,out HObject hObject);
            actionInstance.GetTestResult();
            var image = actionInstance.Getimage();

        }

        private void SaveImage()
        {
            byte[] bytes = Global.BitmapImageToByteArray(ShowImage);
            App.ViewTestEntities.TestDatas.Add(new TestData()
            {
                DateTime = DateTime.Now,
                FridgeCode = "123123123123",
                FridgeModel = "BCD-123",
                TestImage = bytes,
                TestResult = true
            }
            );
            App.ViewTestEntities.SaveChanges();
        }

        #endregion

        #region 相机

        public DelegateCommand OpenCameraCommand { set; get; }

        public DelegateCommand CloseCameraCommand { set; get; }

        private BaslerCamera _baslerCamera;
        private void OpenCamera()
        {
            if (_baslerCamera==null)
            {
                InitCamera();
            }
           
            CameraState = _baslerCamera.OpenCam();
        }

        private void _baslerCamera_eventProcessImage(HalconDotNet.HObject hImage)
        {
            ImageHelper.GenertateRGBBitmap(hImage,out Bitmap bitmap);
            ShowImage = ImageHelper.BitmapToBitmapImage(bitmap);
        }

        private void CloseCamera()
        {
            _baslerCamera.CloseCam();
        }
        #endregion
    }
}