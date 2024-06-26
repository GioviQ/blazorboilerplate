﻿using BlazorBoilerplate.Shared.Interfaces;
using BlazorBoilerplate.Shared.Models.Account;
using BlazorBoilerplate.Shared.Providers;
using BlazorBoilerplate.Shared.Services;
using BlazorBoilerplate.UI.Base.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorBoilerplate.UI.Base.Pages.Account
{
    public class ResetPasswordPage : BaseComponent
    {
        [Inject] AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] protected AppState appState { get; set; }

        protected ResetPasswordViewModel resetPasswordViewModel { get; set; } = new ResetPasswordViewModel();

        [Parameter]
        public string UserId { get; set; }

        protected override void OnInitialized()
        {
            // Blazor does not have query string accessible parameters yet so a hack is needed. Token is too long for path parameter
            var absoluteUrl = navigationManager.Uri;
            var token = absoluteUrl[(absoluteUrl.IndexOf("?token=") + 7)..];

            if (!string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(token))
            {
                resetPasswordViewModel.Token = token;
                resetPasswordViewModel.UserId = UserId;
            }
            else
            {
                viewNotifier.Show("Your url is missing the Reset Token. Fatal Error", ViewNotifierType.Error, "Reset Token is Missing");
            }
        }

        protected async Task SendResetPassword()
        {
            try
            {
                var apiResponse = await ((IdentityAuthenticationStateProvider)authStateProvider).ResetPassword(resetPasswordViewModel);

                if (apiResponse.IsSuccessStatusCode)
                {
                    viewNotifier.Show(L["ResetPasswordSuccessful"], ViewNotifierType.Success);
                    navigationManager.NavigateTo(Constants.Settings.LoginPath);
                }
                else
                {
                    viewNotifier.Show(apiResponse.Message, ViewNotifierType.Error, L["ResetPasswordFailed"]);
                }
            }
            catch (Exception ex)
            {
                viewNotifier.Show(ex.Message, ViewNotifierType.Error, L["ResetPasswordFailed"]);
            }
        }
    }
}
