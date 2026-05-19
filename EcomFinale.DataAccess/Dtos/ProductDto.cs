namespace EcomFinale.DataAccess.Dtos;

public class ProductDto
{
    public int Id {get; set;}

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set;}

    public int StockQuantity {get; set;}

    public int ProductCategoryId {get; set;}
}