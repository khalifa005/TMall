using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Components;
using Tmall.Web.Services;

namespace Tmall.Web.BaseComponent
{
    public class EditProductBase : ComponentBase
    {
        [Inject]
        private IProductService _ProductService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public Product Product { get; set; } = new Product();

        [Parameter]
        public string Id { get; set; }


        public IReadOnlyList<ProductBrand> ProductBrands { get; set; } = new List<ProductBrand>();
        //public int BrandId { get; set; }

        public IReadOnlyList<ProductType> ProductTypes { get; set; } = new List<ProductType>();

        protected override async Task OnInitializedAsync()
        {
            Product = await _ProductService.GetProductByIdTask(int.Parse(Id));

            ProductBrands = await _ProductService.GetProductBrands();

            ProductTypes = await _ProductService.GetProductTypes();
        }

        protected async Task HandleValidSubmit()
        {
            var result = await _ProductService.UpdateProduct(Product);
            if (result != null)
            {
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
