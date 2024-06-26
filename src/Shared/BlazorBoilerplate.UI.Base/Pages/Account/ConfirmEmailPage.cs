﻿using BlazorBoilerplate.Shared.Interfaces;
using BlazorBoilerplate.Shared.Models.Account;
using BlazorBoilerplate.Shared.Providers;
using BlazorBoilerplate.Shared.Services;
using BlazorBoilerplate.UI.Base.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorBoilerplate.UI.Base.Pages.Account
{
    public class ConfirmEmailPage : BaseComponent
    {
        [Inject] AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] protected AppState appState { get; set; }
        protected ConfirmEmailViewModel confirmEmailViewModel { get; set; } = new ConfirmEmailViewModel();

        protected bool disableConfirmButton = false;

        [Parameter]
        public string UserId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            // Blazor does not have query string accessible parameters yet so a hack is needed. Token is too long for path parameter
            var absoluteUrl = navigationManager.Uri;
            var token = absoluteUrl[(absoluteUrl.IndexOf("?token=") + 7)..];

            if (!string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(token))
            {
                disableConfirmButton = true;
                confirmEmailViewModel = new ConfirmEmailViewModel
                {
                    Token = token,
                    UserId = UserId
                };
                await SendConfirmation();
            }
        }

        protected async Task SendConfirmation()
        {
            try
            {
                var apiResponse = await ((IdentityAuthenticationStateProvider)authStateProvider).ConfirmEmail(confirmEmailViewModel);

                if (apiResponse.IsSuccessStatusCode)
                {
                    viewNotifier.Show(apiResponse.Message, ViewNotifierType.Success);

                    navigationManager.NavigateTo("/account/login");
                }
                else
                    viewNotifier.Show(apiResponse.Message, ViewNotifierType.Error);
            }
            catch (Exception ex)
            {
                viewNotifier.Show(ex.Message, ViewNotifierType.Error, L["EmailVerificationFailed"]);
            }
        }
    }
}
