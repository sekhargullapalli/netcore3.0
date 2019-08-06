using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonLibraryDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(Serialize());            
            //SerializetoUtf8Bytes(@$"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\object.obj");

            DeserializefromUtf8Bytes("StaticFiles/object.json");

            Console.WriteLine("Done!");
        }

        #region Serialization 
        public static DemoType GetDemoObject()
        {
            return new DemoType()
            {
                Id = 1,
                Text = "2²=4 & 3³=27",
                EnumType = DemoEnumeration.two,
                Created = DateTime.UtcNow,
                NumValue = 1.23456789,
                IgnoreField = "Will not be serialized",
                NumericArray = new double[] { 1.23, 4.56, 7.89 },
                Dict = new Dictionary<string, int> { { "one", 1 }, { "two", 2 }, { "three", 3 } },
                FileData = File.ReadAllBytes("StaticFiles/SampleText.txt")
            };
        }          
        public static string Serialize()
        {           
            return JsonSerializer.Serialize<DemoType>(GetDemoObject(), new JsonSerializerOptions()
            {
                 AllowTrailingCommas=true,
                 WriteIndented=true,
                 IgnoreNullValues=false,                    
            });
        }
        public static void SerializetoUtf8Bytes(string path)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes<DemoType>(GetDemoObject(), new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
                IgnoreNullValues = false,
            });
            File.WriteAllBytes(path, bytes);
        }
        #endregion Serialization 

        #region Deserializaton
        public static void DeserializefromUtf8Bytes(string path)
        {
            var obj = JsonSerializer.Deserialize<DemoType>(File.ReadAllBytes(path), new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            });
            //Printing Unmatched Properties
            Console.WriteLine("Printing unmatched properties during deserialization");
            foreach (var item in obj.UnMatchedProperties)
            {
                Console.WriteLine(item.Key);
                var element = (JsonElement)item.Value;
                Console.WriteLine($"Value Kind: {element.ValueKind}");
                Console.WriteLine($"Value : {element.ToString()}");
                Console.WriteLine("------------------");

            }
        }

        #endregion Deserializaton


    }
}
