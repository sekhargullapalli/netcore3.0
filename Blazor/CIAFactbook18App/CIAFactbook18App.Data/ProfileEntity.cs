using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CIAFactbook18App.Data
{
    public enum ProfileEntityType { None, Category, Field, SubField }
    /// <summary>
    /// A profile entity object is used to hold the details of a given county in a recursive manner
    /// </summary>
    public class ProfileEntity
    {
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public ProfileEntityType EntityType { get; set; } = ProfileEntityType.None;
        public string DocumentType { get; set; } = "ProfileEntity";
        [BsonElement("Key")]
        public string Key { get; set; } = "";
        [BsonElement("Value")]
        public string Value { get; set; } = "";
        [BsonElement("Note")]
        public string Note { get; set; } = "";
        [BsonElement("Date")]
        public string Date { get; set; } = "";
        [BsonElement("IsHistoricEntity")]
        public bool IsHistoricEntity { get; set; } = false;
        [BsonElement("IsNumericEntity")]
        public bool IsNumericEntity { get; set; } = false;
        [BsonElement("IsGroupedEntity")]
        public bool IsGroupedEntity { get; set; } = false;

        [BsonElement("ComparisonRank")]
        public int? ComparisonRank { get; set; } = null;

        [BsonElement("Children")]
        public List<ProfileEntity> Children { get; set; } = new List<ProfileEntity>();
    }
}
