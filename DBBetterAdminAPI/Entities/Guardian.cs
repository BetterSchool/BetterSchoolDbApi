using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Guardian
    {
        public Guardian()
        {
            Pupils = new HashSet<Pupil>();
        }

        public int Id { get; set; }
        public string WorkPhoneNo { get; set; }
        public int PersonId { get; set; }
        public int AddressId { get; set; }

        public virtual Address Address { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
        public virtual ICollection<Pupil> Pupils { get; set; }
    }
}
