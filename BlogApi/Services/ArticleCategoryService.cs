using ArticlesAPI.DTOs.Category;
using ArticlesAPI.Entities;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Repositories.Interfaces;
using ArticlesAPI.Services.Interfaces;
using BlogApi.Models;

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

    public async Task ValidateAndCreate(List<CategoryArticleDTO> categoryDTOs, int articleId)
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

    public async Task ValidateAndUpdate(IEnumerable<CategoryArticleDTO> categoryArticleDTOs, int articleId)
    {
        var articleCategories = await articleCategoryRepository.GetAllByArticleId(articleId);

        IEnumerable<int> categoriesIdsDb = articleCategories.Select(x => x.CategoryId).ToList();
        IEnumerable<int> categoriesIdsUpdate = categoryArticleDTOs.Select(x => x.Id).ToList();

        GetIdsToAddAndDelete(
            categoriesIdsDb,
            categoriesIdsUpdate,
            out IEnumerable<int> categoriesToAdd,
            out IEnumerable<int> categoriesToRemove
        );

        // RemoveCategories relations
        await RemoveCategories(articleId, categoriesToRemove);

        // Add relations
        await AddCategories(articleId, categoriesToAdd);
    }

    private async Task RemoveCategories(int articleId, IEnumerable<int> categoriesToRemove)
    {
        foreach (var categoryId in categoriesToRemove)
        {
            var articleCategory = await articleCategoryRepository.GetByIds(articleId, categoryId);

            if (articleCategory != null)
            {
                await articleCategoryRepository.Delete(articleCategory);
            }
        }
    }

    private async Task AddCategories(int articleId, IEnumerable<int> categoriesToAdd)
    {
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

    private static void GetIdsToAddAndDelete(
        IEnumerable<int> categoriesIdsDb,
        IEnumerable<int> categoriesIdsUpdate,
        out IEnumerable<int> categoriesToAdd,
        out IEnumerable<int> categoriesToRemove)
    {
        categoriesToAdd = categoriesIdsUpdate.Except(categoriesIdsDb).ToList();
        categoriesToRemove = categoriesIdsDb.Except(categoriesIdsUpdate).ToList();
    }
}
