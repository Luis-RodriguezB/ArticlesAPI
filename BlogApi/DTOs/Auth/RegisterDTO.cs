using System.ComponentModel.DataAnnotations;

namespace BlogApi.DTOs.Auth;
public class RegisterDTO
{
    [Required]
    [StringLength(80)]
    public string Username { get; set; }
    [Required]
    [StringLength(80)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(120)]
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
}
