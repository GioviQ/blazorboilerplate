﻿//Autogenerated from AdminController.tt
using AutoMapper;
using BlazorBoilerplate.Infrastructure.Server;
using BlazorBoilerplate.Infrastructure.Server.Models;
using BlazorBoilerplate.Infrastructure.Storage.Permissions;
using BlazorBoilerplate.Shared.Dto.Admin;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace BlazorBoilerplate.Server.Controllers
{
    /// <summary>
    /// This controller is the entry API for the platform administration.
    /// </summary>
    [OpenApiIgnore]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMapper _autoMapper;
        private readonly IAdminManager _adminManager;

        public AdminController(IMapper autoMapper, IAdminManager adminManager)
        {
            _autoMapper = autoMapper;
            _adminManager = adminManager;
        }

        [HttpGet("Tenant")]
        public ApiResponse Get()
            => new(Status200OK, string.Empty, _autoMapper.Map<TenantDto>(HttpContext.GetMultiTenantContext<TenantInfo>().TenantInfo));

        [HttpGet("Users")]
        [Authorize(Permissions.User.Read)]
        public async Task<ApiResponse> GetUsers([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
            => await _adminManager.GetUsers(pageSize, pageNumber);

        [HttpGet("Permissions")]
        [Authorize]
        public ApiResponse GetPermissions()
            => _adminManager.GetPermissions();

        #region Roles
        [HttpGet("Roles")]
        [Authorize(Permissions.Role.Read)]
        public async Task<ApiResponse> GetRoles([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
            => await _adminManager.GetRoles(pageSize, pageNumber);

        [HttpGet("Role/{name}")]
        [Authorize(Permissions.Role.Read)]
        public async Task<ApiResponse> GetRole(string name)
            => await _adminManager.GetRole(name);


        [HttpPost("Role")]
        [Authorize(Permissions.Role.Create)]
        public async Task<ApiResponse> CreateRole([FromBody] RoleDto roleDto)
            => await _adminManager.CreateRole(roleDto);


        [HttpPut("Role")]
        [Authorize(Permissions.Role.Update)]
        public async Task<ApiResponse> UpdateRole([FromBody] RoleDto roleDto)
            => await _adminManager.UpdateRole(roleDto);


        [HttpDelete("Role/{name}")]
        [Authorize(Permissions.Role.Delete)]
        public async Task<ApiResponse> DeleteRole(string name)
            => await _adminManager.DeleteRole(name);
        #endregion

        #region Clients
        [HttpGet("Clients")]
        [Authorize(Permissions.Client.Read)]
        public async Task<ApiResponse> GetClients([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
            => await _adminManager.GetClients(pageSize, pageNumber);

        [HttpGet("Client/{clientId}")]
        [Authorize(Permissions.Client.Read)]
        public async Task<ApiResponse> GetClient(string clientId)
            => await _adminManager.GetClient(clientId);


        [HttpPost("Client")]
        [Authorize(Permissions.Client.Create)]
        public async Task<ApiResponse> CreateClient([FromBody] ClientDto clientDto)
            => await _adminManager.CreateClient(clientDto);


        [HttpPut("Client")]
        [Authorize(Permissions.Client.Update)]
        public async Task<ApiResponse> UpdateClient([FromBody] ClientDto clientDto)
            => await _adminManager.UpdateClient(clientDto);


        [HttpDelete("Client/{clientId}")]
        [Authorize(Permissions.Client.Delete)]
        public async Task<ApiResponse> DeleteClient(string clientId)
            => await _adminManager.DeleteClient(clientId);
        #endregion

        #region ApiResources
        [HttpGet("ApiResources")]
        [Authorize(Permissions.ApiResource.Read)]
        public async Task<ApiResponse> GetApiResources([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
            => await _adminManager.GetApiResources(pageSize, pageNumber);

        [HttpGet("ApiResource/{name}")]
        [Authorize(Permissions.ApiResource.Read)]
        public async Task<ApiResponse> GetApiResource(string name)
            => await _adminManager.GetApiResource(name);


        [HttpPost("ApiResource")]
        [Authorize(Permissions.ApiResource.Create)]
        public async Task<ApiResponse> CreateApiResource([FromBody] ApiResourceDto apiResourceDto)
            => await _adminManager.CreateApiResource(apiResourceDto);


        [HttpPut("ApiResource")]
        [Authorize(Permissions.ApiResource.Update)]
        public async Task<ApiResponse> UpdateApiResource([FromBody] ApiResourceDto apiResourceDto)
            => await _adminManager.UpdateApiResource(apiResourceDto);


        [HttpDelete("ApiResource/{name}")]
        [Authorize(Permissions.ApiResource.Delete)]
        public async Task<ApiResponse> DeleteApiResource(string name)
            => await _adminManager.DeleteApiResource(name);
        #endregion

        #region IdentityResources
        [HttpGet("IdentityResources")]
        [Authorize(Permissions.IdentityResource.Read)]
        public async Task<ApiResponse> GetIdentityResources([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
            => await _adminManager.GetIdentityResources(pageSize, pageNumber);

        [HttpGet("IdentityResource/{name}")]
        [Authorize(Permissions.IdentityResource.Read)]
        public async Task<ApiResponse> GetIdentityResource(string name)
            => await _adminManager.GetIdentityResource(name);


        [HttpPost("IdentityResource")]
        [Authorize(Permissions.IdentityResource.Create)]
        public async Task<ApiResponse> CreateIdentityResource([FromBody] IdentityResourceDto identityResourceDto)
            => await _adminManager.CreateIdentityResource(identityResourceDto);


        [HttpPut("IdentityResource")]
        [Authorize(Permissions.IdentityResource.Update)]
        public async Task<ApiResponse> UpdateIdentityResource([FromBody] IdentityResourceDto identityResourceDto)
            => await _adminManager.UpdateIdentityResource(identityResourceDto);


        [HttpDelete("IdentityResource/{name}")]
        [Authorize(Permissions.IdentityResource.Delete)]
        public async Task<ApiResponse> DeleteIdentityResource(string name)
            => await _adminManager.DeleteIdentityResource(name);
        #endregion

        #region Tenants
        [HttpGet("Tenants")]
        [Authorize(Permissions.Tenant.Read)]
        public async Task<ApiResponse> GetTenants([FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0)
            => await _adminManager.GetTenants(pageSize, pageNumber);

        [HttpGet("Tenant/{id}")]
        [Authorize(Permissions.Tenant.Read)]
        public async Task<ApiResponse> GetTenant(string id)
            => await _adminManager.GetTenant(id);


        [HttpPost("Tenant")]
        [Authorize(Permissions.Tenant.Create)]
        public async Task<ApiResponse> CreateTenant([FromBody] TenantDto tenantDto)
            => await _adminManager.CreateTenant(tenantDto);


        [HttpPut("Tenant")]
        [Authorize(Permissions.Tenant.Update)]
        public async Task<ApiResponse> UpdateTenant([FromBody] TenantDto tenantDto)
            => await _adminManager.UpdateTenant(tenantDto);


        [HttpDelete("Tenant/{id}")]
        [Authorize(Permissions.Tenant.Delete)]
        public async Task<ApiResponse> DeleteTenant(string id)
            => await _adminManager.DeleteTenant(id);
        #endregion
    }
}
