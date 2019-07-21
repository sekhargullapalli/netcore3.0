using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace Chinook.gRPCClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppContext.SetSwitch(
                           "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport",
                           true);

            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure); //Use Appropriate channel address
            var client = new ChinookProtoBuffer.ChinookPBS.ChinookPBSClient(channel);

            //Some Example RPC Calls

            bool listArtists = false;
            if (listArtists)
            {
                var artists = await client.GetAllArtistsAsync(new Google.Protobuf.WellKnownTypes.Empty());
                foreach (var artist in artists.ArtistCollection)
                    Console.WriteLine($"{artist.ArtistID}\t{artist.Name}");
            }

            bool listEmployees = false;
            if (listEmployees)
            {
                var employees = await client.GetAllEmployeesAsync(new Google.Protobuf.WellKnownTypes.Empty());
                foreach (var employee in employees.EmployeeCollection)
                    Console.WriteLine($"{employee.FirstName}\t{employee.LastName}\t{employee.BirthDay}");
            }

            bool listAlbums = true;
            if (listAlbums)
            {
                var albumns = await client.GetAllAlbumsAsync(new Google.Protobuf.WellKnownTypes.Empty());
                foreach (var album in albumns.AlbumCollection)
                    Console.WriteLine($"{album.Artist.Name}\t{album.Title}");
            }





            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
