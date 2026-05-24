using EcomFinale.DataAccess.Entities;

namespace EcomFinale.Business.Common;

public static class AuditHelper
{
    public static void ApplyAuditValues<T>(T entity, bool isCreated)
         where T : AuditEntity
    {
        var utc = DateTime.UtcNow;
        if (isCreated)
        {
            entity.CreatedBy = 1;
            entity.CreatedAt = utc;
        }
        entity.ModifiedBy = 1;
        entity.ModifiedAt = utc;
    }
}