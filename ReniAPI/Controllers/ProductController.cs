using Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using ReniAPI.Dtos;
using AutoMapper;
using ReniAPI.Errors;
using ReniAPI.Helpers;

namespace ReniAPI.Controllers
{    
    public class ProductController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;

        private readonly IGenericRepository<ProductBrand> _productBrandRepo;

        private readonly IGenericRepository<ProductType> _productTypeRepo;

        private readonly IMapper _mapper;

        public ProductController (IGenericRepository<Product>productsRepo,
        IGenericRepository<ProductBrand>productsBrandRepo, IGenericRepository<ProductType>
        productTypeRepo, IMapper mapper)
        {            
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productsBrandRepo;
            _productsRepo = productsRepo;            
        }

        //We are sending Parameter as a query string and productParams is an object now,
        //Request will start looking to the body and we don't have body when we use http request
        //It will confuse API Controller. You have to tell API to look into property from query string.
        //WE could do that by using  [FromQuery]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
          [FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productParams);    

            var countSpec = new ProductWithTypesAndBrandsSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);    
                
            var products = await _productsRepo.ListAsync(spec);
            
            var data = _mapper
                .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, 
            productParams.PageSize, totalItems, data));
        }

        //[HttpGet("{id}")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);  

            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);

            // return new ProductToReturnDto
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     PictureUrl = product.PictureUrl,
            //     Price = product.Price,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name
            // };
        }

        [HttpGet("{brands}")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

         [HttpGet("{types}")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {            
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}