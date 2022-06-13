using Coursework.Console;
using Coursework.Console.Configuration;
using Coursework.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

var jwtConfig = new JwtConfig
{
    Issuer = "Coursework.Web1",
    Audience = "CourseworkClient",
    Key = "nnuFFSju3Hh0Eamzeey3kznqbvqyYK8Q",
    AccessTokenLifetimeInMinutes = 5,
    RefreshTokenLifetimeInDays = 15
};
var connection = "Server=(localdb)\\mssqllocaldb;Database=CourseworkDB;Trusted_Connection=True;";

//setup our DI
var serviceProvider = new ServiceCollection()
    .AddCore(jwtConfig, connection)
    .AddSingleton<ConsoleApp>()
    .BuildServiceProvider();

//do the actual work here
var app = serviceProvider.GetService<ConsoleApp>();
app.Main();