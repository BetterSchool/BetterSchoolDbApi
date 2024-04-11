namespace BetterAdminDbAPI.Model
{
    public class WaitList
    {
        public int CourseId { get; set; }
        public int PupilId { get; set; }
        public DateTime TimeEnteredQueue { get; set; }
    }
}
