using Template.Application.Common.Interfaces.Persistance;
using Template.Domain.Entities;
using Template.Application.DatabaseContext;
namespace Template.Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        private readonly TemplateContext _context;

        public UserRepository(TemplateContext context)
        {
            _context = context;
        }

        public User? GetUserBySecretId(string secretId)
        {
            return _context.Users.SingleOrDefault(u => u.SecreteID == secretId);
        }
    }
}
