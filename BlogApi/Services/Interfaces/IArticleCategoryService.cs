using ArticlesAPI.DTOs.Category;

namespace ArticlesAPI.Services.Interfaces;
public interface IArticleCategoryService
{
    Task ValidateAndCreate(List<CategoryArticleDTO> categoryDTOs, int articleId);
    Task ValidateAndUpdate(IEnumerable<CategoryArticleDTO> categoryArticleDTOs, int articleId);
}