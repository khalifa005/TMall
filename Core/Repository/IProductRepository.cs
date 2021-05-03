using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Repository
{
    public interface IProductRepository
    {
        //replaced by generic repo
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(int id);

        Task<Product> UpdateProductByIdAsync(Product product);
        Task<Product> CreateProductAsync(Product product);
        void DeleteProductByIdAsync(int id);

        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetTypesAsync();
    }
}
