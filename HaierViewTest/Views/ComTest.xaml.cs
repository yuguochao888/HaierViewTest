using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace HaierViewTest.Views
{
    /// <summary>
    /// ComTest.xaml 的交互逻辑
    /// </summary>
    public partial class ComTest : Window
    {
        public ComTest()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
          if( SerialPort.GetPortNames().Length>0)
            Com_combox.ItemsSource=(SerialPort.GetPortNames());
        }
    }
}
