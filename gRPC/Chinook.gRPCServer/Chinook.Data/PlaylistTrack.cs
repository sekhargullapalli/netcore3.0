using System;
using System.Collections.Generic;

namespace Chinook.Data
{
    public partial class PlaylistTrack
    {
        public long PlaylistId { get; set; }
        public long TrackId { get; set; }

        public virtual Playlist Playlist { get; set; }
        public virtual Track Track { get; set; }
    }
}
