namespace EcomFinale.DataAccess.Dtos;

public class OrderItemDto
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public DateTime CreatedAt {get; set;}

    public DateTime ModifiedAt {get; set;}

    public ProductDto? Product {get; set;}
}