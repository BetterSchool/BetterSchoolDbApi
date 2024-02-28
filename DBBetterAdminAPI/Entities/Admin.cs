using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Admin
    {
        public int Id { get; set; }
        public int PersonId { get; set; }

        public virtual Person Person { get; set; } = null!;
    }
}
