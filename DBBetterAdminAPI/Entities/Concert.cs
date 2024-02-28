using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Concert
    {
        public Concert()
        {
            Calendars = new HashSet<Calendar>();
        }

        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Location { get; set; } = null!;

        public virtual ICollection<Calendar> Calendars { get; set; }
    }
}
