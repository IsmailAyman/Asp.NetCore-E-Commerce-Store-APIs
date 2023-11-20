using EdgeProject.Core.Entities.Order_Aggregation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Core.Services
{
    public interface IOrderServices
    {
        Task<Order?> CreateOrderAsync(string buyerEmail,string basketId,int deliveryMethodId,Address shippingAddress);

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

        Task<Order> GetOrderByIdForUserAsync(int orderId,string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();


    }
}
