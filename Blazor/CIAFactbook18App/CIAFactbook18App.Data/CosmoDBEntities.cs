using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CIAFactbook18App.Data
{
    //Entity classes for cosmos db

    public class ComparableField
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string FieldName { get; set; }
        public string Category { get; set; }
        public bool IsDescending { get; set; }
    }
    public class FieldDefinition
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string FieldName { get; set; }
        public string Definition { get; set; }
    }
    public class Country
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string GEC { get; set; }
        public string ISO_3166_1_Alpha2 { get; set; }
        public string ISO_3166_1_Alpha3 { get; set; }
        public string ISO_3166_1_Numeric { get; set; }
        public string STANAG { get; set; }
        public string Internet { get; set; }
        public string Comment { get; set; }
        public string Flagfile { get; set; }
        public string AnthemFile { get; set; }
        public string Datafile { get; set; }
    }

    
}
