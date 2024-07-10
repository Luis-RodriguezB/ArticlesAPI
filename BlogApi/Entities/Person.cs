using ArticlesAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models;
public class Person : IEntityBase
{
    public int Id { get; set; }
    [Required]
    [StringLength(80)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(120)]
    public string LastName { get; set; }
    public string AboutMe { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User User { get; set; }
    public ICollection<Article> Articles { get; set; }
}
