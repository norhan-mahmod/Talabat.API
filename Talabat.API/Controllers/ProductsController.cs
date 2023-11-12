using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Dtos;
using Talabat.API.Errors;
using Talabat.API.Helper;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductsController(IUnitOfWork unitOfWork ,
                                  IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        //to make sure that the user authorized and have token which i created form him when login or register
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductReturnDto>>> GetProducts([FromQuery]ProductSpecParams Spec)
        {
            var productSpec = new ProductSpecification(Spec);
            var products = await unitOfWork.Repository<Product>().GetAllWithSpecAsync(productSpec);
            var productsDto = mapper.Map<IReadOnlyList<ProductReturnDto>>(products);

            var countSpec = new ProductWithFilterationForCountSpecification(Spec);
            var count = await unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);
            
            return Ok(new Pagination<ProductReturnDto>(Spec.PageSize,Spec.PageIndex,count, productsDto));
        }

        [HttpGet("{id}")]
        // to make swagger documentation expect all types that might be returned from this endpoint we type =>
        [ProducesResponseType(typeof(ProductReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductReturnDto>> GetProduct(int id)
        {
            var productSpec = new ProductSpecification(id);
            var product = await unitOfWork.Repository<Product>().GetByIdWithSpecAsync(productSpec);
            // var product = await productRepo.GetByIdAsync(id);

            if (product is null)
                return NotFound(new ApiErrorResponse(404));
            var productDto = mapper.Map<ProductReturnDto>(product);
            return Ok(productDto);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var brands = await unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
        {
            var types = await unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }
    }
}
