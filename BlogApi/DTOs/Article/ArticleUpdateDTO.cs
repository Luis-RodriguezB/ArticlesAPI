using ArticlesAPI.DTOs.Category;
using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Article;
public class ArticleUpdateDTO
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    public List<CategoryArticleDTO> Categories { get; set; }
}
