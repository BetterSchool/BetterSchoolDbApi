using System;
using System.Collections.Generic;

namespace BetterAdminDbAPI.Entities
{
    public partial class Person
    {
        public Person()
        {
            Admins = new HashSet<Admin>();
            Guardians = new HashSet<Guardian>();
            MessageReceivers = new HashSet<Message>();
            MessageSenders = new HashSet<Message>();
            Payments = new HashSet<Payment>();
            Pupils = new HashSet<Pupil>();
            Rentals = new HashSet<Rental>();
            Teachers = new HashSet<Teacher>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNo { get; set; } = null!;
        public string Email { get; set; } = null!;

        public virtual ICollection<Admin> Admins { get; set; }
        public virtual ICollection<Guardian> Guardians { get; set; }
        public virtual ICollection<Message> MessageReceivers { get; set; }
        public virtual ICollection<Message> MessageSenders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Pupil> Pupils { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
    }
}
