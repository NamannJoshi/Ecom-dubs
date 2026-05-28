namespace EcomFinale.Business.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method)]
public class PaginationAttribute : ResultFilterAttribute
{
    // These properties match your requirements
    public int DefaultPageSize { get; set; } = 10;
    public Type EntityType { get; set; }

    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        // 1. Extract 'page' and 'pageSize' from the query string
        var request = context.HttpContext.Request;
        
        int page = int.TryParse(request.Query["page"], out int p) && p > 0 ? p : 1;
        int pageSize = int.TryParse(request.Query["pageSize"], out int ps) && ps > 0 ? ps : DefaultPageSize;

        // 2. Intercept the result from the controller
        if (context.Result is ObjectResult objectResult && objectResult.Value != null)
        {
            var valueType = objectResult.Value.GetType();

            // 3. Check if the returned value is an IQueryable
            if (typeof(IQueryable).IsAssignableFrom(valueType) && EntityType != null)
            {
                // 4. Use Reflection to call our generic pagination helper below
                var method = typeof(PaginationAttribute)
                    .GetMethod(nameof(ApplyPagination), BindingFlags.NonPublic | BindingFlags.Static)
                    ?.MakeGenericMethod(EntityType);

                if (method != null)
                {
                    // Execute the pagination logic
                    var paginatedData = method.Invoke(null, new object[] { objectResult.Value, page, pageSize });

                    // 5. Replace the original IQueryable with the new paginated list
                    objectResult.Value = paginatedData;
                }
            }
        }

        await next();
    }

    // Generic helper method to apply Skip and Take safely
    private static PagedResult<T> ApplyPagination<T>(IQueryable<T> query, int page, int pageSize)
    {
        var totalCount = query.Count();
        
        // Apply pagination logic and fetch the list
        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<T>
        {
            TotalCount = totalCount,
            CurrentPage = page,
            PageSize = pageSize,
            Items = items
        };
    }

    public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<T> Items { get; set; }
}
}