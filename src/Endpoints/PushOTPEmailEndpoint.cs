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
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace IdentityServer4.Endpoints
{
    internal class PushOTPEmailEndpoint : IEndpointHandler
    {
        private readonly IUsersService _usersService;
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
        public PushOTPEmailEndpoint(
            BearerTokenUsageValidator tokenUsageValidator,
            IUserInfoRequestValidator requestValidator,
            IUserInfoResponseGenerator responseGenerator,
            ILogger<RegisterEndpoint> logger,
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
                _logger.LogWarning("Invalid HTTP method for register endpoint.");
                return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            return await ProcessPushToEmailRequestAsync(context);
        }


        private async Task<IEndpointResult> ProcessPushToEmailRequestAsync(HttpContext context)
        {
            _logger.LogDebug("Start register request");
            string users;
            using (var reader = new StreamReader(context.Request.Body))
            {
                users = reader.ReadToEnd();

                // Do something else
            }

            var message = JsonConvert.DeserializeObject<MessageEmail>(users);

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(message.From, "CGO Indonesia");
            var subject = message.Subject;
            var to = new EmailAddress(message.To, "Example User");
            var plainTextContent = message.Message;
            var htmlContent = "<strong> " + message.Message + "</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var respons = await client.SendEmailAsync(msg);



            _logger.LogDebug("End register request");
            return new SendingEmailResult(message);
        }

        private IEndpointResult Error(string error, string description = null)
        {
            return new ProtectedResourceErrorResult(error, description);
        }
    }
}
