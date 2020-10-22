using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core.Native;

namespace HaierViewTest.Control
{
    /// <summary>
    /// ResultInformation.xaml 的交互逻辑
    /// </summary>
    public partial class ResultInformation : UserControl
    {
        public ResultInformation()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FridgeCodeProperty = DependencyProperty.Register(
            "FridgeCode", typeof(string), typeof(ResultInformation), new PropertyMetadata(default(string)));

        public string FridgeCode
        {
            get => (string) GetValue(FridgeCodeProperty);
            set => SetValue(FridgeCodeProperty, value);
        }

        public static readonly DependencyProperty FridgeModelProperty = DependencyProperty.Register(
            "FridgeModel", typeof(string), typeof(ResultInformation), new PropertyMetadata(default(string), ));

        public string FridgeModel
        {
            get => (string) GetValue(FridgeModelProperty);
            set => SetValue(FridgeModelProperty, value);
        }

       protected PropertyChangedCallback PropertyChangedCallback()
        {

        }
    }
}
