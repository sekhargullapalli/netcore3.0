using System;
using System.Collections.Generic;

namespace Chinook.Data
{
    public partial class InvoiceLine
    {
        public long InvoiceLineId { get; set; }
        public long InvoiceId { get; set; }
        public long TrackId { get; set; }
        public byte[] UnitPrice { get; set; }
        public long Quantity { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Track Track { get; set; }
    }
}
