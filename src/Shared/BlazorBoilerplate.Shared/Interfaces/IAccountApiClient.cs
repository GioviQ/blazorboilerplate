﻿using BlazorBoilerplate.Shared.Dto;
using BlazorBoilerplate.Shared.Models.Account;

namespace BlazorBoilerplate.Shared.Interfaces
{
    public interface IAccountApiClient
    {
        Task<ApiResponseDto<LoginViewModel>> BuildLoginViewModel(string returnUrl);
        Task<ApiResponseDto<LogoutViewModel>> BuildLogoutViewModel(string logoutId);
        Task<ApiResponseDto<LoginResponseModel>> Login(LoginInputModel parameters);
        Task<ApiResponseDto> LoginWith2fa(LoginWith2faInputModel parameters);
        Task<ApiResponseDto> LoginWithRecoveryCode(LoginWithRecoveryCodeInputModel parameters);
        Task<ApiResponseDto> Create(RegisterViewModel parameters);
        Task<ApiResponseDto<LoginResponseModel>> Register(RegisterViewModel parameters);
        Task<ApiResponseDto> ForgotPassword(ForgotPasswordViewModel parameters);
        Task<ApiResponseDto> ResetPassword(ResetPasswordViewModel parameters);
        Task<ApiResponseDto> UpdatePassword(UpdatePasswordViewModel parameters);
        Task<ApiResponseDto> AdminChangePassword(ChangePasswordViewModel parameters);
        Task<ApiResponseDto<LoggedOutViewModel>> Logout(LogoutViewModel logoutViewModel);
        Task<ApiResponseDto> ConfirmEmail(ConfirmEmailViewModel parameters);
        Task<UserViewModel> GetUserViewModel();
        Task<ApiResponseDto<UserViewModel>> GetUserViewModel(string id);
        Task<ApiResponseDto> UpdateUser(UserViewModel userViewModel);
        Task<ApiResponseDto> UpsertUser(UserViewModel userViewModel);
        Task<ApiResponseDto> DeleteUser(string id);
        Task<ApiResponseDto> DeleteMe();
        Task<ApiResponseDto> AdminUpdateUser(UserViewModel userViewModel);
        Task<UserViewModel> GetUser();

        Task<ApiResponseDto<UserViewModel>> EnableAuthenticator(AuthenticatorVerificationCodeViewModel parameters);
        Task<ApiResponseDto<UserViewModel>> DisableAuthenticator();
        Task<ApiResponseDto<UserViewModel>> ForgetTwoFactorClient();
        Task<ApiResponseDto<UserViewModel>> Enable2fa(string userId = null);
        Task<ApiResponseDto<UserViewModel>> Disable2fa(string userId = null);
    }
}
