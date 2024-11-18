using System.Xml.Serialization;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Persistance;
using Template.Contracts.PunchOut;
using Template.Domain.Entities.Punchout;
using Template.Infrastructure.Interfaces;

namespace Template.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(
            IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public string Login(string secretId, string buyerCookie, string url)
        {
            //would be better to implement Mapping service here!!!

            // also check buyerCookie
            if (string.IsNullOrWhiteSpace(secretId))
            {
                return GeneratePunchOutErrorResponse("400", "XML cannot be parsed or is invalid", "POST XML is not valid: ADDITIONAL_INFO");
            }

            var user = _userRepository.GetUserBySecretId(secretId);

            if (user is null)
            {
                return GeneratePunchOutErrorResponse("401", "Bad Request", "Sender identity or shared secret is invalid");
            }

            return GeneratePunchOutSuccessResponse(buyerCookie, url, "200", "Successfull login");
        }

        // classes are prepared, this is temp usage
        private static string GeneratePunchOutErrorResponse(string? code = "", string? text = "", string? message = "")
        {
            return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
                <!DOCTYPE cXML SYSTEM ""http://xml.cXML.org/schemas/cXML/1.2.040/cXML.dtd"">
                <cXML payloadID=""{Guid.NewGuid()}@example.com"" timestamp=""{DateTime.UtcNow:yyyy-MM-ddTHH:mm:sszzz}"">
                    <Response>
                        <Status code=""{code}"" text=""{text}"">
                            {(message)}
                        </Status>";
        }

        private static string GeneratePunchOutSuccessResponse(string buyerCookie, string url, string? code = "", string? text = "")
        {
            // should you use StringBuilder 
            return $@"<!DOCTYPE cXML SYSTEM ""http://xml.cXML.org/schemas/cXML/1.2.040/cXML.dtd"">
                <cXML payloadID=""{Guid.NewGuid()}@example.com"" timestamp=""{DateTime.UtcNow:yyyy-MM-ddTHH:mm:sszzz}"">
                  <Response>
                    <Status code=""{code}"" text=""{text}""></Status>
                      <PunchOutSetupResponse>
                        <StartPage>
                            <URL>{url}punchout?sid={buyerCookie}</URL>
                        </StartPage>
                      </PunchOutSetupResponse>
                  </Response>
                </cXML>";
        }
    }
}
