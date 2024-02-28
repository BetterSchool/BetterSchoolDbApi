using System.ComponentModel.DataAnnotations;

namespace BetterAdminDbAPI.DTO
{
    public class GuardianDTO : PersonDTO
    {
        public int Id { get; set; }
        [Required]
        public string WorkPhoneNo { get; set; }
        [Required]
        public int PersonId { get; set; }
        [Required]
        public int AddressId { get; set; }

        //Address properties
        public string City { get; set; }
        public string Road { get; set; }
        public int PostalCode { get; set; }
    }
}
