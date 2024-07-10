using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Person;
public class PersonArticleDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}
