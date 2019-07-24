using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace CIAFactbook18App.DataSeeding
{
    class Program
    {      
        static void Main(string[] args)
        {
            //secrets.json contains connections strings for cosmos db and is not in source control
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            string connectionString = configuration["CosmosDB"];
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );

            settings.SslSettings = new SslSettings() { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);

            var dbs = mongoClient.ListDatabaseNames().ToList();
            foreach(var db in dbs)
                Console.WriteLine(db);



            Console.WriteLine("Done!");
        }






        static T GetListFromFile<T>(string filepath) =>
            JsonConvert.DeserializeObject<T>(File.ReadAllText(filepath));




    }
}
