using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Tmall.Web.Services
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync();
        
        Task<Product> GetProductByIdTask(int id);

        Task<IReadOnlyList<ProductBrand>> GetProductBrands();

        Task<IReadOnlyList<ProductType>> GetProductTypes();

        Task<Product> UpdateProduct(Product product);
        Task<Product> CreateProduct(Product newProduct);
    }
}
