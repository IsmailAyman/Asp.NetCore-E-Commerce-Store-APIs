using System.ComponentModel.DataAnnotations;

namespace EdgeProject.APIs.Dtos
{
    public class AddressDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string street { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
