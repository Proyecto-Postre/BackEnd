using DulceFe.API.Catalog.Domain.Model.Aggregates;
using DulceFe.API.Catalog.Interfaces.REST.Resources;

namespace DulceFe.API.Catalog.Interfaces.REST.Transform;

public static class ProductResourceFromEntityAssembler
{
    public static ProductResource ToResourceFromEntity(Product entity)
    {
        return new ProductResource(
            entity.Id, 
            entity.Title, 
            entity.Description, 
            entity.Price, 
            entity.Category, 
            entity.Image, 
            entity.IsFeatured, 
            entity.Sales);
    }
}
