using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Course
    {
        public Course()
        {
            Classes = new HashSet<Class>();
            Enrollments = new HashSet<Enrollment>();
            WaitLists = new HashSet<WaitList>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int MaxEnrolled { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TeacherId { get; set; }
        public int ClassRoomId { get; set; }

        public virtual ClassRoom ClassRoom { get; set; } = null!;
        public virtual Teacher Teacher { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<WaitList> WaitLists { get; set; }
    }
}
