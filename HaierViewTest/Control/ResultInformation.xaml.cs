using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core.Native;
using HaierViewTest.Annotations;

namespace HaierViewTest.Control
{
    /// <summary>
    /// ResultInformation.xaml 的交互逻辑
    /// </summary>
    public partial class ResultInformation : UserControl,INotifyPropertyChanged
    {
        public ResultInformation()
        {
            InitializeComponent();
        }


     

        private static Color ResultColor { set; get; }

        #region 冰箱条码

        public static readonly DependencyProperty FridgeCodeProperty = DependencyProperty.Register(
            "FridgeCode", typeof(string), typeof(ResultInformation), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 冰箱条码
        /// </summary>
        public string FridgeCode
        {
            get => (string) GetValue(FridgeCodeProperty);
            set => SetValue(FridgeCodeProperty, value);
        }

        #endregion
        public static readonly DependencyProperty FridgeModelProperty = DependencyProperty.Register(
            "FridgeModel", typeof(string), typeof(ResultInformation), new PropertyMetadata(string.Empty));

        public string FridgeModel
        {
            get => (string) GetValue(FridgeModelProperty);
            set => SetValue(FridgeModelProperty, value);
        }

        public static readonly DependencyProperty TestResultProperty = DependencyProperty.Register(
           "TestResult", typeof(bool), typeof(ResultInformation), new PropertyMetadata(false));

       public bool TestResult
       {
           get { return (bool) GetValue(TestResultProperty); }
           set { SetValue(TestResultProperty, value); }
       }

       public static readonly DependencyProperty TestTimeProperty = DependencyProperty.Register(
           "TestTime", typeof(string), typeof(ResultInformation), new PropertyMetadata(string.Empty));

       public string TestTime
       {
           get { return (string) GetValue(TestTimeProperty); }
           set { SetValue(TestTimeProperty, value); }
       }


       public event PropertyChangedEventHandler PropertyChanged;

       [NotifyPropertyChangedInvocator]
       protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
       {
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
       }
    }
}
