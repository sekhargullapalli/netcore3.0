using System;
using System.Collections.Generic;

namespace Chinook.Data
{
    public partial class MediaType
    {
        public MediaType()
        {
            Track = new HashSet<Track>();
        }

        public long MediaTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Track> Track { get; set; }
    }
}
