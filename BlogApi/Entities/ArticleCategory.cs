using BlogApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.Entities;
public class ArticleCategory
{
    [Required]
    public int ArticleId { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public Article Article { get; set; }
    public Category Category { get; set; }
}
