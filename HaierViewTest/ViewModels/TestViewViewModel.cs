using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DataModel.Models;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Xpf.Editors;
using HaierViewTest.Common;
using HaierViewTest.Interface;

namespace HaierViewTest.ViewModels
{
    [POCOViewModel]
    public class TestViewViewModel
    {
        public TestViewViewModel()
        {
            
            TestCommand=new AsyncCommand<string>(TestAction);

            StartTestCommand=new DelegateCommand(StartTest);


        }

        public virtual BitmapImage ShowImage { set; get; }

        private byte[] _imageBytes;
        public AsyncCommand<string> TestCommand { set; get; }

        public DelegateCommand StartTestCommand { set; get; }

        public virtual SolidColorBrush ConectState { set; get; }

        private async Task  TestAction(string actionName)
        {
            BindImage();
            await Task.Run((() =>
            {
                DealAction(actionName);


            }));




        }

        /// <summary>
        /// 开始测试
        /// </summary>
        private void StartTest()
        {
            
            if (ShowImage==null)
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

          Assembly assembly=   Assembly.LoadFrom("");
           


          IViewTest actionInstance = assembly.CreateInstance("") as IViewTest;
          if (actionInstance==null)
          {return;
          }
  
          actionInstance.StartAction();
          actionInstance.GetTestResult();
       var image=   actionInstance.Getimage();
       
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
    }
}