#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomFinale.DataAccess.Entities;

[Table("ProductCategories")]
public class ProductCategory : AuditEntity
{
    public int Id {get; set;}

    public string Name { get; set; }

    public string Description { get; set; }

    public ICollection<Product> Products { get; set; }
}
