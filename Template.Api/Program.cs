using Template.Infrastructure.DependencyInjection;
using Template.Application.DependencyInjection;
using Template.Application.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Template.Api.Middleware;
using Template.Api.Filters;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();

    builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    builder.Services.AddDbContext<TemplateContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

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
