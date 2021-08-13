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
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading;
using Web.BlazorServer;

namespace Web.BlazorServer
{
    public enum ComponentInputMode
    {
        Create,
        Update,
        Mixed
    }
    public enum MessageType
    {
        Info,
        Warning,
        Error,
        Success
    }
    public enum LogType
    {
        Normal,
        Warn,
        Info,
        Debug,
        Error
    }

    public class Columns
    {
        readonly Dictionary<int, int> Cols = new();

        public int this[int i] => Cols[i];

        public Columns()
        {

        }

        public Columns(params int[] cols)
        {
            Init(cols);
        }

        public void Init(params int[] cols)
        {
            var col = 0;
            foreach (var c in cols)
            {
                col++;
                Cols.Add(col, c);
            }
        }

        public void Resize(params int[] cols)
        {
            if (cols.Length != Cols.Count)
                throw new ArgumentException("Resize cols must be the same size of existing columns");

            var col = 0;
            foreach (var c in cols)
            {
                col++;
                Cols[col] = c;
            }
        }
    }


    public record LogContent(object Message, LogType Type);

   

    //for blazor 
    public class UIComponent : ComponentBase
    {
        [Parameter]
        public SupportedLanguage UserLanguage { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        public IStringLocalizer<App> L { get; set; } = default!; //L for localizer

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
            UserLanguage = (currentCulture == "ar") ? SupportedLanguage.Arabic : SupportedLanguage.English;
            return (currentCulture == "ar") ? SupportedLanguage.Arabic : SupportedLanguage.English;
        }

    }

    public class CommonComponent : UIComponent
    {
        [Inject]
        public IMediator Signal { get; set; } = default!;

        //[Inject]
        //protected MessageSlot Slot { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        //[Inject]
        //public SystemAuthorization Auth { get; set; } = default!;

        //[CascadingParameter]
        //public UserAccessRights UserAccessRights { get; set; } = default!;

        public string UriAbsolutePath => new Uri(NavigationManager.Uri).AbsolutePath;

     
        public readonly CancellationTokenSource TokenSource = new();

        public Columns Columns { get; set; } = new();




        public bool IsLoading { get; set; }

        public bool IsButtonPressed { get; set; }

        public string? GetYesNoAnswer(bool? val, IStringLocalizer<App> l)
        {
            if (val.HasValue)
            {
                return val.Value == true ? l["Yes"] : l["No"];
            }

            return l["Yes"];
        }

    }

    public class InputComponent : CommonComponent
    {
        public ComponentInputMode Mode { get; set; } = ComponentInputMode.Create;

        [Parameter]
        public string OnSuccessRedirectUrl { get; set; } = string.Empty;

        [Parameter]
        public string OnNotFoundRedirectUrl { get; set; } = string.Empty;
    }
}
