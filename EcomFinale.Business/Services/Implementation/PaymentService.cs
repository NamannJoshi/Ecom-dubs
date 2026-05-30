using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Dtos.Requests;
using EcomFinale.DataAccess.Entities;
using EcomFinale.DataAccess.Options;
using EcomFinale.DataAccess.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace EcomFinale.Business.Services.Implementation;

public class PaymentService : IPaymentService
{
    private readonly StripeOptions stripeOptions;
    private readonly IOrderService orderService;
    private readonly ICartService cartService;
    private readonly IPaymentRepository paymentRepository;

    public PaymentService(IOrderService orderService, ICartService cartService, IPaymentRepository paymentRepository, IOptions<StripeOptions> stripeOptions)
    {
        this.orderService = orderService;
        this.cartService = cartService;
        this.paymentRepository = paymentRepository;
        this.stripeOptions = stripeOptions.Value;
    }


    public async Task<string> CheckoutHandler(OrderDto orderCreateDto, Guid idempotencyId)
    {
        var order = await this.orderService.CreateOrder(orderCreateDto, idempotencyId);

        return await this.PaymentInitialization(order.Id);
    }

    public async Task<string> PaymentInitialization(int orderId)
    {
        var order = await this.orderService.GetOrderById(orderId);
        if (order == null || order.Status != OrderStatus.Pending)
        {
            throw new KeyNotFoundException("Order not found");
        }

        var payableAmount = order.OrderItems.Sum(item => item.Quantity * item.UnitPrice);
        var paymentId = Guid.NewGuid();

        var payment = new Payment
        {
            OrderId = orderId,
            AmountPaid = payableAmount,
            PaymentId = paymentId.ToString(),
        };

        // Store transaction/session id
        await this.paymentRepository.Create(payment);

        StripeConfiguration.ApiKey = this.stripeOptions.SecretKey;

        var options = new Stripe.Checkout.SessionCreateOptions
        {
            PaymentMethodTypes = new List<string>
            {
                "card"
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "inr",
                        UnitAmount = (long)(payableAmount * 100), // Convert to smallest currency unit
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Test Product"
                        }
                    },
                    Quantity = 1
                }
            },
            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    { "InternalPaymentId", paymentId.ToString() },
                    { "orderId", orderId.ToString() }
                }
            },

            Metadata = new Dictionary<string, string>
            {
                { "orderId", orderId.ToString() }
            },
            Mode = "payment",
            SuccessUrl = "http://localhost:5161/api/Orders/success",
            CancelUrl = "http://localhost:5161/api/Orders/failed"
        };

        var service = new Stripe.Checkout.SessionService();
        var session = await service.CreateAsync(options);

        return session.Url;
    }

    public async Task WebhookHandler(StripeWebhookDto webhookDto)
    {
        Event stripeEvent;
        var stripeWebhookSecret = this.stripeOptions.WebhookSecret;

        try
        {
            // Verify Stripe signature
            stripeEvent = EventUtility.ConstructEvent(
                webhookDto.Json,
                stripeWebhookSecret,
                webhookDto.WebhookSecret,
                throwOnApiVersionMismatch: false
            );
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Webhook verification failed: {ex.Message}");
        }

        // Handle events
        switch (stripeEvent.Type)
        {
            case "payment_intent.succeeded":
                {
                    var session = stripeEvent.Data.Object as PaymentIntent;

                    var orderId = session.Metadata["orderId"];
                    var paymentId = session.Metadata["InternalPaymentId"];

                    // TODO:
                    // Find order in DB
                    var order = await this.orderService.GetOrderById(int.Parse(orderId));
                    order.Status = OrderStatus.Processing;

                    await this.orderService.UpdateOrder(order.Id, order);

                    await this.cartService.CheckoutCart(order.UserId);
                    // Mark as paid

                    var payment = await this.paymentRepository.GetByOrderId(order.Id)
                                    .FirstOrDefaultAsync(p => p.PaymentId == paymentId) ??
                                    throw new KeyNotFoundException("Payment with stored payment id is not found.");

                    payment.Status = PaymentStatus.Paid;
                    await this.paymentRepository.SaveChanges();
                    break;
                }

            case "payment_intent.payment_failed":
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    // TODO:
                    var paymentId = paymentIntent.Metadata["InternalPaymentId"];
                    _ = int.TryParse(paymentIntent.Metadata["orderId"], out var orderId);

                    if (orderId == 0)
                    {
                        throw new ArgumentException("order id recieved from payment meta data, is of invalid type.");
                    }

                    var payment = await this.paymentRepository.GetByOrderId(orderId)
                                    .Where(p => p.PaymentId == paymentId)
                                    .FirstOrDefaultAsync();

                    await this.orderService.ProductInventoryRollbackByOrder(orderId);

                    payment.Status = PaymentStatus.Failed;
                    await this.paymentRepository.SaveChanges();
                    break;
                }
        }
    }
}