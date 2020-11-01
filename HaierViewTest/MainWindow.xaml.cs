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
using DataModel;
using DataModel.Models;
using HaierViewTest.Views;

namespace HaierViewTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestView testView;
        private ComTest ComTest;
        private Query QueryView;
        public MainWindow()
        {
            using (var test=new ViewTestEntities())
            {
                //TestData data=new TestData();
                //test.TestDatas.Add(data);
                //test.SaveChanges();
            }


            InitializeComponent();
             testView=new TestView();
             ComTest=new ComTest();
             QueryView=new Query();
             HamburgerMenu.Content = testView;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
           
            HamburgerMenu.Content = testView;

        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            HamburgerMenu.Content = ComTest;
        }

        private void QueryButton_OnClick(object sender, RoutedEventArgs e)
        {
            HamburgerMenu.Content = QueryView;
        }
    }
}
