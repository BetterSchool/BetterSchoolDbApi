using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int EnrollmentId { get; set; }

        public virtual Enrollment Enrollment { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
    }
}
