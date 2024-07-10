using ArticlesAPI.DTOs.Category;

namespace ArticlesAPI.DTOs.Article;
public class ArticlePersonDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<CategoryDTO> Categories { get; set; }
}
