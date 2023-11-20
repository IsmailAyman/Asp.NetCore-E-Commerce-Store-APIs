using System.ComponentModel.DataAnnotations;

namespace EdgeProject.APIs.Dtos
{
    public class BasketItemDto
    {
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price must be more than Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be atleast one item")]
        public int Quantity { get; set; }
        [Required]

        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
