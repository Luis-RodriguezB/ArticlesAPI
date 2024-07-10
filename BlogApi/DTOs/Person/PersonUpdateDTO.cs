using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Person;
public class PersonUpdateDTO
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    public string AboutMe { get; set; }
}
