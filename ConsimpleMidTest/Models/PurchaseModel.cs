using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsimpleMidTest.Models
{
    public class PurchaseModel
    {
        private readonly string ConnectionString = "Data Source=consimple.sqlite";
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalCost { get; set; }
        public List<PurchasePivotModel> Items { get; set; } = new List<PurchasePivotModel>();
        
        public void Calculate()
        {
            TotalCost = 0.0M;
            foreach(var item in Items)
            {
                TotalCost += item.Price * item.Amount;
            }
        }
        public void GetAllItems()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                if(connection != null)
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT purchase_pivot.id, amount, purchase.purchase_date, product.title, product.price, client.full_name " +
                        "FROM purchase_pivot " +
                        "INNER JOIN purchase ON purchase_id=purchase.id " +
                        "INNER JOIN product ON product_id=product.id " +
                        "INNER JOIN client ON client_id=client.id " +
                        "WHERE purchase_pivot.purchase_id=@id;";
                    command.Parameters.AddWithValue("@id", Id);

                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Items.Add(new PurchasePivotModel
                            {
                                Id = reader.GetInt32(0),
                                Amount = reader.GetInt32(1),
                                PurchaseDate = DateTime.ParseExact(reader.GetString(2), "yyyy-MM-dd", null),
                                Title = reader.GetString(3),
                                Price = reader.GetDecimal(4),
                                ClientName = reader.GetString(5)
                            });
                        }
                    }
                }
            }
        }
    }
}
