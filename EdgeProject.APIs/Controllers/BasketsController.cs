using AutoMapper;
using EdgeProject.APIs.Dtos;
using EdgeProject.APIs.Errors;
using EdgeProject.Core.Entities;
using EdgeProject.Core.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdgeProject.APIs.Controllers
{
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketsController(IBasketRepository basketRepository, IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
        {
            var basket = await basketRepository.GetBasketAsync(id);

            return basket is null ? new CustomerBasket(id) : basket;
        }
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var createdOrUpdated = await basketRepository.UpdateBasketAsync(mappedBasket);

            if (createdOrUpdated is null)
                return BadRequest(new ApiErrorResponse(400));

            return Ok(createdOrUpdated);

        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await basketRepository.DeleteBasketAsync(id);
        }
    }
}
