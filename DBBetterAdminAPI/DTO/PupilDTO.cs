using System.ComponentModel.DataAnnotations;

namespace BetterAdminDbAPI.DTO
{
    public class PupilDTO : PersonDTO
    {
        public int Id { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string? Note { get; set; }
        public bool PhotoPermission { get; set; }
        public string School { get; set; }
        public int Grade { get; set; }

        //Address properties
        public string City { get; set; }
        public string Road { get; set; }
        public int PostalCode { get; set; }

        //Guardian
        public GuardianDTO? GuardDTO { get; set; }
    }
}
