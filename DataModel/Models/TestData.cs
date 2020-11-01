using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Models
{
    /// <summary>
    /// 冰箱测试结果实体类
    /// </summary>
   public  class TestData
   {
       public TestData()
       {

       }

       [Key]
       public int Id { get; set; }
       public string FridgeModel { set; get; }
       public string FridgeCode { set; get; }
       public bool TestResult { set; get; }
       public byte[] TestImage { get; set; }
       public DateTime DateTime { set; get; } = DateTime.Now;

   }
}
