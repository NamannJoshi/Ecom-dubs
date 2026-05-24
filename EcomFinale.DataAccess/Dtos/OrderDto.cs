using EcomFinale.DataAccess.Entities;
namespace EcomFinale.DataAccess.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public int UserId { get; set; }
}