using ArticlesAPI.Entities;

namespace ArticlesAPI.Repositories.Interfaces;

public interface IArticleCategoryRepository
{
    Task<IEnumerable<ArticleCategory>> GetAll();
    Task<ArticleCategory> GetByIds(int articleId, int categoryId);
    Task<ArticleCategory> GetByArticleId(int articleId);
    Task<ArticleCategory> GetByCategoryId(int categoryId);
    Task<ArticleCategory> Save(ArticleCategory entity);
    Task Update(ArticleCategory entity);
    Task Delete(ArticleCategory entity);
    Task<bool> Exist(ArticleCategory entity);
}
