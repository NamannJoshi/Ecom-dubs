namespace EcomFinale.DataAccess.Repositories;

public interface IUnitOfWork
{
    Task Rollback();

    Task Commit();

    Task BeginTransactionAsync();
}