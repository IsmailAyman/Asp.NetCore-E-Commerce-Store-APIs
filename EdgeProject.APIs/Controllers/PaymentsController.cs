using EdgeProject.APIs.Dtos;
using EdgeProject.APIs.Errors;
using EdgeProject.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdgeProject.APIs.Controllers
{
    [Authorize]
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [ProducesResponseType(typeof(CustomerBasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse),StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]

        public async Task<ActionResult<CustomerBasketDto>> CreatOrUpdatePaymentIntent(string basketId)
        {
            var basket = await paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket is null) return BadRequest(new ApiErrorResponse(400, "A problem with your basket"));
            
            return Ok(basket);
        }
    }
}
