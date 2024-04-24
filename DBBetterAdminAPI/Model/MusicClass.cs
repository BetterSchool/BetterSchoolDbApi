namespace BetterAdminDbAPI.Model
{
    public class MusicClass
    {
        public int? MusicClassId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Cancelled { get; set; }
        public int CourseId { get; set; }
        public int PupilId { get; set; }
    }
}
