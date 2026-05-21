namespace EcomFinale.DataAccess.Repositories.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext context;
    public UnitOfWork(AppDbContext context)
    {
        this.context = context;
    }

    public async Task BeginTransactionAsync()
    {
        await this.context.Database.BeginTransactionAsync();
    }

    public async Task Commit()
    {
        await this.context.SaveChangesAsync();
        await this.context.Database.CommitTransactionAsync();
    }

    public async Task Rollback()
    {
        await this.context.Database.RollbackTransactionAsync();
    }
}