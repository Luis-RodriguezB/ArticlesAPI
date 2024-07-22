using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Rating;
public class RatingDeleteDTO
{
    [Required]
    public int PersonId { get; set; }
    [Required]
    public int ArticleId { get; set; }
}
