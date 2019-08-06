using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Buffers;
using System.Text;
using System.Linq;

namespace JsonLibraryDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(Serialize());    

            //SerializetoUtf8Bytes(@$"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\object.json");

            //DeserializefromUtf8Bytes("StaticFiles/object.json");

            //WriteJsonusingUTF8Writer(@$"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\object2.json");

            ExamineJson();
            
            Console.WriteLine("Done!");
        }

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

        #region Serialization 
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
                ReadCommentHandling= JsonCommentHandling.Skip
            });
            File.WriteAllBytes(path, bytes);
        }
        #endregion Serialization 

        #region Deserializaton
        public static void DeserializefromUtf8Bytes(string path)
        {
            try
            {
                var obj = JsonSerializer.Deserialize<DemoType>(File.ReadAllBytes(path), new JsonSerializerOptions
                {
                    AllowTrailingCommas = true
                });

                //Printing Unmatched Properties while deserializing
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
            catch (JsonException ex)
            {
                Console.WriteLine($"Error at line {ex.LineNumber} , pos {ex.BytePositionInLine} : {ex.Message}");                   
            }
        }

        #endregion Deserializaton

        #region Using UTF8Json Writer
        public static void WriteJsonusingUTF8Writer(string path)
        {
            var writeOptions = new JsonWriterOptions
            {
                Indented = true,
                SkipValidation=false
            };
            var bufferwriter = new ArrayBufferWriter<byte>();
            using (Utf8JsonWriter writer = new Utf8JsonWriter(bufferwriter, writeOptions))
            {
                writer.WriteStartObject();
                writer.WriteNumber("ID", 1);
                writer.WriteNull("ID2");
                writer.WriteBoolean("IsValid",true);                  
                writer.WriteString("CreatedDate", DateTimeOffset.UtcNow);
                writer.WriteNumber("NumValue", 1.2345);
                //Example of a comment
                writer.WriteCommentValue("Example of a comment");
                writer.WriteStartArray("NumArray");
                writer.WriteNumberValue(1.23);
                writer.WriteNumberValue(4.56);
                writer.WriteCommentValue("Example of a comment");

                writer.WriteNumberValue(7.89);
                writer.WriteEndArray();
                writer.WriteStartObject("Object");
                writer.WriteNumber("ID", 1);
                writer.WriteString("Name", "InnerObject");
                writer.WriteBase64String("FileData", File.ReadAllBytes("StaticFiles/SampleText.txt"));
                writer.WriteEndObject();
                writer.WriteEndObject();
            }
            string json = Encoding.UTF8.GetString(bufferwriter.WrittenSpan);
            Console.WriteLine(json);
            File.WriteAllBytes(path,bufferwriter.WrittenSpan.ToArray());
        }
        #endregion Using UTF8Json Writer

        #region Using JsonDocument
        public static void ExamineJson()
        {
            JsonDocument document = JsonDocument.Parse(File.ReadAllBytes("StaticFiles/object2.json"), new JsonDocumentOptions()
            {
                 AllowTrailingCommas=true,
                 CommentHandling= JsonCommentHandling.Skip,     
            });
            JsonElement element = document.RootElement;

            var prop = element.TryGetProperty("NumArray", out var arrayelement);
            Console.WriteLine(arrayelement.ToString());
        }
        #endregion Using JsonDocument
    }
}
