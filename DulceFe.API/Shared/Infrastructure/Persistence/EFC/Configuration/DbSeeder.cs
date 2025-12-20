using DulceFe.API.IAM.Application.Internal.OutboundServices;
using DulceFe.API.IAM.Domain.Model.Aggregates;
using DulceFe.API.Catalog.Domain.Model.Aggregates;
using DulceFe.API.Catalog.Domain.Model.Entities;
using DulceFe.API.Social.Domain.Model.Aggregates;
using DulceFe.API.Promotions.Domain.Model.Aggregates;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public static class DbSeeder
{
    public static void Seed(AppDbContext context, IHashingService hashingService)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User("admin@gmail.com", hashingService.HashPassword("lolasoxd153"), "Admin", "Fe", "admin@gmail.com", "999888777", UserRole.Admin),
                new User("jafethworren@gmail.com", hashingService.HashPassword("lolasoxd153"), "Jafeth", "Worren", "jafeth@gmail.com", "987654321", UserRole.Customer)
            );
            context.SaveChanges();
        }

        if (!context.Coupons.Any())
        {
            context.Coupons.AddRange(
                new Coupon { Code = "NAVIDAD20", DiscountValue = 20, IsPercentage = true, IsActive = true },
                new Coupon { Code = "BIENVENIDA", DiscountValue = 10, IsPercentage = false, IsActive = true }
            );
            context.SaveChanges();
        }
        // Simple seeding without reading file for now to ensure stability
        // We can add file reading if path is confirmed accessible
        
        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Tortas" },
                new Category { Name = "Postres" },
                new Category { Name = "Bebidas" }
            );
            context.SaveChanges();
        }

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Title = "Red Velvet Supreme", Description = "Suave bizcocho de terciopelo rojo con capas de frosting de queso crema.", Price = 55.0m, Category = "Tortas", Image = "", IsFeatured = true, Stock = 15 },
                new Product { Title = "Chocolate Intenso", Description = "Para los amantes del cacao. Bizcocho húmedo con ganache de chocolate 70%.", Price = 90.0m, Category = "Tortas", Image = "", IsFeatured = true, Stock = 10 },
                new Product { Title = "Cheesecake de Frutos Rojos", Description = "Base crujiente de galleta, crema suave de queso y topping artesanal de frutos del bosque.", Price = 120.0m, Category = "Postres", Image = "", IsFeatured = false, Stock = 8 },
                new Product { Title = "Macarons Surtidos (Box x6)", Description = "Delicados bocados franceses en sabores variados: Pistacho, Frambuesa y Vainilla.", Price = 45.0m, Category = "Postres", Image = "", IsFeatured = false, Stock = 25 },
                new Product { Title = "Cappuccino Art", Description = "Espresso doble con leche texturizada y arte latte.", Price = 12.0m, Category = "Bebidas", Image = "", IsFeatured = false, Stock = 50 },
                new Product { Title = "Té Helado de Durazno", Description = "Refrescante infusión de té negro con trozos de durazno natural.", Price = 10.0m, Category = "Bebidas", Image = "", IsFeatured = true, Stock = 50 }
            );
            context.SaveChanges();
        }

        if (!context.Testimonials.Any())
        {
            context.Testimonials.AddRange(
                new Testimonial { Name = "Jafeth Worren Y.", Text = "hola amigos del wasap", Stars = 5, Role = "Cliente Verificado" }
            );
            context.SaveChanges();
        }
    }
}
