using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChinookProtoBuffer;
using Chinook.Data;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Chinook.gRPCServer
{
    public class ChinookService: ChinookPBS.ChinookPBSBase
    {
        Chinook_SqliteContext datacontext;
        public ChinookService(Chinook_SqliteContext _datacontext)
        {
            datacontext = _datacontext;
        }
        public override Task<Artists> GetAllArtists(Empty request, ServerCallContext context)
        {
            Artists artists = new Artists();
            foreach (var artist in datacontext.Artist)
                artists.ArtistCollection.Add(new Artists.Types.Artist()
                {
                    ArtistID=(int)artist.ArtistId,
                    Name=artist.Name
                });
            return Task.FromResult(artists);
        }
    }
}
