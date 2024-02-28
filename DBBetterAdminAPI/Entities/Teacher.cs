using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Teacher
    {
        public Teacher()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public float Hours { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<Course> Courses { get; set; }
    }
}
