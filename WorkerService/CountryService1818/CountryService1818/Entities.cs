using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CountryService1818
{
    public class CountriesContext
    {
        public List<Country> Countries { get; set; } = new List<Country>();
        public List<City> Cities { get; set; } = new List<City>();
        public List<CountryLanguage> Languages { get; set; } = new List<CountryLanguage>();

        public CountriesContext()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "CountryService1818.country.json";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                Countries = JsonSerializer.Deserialize<List<Country>>(reader.ReadToEnd(), new JsonSerializerOptions
                {
                    AllowTrailingCommas = true
                });
            }
            resourceName = "CountryService1818.city.json";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                Cities = JsonSerializer.Deserialize<List<City>>(reader.ReadToEnd(), new JsonSerializerOptions
                {
                    AllowTrailingCommas = true
                });
            }
            resourceName = "CountryService1818.countrylanguage.json";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                Languages = JsonSerializer.Deserialize<List<CountryLanguage>>(reader.ReadToEnd(), new JsonSerializerOptions
                {
                    AllowTrailingCommas = true
                });
            }
            foreach (var country in Countries)
            {
                if (country.Capital.HasValue)
                {
                    var capital = Cities.Where(c => c.ID == country.Capital.Value).FirstOrDefault();
                    if (capital != null) country.CapitalName = capital.Name;
                }
                foreach (var city in Cities.OrderBy(c => c.Name))
                    if (city.CountryCode == country.Code)
                        country.Cities.Add(city);
                foreach (var language in Languages.OrderBy(l => l.Language))
                    if (language.CountryCode == country.Code)
                        country.Languages.Add(language);
            }
        }
    }

    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Continent { get; set; }
        public string Region { get; set; }
        public float SurfaceArea { get; set; }
        public object IndepYear { get; set; }
        public int Population { get; set; }
        public float? LifeExpectancy { get; set; }
        public float? GNP { get; set; }
        public float? GNPOld { get; set; }
        public string LocalName { get; set; }
        public string GovernmentForm { get; set; }
        public string HeadOfState { get; set; }
        public int? Capital { get; set; }
        public string Code2 { get; set; }

        [JsonIgnore]
        public string CapitalName { get; set; }
        [JsonIgnore]
        public List<City> Cities { get; set; } = new List<City>();
        [JsonIgnore]
        public List<CountryLanguage> Languages { get; set; } = new List<CountryLanguage>();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Code: {Code}");
            sb.AppendLine($"Name: {Name}");
            sb.AppendLine($"LocalName: {LocalName}");
            sb.AppendLine($"CapitalName: {CapitalName}");
            sb.AppendLine($"Continent: {Continent}");
            sb.AppendLine($"Region: {Region}");
            sb.AppendLine($"Surface Area: {SurfaceArea.ToString("N2")}");
            sb.AppendLine($"Independence Year: {IndepYear ?? ""}");
            sb.AppendLine($"Population: {Population}");
            sb.AppendLine($"LifeExpectancy: {LifeExpectancy?.ToString("N2") ?? ""}");
            sb.AppendLine($"GNP: {GNP?.ToString("N2") ?? ""}");
            sb.AppendLine($"GNP Old: {GNPOld?.ToString("N2") ?? ""}");
            sb.AppendLine($"Government Form: {GovernmentForm}");
            sb.AppendLine($"Head Of State: {HeadOfState ?? ""}");
            sb.AppendLine();
            if (Languages.Count != 0)
            {
                string langstring = "Languages: ";
                foreach (var lang in Languages)
                    langstring += $"{lang.Language}, ";
                sb.AppendLine(langstring.TrimEnd().Trim(','));
            }
            sb.AppendLine();
            sb.AppendLine();
            if (Cities.Count != 0)
            {
                string citystring = "Cities: ";
                foreach (var city in Cities)
                    citystring += $"{city.Name}, ";
                sb.AppendLine(citystring.TrimEnd().Trim(','));
            }
            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }
    }
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string District { get; set; }
        public int Population { get; set; }
        public override string ToString()
        {
            return $"{Name}, {District} [{Population}]";
        }
    }
    public class CountryLanguage
    {
        public string CountryCode { get; set; }
        public string Language { get; set; }
        public string IsOfficial { get; set; }
        public float Percentage { get; set; }

        public override string ToString()
        {
            return $"{Language} [{Percentage.ToString("N2")} %]{((IsOfficial == "T") ? " - Official" : "")}";
        }
    }
}
