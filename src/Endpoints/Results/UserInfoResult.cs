// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Hosting;
using IdentityServer4.Models.ViewModels;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Http;

namespace IdentityServer4.Endpoints.Results
{
    internal class UserInfoResult : IEndpointResult
    {
        public UsersDto Claims;

        public UserInfoResult(UsersDto claims)
        {
            Claims = claims;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.SetNoCache();
            await context.Response.WriteJsonAsync(Claims);
        }
    }
}