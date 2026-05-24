#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomFinale.DataAccess.Entities;

public class AuditEntity
{
    public int CreatedBy {get; set;}

    public int ModifiedBy {get; set;}

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    [ForeignKey(nameof(CreatedBy))]
    public User CreatedByUser { get; set;}

    [ForeignKey(nameof(ModifiedBy))]
    public User ModifiedByUser { get; set; }
}
