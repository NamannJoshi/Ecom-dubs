#nullable disable
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomFinale.DataAccess.Entities;

public class AuditEntity
{
    public int CreatedBy {get; set;}

    [ForeignKey(nameof(CreatedBy))]
    public User CreatedByUser { get; set;}

    public int ModifiedBy {get; set;}

    [ForeignKey(nameof(ModifiedBy))]
    public User ModifiedByUser { get; set; }
}
