using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Instrument
    {
        public Instrument()
        {
            Rentals = new HashSet<Rental>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Type { get; set; } = null!;

        public virtual InstrumentType TypeNavigation { get; set; } = null!;
        public virtual ICollection<Rental> Rentals { get; set; }
    }
}
