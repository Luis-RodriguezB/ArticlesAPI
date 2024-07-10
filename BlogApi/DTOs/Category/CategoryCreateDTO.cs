using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Category;
public class CategoryCreateDTO
{
    [Required]
    public string Name { get; set; }
}
