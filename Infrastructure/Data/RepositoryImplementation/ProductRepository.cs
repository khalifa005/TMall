using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.RepositoryImplementation
{
    public class ProductRepository : IProductRepository
    {
        //replaced by generic repo

        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(p=>p.Id == id);
        }

        public async Task<Product> UpdateProductByIdAsync(Product Currentproduct)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == Currentproduct.Id);
            product.Name = Currentproduct.Name;
            product.Description= Currentproduct.Description;
            product.Price = Currentproduct.Price;
            product.PictureUrl= Currentproduct.PictureUrl;
            product.ProductBrandId = Currentproduct.ProductBrandId;
            product.ProductTypeId = Currentproduct.ProductTypeId;

            _context.SaveChanges();
            return product;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
                  _context.SaveChanges();
            
             return product;
        }

        public void DeleteProductByIdAsync(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            
            if(product == null)return;

            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}
