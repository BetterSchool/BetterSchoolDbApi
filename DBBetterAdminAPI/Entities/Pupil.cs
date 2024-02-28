using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Pupil
    {
        public Pupil()
        {
            Classes = new HashSet<Class>();
            Enrollments = new HashSet<Enrollment>();
            Rentals = new HashSet<Rental>();
            WaitLists = new HashSet<WaitList>();
        }

        public int Id { get; set; }
        public string? MobileNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string? Note { get; set; }
        public bool PhotoPermission { get; set; }
        public string? School { get; set; }
        public int Grade { get; set; }
        public int PersonId { get; set; }
        public int AddressId { get; set; }
        public int GuardianId { get; set; }

        public virtual Address Address { get; set; } = null!;
        public virtual Guardian Guardian { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; }
        public virtual ICollection<WaitList> WaitLists { get; set; }
    }
}
