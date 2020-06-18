using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Components;

namespace Tmall.Web.BaseComponent
{
    public class DisplayProductsBase : ComponentBase
    {
        [Parameter]
        public Product Product { get; set; }

        [Parameter] 
        public bool ShowFooter { get; set; }

        
        public bool IsSelected { get; set; }

        [Parameter] 
        public EventCallback<bool> OnProductSelection { get; set; }

        public async Task CheckBoxChanged(ChangeEventArgs e)
        {
            IsSelected = (bool)e.Value;
            await OnProductSelection.InvokeAsync(IsSelected);
        }
    }
}
