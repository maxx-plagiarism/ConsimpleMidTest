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
    [Route("api/Client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly string ConnectionString = "Data Source=consimple.sqlite";
        // GET: api/<ClientController>
        [HttpGet]
        public List<ClientModel> Get()
        {
            List<ClientModel> clients = new List<ClientModel>();

            using (var connection = new SqliteConnection(ConnectionString))
            {
                if(connection != null)
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = @"SELECT id, full_name, birthday, registration_date FROM client;";

                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new ClientModel {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Birthday = DateTime.ParseExact(reader.GetString(2), "yyyy-MM-dd", null),
                                RegistrationDate = DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd", null)
                            });
                        }
                    }
                }
            }
            
            return clients;
        }

        // GET api/<ClientController>/5
        [HttpGet("{id}")]
        public ClientModel Get(int id)
        {
            ClientModel client = new ClientModel();

            using(var connection = new SqliteConnection(ConnectionString))
            {
                if(connection != null)
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = @"SELECT id, full_name, birthday, registration_date FROM client WHERE id=@id;";
                    command.Parameters.AddWithValue("@id", id);
                    command.Prepare();

                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            client.Id = reader.GetInt32(0);
                            client.FullName = reader.GetString(1);
                            client.Birthday = DateTime.ParseExact(reader.GetString(2), "yyyy-MM-dd", null);
                            client.RegistrationDate = DateTime.ParseExact(reader.GetString(3), "yyyy-MM-dd", null);
                        }
                    }
                }
            }
            return client;
        }

        // POST api/<ClientController>
        [HttpPost]
        public void Post([FromBody]ClientModel client)
        {
            if(client != null)
            {
                using(var connection = new SqliteConnection(ConnectionString))
                {
                    if(connection != null)
                    {
                        connection.Open();
                        
                        var command = connection.CreateCommand();
                        command.CommandText = "INSERT INTO client(full_name, birthday, registration_date) VALUES(@full_name, @birthday, @registration_date);";
                        command.Parameters.AddWithValue("@full_name", client.FullName);
                        command.Parameters.AddWithValue("@birthday", client.Birthday.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@registration_date", client.RegistrationDate.ToString("yyyy-MM-dd"));
                        // command.Prepare();
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        // PUT api/<ClientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
