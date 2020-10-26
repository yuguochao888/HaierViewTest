using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataModel.Models;
using HaierViewTest.Common;
using Image = System.Drawing.Image;

namespace HaierViewTest.Views
{
    /// <summary>
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class TestView : UserControl
    {
        public TestView()
        {
            InitializeComponent();
            Initialize();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           



            logBox.AppendText("开始测试-------"+ DateTime.Now+Environment.NewLine);

           var image= Image.FromFile(@"...\\...\\Image\\testpicture2.jpg");
         byte[]  saveBytes=  Global.ImageToBytes(image);
         App.ViewTestEntities.TestDatas.Add(new TestData()
             {
             DateTime = DateTime.Now,
             FridgeCode = "123123123123",
             FridgeModel = "BCD-123",
             TestImage = saveBytes,
             TestResult = true

         }
         );
         App.ViewTestEntities.SaveChanges();

            NGSet();
        }


        private void NGSet()
        {
            ResultBorder.Background = new SolidColorBrush(Colors.Red);
            ResultText.Text = "不合格";
        }

        private void OKSet()
        {

            ResultBorder.Background = new SolidColorBrush(Colors.Green);
            ResultText.Text = "合格";
        }

        private void Initialize()
        {
            
        }
    }
}
