using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using DataModel;
using DataModel.Models;
using DevExpress.DataProcessing;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using DevExpress.Xpf.Core.ConditionalFormattingManager;
using HaierViewTest.Common;

namespace HaierViewTest.ViewModels
{
    [POCOViewModel]
    public class QueryViewModel
    {
      public  QueryViewModel()
        {
            QueryCommand=new DelegateCommand(Query);
           
            SelectionChangedCommand=new DelegateCommand(SelectValue);
            TestDataCollection = App.ViewTestEntities.TestDatas.ToList().ToObservableCollection();

        }
        public virtual ObservableCollection<TestData> TestDataCollection { set; get; }

       public virtual TestData SelectedTestData { set; get; }

        public virtual bool Result { set; get; }
        
        public virtual string Model { set; get; } 

       public virtual string BarCode { set;get; }
       public virtual string StartTime { set; get; }

       public virtual BitmapImage ShowImage { set; get; }

       private DelegateCommand _queryCommand;
       public DelegateCommand QueryCommand { set; get; }

       public DelegateCommand SelectionChangedCommand { set; get; }

       public void Query()
       {
           TestDataCollection =App.ViewTestEntities.TestDatas.ToObservableCollection();
        }


       public void SelectValue()
       {
           if (SelectedTestData==null)
           {
               return;
           }

           if (SelectedTestData.TestImage==null)
           {
               return;
           }
           Result = SelectedTestData.TestResult;
           StartTime = SelectedTestData.DateTime.ToString("yyyy-MM-dd hh:mm:ss");
           BarCode = SelectedTestData.FridgeCode;
           Model = SelectedTestData.FridgeModel;

           BitmapImage newBitmapImage = new BitmapImage();

           newBitmapImage.BeginInit();

           newBitmapImage.StreamSource = new System.IO.MemoryStream(SelectedTestData.TestImage);

            newBitmapImage.EndInit();
            newBitmapImage.Freeze();
           ShowImage = newBitmapImage;
          
       }


    }
}