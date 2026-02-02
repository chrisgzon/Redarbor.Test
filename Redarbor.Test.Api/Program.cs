using Redarbor.Test.Api.Extensions;
using Redarbor.Test.Api.Middleware;
using Redarbor.Test.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Custom service extensions
builder.Services.AddCorsConfiguration();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// OAuth2/JWT Authentication
builder.Services.AddOAuth2Authentication(builder.Configuration);

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Middleware pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }