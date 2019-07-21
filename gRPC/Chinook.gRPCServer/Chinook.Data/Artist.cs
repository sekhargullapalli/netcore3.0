using System;
using System.Collections.Generic;

namespace Chinook.Data
{
    public partial class Artist
    {
        public Artist()
        {
            Album = new HashSet<Album>();
        }

        public long ArtistId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Album> Album { get; set; }
    }
}
