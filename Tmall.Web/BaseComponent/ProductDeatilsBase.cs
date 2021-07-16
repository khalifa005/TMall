using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Components;
using Tmall.Web.Services;

namespace Tmall.Web.BaseComponent
{
    public class ProductDeatilsBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Parameter] public string Id { get; set; } = "1";

        #region test

        public string MouseCordinates { get; set; }
        public string CssHideClass { get; set; } = null;
        public string BtnText { get; set; } = "Hide Footer";

        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = "Tom";
        protected string Gender { get; set; } = "Male";

        #endregion

        public Product Product { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Product = await ProductService.GetProductByIdTask(int.Parse(Id));
        }

        protected void BtnClick()
        {
            if (BtnText == "Hide Footer")
            {
                CssHideClass = "hide-footer";
                BtnText = "Show Footer";
            }
            else
            {
                CssHideClass = null;
                BtnText = "Hide Footer";
            }
        }
    }
}
