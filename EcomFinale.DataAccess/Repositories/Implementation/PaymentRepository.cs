using EcomFinale.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcomFinale.DataAccess.Repositories.Implementation;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext context;

    public PaymentRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Payment> Create(Payment payment)
    {
        this.context.Payments.Add(payment);
        await this.context.SaveChangesAsync();
        return payment;
    }


    public Task SaveChanges()
    {
        throw new NotImplementedException();
    }

    public async Task<Payment?> GetById(int id)
    {
        return await this.context.Payments.FindAsync(id);
    }

    public IQueryable<Payment> GetPayments(int? userId = null)
    {
        var query = this.context.Payments.AsQueryable();

        if (userId.HasValue)
        {
            query = query.Where(p => p.CreatedByUser.Id == userId);
        }

        return query;
    }

    public IQueryable<Payment> GetByOrderId(int orderId)
    {
        return this.GetPayments().Where(p => p.OrderId == orderId);
    }
}