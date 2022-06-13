using System.Reflection;
using Coursework.Core.Configuration;
using Coursework.Core.Data;
using Coursework.Core.Extensions.Decorators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Coursework.Core.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddCore(this IServiceCollection services, IJwtConfiguration jwtConfiguration, string connection) => services
        .AddDbContext<AppDbContext>(options => options.UseSqlServer(connection))
        .AddSingleton<IJwtConfiguration>(jwtConfiguration)
        .AddMediatR(Assembly.GetExecutingAssembly())
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
}