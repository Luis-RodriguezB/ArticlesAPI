using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Auth;
public class UserEmailDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
