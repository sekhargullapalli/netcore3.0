using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonLibraryDemo
{
    public class ByteArrayToString : JsonConverter<byte[]> 
    {
        public override byte[] Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            return Encoding.UTF8.GetBytes(reader.GetString());
        }
        public override void Write(Utf8JsonWriter writer, byte[] val, JsonSerializerOptions options)
        {
            writer.WriteStringValue(Encoding.UTF8.GetString(val));
        }
    }    
}
