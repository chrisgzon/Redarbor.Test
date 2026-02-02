using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Redarbor.Test.Application.Behaviors;
using Redarbor.Test.Application.Interfaces;
using Redarbor.Test.Application.Mappings;
using Redarbor.Test.Domain.Interfaces;
using Redarbor.Test.Infrastructure.Identity;
using Redarbor.Test.Infrastructure.Persistence.Read;
using Redarbor.Test.Infrastructure.Persistence.Read.Repositories;
using Redarbor.Test.Infrastructure.Persistence.Write;
using Redarbor.Test.Infrastructure.Persistence.Write.Repositories;
using System.Reflection;

namespace Redarbor.Test.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // MediatR
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.Load("RedArbor.Test.Application")));

            // FluentValidation
            services.AddValidatorsFromAssembly(Assembly.Load("RedArbor.Test.Application"));

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // MediatR Behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database Context - Entity Framework
            services.AddDbContext<RedarborEFDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("RedarborConnection"),
                    b => b.MigrationsAssembly(typeof(RedarborEFDbContext).Assembly.FullName)));

            // Dapper Context
            services.AddSingleton<RedarborDapperContext>();

            // Repositories
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeeQueryRepository, EmployeeQueryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Identity Services
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IAuthService, IdentityService>();

            return services;
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RedArbor Employee API",
                    Version = "v1",
                    Description = "API for managing employees with CQRS pattern"
                });

                // Add JWT Authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
