using MongoDB.Bson.Serialization.Attributes;

namespace CIAFactbook18App.Data
{
    //Entity classes for cosmos db

    public class ComparableField
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement]
        public string FieldName { get; set; }
        [BsonElement]
        public string Category { get; set; }
        [BsonElement]
        public bool IsDescending { get; set; }
    }
    public class FieldDefinition
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement]
        public string FieldName { get; set; }
        [BsonElement]
        public string Definition { get; set; }
    }
    public class Country
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement]
        public string Name { get; set; }
        [BsonElement]
        public string GEC { get; set; }
        [BsonElement]
        public string ISO_3166_1_Alpha2 { get; set; }
        [BsonElement]
        public string ISO_3166_1_Alpha3 { get; set; }
        [BsonElement]
        public string ISO_3166_1_Numeric { get; set; }
        [BsonElement]
        public string STANAG { get; set; }
        [BsonElement]
        public string Internet { get; set; }
        [BsonElement]
        public string Comment { get; set; }
        [BsonElement]
        public string Flagfile { get; set; }
        [BsonElement]
        public string AnthemFile { get; set; }
        [BsonElement]
        public string Datafile { get; set; }
    }

    
}
