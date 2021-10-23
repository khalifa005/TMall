using Application.MediatorHandlers.ProductHandlers;
using Core.Specification.SpecificationCases;
using Khalifa.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class ProducrMediatorController : BaseApiController
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]

        public async Task<ActionResult<GetProduct.Response>> GetProductById(int id)
        {
            return await Mediator.Send(new GetProduct.Request { Id = id });
        }



        [HttpGet]
        [ProducesResponseType(typeof(GetProductsWithPagining.Response), StatusCodes.Status200OK)]
        public async Task<GetProductsWithPagining.Response> GetProductsWithPaging(string name, string sort, int brandId, CancellationToken cancellation)
        {
            var filterUI = ProductFilterUI.Default();

            filterUI.BrandId = brandId > 0 ? brandId : Config.AllOptionValueShort;
            filterUI.ProductName = name;

            var filter = filterUI.GetFilter();

            var sorter = ProductSorter.ByCreateDateDesc();

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "priceAsc":
                        sorter = ProductSorter.ByPricrAsc();
                        break;

                    case "priceDesc":
                        sorter = ProductSorter.ByPricrDesc();
                        break;

                    default:
                        sorter = ProductSorter.ByCreateDateDesc();
                        break;
                }
            }

            return await Mediator.Send(new GetProductsWithPagining.Request(filter, sorter, Page: 1, PageSize: 2), cancellation);
        }
    
    }
}
