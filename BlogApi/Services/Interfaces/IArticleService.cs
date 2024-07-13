using ArticlesAPI.DTOs.Article;
using ArticlesAPI.DTOs.Filters;
using BlogApi.DTOs.Blog;

namespace ArticlesAPI.Services.Interfaces;
public interface IArticleService
{
    Task<List<ArticleDTO>> GetAll();
    Task<List<ArticleDTO>> GetAll(ArticleFilter articleFilter);
    Task<List<ArticleDTO>> Search(ArticleFilter articleFilter);
    Task<ArticleDTO> GetById(int id);
    Task<ArticleDTO> Save(ArticleCreateDTO entity);
    Task Update(int id, ArticleUpdateDTO entity, string userId);
    Task Delete(int id, string userId);
}
