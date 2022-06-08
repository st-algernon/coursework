using Coursework_server.Configs;
using Coursework_server.Data;
using Coursework_server.Data.Hubs;
using Coursework_server.Data.Services;
using Coursework_server.Extensions.Middleware;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = JwtConfig.Issuer,

    ValidateAudience = true,
    ValidAudience = JwtConfig.Audience,

    ValidateLifetime = true,

    ValidateIssuerSigningKey = true,
    IssuerSigningKey = JwtConfig.GetSymmetricSecurityKey()
};

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

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
                new string[] {}
            }
        });
    });

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
});
builder.Services.AddSingleton<IUserIdProvider, UserIdAccessor>();

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<CollectionService>();
builder.Services.AddTransient<TopicService>();
builder.Services.AddTransient<TagService>();
builder.Services.AddTransient<ItemService>();
builder.Services.AddTransient<CommentService>();
builder.Services.AddTransient<StorageService>();
builder.Services.AddTransient<JwtService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

await AppDbInitializer.SeedAsync(app);

app.UseHttpsRedirection();

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