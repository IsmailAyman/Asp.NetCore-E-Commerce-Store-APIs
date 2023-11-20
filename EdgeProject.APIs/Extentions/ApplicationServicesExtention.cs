using EdgeProject.APIs.Errors;
using EdgeProject.APIs.Helpers;
using EdgeProject.Core;
using EdgeProject.Core.Repository;
using EdgeProject.Core.Services;
using EdgeProject.Repository;
using EdgeProject.Service;
using Microsoft.AspNetCore.Mvc;

namespace EdgeProject.APIs.Extentions
{
    public static class ApplicationServicesExtention
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped<IOrderServices,OrderSevice>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddAutoMapper(typeof(MappingProfiles));


            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errors = ActionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage).ToArray();
                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });

            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            return Services;

        }
    }
}
