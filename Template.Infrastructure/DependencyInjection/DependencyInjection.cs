using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Interfaces.Persistance;
using Template.Application.Common.Interfaces.Services;
using Template.Application.Services.Authentication;
using Template.Application.Services.Logging;
using Template.Application.Services.MappingService;
using Template.Infrastructure.Authentication;
using Template.Infrastructure.Persistance;
using Template.Infrastructure.Services;

namespace Template.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ILoggingService, LoggingService>();

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<ICxmlMappingService, CxmlMappingService>();

            return services;
        }
    }
}
