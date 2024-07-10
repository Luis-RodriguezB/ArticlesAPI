using ArticlesAPI.DTOs.Category;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.DTOs.Blog;
public class ArticleCreateDTO
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Content { get; set; }
    [Required]
    public int PersonId { get; set; }
    public List<CategoryArticleDTO> Categories { get; set; }
}
