using ArticlesAPI.DTOs.Article;

namespace ArticlesAPI.DTOs.Person;
public class PersonUserDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string AboutMe { get; set; }
    public List<ArticlePersonDTO> Articles { get; set; }
}
