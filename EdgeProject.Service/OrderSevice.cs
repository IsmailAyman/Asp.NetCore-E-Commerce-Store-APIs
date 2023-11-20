using EdgeProject.Core;
using EdgeProject.Core.Entities;
using EdgeProject.Core.Entities.Order_Aggregation;
using EdgeProject.Core.Repository;
using EdgeProject.Core.Services;
using EdgeProject.Core.Specifications.Order_Spec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Service
{
    public class OrderSevice : IOrderServices
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentService paymentService;

        public OrderSevice(IBasketRepository basketRepository,IUnitOfWork unitOfWork,IPaymentService paymentService)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);

            var orderItems = new List<OrderItem>();

            if (basket?.Items?.Count > 0)
            {
                foreach(var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductOrderItem(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered,product.Price,item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            var subTotal = orderItems.Sum(item=>item.Price * item.Quantity);

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var spec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
            var existOrder = await unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            if(existOrder is not null) 
            {
                unitOfWork.Repository<Order>().Delete(existOrder);

                await paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }

            var order = new Order(buyerEmail, shippingAddress,basket.PaymentIntentId ,deliveryMethod, orderItems, subTotal);

            await unitOfWork.Repository<Order>().Add(order);
            var result = await unitOfWork.Complete();
            if (result <= 0)
                return null;

            return order;

        }


        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new Orderspecification(buyerEmail);
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new Orderspecification(orderId, buyerEmail);
             var order = await unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            return order;
        }


        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            return deliveryMethods;
        }
    }
}
