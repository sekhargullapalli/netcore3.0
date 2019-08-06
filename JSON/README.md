# Exploring System.Text.Json 

.NET Core 3.0 comes with an addition of `System.Text.Json` for serializing, deserializing and also a DOM model for traversing and supporting query APIS for JSON documents. `System.Text.Json` is used by default for serializing and desrializing JSON payloads in ASP.NET Core  and SignalR applicaitons in latest previews. The docs for this namespace are available [here](https://docs.microsoft.com/en-us/dotnet/api/system.text.json?view=netcore-3.0).

In this repo, examples are provided for standard seraialization/ deserialization and reading/ writing tokens etc. Some of the features I found interesting are:

## JSON Comments
C++ style comments can be written and read from JSON`

`/*Example of a comment*/`

## Custom Converters
Converters implment the `JSONConverter` and in this example a byte array to stirng converter is shown
```
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
```

## JsonExtensionData Attribute
 When placed on a property of type IDictionary<TKey, TValue>, any properties that do not have a matching member are added to that dictionary during deserialization and written during serialization.



