using ArticlesAPI.DTOs.Others;
using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Filters;
public class UserFilter : PaginationDTO
{
    public string Username { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    [RegularExpression("^(username|name|lastname|email)$", ErrorMessage = "TypeOrder must be 'username' or 'name' or 'lastname'.")]
    public string TypeOrder { get; set; } = "username";
    [RegularExpression("^(asc|desc)$", ErrorMessage = "OrderBy must be 'asc' or 'desc'.")]
    public string OrderBy { get; set; }
}
