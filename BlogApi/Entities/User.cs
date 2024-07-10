using Microsoft.AspNetCore.Identity;

namespace BlogApi.Models;
public class User : IdentityUser
{
    public Person Person { get; set; }
}
