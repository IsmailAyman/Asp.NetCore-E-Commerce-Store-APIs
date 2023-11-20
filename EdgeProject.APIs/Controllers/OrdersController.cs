using AutoMapper;
using EdgeProject.APIs.Dtos;
using EdgeProject.APIs.Errors;
using EdgeProject.Core.Entities.Order_Aggregation;
using EdgeProject.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using System.Xml;

namespace EdgeProject.APIs.Controllers
{
    [Authorize]
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderServices orderServices;
        private readonly IMapper mapper;

        public OrdersController(IOrderServices orderServices,IMapper mapper)
        {
            this.orderServices = orderServices;
            this.mapper = mapper;
        }
        [ProducesResponseType(typeof(Order),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var address = mapper.Map<AddressDto,Address>(orderDto.ShippingAddress);
            var order = await orderServices.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);

            if (order is null)
                return BadRequest(new ApiErrorResponse(400));

            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await orderServices.GetOrdersForUserAsync(buyerEmail);
            var mappedOrder = mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDto>>(orders);

            return Ok(mappedOrder);

        }
       
        
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]

        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await orderServices.GetOrderByIdForUserAsync(id , buyerEmail);

            if (order is null) return NotFound(new ApiErrorResponse(404));

            var mappedOrder = mapper.Map<Order,OrderToReturnDto>(order);

            return Ok(mappedOrder);
        }

        [HttpGet("{deliverymethods}")]

        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethods = await orderServices.GetDeliveryMethodsAsync();
            return Ok(deliveryMethods);

        } 

    }
}
