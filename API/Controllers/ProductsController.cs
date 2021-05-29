using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using Application.MediatorHandlers.ProductHandlers;
using AutoMapper;
using Core.Entities;
using Core.Repository;
using Core.Specification.SpecificationCases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseApiController
    {
        //IProductrepo is replaced by teh generic repo

        //so rather than injecting one repo IProductrepo now we are injecting 3 repo so later we will
        //use unit of work pattern  used to solve this constructor injection

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IMapper _mapper;

        public ProductsController(
            IGenericRepository<Product> productRepository, 
            IGenericRepository<ProductType> productTypeRepository,
            IGenericRepository<ProductBrand> productBrandRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _productBrandRepository = productBrandRepository;
            _mapper = mapper;
        }

        [HttpGet("test/mediator/{id}")]
        public async Task<ActionResult<GetProduct.Response>> GetProductByIdMediator(int id)
        {
            return await Mediator.Send(new GetProduct.Request { Id = id });
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts()
        {
            var spect = new ProductWithBrandAndTypeSpecification();

            var products = await _productRepository.GetListOfEntitiesWithSpectAsync(spect);

            //return products.Select(product => new ProductDto(product)).ToList();
            return Ok(
                _mapper.Map<IReadOnlyList<Product>,
                IReadOnlyList<ProductDto>>(products));
            
        }

        [HttpGet("{id}")]
        [ProducesResponseType( typeof(ApiResponses), StatusCodes.Status200OK)]
        [ProducesResponseType( typeof(ApiResponses), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            //1 spec
            var spec = new ProductWithBrandAndTypeSpecification(id);
            //3 spec
            var product = await _productRepository.GetEntityWithSpecAsync(spec);

            if(product is null)
                return NotFound(new ApiResponses(StatusCodes.Status404NotFound));
            //return new ProductDto(product); //this is a recommended technique to have better control
            return _mapper.Map<Product, ProductDto>(product);
        }

       

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            //can't return IReadOnlyList directlly so we meed to wrap it with ok response
            return Ok(await _productBrandRepository.GetAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetTypes()
        {
            return Ok(await _productTypeRepository.GetAllAsync());
        }

        //[HttpPost("add")]
        //public async Task<ActionResult<Product>> AddProduct(Product product)
        //{
        //    var result= await _productRepository.CreateProductAsync(product);
        //    return Ok(result);
        //}

        //[HttpPut("edit")]
        //public async Task<ActionResult<Product>> UpdateProduct(Product product)
        //{
        //    var result= await _productRepository.UpdateProductByIdAsync(product);
        //    return Ok(result);
        //}

        //[HttpDelete("delete")]
        //public ActionResult DeleteProduct(int id)
        //{
        //    _productRepository.DeleteProductByIdAsync(id);
        //    return Ok();
        //}
    }
} 