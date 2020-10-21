using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.Models;

namespace DataModel
{
   public class ViewTestEntities : DbContext
    {
        public ViewTestEntities() : base("name=HaierViewTest")
        {

        }

        public DbSet<TestData> TestDatas { get; set; }
        

    }
}
