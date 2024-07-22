using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.DTOs.Rating;
public class RatingCreateDTO
{
    [Required]
    public int ArticleId { get; set; }
    [Required]
    public int PersonId { get; set; }
    [Required]
    public bool Like { get; set; }
    [Required]
    public bool Dislike { get; set; }
}
