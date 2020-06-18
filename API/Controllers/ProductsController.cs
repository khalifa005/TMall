using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
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

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
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