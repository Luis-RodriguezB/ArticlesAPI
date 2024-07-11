using ArticlesAPI.DTOs.Others;
using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Filters;
public class ArticleFilter : PaginationDTO
{
    public string Title { get; set; }
    public DateTime? Date { get; set; } = null;
}
