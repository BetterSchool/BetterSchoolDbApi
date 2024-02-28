using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Enrollment
    {
        public Enrollment()
        {
            Payments = new HashSet<Payment>();
        }

        public int Id { get; set; }
        public int PupilId { get; set; }
        public int CourseId { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual Pupil Pupil { get; set; } = null!;
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
