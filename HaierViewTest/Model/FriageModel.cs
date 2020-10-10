using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace HaierViewTest.Model
{
    public class FridgeModel
    {
        [Key]
        public int ID { set; get; }
        public string ModelName { get; set; }

        public string CodeValue { set; get; }

        public string CodeLength { set; get; }


    }
}