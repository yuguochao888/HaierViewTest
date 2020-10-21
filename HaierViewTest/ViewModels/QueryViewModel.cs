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

        QueryViewModel()
        {
            TestDatascCollection.AddRange(DataProvider.GetTestDatas().Where(x=>x.DateTime));

        }


    }
}