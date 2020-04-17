using IdentityServer4.Hosting;
using IdentityServer4.Models;
using IdentityServer4.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints.Results
{
    internal class CreateRoles : IEndpointResult
    {
        public RolesForm Users;

        public CreateRoles(RolesForm users)
        {
            Users = users;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.SetNoCache();
            await context.Response.WriteJsonAsync(Users);
        }
    }
}
