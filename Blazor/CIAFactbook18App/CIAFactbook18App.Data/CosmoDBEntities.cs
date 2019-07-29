using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CIAFactbook18App.Data
{
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
