namespace BetterAdminDbAPI.Model
{
    public class Concert
    {
        public int ConcertId { get; set; }
        public string ConcertName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ConcertLocation { get; set; }
    }
}
