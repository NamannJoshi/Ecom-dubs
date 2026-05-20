using EcomFinale.DataAccess.Entities;

namespace EcomFinale.Business.Common;

public static class AuditHelper
{
    public static void ApplyAuditValues<T>(T entity, bool isCreated)
         where T : AuditEntity
    {
        if (isCreated)
        {
            entity.CreatedBy = 1;
        }
        entity.ModifiedBy = 1;
    }
}