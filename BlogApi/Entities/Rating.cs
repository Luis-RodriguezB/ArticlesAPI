using BlogApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ArticlesAPI.Entities;
public class Rating
{
    [Required]
    public bool Like { get; set; }
    public bool DisLike { get; set; }
    public int PersonId { get; set; }
    public int ArticleId { get; set; }
    public virtual Person Person { get; set; }
    public virtual Article Article { get; set; }
}
