using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Filters;
public class ArticleFilter
{
    public string Title { get; set; }
    public DateTime? Date { get; set; } = null;
}
