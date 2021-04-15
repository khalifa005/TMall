using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Core.Specification.SpecificationCases
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecification()
        {
            AddInclude(x=> x.ProductType);
            AddInclude(x=> x.ProductBrand);
        }

        public ProductWithBrandAndTypeSpecification(int id)
        :base(x =>x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}
