using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsimpleMidTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConsimpleMidTest.Controllers
{
    [Route("api/Item")]
    [ApiController]
    public class PurchesePivotController : ControllerBase
    {
        private readonly string ConnectionString = "Data Source=consimple.sqlite";
        // GET: api/<PurchesePivotController>
        [HttpGet]
        public List<PurchasePivotModel> Get()
        {
            List<PurchasePivotModel> items = null;
            using(var connection = new SqliteConnection(ConnectionString))
            {
                if(connection != null)
                {
                    connection.Open();
                    items = new List<PurchasePivotModel>();

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT purchase_pivot.id, amount, purchase.purchase_date, product.title, product.price " +
                        "FROM purchase_pivot " +
                        "INNER JOIN purchase ON purchase_id=purchase.id " +
                        "INNER JOIN product ON product_id=product.id;";
                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new PurchasePivotModel
                            { 
                                Id = reader.GetInt32(0),
                                Amount = reader.GetInt32(1),
                                PurchaseDate = DateTime.ParseExact(reader.GetString(2), "yyyy-MM-dd", null),
                                Title = reader.GetString(3),
                                Price = reader.GetDecimal(4)
                            });
                        }
                    }
                }
            }
            return items;
        }

        // GET api/<PurchesePivotController>/5
        [HttpGet("{id}")]
        public PurchasePivotModel Get(int id)
        {
            PurchasePivotModel item = null;
            using(var connection = new SqliteConnection(ConnectionString))
            {
                if(connection != null)
                {
                    connection.Open();
                    item = new PurchasePivotModel();

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT purchase_pivot.id, amount, purchase.purchase_date, product.title, product.price " +
                        "FROM purchase_pivot " +
                        "INNER JOIN purchase ON purchase_id=purchase.id " +
                        "INNER JOIN product ON product_id=product.id " +
                        "WHERE purchase_pivot.id=@id;";
                    command.Parameters.AddWithValue("@id", id);
                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item.Id = reader.GetInt32(0);
                            item.Amount = reader.GetInt32(1);
                            item.PurchaseDate = DateTime.ParseExact(reader.GetString(2), "yyyy-MM-dd", null);
                            item.Title = reader.GetString(3);
                            item.Price = reader.GetDecimal(4);
                        }
                    }
                }
            }
            return item;
        }

        // POST api/<PurchesePivotController>
        [HttpPost]
        public void Post([FromBody] PurchasePivotModel item)
        {
            if(item != null)
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    if(connection != null)
                    {
                        connection.Open();

                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO purchase_pivot(purchase_id, product_id, amount) VALUES(@purchase_id, @product_id, @amount);";
                        command.Parameters.AddWithValue("@purchase_id", item.PurchaseId);
                        command.Parameters.AddWithValue("@product_id", item.ProductId);
                        command.Parameters.AddWithValue("@amount", item.Amount);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        // PUT api/<PurchesePivotController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PurchesePivotController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
