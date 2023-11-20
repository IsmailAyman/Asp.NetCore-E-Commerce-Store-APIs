using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Core.Entities
{
    public class CustomerBasket
    {
        public CustomerBasket(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } 

        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }

        public int? DeliveryMethodsId {  get; set; }

        public decimal ShippingCost { get; set; }

    }
}
