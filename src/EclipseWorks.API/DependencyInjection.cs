using System.Reflection;
using EclipseWorks.Application.Handlers;
using EclipseWorks.Application.Validators;
using EclipseWorks.Domain.Interfaces.Abstractions;
using EclipseWorks.Domain.Interfaces.Repositories;
using EclipseWorks.Infrastructure.Implementations;
using EclipseWorks.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace EclipseWorks.API;

public static class DependencyInjection
{
    public static IServiceCollection BaseRegister(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .RegisterRepositories()
            .RegisterDBContext(configuration)
            .RegisterMediatR()
            .RegisterServices()
            .RegisterSwagger()
            .RegisterValidators();

        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEclipseUnitOfWork), typeof(EclipseUnitOfWork));
        services.AddScoped(typeof(IProjectRepository), typeof(ProjectRepository));
        services.AddScoped(typeof(ITaskRepository), typeof(TaskRepository));
        services.AddScoped(typeof(ITaskHistoryRepository), typeof(TaskHistoryRepository));
        services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
        services.AddScoped(typeof(IProjectUserRepository), typeof(ProjectUserRepository));
        services.AddScoped(typeof(ITaskUserRepository), typeof(TaskUserRepository));
        return services;
    }

    private static IServiceCollection RegisterDBContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Database")));
        services.AddScoped<DbContext, ApplicationDbContext>();

        return services;
    }

    public static void MigrateDatabase(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while migrating the database.", ex);
            }
        }
    }

    private static IServiceCollection RegisterMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(SampleHandler).Assembly);
        });

        return services;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Eclipseworks swagger",
                Contact = new OpenApiContact
                {
                    Name = "eclipseworks development team",
                    Email = "eclipseworks@eclipseworks.com",
                    Url = new Uri("http://eclipseworks.com.br")
                }
            });
        });

        return services;
    }

    private static IServiceCollection RegisterValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<SampleValidator>();
        services.AddFluentValidationAutoValidation();
        return services;
    }
}