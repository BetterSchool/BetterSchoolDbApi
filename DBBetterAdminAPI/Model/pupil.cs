using BetterAdminDbAPI.Model.@enum;

namespace BetterAdminDbAPI.Model
{
    public class Pupil
    {
        public string Email { get; set; }
        public string HashedSaltedPassword { get; set; }
        public string Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public GenderEnum Gender { get; set; }
        public DateOnly EnrollmentDate { get; set; }
        public string? Note { get; set; }
        public bool PhotoPermission { get; set; }
        public string School { get; set; }
        public int Grade { get; set; }
        public string City { get; set; }
        public string Road { get; set; }
        public string PostalCode { get; set; }
        public string? GuardianEmail { get; set; }
    }
}
