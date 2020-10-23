using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using DataModel;
using DataModel.Models;
using DevExpress.DataProcessing;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core.ConditionalFormattingManager;

namespace HaierViewTest.ViewModels
{
    [POCOViewModel]
    public class QueryViewModel
    {
        public ObservableCollection<TestData> TestDatascCollection=new ObservableCollection<TestData>();

      

        public virtual bool Result { set; get; }
        
        public virtual string Model { set; get; } 

       public virtual string BarCode { set;get; }
       public virtual string StartTime { set; get; }

       DelegateCommand _queryCommand;
       public DelegateCommand QueryCommand
        {
           get
           {
               return _queryCommand ??
                      (_queryCommand = new DelegateCommand(Query));
           }
       }


       public void Query()
       {
           Result = !Result;
           StartTime = "1222222222222222";
           BarCode = DateTime.Now.ToString();
           Model = "BCD555555";
       }



    }
}