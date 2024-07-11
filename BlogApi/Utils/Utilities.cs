using ArticlesAPI.DTOs.Others;

namespace ArticlesAPI.Utils;
public static class Utilities
{
    public static PaginationDTO GetPaginationDTO<T>(T filter) where T : PaginationDTO
    {
        return new PaginationDTO
        {
            Page = filter.Page,
            RecordsByPage = filter.RecordsByPage,
        };
    }
}
