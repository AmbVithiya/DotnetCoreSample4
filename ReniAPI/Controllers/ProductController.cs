using Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using ReniAPI.Dtos;
using AutoMapper;

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

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductWithTypesAndBrandsSpecification();            
            var products = await _productsRepo.ListAsync(spec);
            
            return Ok(_mapper
                .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        //[HttpGet("{id}")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);  

            var product = await _productsRepo.GetEntityWithSpec(spec);

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