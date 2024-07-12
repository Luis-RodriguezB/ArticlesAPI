using ArticlesAPI.DTOs.Others;
using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Filters;
public class PersonFilter : PaginationDTO
{
    public string Name { get; set; }
    public string LastName { get; set; }
    [RegularExpression("^(name|lastname)$", ErrorMessage = "TypeOrder must be 'name' or 'lastname'.")]
    public string TypeOrder { get; set; }
    [RegularExpression("^(asc|desc)$", ErrorMessage = "OrderBy must be 'asc' or 'desc'.")]
    public string OrderBy { get; set; }
}
