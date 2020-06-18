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
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProductsAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            return Ok(await _productRepository.GetBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetTypes()
        {
            return Ok(await _productRepository.GetTypesAsync());
        }

        [HttpPost("add")]
        public async Task<ActionResult<Product>> AddProduct(Product product)
        {
            var result= await _productRepository.CreateProductAsync(product);
            return Ok(result);
        }

        [HttpPut("edit")]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            var result= await _productRepository.UpdateProductByIdAsync(product);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public ActionResult DeleteProduct(int id)
        {
            _productRepository.DeleteProductByIdAsync(id);
            return Ok();
        }
    }
} 