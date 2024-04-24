namespace BetterAdminDbAPI.Model
{
    public class Guardian
    {
        public int? GuardianId { get; set; }
        public string HashedSaltedPassword { get; set; }
        public string Salt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string City { get; set; }
        public string Road { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
    }
}
