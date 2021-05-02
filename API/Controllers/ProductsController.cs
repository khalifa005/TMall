using System.Collections.Generic;
using System.Threading.Tasks;
using Application.MediatorHandlers.ProductHandlers;
using Core.Entities;
using Core.Repository;
using Core.Specification.SpecificationCases;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;

        public ProductsController(
            IGenericRepository<Product> productRepository, 
            IGenericRepository<ProductType> productTypeRepository,
            IGenericRepository<ProductBrand> productBrandRepository)
        {
            _productRepository = productRepository;
            _productTypeRepository = productTypeRepository;
            _productBrandRepository = productBrandRepository;
        }

        [HttpGet("test/mediator/{id}")]
        public async Task<ActionResult<GetProduct.Response>> GetProductByIdMediator(int id)
        {
            return await Mediator.Send(new GetProduct.Request { Id = id });
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var spect = new ProductWithBrandAndTypeSpecification();
            var products = await _productRepository.GetListOfEntitiesWithSpectAsync(spect);

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);
            return await _productRepository.GetEntityWithSpecAsync(spec);
        }

       

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            //cann't return IReadOnlyList directlly so we meed to wrap it with ok response
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