using System.Linq;
using System.Threading.Tasks;
using ChinookProtoBuffer;
using Chinook.Data;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

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
        public override Task<MediaTypes> GetAllMediaTypes(Empty request, ServerCallContext context)
        {
            MediaTypes mtypes = new MediaTypes();
            foreach (var mtype in datacontext.MediaType)
                mtypes.MediaTypeCollection.Add(new MediaTypes.Types.MediaType()
                {
                    MediaTypeID = (int)mtype.MediaTypeId,
                    Name = mtype.Name
                });
            return Task.FromResult(mtypes);
        }
        public override Task<Genres> GetAllGenres(Empty request, ServerCallContext context)
        {
            Genres genres = new Genres();
            foreach (var genre in datacontext.Genre)
                genres.GenreCollection.Add(new Genres.Types.Genre()
                {
                    GenreID = (int)genre.GenreId,
                    Name = genre.Name
                });
            return Task.FromResult(genres);
        }
        public override Task<Albums> GetAllAlbums(Empty request, ServerCallContext context)
        {
            Albums albums = new Albums();
            foreach (var album in datacontext.Album.Include(a=>a.Artist))
            {
                albums.AlbumCollection.Add(new Albums.Types.Album()
                {
                    AlbumID = (int)album.AlbumId,
                    Title = album.Title,
                    Artist = new Artists.Types.Artist()
                    {
                        ArtistID = (int)album.Artist.ArtistId,
                        Name = album.Artist.Name
                    }
                });
            }
            return Task.FromResult(albums);
        }
        public override Task<StoreEmployees> GetAllEmployees(Empty request, ServerCallContext context)
        {
            StoreEmployees employees = new StoreEmployees();
            foreach (var employee in datacontext.Employee)
                employees.EmployeeCollection.Add(new StoreEmployees.Types.StoreEmployee()
                {
                    FirstName = employee.FirstName,
                    LastName=employee.LastName,
                    HireDate = Timestamp.FromDateTime(employee.HireDate.ConvertToDateTime()),
                    BirthDay = Timestamp.FromDateTime(employee.BirthDate.ConvertToDateTime()),
                });
            return Task.FromResult(employees);
        }


        public override Task<Albums> GetAlbumsByArtist(Artists.Types.Artist request, ServerCallContext context)
        {
            int id = request.ArtistID;
            Albums albums = new Albums();
            foreach (var album in datacontext.Album.Include(a => a.Artist).Where(a=>a.ArtistId==id))
                albums.AlbumCollection.Add(new Albums.Types.Album()
                {
                    AlbumID = (int)album.AlbumId,
                    Title = album.Title,
                    Artist = new Artists.Types.Artist()
                    {
                        ArtistID = (int)album.Artist.ArtistId,
                        Name = album.Artist.Name
                    }
                });
            return Task.FromResult(albums);
        }
        public override Task<Tracks> GetTracksByAlbum(Albums.Types.Album request, ServerCallContext context)
        {
            int id = request.AlbumID;
            Tracks tracks = new Tracks();
            foreach (var track in datacontext.Track.Include(t => t.Album).Where(t => t.AlbumId == id))
                tracks.TrackCollection.Add(new Tracks.Types.Track()
                {
                    TrackID=(int)track.TrackId,
                    Name=track.Name,
                    Composer=track.Composer,
                    MilliSeconds = (ulong)track.Milliseconds,
                    SizeinMB = (ulong)(track.Bytes??0/1048576)              
                });
            return Task.FromResult(tracks);
        }
        public override Task<Tracks> GetTracksByGenre(Genres.Types.Genre request, ServerCallContext context)
        {
            int id = request.GenreID;
            Tracks tracks = new Tracks();
            foreach (var track in datacontext.Track.Include(t => t.Genre).Where(t => t.GenreId == id))
                tracks.TrackCollection.Add(new Tracks.Types.Track()
                {
                    TrackID = (int)track.TrackId,
                    Name = track.Name,
                    Composer = track.Composer,
                    MilliSeconds = (ulong)track.Milliseconds,
                    SizeinMB = (ulong)(track.Bytes ?? 0 / 1048576)
                });
            return Task.FromResult(tracks);
        }
    }
}
