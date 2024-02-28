using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class InstrumentType
    {
        public InstrumentType()
        {
            Instruments = new HashSet<Instrument>();
        }

        public int Id { get; set; }
        public string Type { get; set; } = null!;

        public virtual ICollection<Instrument> Instruments { get; set; }
    }
}
