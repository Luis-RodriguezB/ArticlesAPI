using ArticlesAPI.DTOs.Rating;

namespace ArticlesAPI.Services.Interfaces;
public interface IRatingService
{
    Task Save(RatingCreateDTO entity);
    Task Update(RatingCreateDTO entity);
    Task Delete(RatingDeleteDTO ratingDeleteDTO);
}
