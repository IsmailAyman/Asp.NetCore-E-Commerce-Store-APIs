using System.ComponentModel.DataAnnotations;

namespace EdgeProject.APIs.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }

        public int? DeliveryMethodsId { get; set; }

        public decimal ShippingCost { get; set; }

        public List<BasketItemDto> Items { get; set; }

    }
}
