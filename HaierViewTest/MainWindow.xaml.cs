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
using HaierViewTest.Views;

namespace HaierViewTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestView testView;
        public MainWindow()
        {
            InitializeComponent();
             testView=new TestView();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
           
            HamburgerMenu.Content = testView;

        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
