using ArticlesAPI.DTOs.Category;
using ArticlesAPI.DTOs.Person;

namespace BlogApi.DTOs.Blog;

public class ArticleDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public PersonArticleDTO Person { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<CategoryDTO> Categories { get; set; }
}
