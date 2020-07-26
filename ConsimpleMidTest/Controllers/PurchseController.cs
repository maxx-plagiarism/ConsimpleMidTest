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
    [Route("api/Purchase")]
    [ApiController]
    public class PurchseController : ControllerBase
    {
        private readonly string ConnectionString = "Data Source=consimple.sqlite";

        // GET: api/<PurchseController>
        [HttpGet]
        public List<PurchaseModel> Get()
        {
            List<PurchaseModel> purchases = new List<PurchaseModel>();

            using(var connection = new SqliteConnection(ConnectionString))
            {
                if(connection != null)
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT id, purchase_date FROM purchase;";
                    
                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            purchases.Add(new PurchaseModel
                            {
                                Id = reader.GetInt32(0),
                                PurchaseDate = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", null)
                            });
                        }
                    }
                    foreach(var purchase in purchases)
                    {
                        purchase.GetAllItems();
                        purchase.Calculate();
                    }
                }
            }
            return purchases;
        }

        // GET api/<PurchseController>/5
        [HttpGet("{id}")]
        public PurchaseModel Get(int id)
        {
            PurchaseModel purchase = new PurchaseModel();

            using(var connection = new SqliteConnection(ConnectionString))
            {
                if(connection != null)
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT id, purchase_date FROM purchase WHERE id=@id;";
                    command.Parameters.AddWithValue("@id", id);
                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            purchase.Id = reader.GetInt32(0);
                            purchase.PurchaseDate = DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", null);
                        }
                        purchase.GetAllItems();
                        purchase.Calculate();
                    }
                }
            }
            return purchase;
        }

        // POST api/<PurchseController>
        [HttpPost]
        public void Post([FromBody] PurchaseModel purchase)
        {
            if(purchase != null)
            {
                using(var connection = new SqliteConnection(ConnectionString))
                {
                    if(connection != null)
                    {
                        connection.Open();

                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO purchase(purchase_date) VALUES(@purchase_date);";
                        command.Parameters.AddWithValue("@purchase_date", purchase.PurchaseDate.ToString("yyyy-MM-dd"));
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        // PUT api/<PurchseController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PurchseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [Route("/api/Purchase/Days/{days}")]
        [HttpGet]
        public List<PurchasePivotModel> GetLastPurchases(int days)
        {
            List<PurchasePivotModel> purchases = new List<PurchasePivotModel>();
            string now = DateTime.Now.ToString("yyyy-MM-dd");
            string before = DateTime.Now.AddDays(-days).ToString("yyyy-MM-dd");

            using(var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT client_id, client.full_name, purchase.purchase_date FROM purchase_pivot " +
                    "INNER JOIN client ON client_id=client.id " +
                    "INNER JOIN purchase ON purchase_id=purchase.id " +
                    "WHERE purchase_date >= @before AND purchase_date <= @now " +
                    "ORDER BY purchase.purchase_date DESC;";
                command.Parameters.AddWithValue("@before", before);
                command.Parameters.AddWithValue("@now", now);

                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        purchases.Add(new PurchasePivotModel
                        {
                            ClientId = reader.GetInt32(0),
                            ClientName = reader.GetString(1),
                            PurchaseDate = DateTime.ParseExact(reader.GetString(2), "yyyy-MM-dd", null)
                        });
                    }
                }
            }
            return purchases;
        }
    }
}
