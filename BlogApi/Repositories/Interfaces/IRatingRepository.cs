using ArticlesAPI.Entities;

namespace ArticlesAPI.Repositories.Interfaces;
public interface IRatingRepository
{
    Task Save(Rating rating);
    Task Delete(Rating rating);
    Task<bool> Exist(int personId, int articleId);
    Task<Rating> GetByIds(int personId, int articleId);
    Task Update(Rating rating);
}
