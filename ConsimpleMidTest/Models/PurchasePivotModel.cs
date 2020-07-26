using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsimpleMidTest.Models
{
    public class PurchasePivotModel
    {
        public int Id { get; set; }
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Amount { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
    }
}
