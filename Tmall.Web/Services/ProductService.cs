using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Components;

namespace Tmall.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _httpClient.GetJsonAsync<Product[]>("api/products");
        }

        public async Task<Product> GetProductByIdTask(int id)
        {
            return await _httpClient.GetJsonAsync<Product>($"api/products/{id}");
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrands()
        {
            return await _httpClient.GetJsonAsync<ProductBrand[]>("api/products/brands");
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypes()
        {
            return await _httpClient.GetJsonAsync<ProductType[]>("api/products/types");
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            return await _httpClient.PutJsonAsync<Product>("api/products/edit", product);
        }
        public async Task<Product> CreateProduct(Product product)
        {
            return await _httpClient.PostJsonAsync<Product>("api/products/add", product);
        }
    }
}
