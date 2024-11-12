using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Persistance;
using Template.Contracts.PunchOut;
using Template.Domain.Entities;

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

        public AuthenticationResult Login(string secretId)
        {
            if (string.IsNullOrWhiteSpace(secretId))
            {
                return new AuthenticationResult(new(), new PunchOutResponse("400", "Secret key is empty"));
            }

            var user = _userRepository.GetUserBySecretId(secretId);

            if (user is null)
            {
                return new AuthenticationResult(new(), new PunchOutResponse("400", "Cannot find user"));
            }

            return new AuthenticationResult(user, new PunchOutResponse("200","User is found"));
        }
    }
}
