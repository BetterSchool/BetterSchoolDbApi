using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Address
    {
        public Address()
        {
            Guardians = new HashSet<Guardian>();
            Pupils = new HashSet<Pupil>();
        }

        public int Id { get; set; }
        public string City { get; set; } = null!;
        public string Road { get; set; } = null!;
        public int PostalCode { get; set; }

        public virtual ICollection<Guardian> Guardians { get; set; }
        public virtual ICollection<Pupil> Pupils { get; set; }
    }
}
