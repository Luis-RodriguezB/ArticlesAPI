using Microsoft.EntityFrameworkCore;

namespace ArticlesAPI.Helpers;
public static class HttpContextExtensions
{
    public async static Task InserPaginationParams<T>(this HttpContext httpContext,
        IQueryable<T> queryable, int recordsByPage)
    {
        double records = await queryable.CountAsync();
        double recordsPages = Math.Ceiling(records / recordsByPage);
        httpContext.Response.Headers.Append("recordsPages", recordsPages.ToString());
    }
}
