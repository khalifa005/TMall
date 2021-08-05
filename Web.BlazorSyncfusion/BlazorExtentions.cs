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
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Components.Authorization;
using System.Threading;

namespace Web.BlazorSyncfusion
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

    public class ConfirmMessageContent : UIComponent
    {
        protected bool ShowConfirmation { get; set; }

        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public string Message { get; set; } = string.Empty;

        public void Show()
        {
            ShowConfirmation = true;
            StateHasChanged();
        }

        [Parameter]
        public EventCallback<bool> ConfirmationChanged { get; set; }

        protected async Task OnConfirmationChange(bool value)
        {
            ShowConfirmation = false;
            await ConfirmationChanged.InvokeAsync(value);
        }
    }

   
    public record MessageContent(string Message, MessageType Type);

    public class MessageSlot
    {
        private ConcurrentQueue<MessageContent> Messages { get; set; } = new();

        public void Store(MessageContent content)
        {
            Messages.Enqueue(content);
        }

        public (bool, MessageContent?) Pickup()
        {
            Messages.TryDequeue(out var msg);
            if (msg != null)
                return (true, msg);

            return (false, null);
        }
    }

    public class MessageDisplay
    {
        public string Content { get; set; } = string.Empty;
    }

    public record LogContent(object Message, LogType Type);

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

    public class CommonComponent : UIComponent
    {
        [Inject]
        public IMediator Signal { get; set; } = default!;

        [Inject]
        protected MessageSlot Slot { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        //[Inject]
        //public SystemAuthorization Auth { get; set; } = default!;

        //[CascadingParameter]
        //public UserAccessRights UserAccessRights { get; set; } = default!;

        public string UriAbsolutePath => new Uri(NavigationManager.Uri).AbsolutePath;

        //public bool HasRight(string resource, AuthorizationRight right)
        //{
        //    /// If you are super admin, you pretty much has everything
        //    if (UserInfo is object)
        //    {
        //        //if (UserRoleIds.IsSuperAdmin(UserInfo.RoleId))
        //        //    return true; // todo: uncomment once all rights are set
        //    }

        //    var access = right switch
        //    {
        //        AuthorizationRight.Create => AccessRight.Create,
        //        AuthorizationRight.Delete => AccessRight.Delete,
        //        AuthorizationRight.Execute => AccessRight.Execute,
        //        AuthorizationRight.Read => AccessRight.Read,
        //        AuthorizationRight.Update => AccessRight.Update,
        //        AuthorizationRight.View => AccessRight.View,
        //        AuthorizationRight.Write => AccessRight.Write,
        //        AuthorizationRight.OpenPage => AccessRight.OpenPage,
        //        _ => throw new ArgumentException()
        //    };

        //    var (has, _) = Auth.HasAccess((UriAbsolutePath, resource, access), UserAccessRights);

        //    return has;
        //}

        //public IFluentDisplay DisplayRight(string resource, AuthorizationRight right)
        //{
        //    return HasRight(resource, right) ? Display.Always : Display.None;
        //}

        //public AuthenticatedUserInfo? UserInfo { get; set; }

        //public async Task<bool> InitAuthenticationInfoAsync()
        //{
        //    var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        //    var isAuthenticated = authState.User!.Identity!.IsAuthenticated;
        //    if (isAuthenticated)
        //        UserInfo = new AuthenticatedUserInfo(authState.User.Claims);

        //    return isAuthenticated;
        //}

        public MessageDisplay Message = new();

        public readonly CancellationTokenSource TokenSource = new();

        public Columns Columns { get; set; } = new();



        public void ShowMessage(string? msg, MessageType type)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                //AlertRef?.Hide();
                return;
            }

            Message.Content = msg;
           // Message.Color = MatchColor(type);

           // AlertRef?.Show();
        }

        public void StoreMessage(string? msg, MessageType type)
        {
            if (string.IsNullOrWhiteSpace(msg))
                return;

            var payload = new MessageContent(msg, type);

            Slot.Store(payload);
        }

        /// <summary>
        /// Only call this in OnAfterRender. It won't work on OnInitialized or OnParameterSet
        /// </summary>
        public void PickupMessage(bool stateHasChanged = false)
        {
            var (exists, message) = Slot.Pickup();

            if (exists)
            {
                ShowMessage(message!.Message, message.Type);
                if (stateHasChanged)
                    StateHasChanged();
            }
        }

     

        public bool IsLoading { get; set; }

        public bool IsButtonPressed { get; set; }

        public string? GetYesNoAnswer(bool? val, ISyncfusionStringLocalizer l)
        {
            if (val.HasValue)
            {
                return val.Value == true ? l.GetText("Yes") : l.GetText("No");
            }

            return l.GetText("Yes");
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
