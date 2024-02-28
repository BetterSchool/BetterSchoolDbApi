using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Class
    {
        public Class()
        {
            Calendars = new HashSet<Calendar>();
        }

        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Cancelled { get; set; }
        public int CourseId { get; set; }
        public int? PupilId { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual Pupil? Pupil { get; set; }
        public virtual ICollection<Calendar> Calendars { get; set; }
    }
}
