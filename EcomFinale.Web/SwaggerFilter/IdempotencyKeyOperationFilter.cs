using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EcomFinale.Web.SwaggerFilter;

public class IdempotencyKeyOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var method = context.ApiDescription.HttpMethod?.ToUpper();
        if (method is not ("POST" or "PUT" or "PATCH"))
            return;

        var parameter = new OpenApiParameter
        {
            Name = "Idempotency-Key",
            In = ParameterLocation.Header,
            Required = false,
            Description = "A unique key to ensure idempotent request processing. Use a UUID v4.",
            Schema = new OpenApiSchema
            {
                Type = JsonSchemaType.String,   // ✅ fix: use JsonSchemaType enum
                Format = "uuid"
                // ✅ fix: no Example/Any here, the Any namespace is gone in v2
            }
        };

        // ✅ fix: IList doesn't support ??=, just null-check manually
        if (operation.Parameters == null)
            operation.Parameters = new List<IOpenApiParameter>();  // ✅ fix: IOpenApiParameter interface

        operation.Parameters.Add(parameter);
    }
}