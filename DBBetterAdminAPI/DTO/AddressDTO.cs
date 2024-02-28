using System.ComponentModel.DataAnnotations;

namespace BetterAdminDbAPI.DTO
{
    public class AddressDTO
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Road { get; set; }
        public int PostalCode { get; set; }
    }
}
