using System.Reflection;
using Coursework.Core.Configuration;
using Coursework.Core.Data;
using Coursework.Core.Extensions.Decorators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Coursework.Core.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddCore(this IServiceCollection services, IJwtConfiguration jwtConfiguration, string connection) => services
        .AddDbContext<AppDbContext>(options => options.UseSqlServer(connection))
        .AddSingleton<IJwtConfiguration>(jwtConfiguration)
        .AddMediatR(Assembly.GetExecutingAssembly())
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
}