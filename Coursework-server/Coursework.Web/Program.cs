using Coursework.Core.Data;
using Coursework.Core.Extensions;
using Coursework.Web.Configs;
using Coursework.Web.Extensions.Middleware;
using Coursework.Web.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var jwtConfig = new JwtConfig
{
    Issuer = "Coursework.Web1",
    Audience = "CourseworkClient",
    Key = "nnuFFSju3Hh0Eamzeey3kznqbvqyYK8Q",
    AccessTokenLifetimeInMinutes = 5,
    RefreshTokenLifetimeInDays = 15
};
var tokenValidationParameters = new TokenValidationParameters
{
	ValidateIssuer = true,
	ValidIssuer = jwtConfig.Issuer,

	ValidateAudience = true,
	ValidAudience = jwtConfig.Audience,

	ValidateLifetime = true,

	ValidateIssuerSigningKey = true,
	IssuerSigningKey = jwtConfig.GetSymmetricSecurityKey()
};
var root = Directory.GetCurrentDirectory();
var pathToFile = builder.Configuration.GetValue<string>("Logging:FileLogger:Path");

// Add services to the container.
builder.Services.AddCore(jwtConfig, connection);

builder.Logging.AddFile(root + pathToFile);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddNSwag();

builder.Services.AddSwaggerGen(swagger =>
{
	swagger.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "JWT Token Authentication API",
		Description = "ASP.NET Core 5.0 Web API"
	});
	swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer\"",
	});
	swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new string[] { }
		}
	});
});

builder.Services.AddSignalR(hubOptions => { hubOptions.EnableDetailedErrors = true; });

builder.Services.AddSingleton<IUserIdProvider, UserIdAccessor>();

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false;
		options.TokenValidationParameters = tokenValidationParameters;
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				var accessToken = context.Request.Query["access_token"];
				var path = context.HttpContext.Request.Path;

				if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
				{
					context.Token = accessToken;
				}

				return Task.CompletedTask;
			}
		};
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger(new Action<SwaggerOptions>(_ => { }));
	app.UseSwaggerUI();
}

if (!NSwagConfiguration.IsOpenApiSchemeGenerationRuntime)
{
	await AppDbInitializer.SeedAsync(app);
}

app.UseHttpsRedirection();

app.UseNSwag();

app.UseCors(x => x
	.AllowAnyOrigin()
	.AllowAnyMethod()
	.AllowAnyHeader());

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionSerializerMiddleware();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllers();
	endpoints.MapHub<CommentsHub>("hubs/comments");
});

app.Run();