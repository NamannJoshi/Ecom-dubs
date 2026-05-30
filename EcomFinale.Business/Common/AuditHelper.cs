using EcomFinale.DataAccess.Entities;

namespace EcomFinale.Business.Common;

public static class AuditHelper
{
    public static void ApplyAuditValues<T>(T entity, int userId, bool isCreated)
         where T : AuditEntity
    {
        var utc = DateTime.UtcNow;
        if (isCreated)
        {
            entity.CreatedBy = userId;
            entity.CreatedAt = utc;
        }
        entity.ModifiedBy = userId;
        entity.ModifiedAt = utc;
    }
}