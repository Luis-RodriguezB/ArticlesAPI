using ArticlesAPI.Entities;

namespace ArticlesAPI.Repositories.Interfaces;

public interface IArticleCategoryRepository
{
    Task<IEnumerable<ArticleCategory>> GetAll();
    Task<IEnumerable<ArticleCategory>> GetAllByArticleId(int articleId);
    Task<ArticleCategory> GetByIds(int articleId, int categoryId);
    Task<ArticleCategory> Save(ArticleCategory entity);
    Task Update(ArticleCategory entity);
    Task Delete(ArticleCategory entity);
    Task<bool> Exist(ArticleCategory entity);
}
