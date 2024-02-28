using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Rental
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int InstrumentId { get; set; }
        public int PupilId { get; set; }
        public int Payer { get; set; }

        public virtual Instrument Instrument { get; set; } = null!;
        public virtual Person PayerNavigation { get; set; } = null!;
        public virtual Pupil Pupil { get; set; } = null!;
    }
}
