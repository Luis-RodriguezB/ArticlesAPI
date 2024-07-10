using ArticlesAPI.DTOs.Others;

namespace ArticlesAPI.Helpers;
public static class QueryableExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable,
        PaginationDTO paginationDTO)
    {
        return queryable
            .Skip((paginationDTO.Page - 1) * paginationDTO.RecordsByPage)
            .Take(paginationDTO.RecordsByPage);

    }
}
