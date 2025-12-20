using System.Reflection;
using FluentValidation;
using Serilog;
using Serilog.Events;
using DulceFe.API.IAM.Application.Internal.CommandServices;
using DulceFe.API.IAM.Application.Internal.OutboundServices;
using DulceFe.API.IAM.Application.Internal.QueryServices;
using DulceFe.API.IAM.Domain.Repositories;
using DulceFe.API.IAM.Domain.Services;
using DulceFe.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using DulceFe.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using DulceFe.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using DulceFe.API.IAM.Infrastructure.Tokens.JWT.Services;
using DulceFe.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using DulceFe.API.Catalog.Application.Internal.QueryServices;
using DulceFe.API.Catalog.Domain.Repositories;
using DulceFe.API.Catalog.Domain.Services;
using DulceFe.API.Catalog.Infrastructure.Persistence.EFC.Repositories;
using DulceFe.API.Shared.Domain.Repositories;
using DulceFe.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using DulceFe.API.Shared.Infrastructure.Middleware.Extensions;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Sales.Domain.Repositories;
using DulceFe.API.Sales.Domain.Services;
using DulceFe.API.Sales.Application.Internal.CommandServices;
using DulceFe.API.Sales.Application.Internal.QueryServices;
using DulceFe.API.Sales.Infrastructure.Persistence.EFC.Repositories;
using DulceFe.API.Social.Domain.Repositories;
using DulceFe.API.Social.Infrastructure.Persistence.EFC.Repositories;
using DulceFe.API.Promotions.Domain.Model.Aggregates;
using DulceFe.API.Promotions.Domain.Repositories;
using DulceFe.API.Promotions.Domain.Services;
using DulceFe.API.Promotions.Application.Internal.CommandServices;
using DulceFe.API.Promotions.Application.Internal.QueryServices;
using DulceFe.API.Promotions.Infrastructure.Persistence.EFC.Repositories;
using DulceFe.API.Services.Domain.Repositories;
using DulceFe.API.Services.Domain.Services;
using DulceFe.API.Services.Application.Internal.CommandServices;
using DulceFe.API.Services.Application.Internal.QueryServices;
using DulceFe.API.Services.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/dulcefe-log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new KebabCaseRouteNamingConvention());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Dulce Fe API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrEmpty(connectionString))
    {
        // Use hardcoded version to avoid SIGSEGV in some Linux environments during AutoDetect
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 40)); 
        options.UseMySql(connectionString, serverVersion);
    }
});

// Configure TokenSettings
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

// Shared Bounded Context Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IAM Bounded Context Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();

// Catalog Bounded Context Injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductQueryService, ProductQueryService>();

// Sales Bounded Context Injection
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderCommandService, OrderCommandService>();
builder.Services.AddScoped<IOrderQueryService, OrderQueryService>();

// Social Bounded Context Injection
builder.Services.AddScoped<ITestimonialRepository, TestimonialRepository>();

// Promotions Bounded Context Injection
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<ICouponCommandService, CouponCommandService>();
builder.Services.AddScoped<ICouponQueryService, CouponQueryService>();

// Services Bounded Context Injection
builder.Services.AddScoped<ICateringInquiryRepository, CateringInquiryRepository>();
builder.Services.AddScoped<ICateringInquiryCommandService, CateringCommandService>();
builder.Services.AddScoped<ICateringInquiryQueryService, CateringQueryService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", 
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGlobalExceptionHandlerMiddleware();
app.UseSwagger();
app.UseSwaggerUI();

// Auto-migrate database on startup
try 
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AppDbContext>();
        var hashingService = services.GetRequiredService<IHashingService>();
        
        Console.WriteLine("Applying migrations/ensuring database exists...");
        context.Database.EnsureCreated();
        
        Console.WriteLine("Seeding database...");
        DbSeeder.Seed(context, hashingService);
        Console.WriteLine("Database initialization completed.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred during database initialization: {ex.Message}");
    // We don't throw here to allow the app to start even if seeding fails
}

app.UseStaticFiles();

app.UseCors("AllowAllPolicy");

// IAM Middleware
app.UseRequestAuthorization();

app.UseAuthorization();
app.MapControllers();

// Redirect root to Swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

// Render uses the PORT environment variable
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");
