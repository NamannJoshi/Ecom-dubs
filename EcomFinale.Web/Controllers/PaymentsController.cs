using EcomFinale.Business.Services;
using EcomFinale.DataAccess.Dtos;
using EcomFinale.DataAccess.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EcomFinale.Web.Controllers;

[ApiController]
[Route("api/Payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        this.paymentService = paymentService;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderCreateDto, [FromHeader(Name = "Idempotency-Key")] string? idempotencyKey)
    {
        var idempotencyId = idempotencyKey ?? throw new ArgumentException("Idempotency-Key header is required");

        if (!Guid.TryParse(idempotencyId, out var parsedIdempotencyId))
        {
            throw new ArgumentException("Idempotency-Key header must be a valid GUID");
        }
        var createdOrder = await this.paymentService.CheckoutHandler(orderCreateDto, parsedIdempotencyId);
        return Redirect(createdOrder);
    }

    [HttpPost("payment-response")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body)
            .ReadToEndAsync();

        var stripeSignature = Request.Headers["Stripe-Signature"];
        if (string.IsNullOrEmpty(stripeSignature))
        {
            return BadRequest("Missing Stripe-Signature header");
        }
        
        await this.paymentService.WebhookHandler(new StripeWebhookDto
        {
            Json = json,
            WebhookSecret = stripeSignature,
        });
        return Ok();
    }
}