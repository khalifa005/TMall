using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification.SpecificationCases
{
    public class ProductWithFilterForCountSpecification : BaseSpecification<Product>
        
    {
        public ProductWithFilterForCountSpecification(ProductSpectParams productParams)
            : base(x =>
            (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search))

            &&(!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId)

             && (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))// to filter with criteria
                                                                                            //(!typeId.HasValue || x.ProductTypeId == typeId) this mean the if the typeId.HasValue execute
        {

        }
    }
}
