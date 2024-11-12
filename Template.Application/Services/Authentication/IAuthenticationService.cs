namespace Template.Application.Services.Authentication

{
    public interface IAuthenticationService
    {
        AuthenticationResult Login(string secretId);
    }
}
