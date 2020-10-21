using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Models;

namespace DataModel
{
   public  class DataProvider
   {
       private static ViewTestEntities viewTestEntities;

       private static List<TestData> testDatas;

       static DataProvider()
       {
            viewTestEntities=new ViewTestEntities();
            testDatas=new List<TestData>();
       }


       public static List<TestData> GetTestDatas()
       {
            viewTestEntities.TestDatas.ToList().ForEach(p=>testDatas.Add(p));
            return testDatas;
       }


   }
}
