using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JsonLibraryDemo
{
    public enum DemoEnumeration { one=1, two, three}
    public class DemoType
    {
        //Serialized as ID
        [JsonPropertyName("ID")]
        public int Id { get; set; }

        //Depends on IgnoreNullValues JsonSerializerOption when serializing and deserializing
        [JsonPropertyName("ID2")]
        public int? Id2 { get; set; } = null;

        //Enum serialized as string
        [JsonPropertyName("EnumProperty"), JsonConverter(typeof(JsonStringEnumConverter))]
        public DemoEnumeration EnumType { get; set; } = DemoEnumeration.one;

        [JsonPropertyName("Text")]
        public string Text { get; set; }

        [JsonPropertyName("CreatedAt")]
        public DateTime Created { get; set; }

        [JsonPropertyName("NumericValue")]
        public double NumValue { get; set; }

        [JsonPropertyName("NumericArray")]
        public double[] NumericArray { get; set; }

        [JsonPropertyName("Dictionary")]
        public Dictionary<string, int> Dict { get; set; } = new Dictionary<string, int>();

        //Property will be ignored during serializing and deserializing
        [JsonIgnore]
        public string IgnoreField { get; set; } = "This will be ignored";

        [JsonPropertyName("FileData"), JsonConverter(typeof(ByteArrayToString))]
        public byte[] FileData { get; set; } = new byte[0];


        //JsonExtensionData Attribute
        // When placed on a property of type IDictionary<TKey, TValue>, any properties 
        // that do not have a matching member are added to that dictionary during 
        // deserialization and written during serialization.
        [JsonExtensionData]
        public Dictionary<string, object> UnMatchedProperties { get; set; } = new Dictionary<string, object>();

    }

    

    
}
