using AutoMapper;
using EdgeProject.APIs.Dtos;
using EdgeProject.APIs.Errors;
using EdgeProject.APIs.Helpers;
using EdgeProject.Core;
using EdgeProject.Core.Entities;
using EdgeProject.Core.Repository;
using EdgeProject.Core.Specifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EdgeProject.APIs.Controllers
{
   
    public class ProductsController : ApiBaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        
        public ProductsController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productSpec)
        {
            var spec = new ProductSpecification(productSpec);
            var products = await unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            var data = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var CountSpec = new ProductWithFilterationForCountSpecification(productSpec);
            var Count = await unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(productSpec.PageIndex,productSpec.PageSize,Count,data));
        }

        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]

        public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProduct(int id)
        {
            var spec = new ProductSpecification(id);
            var product = await unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);
            if (product is null)
                return NotFound(new ApiErrorResponse(404));
            var MappedProduct = mapper.Map<Product,ProductToReturnDto>(product);

            return Ok(MappedProduct);

        }

        [HttpGet("brands")]

        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var Brands = await unitOfWork.Repository<ProductBrand>().GetAllAsync();

            return Ok(Brands);
        }


        [HttpGet("types")]

        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
        {
            var Types = await unitOfWork.Repository<ProductType>().GetAllAsync();

            return Ok(Types);
        }

    }
}
