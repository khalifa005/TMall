using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Components;
using Tmall.Web.Services;

namespace Tmall.Web.BaseComponent
{
    public class ProductBase : ComponentBase
    {
        [Inject] 
        public IProductService ProductService { get; set; }

        public IReadOnlyList<Product> Products { get; set; }

        public bool ShowFooter { get; set; } = true;

        protected int SelectedProductsCount { get; set; } = 0;

        protected void ProductSelectionChanged(bool isSelected)
        {
            if (isSelected)
            {
                SelectedProductsCount++;
            }
            else
            {
                SelectedProductsCount--;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Products = (await ProductService.GetProductsAsync()).ToList();
        }
    }
}
