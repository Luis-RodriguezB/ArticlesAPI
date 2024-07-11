using ArticlesAPI.Entities;
using ArticlesAPI.HandleErrors;
using BlogApi;
using Microsoft.EntityFrameworkCore;

namespace ArticlesAPI.Repositories;

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

public class ArticleCategoryRepository : IArticleCategoryRepository
{
    private readonly ApplicationDbContext context;

    public ArticleCategoryRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<ArticleCategory>> GetAll()
    {
        return await context.ArticleCategories.ToListAsync();
    }

    public async Task<ArticleCategory> GetByIds(int articleId, int categoryId)
    {
        return await context.ArticleCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ArticleId == articleId && x.CategoryId == categoryId);
    }

    public async Task<ArticleCategory> GetByArticleId(int articleId)
    {
        return await context.ArticleCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ArticleId == articleId);
    }

    public async Task<ArticleCategory> GetByCategoryId(int categoryId)
    {
        return await context.ArticleCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CategoryId == categoryId);
    }

    public async Task<ArticleCategory> Save(ArticleCategory entity)
    {
        if (await Exist(entity))
        {
            throw new BadRequestException($"Already exist a relation with the article with the id {entity.ArticleId} and the category with the id {entity.CategoryId}");
        }

        context.ArticleCategories.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task Update(ArticleCategory entity)
    {
        if (!await Exist(entity))
        {
            throw new NotFoundException($"There is no relation to the article with the id {entity.ArticleId} and the category with the id {entity.CategoryId}");
        }

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task Delete(ArticleCategory entity)
    {
        if (!await Exist(entity))
        {
            throw new NotFoundException($"There is no relation to the article with the id {entity.ArticleId} and the category with the id {entity.CategoryId}");
        }

        context.ArticleCategories.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task<bool> Exist(ArticleCategory entity)
    {
        return await context.ArticleCategories
            .AsNoTracking()
            .AnyAsync(x => x.ArticleId == entity.ArticleId && x.CategoryId == entity.CategoryId);
    }
}
