using Template.Domain.Entities;

namespace Template.Application.Common.Interfaces.Persistance
{
    public interface IUserRepository
    {
        User? GetUserBySecretId(string secretId);
    }
}
