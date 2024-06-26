﻿using BlazorBoilerplate.Shared;
using BlazorBoilerplate.Shared.Dto;
using BlazorBoilerplate.Shared.Extensions;
using BlazorBoilerplate.Shared.Interfaces;
using BlazorBoilerplate.Shared.Models.Account;
using BlazorBoilerplate.Shared.Providers;
using BlazorBoilerplate.Shared.Services;
using BlazorBoilerplate.UI.Base.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorBoilerplate.UI.Base.Pages.Account
{
    public class LoginWith2faPage : BaseComponent
    {
        [Inject] AuthenticationStateProvider authStateProvider { get; set; }
        [Inject] protected AppState appState { get; set; }

        string navigateTo = null;
        IdentityAuthenticationStateProvider identityAuthenticationStateProvider;
        protected bool forgotAuthenticatorToggle = false;
        protected LoginWith2faInputModel loginViewModel { get; set; } = new LoginWith2faInputModel();
        protected LoginWithRecoveryCodeInputModel forgotAuthenticatorInputModel { get; set; } = new LoginWithRecoveryCodeInputModel();

        string ReturnUrl;

        protected override void OnInitialized()
        {
            if (navigationManager.TryGetQueryString("ReturnUrl", out string url))
                ReturnUrl = url;

            identityAuthenticationStateProvider = (IdentityAuthenticationStateProvider)authStateProvider;
        }

        protected override async Task OnParametersSetAsync()
        {
            if (AppState.Runtime == BlazorRuntime.WebAssembly)
            {
                var user = (await authenticationStateTask).User;

                if (user.Identity.IsAuthenticated && navigateTo != null)
                    navigationManager.NavigateTo(navigateTo);
            }
        }

        protected async Task SubmitLogin()
        {
            try
            {
                loginViewModel.ReturnUrl = ReturnUrl;
                var response = await identityAuthenticationStateProvider.LoginWith2fa(loginViewModel);

                await PostLoginProcess(response);
            }
            catch (Exception ex)
            {
                viewNotifier.Show(ex.Message, ViewNotifierType.Error, L["LoginFailed"]);
            }
        }

        protected async Task ForgotAuthenticator()
        {
            try
            {
                forgotAuthenticatorInputModel.ReturnUrl = ReturnUrl;
                var response = await identityAuthenticationStateProvider.LoginWithRecoveryCode(forgotAuthenticatorInputModel);

                await PostLoginProcess(response);
            }
            catch (Exception ex)
            {
                viewNotifier.Show(ex.Message, ViewNotifierType.Error, L["LoginFailed"]);
            }
        }

        async Task PostLoginProcess(ApiResponseDto response)
        {
            if (response.IsSuccessStatusCode)
            {
                if (AppState.Runtime == BlazorRuntime.WebAssembly)
                {
                    if (string.IsNullOrEmpty(ReturnUrl))
                    {
                        var userProfile = await appState.GetUserProfile();

                        navigateTo = navigationManager.BaseUri + (!string.IsNullOrEmpty(userProfile?.LastPageVisited) ? userProfile?.LastPageVisited : "/dashboard");
                    }
                    else
                        navigateTo = ReturnUrl;

                    navigationManager.NavigateTo(navigateTo);
                }
            }
            else
                viewNotifier.Show(response.Message, ViewNotifierType.Error, L["LoginFailed"]);
        }
    }
}
