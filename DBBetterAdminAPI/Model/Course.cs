namespace BetterAdminDbAPI.Model
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int MaxEnrolled { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TeacherId { get; set; }
        public string ClassroomName { get; set; }
    }
}
