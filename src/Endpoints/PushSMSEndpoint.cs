using IdentityModel;
using IdentityServer4.Endpoints.Results;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using IdentityServer4.Models.ViewModels;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer4.Endpoints
{
    internal class PushSMSEndpoint : IEndpointHandler
    {
        private readonly IClientSecretValidator _clientValidator;
        private readonly ITokenRequestValidator _requestValidator;
        private readonly ITokenResponseGenerator _responseGenerator;
        private readonly IEventService _events;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenEndpoint" /> class.
        /// </summary>
        /// <param name="clientValidator">The client validator.</param>
        /// <param name="requestValidator">The request validator.</param>
        /// <param name="responseGenerator">The response generator.</param>
        /// <param name="events">The events.</param>
        /// <param name="logger">The logger.</param>
        public PushSMSEndpoint(
            IClientSecretValidator clientValidator,
            ITokenRequestValidator requestValidator,
            ITokenResponseGenerator responseGenerator,
            IEventService events,
            ILogger<TokenEndpoint> logger)
        {
            _clientValidator = clientValidator;
            _requestValidator = requestValidator;
            _responseGenerator = responseGenerator;
            _events = events;
            _logger = logger;
        }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            if (!HttpMethods.IsGet(context.Request.Method) && !HttpMethods.IsPost(context.Request.Method))
            {
                _logger.LogWarning("Invalid HTTP method for register endpoint.");
                return new StatusCodeResult(HttpStatusCode.MethodNotAllowed);
            }

            return await ProcessPushSMSRequestAsync(context);
        }
        private async Task<IEndpointResult> ProcessPushSMSRequestAsync(HttpContext context)
        {

            string smsRequest;
            var accountId = "cgoindonesia_4jJ7C_hq";
            var token = "Y7kO26EbCAsXxL0cE4dieE0vUN5cTBjevgB5lVqY4Kk";
            using (var reader = new StreamReader(context.Request.Body))
            {
                smsRequest = reader.ReadToEnd();

                // Do something else
            }
            var message = JsonConvert.DeserializeObject<MessageSMS>(smsRequest);
            using (var handler = new HttpClientHandler())
            {
                //handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                using (var client = new HttpClient(handler))

                {
                    //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    var url = "https://api.twilio.com/2010-04-01/Accounts/AC4cdef007e463c9d7182820528e209542/Messages.json";
                    var dict = new Dictionary<string, string>();
                    dict.Add("To", message.Destination);
                    dict.Add("From", "+12562570821");
                    dict.Add("MessagingServiceSid", "MG0ec4bc5448a4386737c4e6b610aa8977");
                    dict.Add("Body", message.Text);
                    var req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(dict) };
                    req.Headers.Add("Authorization", "Basic QUM0Y2RlZjAwN2U0NjNjOWQ3MTgyODIwNTI4ZTIwOTU0Mjo2ZGE1YTEyY2MwN2JkZWUzNTdiMjU5ZWRkMDdjZjAxOA==");
                    var res = await client.SendAsync(req);
                    //var respon = JsonSerializer.Deserialize<WSO2Response>(resultContent);
                    //Console.WriteLine(resultContent);
                }
            };
            return new SendingSMSResult(message);
        }

        private TokenErrorResult Error(string error, string errorDescription = null, Dictionary<string, object> custom = null)
        {
            var response = new TokenErrorResponse
            {
                Error = error,
                ErrorDescription = errorDescription,
                Custom = custom
            };

            return new TokenErrorResult(response);
        }

        private void LogTokens(TokenResponse response, TokenRequestValidationResult requestResult)
        {
            var clientId = $"{requestResult.ValidatedRequest.Client.ClientId} ({requestResult.ValidatedRequest.Client?.ClientName ?? "no name set"})";
            var subjectId = requestResult.ValidatedRequest.Subject?.GetSubjectId() ?? "no subject";

            if (response.IdentityToken != null)
            {
                _logger.LogTrace("Identity token issued for {clientId} / {subjectId}: {token}", clientId, subjectId, response.IdentityToken);
            }
            if (response.RefreshToken != null)
            {
                _logger.LogTrace("Refresh token issued for {clientId} / {subjectId}: {token}", clientId, subjectId, response.RefreshToken);
            }
            if (response.AccessToken != null)
            {
                _logger.LogTrace("Access token issued for {clientId} / {subjectId}: {token}", clientId, subjectId, response.AccessToken);
            }
        }
    }
}
