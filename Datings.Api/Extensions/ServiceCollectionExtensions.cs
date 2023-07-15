using System.Reflection;
using System.Text;
using Datings.Api.BL;
using Datings.Api.Common.Abstractions;
using Datings.Api.Common.Implementations;
using Datings.Api.Common.Implementations.CodeValidator;
using Datings.Api.Common.Implementations.Email;
using Datings.Api.Common.Implementations.SmsService;
using Datings.Api.Common.Models.Options;
using Datings.Api.Data;
using Datings.Api.Data.Entities;
using Datings.Api.Identity;
using Datings.Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Datings.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddBusinessLogic(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.Configure<MinioOptions>(
            builder.Configuration.GetSection("Minio"));
        builder.Services.AddScoped<IFileStorage, MinioStorage>();
        builder.Services.AddScoped<IAccountsService, AccountsService>();
        builder.Services.AddStackExchangeRedisCache(options => {
            options.Configuration = builder.Configuration["Redis:Address"];
            options.InstanceName = builder.Configuration["Redis:Instance"];
        });
        
        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddScoped<ISmsService, MockSmsService>();
            builder.Services.AddSingleton<ICodeValidator, MemoryCodeValidator>();
            builder.Services.AddScoped<IEmailService, MockEmailService>();
        }
        else
        {
            builder.Services.AddScoped<ISmsService, SmsService>();
            builder.Services.AddSingleton<ICodeValidator, RedisCodeValidator>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services
                .AddFluentEmail("rave9373@gmail.com")
                .AddSmtpSender("localhost", 25);
        }
    }

    public static void AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(c => c.AddPolicy("cors", opt =>
        {
            opt.AllowAnyHeader();
            opt.AllowCredentials();
            opt.AllowAnyMethod();
            opt.WithOrigins(builder.Configuration.GetSection("Cors:Urls").Get<string[]>()!);
        }));
    }

    public static void AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "ExcalibQuestions", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    Array.Empty<string>()
                }
            });
        });
    }
    
    public static void AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"]!,
                    ValidAudience = builder.Configuration["Jwt:Audience"]!,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
                };
            });
        builder.Services.AddAuthorization(options => options.DefaultPolicy =
            new AuthorizationPolicyBuilder
                    (JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
        builder.Services.AddIdentity<ApplicationUser, IdentityRole<long>>()
            .AddEntityFrameworkStores<DataContext>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddSignInManager<SignInManager<ApplicationUser>>();
        builder.Services.AddScoped<UserProvider>();
    }
    
    public static void AddData(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DataContext>(opt => 
            opt.UseNpgsql(builder.Configuration.GetConnectionString("Db")));
        builder.Services.AddScoped<DataSeeder>();
    }

    public static void AddControllers(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(opt => opt.Filters.Add(new ApiExceptionFilter()));
        builder.Services.AddApiVersioning(config =>
        {
            config.ApiVersionReader = new HeaderApiVersionReader("ApiVersion");
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
        });
    }

    public static void AddLogging(this WebApplicationBuilder builder)
    {
        const string logTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u}] [{SourceContext}] {Message}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(outputTemplate: logTemplate)
            .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day, outputTemplate: logTemplate)
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticSearch:Url"]!))
            {
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{builder.Environment.EnvironmentName.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            })
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
    }

    public static void UseSwaggerApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(x =>
        {
            x.SwaggerEndpoint("/swagger/v1/swagger.json", "ExcalibQuestions Web API v1");
            x.RoutePrefix = "swagger";
        });
    }

    public static void SetCurrentUser(this WebApplication app)
    {
        app.UseMiddleware<SetCurrentUserMiddleware>();
    }
}