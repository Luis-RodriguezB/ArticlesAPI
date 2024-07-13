using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.Interfaces;
using BlogApi.Models;

namespace ArticlesAPI.Repositories.Interfaces;

public interface IArticleRepository : IRepositoryBase<Article>
{
    Task<IEnumerable<Article>> GetAll(ArticleFilter articleFilter);
    Task<IEnumerable<Article>> GetAll(ArticleFilter articleFilter, IQueryable<Article> queryable);
    Task<IEnumerable<Article>> Search(ArticleFilter articleFilter);
    Task<IEnumerable<Article>> GetAllByPersonId(int personId, ArticleFilter articleFilter);
    Task<Article> GetByIdAndPersonId(int id, int personId);
    Task<bool> IsArticleBelongPerson(int id, int personId);
}
