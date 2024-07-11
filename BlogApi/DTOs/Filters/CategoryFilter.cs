using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Filters;
public class CategoryFilter
{
    public string Name { get; set; }
    [RegularExpression("^(asc|desc)$", ErrorMessage = "OrderBy must be 'asc' or 'desc'.")]
    public string OrderBy { get; set; }
}
