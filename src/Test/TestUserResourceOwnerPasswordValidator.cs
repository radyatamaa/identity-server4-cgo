// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Validation;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.Serivces;

namespace IdentityServer4.Test
{
    /// <summary>
    /// Resource owner password validator for test users
    /// </summary>
    /// <seealso cref="IdentityServer4.Validation.IResourceOwnerPasswordValidator" />
    public class TestUserResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUsersService _usersService;
        private readonly TestUserStore _users;
        private readonly ISystemClock _clock;
        //private readonly IUMStore _uMStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestUserResourceOwnerPasswordValidator"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <param name="clock">The clock.</param>
        public TestUserResourceOwnerPasswordValidator(TestUserStore users, ISystemClock clock,IUsersService usersService)
        {
            _usersService = usersService;
            _users = users;
            _clock = clock;
            //_uMStore = uMStore;
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (await _usersService.ValidateCredentials(context.UserName, context.Password))
            {
                var user = await _usersService.FindByUsername(context.UserName);
                context.Result = new GrantValidationResult(
                    user.SubjectId ?? throw new ArgumentException("Subject ID not set", nameof(user.SubjectId)), 
                    OidcConstants.AuthenticationMethods.Password, _clock.UtcNow.UtcDateTime, 
                    user.Claims);
            }

            //return Task.CompletedTask;
        }
    }
}