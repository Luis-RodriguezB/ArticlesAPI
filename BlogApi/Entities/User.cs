using Microsoft.AspNetCore.Identity;

namespace BlogApi.Models;
public class User : IdentityUser
{
    public virtual Person Person { get; set; }
}
