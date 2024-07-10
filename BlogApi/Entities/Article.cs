using ArticlesAPI.Entities;
using ArticlesAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models;

public class Article : IEntityBase
{
    public int Id { get; set; }
    [Required]
    [StringLength(120)]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int PersonId { get; set; }
    public Person Person { get; set; }
    public ICollection<ArticleCategory> ArticleCategories { get; set; }
}
