using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using CIAFactbook18App.Data;
using System.Threading;
using MongoDB.Bson;

namespace CIAFactbook18App.DataSeeding
{
    class Program
    {      
        static void Main(string[] args)
        {
            IMongoDatabase database = getCIAFactbook18DB();

            //SeedCountries(database);
            //ReadCountries(database);

            //SeedComparableFields(database);
            //ReadComparableFields(database);

            //SeedNotesandDefs(database);
            //ReadNotesandDefs(database);

            //SeedCountryDetails(database);
            //ReadCountryDetails(database);

            Console.WriteLine("Done!");
        }

        static IMongoDatabase getCIAFactbook18DB()
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
            return mongoClient.GetDatabase("ciafactbook18");
        }

        private static void SeedCountries(IMongoDatabase database)
        {
            var countries = DataSeed.Seed<List<Country>>(@"./seedData/countrieslist.json");          
            var countries_collection = database.GetCollection<Country>("countries");
            foreach(var country in countries)
            {
                Console.WriteLine(country.Name);
                countries_collection.InsertOne(country);                
                Thread.Sleep(100);
            }            
        }
        private static void ReadCountries(IMongoDatabase database)
        {
            var countries_collection = database.GetCollection<Country>("countries");
            foreach(var item in countries_collection.Find(new BsonDocument()).ToList())
            {
                Console.WriteLine($"{item.ID}:  {item.Name}");
                Thread.Sleep(100);
            }
        }

        private static void SeedComparableFields(IMongoDatabase database)
        {
            var comparablefields = DataSeed.Seed<List<ComparableFields>>(@"./seedData/comparablefields.json");
            var comparablefields_collection = database.GetCollection<ComparableFields>("comparablefields");
            foreach (var field in comparablefields)
            {
                Console.WriteLine(field.FieldName);
                comparablefields_collection.InsertOne(field);
                Thread.Sleep(100);
            }
        }
        private static void ReadComparableFields(IMongoDatabase database)
        {            
            var comparablefields_collection = database.GetCollection<ComparableFields>("comparablefields");
            foreach (var item in comparablefields_collection.Find(new BsonDocument()).ToList())
            {
                Console.WriteLine($"{item.ID}:  {item.FieldName}");
                Thread.Sleep(100);
            }
        }

        private static void SeedNotesandDefs(IMongoDatabase database)
        {
            var notesanddefs = DataSeed.Seed<Dictionary<string, string>>(@"./seedData/notesanddefs.json");
            var notesanddefs_collection = database.GetCollection<NotesAndDefs>("notesanddefs");
            foreach (KeyValuePair<string, string> kvp in notesanddefs)
            {
                notesanddefs_collection.InsertOne(new NotesAndDefs()
                {
                    FieldName = kvp.Key,
                    Definition = kvp.Value
                });
                Console.WriteLine(kvp.Key);
            }
        }
        private static void ReadNotesandDefs(IMongoDatabase database)
        {
            var notesanddefs_collection = database.GetCollection<NotesAndDefs>("notesanddefs");
            foreach (var item in notesanddefs_collection.Find(new BsonDocument()).ToList())
            {
                Console.WriteLine($"{item.ID}:  {item.FieldName}");
                Thread.Sleep(100);
            }
        }

        private static void SeedCountryDetails(IMongoDatabase database)
        {
            var countrydetails = DataSeed.Seed<Dictionary<string, List<ProfileEntity>>>(@"./seedData/countrydetails.json");
            var countrydetails_collection = database.GetCollection<CountryEntity>("countrydetails");
            foreach (KeyValuePair<string, List<ProfileEntity>> kvp in countrydetails)
            {
                Console.WriteLine($"Inserting {kvp.Key}");
                countrydetails_collection.InsertOne(new CountryEntity()
                {
                    CountryCode = kvp.Key,
                    CountryData = kvp.Value
                });
                Thread.Sleep(250);
            }
        }
        private static void ReadCountryDetails(IMongoDatabase database)
        {
            var countrydetails_collection = database.GetCollection<CountryEntity>("countrydetails");
            foreach (var item in countrydetails_collection.Find(new BsonDocument()).ToList())
            {
                Console.WriteLine($"{item.ID}:  {item.CountryCode}");
                Thread.Sleep(100);
            }
        }




    }
    class DataSeed
    {
        public static T Seed<T>(string localjsonpath)
        {
            string content = File.ReadAllText(localjsonpath);
            T desObj = JsonConvert.DeserializeObject<T>(content);
            return desObj;

        }
    }
}
