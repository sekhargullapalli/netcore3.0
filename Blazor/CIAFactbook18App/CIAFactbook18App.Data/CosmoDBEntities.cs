using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace CIAFactbook18App.Data
{
    public class Factbook18CosmosDbContext
    {
        IMongoDatabase database;
        string dbname = "ciafactbook18";
        string collectionname = "factbook18context";

        public List<Country> Countries { get; set; } = new List<Country>();
        public List<ComparableFields> ComparableFields { get; set; } = new List<ComparableFields>();
        public List<NotesAndDefs> NotesAndDefs { get; set; } = new List<NotesAndDefs>();
        public List<CountryEntity> CountryDetails { get; set; } = new List<CountryEntity>();
               
        public Factbook18CosmosDbContext(string connectionstring)
        {            
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionstring)
            );
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            database = mongoClient.GetDatabase(dbname);
            Populate();
        }
        //Populate all entity lists on instantiation
        void Populate()
        {
            //Reading countries
            var countries_collection = database.GetCollection<Country>(collectionname);
            FilterDefinition<Country> filter = Builders<Country>.Filter.Eq("DocumentType", "Country");            
            Countries = countries_collection.Find(filter).ToList();

            //Reading comparable fields
            var comparablefields_collection = database.GetCollection<ComparableFields>(collectionname);
            FilterDefinition<ComparableFields> filter2 = Builders<ComparableFields>.Filter.Eq("DocumentType", "ComparableFields");
            ComparableFields = comparablefields_collection.Find(filter2).ToList();

            //Reading notes and defs
            var notesanddefs_collection = database.GetCollection<NotesAndDefs>(collectionname);
            FilterDefinition<NotesAndDefs> filter3 = Builders<NotesAndDefs>.Filter.Eq("DocumentType", "NotesAndDefs");            
            NotesAndDefs = notesanddefs_collection.Find(filter3).ToList();

            //Reading country details
            var countrydetails_collection = database.GetCollection<CountryEntity>(collectionname);
            FilterDefinition<CountryEntity> filter4 = Builders<CountryEntity>.Filter.Eq("DocumentType", "CountryEntity");
            CountryDetails = countrydetails_collection.Find(filter4).ToList();
        }
    }
    //Entity classes for cosmos db   
    public class CountryEntity
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string DocumentType { get; set; } = "CountryEntity";
        [BsonElement("CountryCode")]
        public string CountryCode { get; set; }
        [BsonElement("CountryData")]
        public List<ProfileEntity> CountryData { get; set; }
    }
    public class ComparableFields
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string DocumentType { get; set; } = "ComparableFields";
        [BsonElement("FieldName")]
        public string FieldName { get; set; }
        [BsonElement("Category")]
        public string Category { get; set; }
        [BsonElement("IsDescending")]
        public bool IsDescending { get; set; }
    }
    public class NotesAndDefs
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string DocumentType { get; set; } = "NotesAndDefs";
        [BsonElement("FieldName")]
        public string FieldName { get; set; }
        [BsonElement("Definition")]
        public string Definition { get; set; }
    }
    public class Country
    {
        [BsonId]
        public ObjectId ID { get; set; }
        public string DocumentType { get; set; } = "Country";
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("GEC")]
        public string GEC { get; set; }
        [BsonElement("ISO_3166_1_Alpha2")]
        public string ISO_3166_1_Alpha2 { get; set; }
        [BsonElement("ISO_3166_1_Alpha3")]
        public string ISO_3166_1_Alpha3 { get; set; }
        [BsonElement("ISO_3166_1_Numeric")]
        public string ISO_3166_1_Numeric { get; set; }
        [BsonElement("STANAG")]
        public string STANAG { get; set; }
        [BsonElement("Internet")]
        public string Internet { get; set; }
        [BsonElement("Comment")]
        public string Comment { get; set; }
        [BsonElement("Flagfile")]
        public string Flagfile { get; set; }
        [BsonElement("AnthemFile")]
        public string AnthemFile { get; set; }
        [BsonElement("Datafile")]
        public string Datafile { get; set; }
    }
}
