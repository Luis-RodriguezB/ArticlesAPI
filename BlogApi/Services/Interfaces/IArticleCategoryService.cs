using ArticlesAPI.DTOs.Category;

namespace ArticlesAPI.Services.Interfaces;
public interface IArticleCategoryService
{
    Task ValidateAndCreateArticleCategories(List<CategoryArticleDTO> categoryDTOs, int articleId);
    Task ValidateAndUpdateArticleCategories(IEnumerable<int> categoriesIdDb, IEnumerable<int> categoriesIdToUpdate, int articleId);
}