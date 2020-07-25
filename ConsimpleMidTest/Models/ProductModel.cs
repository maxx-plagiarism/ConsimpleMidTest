using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsimpleMidTest.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Category { get; set; }
        public int Code { get; set; }
        public decimal Price { get; set; }
    }
}
