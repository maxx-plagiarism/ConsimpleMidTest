using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsimpleMidTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var connection = new SqliteConnection("Data Source=consimple.sqlite"))
            {
                if (connection != null)
                {
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS client(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        full_name TEXT,
                        birthday TEXT,
                        registration_date TEXT
                    );";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS product(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        title TEXT,
                        category INTEGER,
                        code INTEGER,
                        price REAL
                    );";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS purchase(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        purchase_date TEXT
                    );";
                    command.ExecuteNonQuery();

                    command.CommandText =
                        @"CREATE TABLE IF NOT EXISTS purchase_pivot(
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        purchase_id INTEGER,
                        product_id INTEGER,
                        amount INTEGER
                    );";
                    command.ExecuteNonQuery();
                }
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
