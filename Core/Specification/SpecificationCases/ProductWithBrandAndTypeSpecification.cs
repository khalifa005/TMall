using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Core.Specification.SpecificationCases
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecification(ProductSpectParams productParams)//sort from query string
            : base(x =>
             (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search))
            &&
             (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) 
            && (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId))// to filter with criteria
                                                               //(!typeId.HasValue || x.ProductTypeId == typeId) this mean the if the typeId.HasValue execute => x.ProductTypeId == typeId and it will be added to the criteria
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);

            AddOrderBy(x => x.Name);

            ApplyPaging(productParams.PageSize * (productParams.PageIndex -1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(x => x.Price);
                        break;

                    case "priceDesc":
                        AddOrderByDesc(x => x.Price);
                        break;

                    default:
                        AddOrderBy(x => x.Name);
                        break;
                }
            }

        }

        public ProductWithBrandAndTypeSpecification(int id)
        : base(x => x.Id == id)
        {
            //2 spec
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }


    }
}
