using Template.Domain.Entities;

namespace Template.Contracts.Authentication
{
    public record AuthenticationResponse
    (
        User? User,
        string Message
     );
}
