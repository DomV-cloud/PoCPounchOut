using Template.Infrastructure.Interfaces;

namespace Template.Application.Services.Authentication

{
    public interface IAuthenticationService
    {
        string Login(string secretId, string buyerCookie, string url);
    }
}
