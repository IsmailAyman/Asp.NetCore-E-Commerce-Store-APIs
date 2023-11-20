using EdgeProject.Core;
using EdgeProject.Core.Entities;
using EdgeProject.Core.Entities.Order_Aggregation;
using EdgeProject.Core.Repository;
using EdgeProject.Core.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = EdgeProject.Core.Entities.Product;

namespace EdgeProject.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            this.configuration = configuration;
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];

            var basket = await basketRepository.GetBasketAsync(basketId);

            if (basket is null) return null;

            var shippingPrice = 0m;
            if (basket.DeliveryMethodsId.HasValue)
            {
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodsId.Value);
                shippingPrice = deliveryMethod.Cost;
                basket.ShippingCost = deliveryMethod.Cost;
            }

            if(basket?.Items?.Count > 0)
            {
                foreach( var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if(string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long) shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }    
                };
                paymentIntent = await service.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await basketRepository.UpdateBasketAsync(basket);

            return basket;

        }
    }
}
