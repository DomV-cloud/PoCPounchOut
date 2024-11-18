using Microsoft.AspNetCore.Mvc;
using Template.Api.Filters;
using Template.Application.Services.Authentication;
using Template.Application.Services.MappingService;
using Template.Infrastructure.Interfaces;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    [ErrorHandlingFilter]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly ICxmlMappingService _cxmlMappingService;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            ILogger<AuthenticationController> logger,
            ICxmlMappingService cxmlMappingService)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _cxmlMappingService = cxmlMappingService;
        }

        /// <summary>
        /// We need to substract "Secret ID" and authenticate user via SID
        /// </summary>
        /// <param name="reponse"></param>
        /// <returns></returns>
        [HttpPost("punchOut", Name = "punchOut")]
        public async Task<IActionResult> LoginPunchOut()
        {
            // getting raw Request content
            Request.EnableBuffering();

            Request.Body.Position = 0;

            var rawRequestBody = await new StreamReader(Request.Body).ReadToEndAsync();

            // map cXML values SID
            var parsedSiD = _cxmlMappingService.ParseSecretId(rawRequestBody);

            var parsedCookie = _cxmlMappingService.ParseBuyerCookie(rawRequestBody);

            var parsedUrl = _cxmlMappingService.ParseUrl(rawRequestBody);

            // send request to database and
            //check if user with this SID exists
            var response = _authenticationService.Login(parsedSiD, parsedCookie, parsedUrl);

            return Content(response, "application/xml");
        }
    }
}
