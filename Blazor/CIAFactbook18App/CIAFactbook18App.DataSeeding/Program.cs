using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using CIAFactbook18App.Data;
using System.Threading;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace CIAFactbook18App.DataSeeding
{
    class Program
    {      
        static void Main(string[] args)
        {
            Console.WriteLine("Done!");
        }

        #region Testing cosmos db datacontext

        private static void TestingFactbook18CosmosDbContext()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            string connectionString = configuration["CosmosDB"];
            Factbook18CosmosDbContext context = new Factbook18CosmosDbContext(connectionString);

            Console.WriteLine("Context created ...");

            //Console.WriteLine("Listing countries.........");
            //foreach (var item in context.Countries)
            //    Console.WriteLine(item.Name);

            //Console.WriteLine("Loading coparable fields...");
            //foreach (var item in context.ComparableFields)
            //    Console.WriteLine(item.FieldName);

            //Console.WriteLine("Loading notes and defs...");
            //foreach (var item in context.NotesAndDefs)
            //    Console.WriteLine(item.FieldName);

            Console.WriteLine("Testing GetCountryDetails function ...");
            string CountryCode = "SV";
            CountryEntity c = context.GetCountryDetails(CountryCode);
            if (c!=null)
                Console.WriteLine(c.CountryData[0].Children[0].Value);

        }

        #endregion Testing cosmos db datacontext

        #region Code for seeding json data to cosmos db and anthem files to data storage

        /// <summary>
        /// Seed data in JSON files to COSMOSDB using MongoDB driver
        /// </summary>
        private static void SeedCosmosDB()
        {
            #region Seeding JSON data to cosmos db

            IMongoDatabase database = getCIAFactbook18DB();
            string collectionname = "factbook18context";


            //Commented to prevent re-seeding

            //SeedCountries(database,collectionname);
            //SeedComparableFields(database, collectionname);
            //SeedNotesandDefs(database, collectionname);
            //SeedCountryDetails(database, collectionname);

            //ReadCountries(database, collectionname);
            //ReadComparableFields(database, collectionname);
            //ReadNotesandDefs(database, collectionname);
            //ReadCountryDetails(database, collectionname);

            #endregion Seeding JSON data to cosmos db
        }
        /// <summary>
        /// Seed the national anthem files to storage
        /// </summary>
        static void SeedAnthemFiles()
        {
            #region Saving national anthem audio files to data store

            //Commented to prevent re-seeding of storage account


            //AzureBlbStorageService blbStorage = getStorageService();
            //string containername = "anthemscontainer";
            //string directory = @"C:\Users\sekha\Desktop\audios\original";            
            //UploadAnthemFiles(blbStorage, containername, directory);

            #endregion Saving national anthem audio files to data store
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
        static AzureBlbStorageService getStorageService()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("secrets.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            string connectionString = configuration["AzureBlbStorage"];
            return new AzureBlbStorageService(connectionString);
        }
        private static void SeedCountries(IMongoDatabase database,string collectionname)
        {
            var countries = DataSeed.Seed<List<Country>>(@"./seedData/countrieslist.json");          
            var countries_collection = database.GetCollection<Country>(collectionname);
            foreach(var country in countries)
            {
                Console.WriteLine(country.Name);
                countries_collection.InsertOne(country);                
                Thread.Sleep(100);
            }            
        }
        private static void ReadCountries(IMongoDatabase database,string collectionname)
        {
            var countries_collection = database.GetCollection<Country>(collectionname);
            FilterDefinition<Country> filter = Builders<Country>.Filter.Eq("DocumentType", "Country");
            int id = 1;
            foreach (var item in countries_collection.Find(filter).ToList())
            {
                Console.WriteLine($"{id} : {item.ID}:  {item.Name}");id++;                
                Thread.Sleep(100);
            }
        }
        private static void SeedComparableFields(IMongoDatabase database, string collectionname)
        {
            var comparablefields = DataSeed.Seed<List<ComparableFields>>(@"./seedData/comparablefields.json");
            var comparablefields_collection = database.GetCollection<ComparableFields>(collectionname);
            foreach (var field in comparablefields)
            {
                Console.WriteLine(field.FieldName);
                comparablefields_collection.InsertOne(field);
                Thread.Sleep(100);
            }
        }
        private static void ReadComparableFields(IMongoDatabase database, string collectionname)
        {            
            var comparablefields_collection = database.GetCollection<ComparableFields>(collectionname);
            FilterDefinition<ComparableFields> filter = Builders<ComparableFields>.Filter.Eq("DocumentType", "ComparableFields");
            int id = 1;
            foreach (var item in comparablefields_collection.Find(filter).ToList())
            {
                Console.WriteLine($"{id} : {item.ID}:  {item.FieldName}"); id++;
                Thread.Sleep(100);
            }
        }
        private static void SeedNotesandDefs(IMongoDatabase database, string collectionname)
        {
            var notesanddefs = DataSeed.Seed<Dictionary<string, string>>(@"./seedData/notesanddefs.json");
            var notesanddefs_collection = database.GetCollection<NotesAndDefs>(collectionname);
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
        private static void ReadNotesandDefs(IMongoDatabase database, string collectionname)
        {
            var notesanddefs_collection = database.GetCollection<NotesAndDefs>(collectionname);
            FilterDefinition<NotesAndDefs> filter = Builders<NotesAndDefs>.Filter.Eq("DocumentType", "NotesAndDefs");
            int id = 1;
            foreach (var item in notesanddefs_collection.Find(filter).ToList())
            {
                Console.WriteLine($"{id} : {item.ID}:  {item.FieldName}"); id++;
                Thread.Sleep(100);
            }
        }
        private static void SeedCountryDetails(IMongoDatabase database, string collectionname)
        {
            var countrydetails = DataSeed.Seed<Dictionary<string, List<ProfileEntity>>>(@"./seedData/countrydetails.json");
            var countrydetails_collection = database.GetCollection<CountryEntity>(collectionname);
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
        private static void ReadCountryDetails(IMongoDatabase database, string collectionname)
        {
           
            var countrydetails_collection = database.GetCollection<CountryEntity>(collectionname);
            FilterDefinition<CountryEntity> filter = Builders<CountryEntity>.Filter.Eq("DocumentType", "CountryEntity");
            int id = 1;
            foreach (var item in countrydetails_collection.Find(filter).ToList())
            {
                Console.WriteLine($"{id} :  {item.ID}:  {item.CountryCode}"); id++;
                Thread.Sleep(100);
            }
        }        
        private static void UploadAnthemFiles(AzureBlbStorageService blbStorage, string containername, string directory)
        {
            var files = new DirectoryInfo(directory).EnumerateFiles();
            foreach (var file in files)
            {
                string filename = file.Name;
                Console.WriteLine($"Uploading {filename}");
                Task uploadtask = blbStorage.UploadFile(containername, filename, File.ReadAllBytes(file.FullName));
                uploadtask.Wait();
            }
        }

        #endregion Code for seeding json data to cosmos db and anthem files to data storage

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
