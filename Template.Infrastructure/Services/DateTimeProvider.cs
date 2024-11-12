using Template.Application.Common.Interfaces.Services;

namespace Template.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        DateTime IDateTimeProvider.UtcNow => DateTime.UtcNow;
    }
}
