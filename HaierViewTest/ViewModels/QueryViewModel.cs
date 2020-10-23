using System;
using System.Collections.ObjectModel;
using System.Linq;
using DataModel;
using DataModel.Models;
using DevExpress.DataProcessing;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;

namespace HaierViewTest.ViewModels
{
    [POCOViewModel]
    public class QueryViewModel
    {
        public ObservableCollection<TestData> TestDatascCollection=new ObservableCollection<TestData>();



        public virtual bool Result { set; get; } = true;

        public virtual string FridgeModel { set; get; } = "BCD_12345";

       public virtual string FridgeCode { set;get; } = "123456789";
       public DateTime StartTime=DateTime.Now;

       DelegateCommand _SaveCommand;
       public DelegateCommand QueryCommand
        {
           get
           {
               return _SaveCommand ??
                      (_SaveCommand = new DelegateCommand(Query));
           }
       }


       public void Query()
       {
           Result = !Result;

       }



    }
}