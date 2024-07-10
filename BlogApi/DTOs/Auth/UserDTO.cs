using ArticlesAPI.DTOs.Person;

namespace ArticlesAPI.DTOs.Auth;
public class UserDTO
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public PersonUserDTO Person { get; set; }
}
