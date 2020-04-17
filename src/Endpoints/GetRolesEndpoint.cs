using System.Threading.Tasks;
using IdentityServer4.Validation;
using IdentityServer4.ResponseHandling;
using Microsoft.Extensions.Logging;
using IdentityServer4.Hosting;
using IdentityServer4.Endpoints.Results;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using System.Net;
using IdentityServer4.Serivces;
using System.IO;
using Newtonsoft.Json;
using IdentityServer4.Models;
using IdentityServer4.Models.ViewModels;
using System;
using IdentityServer4.Services;

namespace IdentityServer4.Endpoints
{
    internal class GetRolesEndpoint : IEndpointHandler
    {
        private readonly IRolesService _roleService;
        private readonly BearerTokenUsageValidator _tokenUsageValidator;
        private readonly IUserInfoRequestValidator _requestValidator;
        private readonly IUserInfoResponseGenerator _responseGenerator;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterEndpoint" /> class.
        /// </summary>
        /// <param name="tokenUsageValidator">The token usage validator.</param>
        /// <param name="requestValidator">The request validator.</param>
        /// <param name="responseGenerator">The response generator.</param>
        /// <param name="logger">The logger.</param>
        public GetRolesEndpoint(
            BearerTokenUsageValidator tokenUsageValidator,
            IUserInfoRequestValidator requestValidator,
            IUserInfoResponseGenerator responseGenerator,
            ILogger<RegisterEndpoint> logger,
            IRolesService rolesService)
        {
            _roleService = rolesService;
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
                _logger.LogWarning("Invalid HTTP method for register endpoint.");
                return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            return await ProcessGetRolesRequestAsync(context);
        }


        private async Task<IEndpointResult> ProcessGetRolesRequestAsync(HttpContext context)
        {
            _logger.LogDebug("Start register request");
            var form = (await context.Request.ReadFormAsync()).AsNameValueCollection();
            var type = form.Get("type");
            try
            {
                //var userForm = JsonConvert.DeserializeObject<UsersForm>(users);

                var response = await _roleService.GetRolesByType(int.Parse(type));


                _logger.LogDebug("End register request");
                return new ListRolesResult(response);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private IEndpointResult Error(string error, string description = null)
        {
            return new ProtectedResourceErrorResult(error, description);
        }
    }
}
