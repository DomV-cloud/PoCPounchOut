namespace Template.Infrastructure.Interfaces
{
    public interface IPunchOutResponseGenerator
    {
        //  Type	Code	Text	Message  Cookie
        string GeneratePunchOutResponse(string? code = "", string? text = "", string? message = "", string? buyerCookie = "");
    }
}
