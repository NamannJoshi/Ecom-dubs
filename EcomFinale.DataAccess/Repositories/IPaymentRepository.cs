using EcomFinale.DataAccess.Entities;

namespace EcomFinale.DataAccess.Repositories;

public interface IPaymentRepository
{
    IQueryable<Payment> GetPayments(int? userId = null);

    Task<Payment> Create(Payment payment);

    Task<Payment?> GetById(int id);

    IQueryable<Payment> GetByOrderId(int orderId);

    Task SaveChanges();
}