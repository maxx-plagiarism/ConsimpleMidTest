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
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly string ConnectionString = "Data Source=consimple.sqlite";
        // GET: api/<ProductController>
        [HttpGet]
        public List<ProductModel> Get()
        {
            List<ProductModel> products = new List<ProductModel>();

            using(var connection = new SqliteConnection(ConnectionString))
            {
                if(connection != null)
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT id, title, category, code, price FROM product;";
                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProductModel
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Category = reader.GetInt32(2),
                                Code = reader.GetInt32(3),
                                Price = reader.GetDecimal(4)
                            });
                        }
                    }
                }
            }
            return products;
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public ProductModel Get(int id)
        {
            ProductModel product = null;

            using(var connection = new SqliteConnection(ConnectionString))
            {
                if(connection != null)
                {
                    connection.Open();
                    
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT id, title, category, code, price FROM product WHERE id=@id;";
                    command.Parameters.AddWithValue("@id", id);
                    product = new ProductModel();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            product.Id = reader.GetInt32(0);
                            product.Title = reader.GetString(1);
                            product.Category = reader.GetInt32(2);
                            product.Code = reader.GetInt32(3);
                            product.Price = reader.GetDecimal(4);
                        }
                    }
                }
            }
            return product;
        }

        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] ProductModel product)
        {
            if(product != null)
            {
                using (var connection = new SqliteConnection(ConnectionString))
                {
                    if(connection != null)
                    {
                        connection.Open();

                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO product(title, category, code, price) VALUES(@title, @category, @code, @price);";
                        command.Parameters.AddWithValue("@title", product.Title);
                        command.Parameters.AddWithValue("@category", product.Category);
                        command.Parameters.AddWithValue("@code", product.Code);
                        command.Parameters.AddWithValue("@price", product.Price);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
