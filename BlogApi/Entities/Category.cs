using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.Entities;
public class Category
{
    public int Id { get; set; }
    [Required]
    [StringLength(80)]
    public string Name { get; set; }
    public string NameNormalized { get; set; }
    public ICollection<ArticleCategory> ArticleCategories { get; set; }
}
