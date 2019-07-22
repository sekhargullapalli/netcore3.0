<img src="grpc-logo-blk.png"  width='290' height='140'>

# gRPC in .NET CORE 3.0 Preview

The Asp.NET Core 3.0 gRPC Service preview is explored here. The github repository for this project is available [here](https://github.com/sekhargullapalli/netcore3.0/tree/master/gRPC). The official Microsoft tutorial can be found [here](https://docs.microsoft.com/en-us/aspnet/core/grpc/?view=aspnetcore-3.0)  

### **Note**
 _Note that Visual Studio 2019 with preview features enabled is required together with the .net core 3.0 preview installed to run the gRPC server and client applicaitons. In the gRPC server application, the launchsettings.json is included (this is required and without which the server opens in a browser) - as of 22 July 2019._ 


# 1. gRPC

## 1.1 About

gRPC in a language agnostic Remote Procedure Call mechanism developed at Google and uses the HTTP/2 for transport. gRPC is getting popular for development of microservices as and alternate to JSON based REST APIs. gRPC uses Protocol Buffers instead on JSON, which are binary serializad payloads with a defined schema. These payloads can be parsed by several popular languages, if the schema (`.proto` file) is available. Similar to web APIs, there is plugin support for load balancing, authentication etc. Unary/streaming synchronous/asynchronus calls are possible using gRPC. Several online bench marking show considerably low memory footprint and higher speeds when using gRPC compared to JSON based REST APIs.

The ASP.NET Core gRPC service type projects are included in the .net core 3.0 preview and the C# implementation currently relies on the the native library writter in C at gRPC page. Accoring the Microsoft, work is in progress for a fully managed implementation for Kestrel HTTP server. - as of 22 July 2019.


# 2. Protocol Buffers

## 2.1 About

Protocol Buffers offer an extensible mechanism for serializing structured data and utilizing it across several platforms [2]. Currently the proto (proto3) definitions can be resolved in several languages including C#, Java, Python, C++, Objective-C, Dart, Go, Ruby etc.

Protocol Buffers have binary representation and generally have a smaller footprint compared to `JSON` or `XML` formats. Several advantages such as speed, compactness, pre defined schema, simple interoperability and relatively low boiler plate code are offered. Validations and versioning support are inbuilt into the proto structure. These features make protocol buffers popular for micro services, especialy when the server application is not written in `JavaScript`.

proto3 version of prototype buffers support datatypes such as `double, float, int32, int64, uint32, uint64, sint32, sint64, fixed32, fixed64, sfixed32, sfixed64, bool, string and bytes`. The `protoc` compiles the `.proto` file to the corresponding types and service buffers in the programming language of choice. Additionally `.proto` file can also contain Enumerations, Nested Types, repeated types (represents arrays) and maps (dictionaries). Keywords such as `reserved` are used to represent obsolete fields and are very handy for backward compatibility. The `.proto` files also support import, package directives for working across files and namespaces.

Services (similar to controllers) and RPCs (similar to routes) are also defined in the `.proto` file and must be implemented in the programming language of choice.

## 2.2 Example

Here is a description of a `.proto` file used in this application.
The directives `import` is used to bring in additional .proto definitions

```
syntax = "proto3";
import "google/protobuf/timestamp.proto";
import "google/protobuf/empty.proto";
option csharp_namespace = "ChinookProtoBuffer";
```

Messages are templates for the payloads (like classes) and can be nested. The `repeated` fields represent enumerable collections
```
message Artists{
	message Artist {
		int32 ArtistID = 1;
		string Name = 2;	
	}
	repeated Artist ArtistCollection = 1;
}
```
Finally, the services are defined as method signatures which take Message inputs and return Message outputs
```
service ChinookPBS {

	rpc GetAllArtists (google.protobuf.Empty) returns (Artists);
	rpc GetAlbumsByArtist (Artists.Artist) returns (Albums);	
}
```

## 2.3 Compiling `.proto` file
Visual Studio compiles the `.proto` files to necassary boiler plate C# code. This requires the Protobuf definition to be included in the ItemGroup section in the project file.
```
 <Protobuf Include="Protos\chinook.proto" GrpcServices="Server" />
 <Content Include="@(Protobuf)" />
```
This can be manually acheived using the protoc compiler which can be downloaded [here](http://central.maven.org/maven2/com/google/protobuf/protoc/) for your operating system of choice.
```
protoc -I=$SRC_DIR --csharp_out=$DST_DIR $SRC_DIR/chinook.proto
```
Using appropriate command line arguments, the boiler plate code for several languages can be created.


# 3. DataSet Used
The [Chinook](https://github.com/lerocha/chinook-database) SQLite database is used in this demo project. The Entity Framework Core Tools (3.0..0-preview6) database approach is used for data modeling. The entity classes are created using:

`Scaffold-DbContext "Filename=Chinook_Sqlite.sqlite" Microsoft.EntityFrameworkCore.Sqlite`

The DateTime properties are mapped to `byte[]` during Scaffolding. They are parsed to DateTime using the extension method:

```
public static DateTime ConvertToDateTime(this byte[] bytes) =>
            DateTime.Parse(System.Text.Encoding.UTF8.GetString(bytes)).ToUniversalTime();
```
and then converted to `Google.Protobuf.WellKnownTypes.Timestamp` using
```
 BirthDay = Timestamp.FromDateTime(employee.BirthDate.ConvertToDateTime()),
```

# 4. References
[1] gRPC.io https://grpc.io/
[2] Protocol Buffer Developer Guide https://developers.google.com/protocol-buffers/



