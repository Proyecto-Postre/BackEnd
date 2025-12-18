namespace DulceFe.API.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}
