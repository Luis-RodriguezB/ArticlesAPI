using ArticlesAPI.DTOs.Category;
using ArticlesAPI.Entities;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Repositories.Interfaces;
using ArticlesAPI.Services.Interfaces;

namespace ArticlesAPI.Services;

public class ArticleCategoryService : IArticleCategoryService
{
    private readonly IArticleCategoryRepository articleCategoryRepository;
    private readonly ICategoryRepository categoryRepository;

    public ArticleCategoryService(IArticleCategoryRepository articleCategoryRepository,
        ICategoryRepository categoryRepository)
    {
        this.articleCategoryRepository = articleCategoryRepository;
        this.categoryRepository = categoryRepository;
    }

    public async Task ValidateAndCreateArticleCategories(List<CategoryArticleDTO> categoryDTOs, int articleId)
    {
        foreach (var category in categoryDTOs)
        {
            if (!await categoryRepository.Exist(category.Id))
            {
                throw new NotFoundException($"The category with the id {category.Id} not found");
            }

            var articleCategory = new ArticleCategory
            {
                ArticleId = articleId,
                CategoryId = category.Id,
            };
            await articleCategoryRepository.Save(articleCategory);
        }
    }

    public async Task ValidateAndUpdateArticleCategories(IEnumerable<int> categoriesIdDb, IEnumerable<int> categoriesIdToUpdate, int articleId)
    {
        var categoriesToAdd = categoriesIdToUpdate.Except(categoriesIdDb).ToList();
        var categoriesToRemove = categoriesIdDb.Except(categoriesIdToUpdate).ToList();

        // Remove relations
        foreach (var categoryId in categoriesToRemove)
        {
            var articleCategory = await articleCategoryRepository.GetByIds(articleId, categoryId);

            if (articleCategory != null)
            {
                await articleCategoryRepository.Delete(articleCategory);
            }
        }

        // Add relations
        foreach (var categoryId in categoriesToAdd)
        {
            if (!await categoryRepository.Exist(categoryId))
            {
                throw new NotFoundException($"The category with the id {categoryId} not found");
            }

            var articleCategory = new ArticleCategory
            {
                ArticleId = articleId,
                CategoryId = categoryId,
            };

            await articleCategoryRepository.Save(articleCategory);
        }
    }
}
