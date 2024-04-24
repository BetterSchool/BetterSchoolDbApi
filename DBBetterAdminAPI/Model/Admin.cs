namespace BetterAdminDbAPI.Model
{
    public class Admin
    {
        public string HashedSaltedPassword { get; set; }
        public string Salt { get; set; }
        public int AdminId { get; set; }
        public string Email { get; set; }
    }
}
