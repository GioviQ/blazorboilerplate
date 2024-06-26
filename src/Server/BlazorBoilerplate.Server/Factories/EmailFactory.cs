﻿using BlazorBoilerplate.Infrastructure.Server;
using BlazorBoilerplate.Shared.Dto.Email;
using FormatWith;
using Microsoft.Extensions.Localization;

namespace BlazorBoilerplate.Server.Factories
{
    public class EmailFactory : IEmailFactory
    {
        protected readonly IStringLocalizer<EmailFactory> L;
        private readonly string baseUrl;
        public EmailFactory(IStringLocalizer<EmailFactory> l, IConfiguration configuration)
        {
            L = l;
            baseUrl = configuration[$"{nameof(BlazorBoilerplate)}:ApplicationUrl"];
        }

        public EmailMessageDto BuildTestEmail(string recipient)
        {
            var emailMessage = new EmailMessageDto
            {
                Body = L["TestEmail.template"].Value
                .FormatWith(new { baseUrl, user = recipient, testDate = DateTime.Now }),

                //emailMessage.Subject = L["TestEmail.subject", recipient];
                Subject = $"Test email from {baseUrl}"
            };
            emailMessage.Body = "Test email completed.";

            return emailMessage;
        }
        public EmailMessageDto GetPlainTextTestEmail(DateTime date)
        {
            var emailMessage = new EmailMessageDto
            {
                Body = L["PlainTextTestEmail.template"].Value
                .FormatWith(new { date })
            };

            emailMessage.Subject = L["PlainTextTestEmail.subject", emailMessage.ToAddresses[0].Name];

            emailMessage.IsHtml = false;

            return emailMessage;
        }
        public EmailMessageDto BuildNewUserConfirmationEmail(string fullName, string userName, string callbackUrl)
        {
            var emailMessage = new EmailMessageDto
            {
                Body = L["NewUserConfirmationEmail.template"].Value
                .FormatWith(new { baseUrl, name = fullName, userName, callbackUrl }),

                Subject = L["NewUserConfirmationEmail.subject", fullName]
            };

            return emailMessage;
        }
        public EmailMessageDto BuildNewUserEmail(string fullName, string userName, string emailAddress, string password)
        {
            var emailMessage = new EmailMessageDto
            {
                Body = L["NewUserEmail.template"].Value
                .FormatWith(new { baseUrl, fullName = userName, userName, email = emailAddress, password }),

                Subject = L["NewUserEmail.subject", fullName]
            };

            return emailMessage;
        }
        public EmailMessageDto BuilNewUserNotificationEmail(string creator, string name, string userName, string company, string roles)
        {
            var emailMessage = new EmailMessageDto
            {
                //placeholder not actually implemented

                Body = L["NewUserNotificationEmail.template"].Value
                .FormatWith(new { baseUrl, creator, name, userName, roles, company }),

                Subject = L["NewUserNotificationEmail.subject", userName]
            };

            return emailMessage;
        }
        public EmailMessageDto BuildForgotPasswordEmail(string name, string callbackUrl, string token)
        {
            var emailMessage = new EmailMessageDto
            {
                Body = L["ForgotPassword.template"].Value
                .FormatWith(new { baseUrl, name, callbackUrl, token }),

                Subject = L["ForgotPassword.subject", name]
            };

            return emailMessage;
        }
        public EmailMessageDto BuildPasswordResetEmail(string userName)
        {
            var emailMessage = new EmailMessageDto
            {
                Body = L["PasswordReset.template"].Value
                .FormatWith(new { baseUrl, userName }),

                Subject = L["PasswordReset.subject", userName]
            };

            return emailMessage;
        }

        public EmailMessageDto BuildNewAccessEmail(string fullName, string userName, string ipAddress, DateTimeOffset date)
        {
            var emailMessage = new EmailMessageDto
            {
                Body = L["NewAccessEmail.template"].Value
                .FormatWith(new { baseUrl, fullName, userName, ipAddress, date }),

                Subject = L["NewAccessEmail.subject", ipAddress, date]
            };

            return emailMessage;
        }
    }
}
