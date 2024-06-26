﻿using BlazorBoilerplate.Constants;
using BlazorBoilerplate.Infrastructure.AuthorizationDefinitions;
using BlazorBoilerplate.Infrastructure.Storage.DataModels;
using BlazorBoilerplate.Shared.Models;
using BlazorBoilerplate.Storage;
using Breeze.AspNetCore;
using Breeze.Persistence;
using Breeze.Persistence.EFCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BlazorBoilerplate.Server.Controllers
{
    [Route("api/data/[action]")]
    [Authorize(AuthenticationSchemes = AuthSchemes)]
    [BreezeQueryFilter]
    public class ApplicationController : BaseController
    {
        private readonly IAuthorizationService authorizationService;
        private readonly ApplicationPersistenceManager persistenceManager;
        public ApplicationController(IAuthorizationService authorizationService, ApplicationPersistenceManager persistenceManager)
        {
            this.authorizationService = authorizationService;
            this.persistenceManager = persistenceManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public string Metadata()
        {
            return persistenceManager.Metadata();
        }

        [HttpGet]
        public Task<UserProfile> UserProfile()
        {
            return persistenceManager.GetUserProfile();
        }

        [AllowAnonymous]
        [HttpGet]
        public IQueryable<TenantSetting> TenantSettings()
        {
            return persistenceManager.GetEntities<TenantSetting>().AsNoTracking();
        }

        [HttpGet]
        public IQueryable<ApplicationUser> Users([FromQuery] UserFilter filter)
        {
            return persistenceManager.GetEntities<ApplicationUser>().AsNoTracking().Include(i => i.UserRoles)
                .ThenInclude(i => i.Role)
                .OrderBy(i => i.UserName)
                .Where(u =>
                    (filter.Search == null || u.UserName.Contains(filter.Search) || u.Email.Contains(filter.Search))
                    && (filter.EmailConfirmed == null || u.EmailConfirmed == filter.EmailConfirmed)
                    && (filter.PhoneNumberConfirmed == null || u.PhoneNumberConfirmed == filter.PhoneNumberConfirmed));
        }

        [HttpGet]
        public async Task<IQueryable<AuthenticationTicket>> AuthenticationTickets(Guid? userId)
        {
            if (userId == null || !(await authorizationService.AuthorizeAsync(User, Policies.For(UserFeatures.UserManager))).Succeeded)
                userId = GetUserId();

            return persistenceManager.GetEntities<AuthenticationTicket>().Where(i => i.UserId == userId).AsNoTracking().OrderByDescending(i => i.LastActivity);
        }

        [HttpGet]
        public async Task<IQueryable<ApplicationUser>> LoggedUsers()
        {
            var query = persistenceManager.GetEntities<AuthenticationTicket>().AsNoTracking()
                .Where(i => i.User.UserName != DefaultUserNames.Administrator)
                .Select(i => i.User)
                .Distinct();

            if (!(await authorizationService.AuthorizeAsync(User, Policies.For(UserFeatures.UserManager))).Succeeded)
            {
                var userId = GetUserId();

                query = query.Where(i => i.Id == userId);
            }

            return query
                .Include(i => i.Person)
                .Include(i => i.AuthenticationTickets)
                .OrderBy(i => i.UserName);
        }

        [HttpGet]
        [AuthorizeForFeature(UserFeatures.Administrator)]
        public IQueryable<ApplicationRole> Roles()
        {
            return persistenceManager.GetEntities<ApplicationRole>().AsNoTracking().OrderBy(i => i.Name);
        }

        [HttpGet]
        [AuthorizeForFeature(UserFeatures.UserManager)]
        public IQueryable<Person> People([FromQuery] string filter)
        {
            filter = filter?.ToLower();

            var userId = GetUserId();

            return persistenceManager.Context.Persons
                .Include(i => i.User)
                .Include(i => i.CreatedBy)
                .Where(i => i.User != null && i.User.Id != userId && (filter == null
                || i.FirstName.ToLower().Contains(filter)
                || i.LastName.ToLower().Contains(filter))).AsNoTracking()
                .OrderBy(i => i.LastName)
                .ThenBy(i => i.FirstName);
        }

        [HttpGet]
        [AuthorizeForFeature(UserFeatures.Administrator)]
        public IQueryable<DbLog> Logs()
        {
            return persistenceManager.GetEntities<DbLog>().AsNoTracking().OrderByDescending(i => i.TimeStamp);
        }

        [HttpGet]
        [AuthorizeForFeature(UserFeatures.Administrator)]
        public IQueryable<ApiLogItem> ApiLogs()
        {
            return persistenceManager.GetEntities<ApiLogItem>().AsNoTracking().OrderByDescending(i => i.RequestTime);
        }

        [AllowAnonymous]
        [HttpPost]
        public SaveResult SaveChanges([FromBody] JObject saveBundle)
        {
            try
            {
                return persistenceManager.SaveChanges(saveBundle);
            }
            catch (EntityErrorsException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var errors = new List<EFEntityError>
                {
                    new EFEntityError(null, null, ex.GetBaseException().Message, null)
                };

                throw new EntityErrorsException(errors);
            }
        }
    }
}
