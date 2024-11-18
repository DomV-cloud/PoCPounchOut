using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Template.Api.Filters;
using Template.Api.Middleware;
using Template.Application.DatabaseContext;
using Template.Application.DependencyInjection;
using Template.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddConsole()
            .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
        loggingBuilder.AddDebug();
    });

    builder.Services
        .AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>())
        .AddXmlSerializerFormatters();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    builder.Services.AddDbContext<TemplateContext>(options => {
        options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
        options.EnableSensitiveDataLogging(true);
    });

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(
            builder =>
            {
                builder.AllowAnyOrigin()
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();
