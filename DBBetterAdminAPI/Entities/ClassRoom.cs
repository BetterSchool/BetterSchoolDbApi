using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class ClassRoom
    {
        public ClassRoom()
        {
            Courses = new HashSet<Course>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Course> Courses { get; set; }
    }
}
