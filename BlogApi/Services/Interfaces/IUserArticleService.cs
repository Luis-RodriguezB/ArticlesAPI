using ArticlesAPI.DTOs.Filters;
using BlogApi.DTOs.Blog;

namespace ArticlesAPI.Services.Interfaces;
public interface IUserArticleService
{
    Task<List<ArticleDTO>> GetArticlesByPersonId(int personId, ArticleFilter articleFilter);
    Task<ArticleDTO> GetArticleByPersonId(int id, int personId);
}
