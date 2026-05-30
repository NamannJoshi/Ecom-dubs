namespace EcomFinale.DataAccess.Dtos.Requests;

public class StripeWebhookDto
{
    public string Json {get; set;}

    public string WebhookSecret {get; set;}
}