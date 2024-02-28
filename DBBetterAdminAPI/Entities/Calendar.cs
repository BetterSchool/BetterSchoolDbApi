using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Calendar
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int ConcertId { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual Concert Concert { get; set; } = null!;
    }
}
