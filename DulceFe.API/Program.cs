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
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using DulceFe.API.Sales.Domain.Repositories;
using DulceFe.API.Sales.Infrastructure.Persistence.EFC.Repositories;
using DulceFe.API.Social.Domain.Repositories;
using DulceFe.API.Social.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

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
    if (connectionString != null)
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
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

// Social Bounded Context Injection
builder.Services.AddScoped<ITestimonialRepository, TestimonialRepository>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy", 
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable Swagger in all environments for easier testing of the 75 HUs on Render
app.UseSwagger();
app.UseSwaggerUI();

// Auto-migrate database on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var hashingService = scope.ServiceProvider.GetRequiredService<IHashingService>();
    context.Database.EnsureCreated();
    DbSeeder.Seed(context, hashingService);
}

app.UseCors("AllowAllPolicy");

// IAM Middleware
app.UseRequestAuthorization();

// app.UseHttpsRedirection(); // Can be optional on Render if they handle TLS at the edge

app.UseAuthorization();
app.MapControllers();

// Ensure the app listens on the PORT environment variable (required by Render)
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    app.Run($"http://0.0.0.0:{port}");
}
else
{
    app.Run();
}
