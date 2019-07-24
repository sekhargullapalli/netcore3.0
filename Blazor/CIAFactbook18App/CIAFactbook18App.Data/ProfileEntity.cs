using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace CIAFactbook18App.Data
{
    public enum ProfileEntityType { None, Category, Field, SubField }

    /// <summary>
    /// A profile entity object is used to hold the details of a given county in a recursive manner
    /// </summary>
    public class ProfileEntity
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ProfileEntityType EntityType { get; set; } = ProfileEntityType.None;
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
        public string Note { get; set; } = "";
        public string Date { get; set; } = "";

        public bool IsHistoricEntity { get; set; } = false;
        public bool IsNumericEntity { get; set; } = false;
        public bool IsGroupedEntity { get; set; } = false;

        public int? ComparisonRank { get; set; } = null;

        public List<ProfileEntity> Children { get; set; } = new List<ProfileEntity>();
    }
}
