namespace BetterAdminDbAPI.Model
{
    public class Rental
    {
        public int RentalId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int InstrumentId { get; set; }
        public int PupilId { get; set; }
    }
}
