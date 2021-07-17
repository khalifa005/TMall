using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using System.Resources;
using Khalifa.Framework;
using Syncfusion.Blazor;

namespace Web.BlazorSyncfusion
{
    public class SyncfusionLocalizer : ISyncfusionStringLocalizer
    {
        // To get the locale key from mapped resources file
        public string GetText(string key)
        {
            return this.ResourceManager.GetString(key);
        }

        // To access the resource file and get the exact value for locale key

        public System.Resources.ResourceManager ResourceManager
        {
            get
            {
                return Web.BlazorSyncfusion.Resources.SfResources.ResourceManager;
            }
        }
    }

    //for blazor 
    public class UIComponent : ComponentBase
    {
        [Parameter]
        public SupportedLanguage UserLanguage { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        public ISyncfusionStringLocalizer L { get; set; } = default!;

        [Inject]
        public IMediator Signal { get; set; } = default!;

        public bool IsArabic() => UserLanguage == SupportedLanguage.Arabic;

        public string? WhenCulture(string? nameArabic, string? nameEnglish)
        {
            if (IsArabic())
                return !string.IsNullOrWhiteSpace(nameArabic) ? nameArabic : nameEnglish;
            else
                return !string.IsNullOrWhiteSpace(nameEnglish) ? nameEnglish : nameArabic;
        }

        public SupportedLanguage GetCurrentLanguage()
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture.Name;
            return (currentCulture == "ar") ? SupportedLanguage.Arabic : SupportedLanguage.English;
        }

    }
}
