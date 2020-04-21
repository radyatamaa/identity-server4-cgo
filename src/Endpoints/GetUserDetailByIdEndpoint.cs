// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Threading.Tasks;
using IdentityServer4.Validation;
using IdentityServer4.ResponseHandling;
using Microsoft.Extensions.Logging;
using IdentityServer4.Hosting;
using IdentityServer4.Endpoints.Results;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using System.Net;
using Newtonsoft.Json;
using System;
using IdentityServer4.Serivces;
using IdentityServer4.Models.ViewModels;

namespace IdentityServer4.Endpoints
{
    /// <summary>
    /// The userinfo endpoint
    /// </summary>
    /// <seealso cref="IdentityServer4.Hosting.IEndpointHandler" />
    internal class GetUserDetailByIdEndpoint : IEndpointHandler
    {
        private readonly IUsersService _usersService;
        private readonly BearerTokenUsageValidator _tokenUsageValidator;
        private readonly IUserInfoRequestValidator _requestValidator;
        private readonly IUserInfoResponseGenerator _responseGenerator;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoEndpoint" /> class.
        /// </summary>
        /// <param name="tokenUsageValidator">The token usage validator.</param>
        /// <param name="requestValidator">The request validator.</param>
        /// <param name="responseGenerator">The response generator.</param>
        /// <param name="logger">The logger.</param>
        public GetUserDetailByIdEndpoint(
            BearerTokenUsageValidator tokenUsageValidator,
            IUserInfoRequestValidator requestValidator,
            IUserInfoResponseGenerator responseGenerator,
            ILogger<UserInfoEndpoint> logger,
            IUsersService usersService)
        {
            _usersService = usersService;
            _tokenUsageValidator = tokenUsageValidator;
            _requestValidator = requestValidator;
            _responseGenerator = responseGenerator;
            _logger = logger;
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (!HttpMethods.IsGet(context.Request.Method) && !HttpMethods.IsPost(context.Request.Method))
            {
                _logger.LogWarning("Invalid HTTP method for userinfo endpoint.");
                return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            return await ProcessGetUserByIdRequestAsync(context);
        }

        private async Task<IEndpointResult> ProcessGetUserByIdRequestAsync(HttpContext context)
        {
            _logger.LogDebug("Start userinfo request");

            // userinfo requires an access token on the request
            var tokenUsageResult = await _tokenUsageValidator.ValidateAsync(context);
            if (tokenUsageResult.TokenFound == false)
            {
                var error = "No access token found.";

                _logger.LogError(error);
                return Error(OidcConstants.ProtectedResourceErrors.InvalidToken);
            }

            // validate the request
            _logger.LogTrace("Calling into userinfo request validator: {type}", _requestValidator.GetType().FullName);
            var validationResult = await _requestValidator.ValidateRequestAsync(tokenUsageResult.Token);

            if (validationResult.IsError)
            {
                //_logger.LogError("Error validating  validationResult.Error);
                return Error(validationResult.Error);
            }

            // generate response
            _logger.LogTrace("Calling into userinfo response generator: {type}", _responseGenerator.GetType().FullName);
            var response = await _responseGenerator.ProcessAsync(validationResult);
            var id = response.Values;
            var serelizeArrayId = JsonConvert.SerializeObject(id);
            serelizeArrayId = serelizeArrayId.Replace("[", "");
            serelizeArrayId = serelizeArrayId.Replace("]", "");
            serelizeArrayId = serelizeArrayId.Replace("\"", "");
            var form = (await context.Request.ReadFormAsync()).AsNameValueCollection();
            var userID = form.Get("id");
            var isDetail = form.Get("isDetail");
            var respon = new UsersDto();
            if (isDetail != null && isDetail != "")
            {
                respon = await _usersService.GetByDetail(userID,true);
            }
            else
            {
                respon = await _usersService.GetByDetail(userID, false);
            }
            _logger.LogDebug("End userinfo request");
            return new UserInfoResult(respon);
        }

        private IEndpointResult Error(string error, string description = null)
        {
            return new ProtectedResourceErrorResult(error, description);
        }
    }
}