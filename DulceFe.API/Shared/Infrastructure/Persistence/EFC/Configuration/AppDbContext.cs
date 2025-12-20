using Microsoft.EntityFrameworkCore;
using DulceFe.API.IAM.Domain.Model.Aggregates;
using DulceFe.API.Catalog.Domain.Model.Aggregates;
using DulceFe.API.Catalog.Domain.Model.Entities;
using DulceFe.API.Sales.Domain.Model.Aggregates;
using DulceFe.API.Social.Domain.Model.Aggregates;
using DulceFe.API.Promotions.Domain.Model.Aggregates;
using DulceFe.API.Services.Domain.Model.Aggregates;
using DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

namespace DulceFe.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<CateringInquiry> CateringInquiries { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<WorkshopSubscription> WorkshopSubscriptions { get; set; }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply Snake Case Naming Convention
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            var tableName = entity.GetTableName();
            if (!string.IsNullOrEmpty(tableName)) entity.SetTableName(tableName.ToSnakeCase());

            foreach (var property in entity.GetProperties())
                property.SetColumnName(property.GetColumnName().ToSnakeCase());

            foreach (var key in entity.GetKeys())
            {
                var keyName = key.GetName();
                if (!string.IsNullOrEmpty(keyName)) key.SetName(keyName.ToSnakeCase());
            }

            foreach (var foreignKey in entity.GetForeignKeys())
            {
                var foreignKeyName = foreignKey.GetConstraintName();
                if (!string.IsNullOrEmpty(foreignKeyName)) foreignKey.SetConstraintName(foreignKeyName.ToSnakeCase());
            }

            foreach (var index in entity.GetIndexes())
            {
                var indexName = index.GetDatabaseName();
                if (!string.IsNullOrEmpty(indexName)) index.SetDatabaseName(indexName.ToSnakeCase());
            }
        }

        builder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>();
    }
}
