namespace BetterAdminDbAPI.Model
{
    public class Teacher
    {
        public string HashedSaltedPassword { get; set; }
        public string Salt { get; set; }
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public double WorkHours { get; set; }
        public string Email { get; set; }
    }
}
