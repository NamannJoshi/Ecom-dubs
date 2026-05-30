using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Dtos.Requests;

namespace EcomFinale.Business.Services;

public interface IPaymentService
{
    Task<string> CheckoutHandler(OrderDto orderCreateDto, Guid idempotencyId);

    Task<string> PaymentInitialization(int orderId);

    Task WebhookHandler(StripeWebhookDto webhookDto);
}