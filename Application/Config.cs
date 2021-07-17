using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{

    public enum SupportedLanguage
    {
        None,
        Arabic,
        English,
        French
    }

    public static class SupportedLanguageExtensions
    {
        public static string Code(this SupportedLanguage self)
        {
            return self switch
            {
                SupportedLanguage.Arabic => "ar",
                SupportedLanguage.English => "en-US",
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static SupportedLanguage ToSupportedLang(this string self)
        {
            return self switch
            {
                "ar" => SupportedLanguage.Arabic,
                "en-US" => SupportedLanguage.English,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
    
    public class UIComponent : ComponentBase
    {
        [Parameter]
        public SupportedLanguage UserLanguage { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        public IStringLocalizer<Global> T { get; set; } = default!;

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
