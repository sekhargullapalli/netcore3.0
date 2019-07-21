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

            var artists = await client.GetAllArtistsAsync(new Google.Protobuf.WellKnownTypes.Empty());
            foreach(var artist in artists.ArtistCollection)
                Console.WriteLine($"{artist.ArtistID}\t{artist.Name}");

            Console.WriteLine("Done!");
            Console.ReadLine();
        }
    }
}
