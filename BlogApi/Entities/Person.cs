using ArticlesAPI.Entities;
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
    public virtual User User { get; set; }
    public virtual ICollection<Article> Articles { get; set; }
    public virtual ICollection<Rating> Ratings { get; set; }

    public string Email
    {
        get
        {
            return User.Email;
        }
    }
}
