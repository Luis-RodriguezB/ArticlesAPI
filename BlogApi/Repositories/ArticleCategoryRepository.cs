﻿using ArticlesAPI.Entities;
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

public class ArticleCategoryRepository : IDisposable, IArticleCategoryRepository
{
    private readonly ApplicationDbContext _context;

    public ArticleCategoryRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<ArticleCategory>> GetAll()
    {
        return await _context.ArticleCategories.ToListAsync();
    }

    public async Task<ArticleCategory> GetByIds(int articleId, int categoryId)
    {
        return await _context.ArticleCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ArticleId == articleId && x.CategoryId == categoryId);
    }

    public async Task<ArticleCategory> GetByArticleId(int articleId)
    {
        return await _context.ArticleCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ArticleId == articleId);
    }

    public async Task<ArticleCategory> GetByCategoryId(int categoryId)
    {
        return await _context.ArticleCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.CategoryId == categoryId);
    }

    public async Task<ArticleCategory> Save(ArticleCategory entity)
    {
        if (await Exist(entity))
        {
            throw new BadRequestException($"Already exist a relation with the article with the id {entity.ArticleId} and the category with the id {entity.CategoryId}");
        }

        _context.ArticleCategories.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task Update(ArticleCategory entity)
    {
        if (!await Exist(entity))
        {
            throw new NotFoundException($"There is no relation to the article with the id {entity.ArticleId} and the category with the id {entity.CategoryId}");
        }

        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(ArticleCategory entity)
    {
        if (!await Exist(entity))
        {
            throw new NotFoundException($"There is no relation to the article with the id {entity.ArticleId} and the category with the id {entity.CategoryId}");
        }

        _context.ArticleCategories.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exist(ArticleCategory entity)
    {
        return await _context.ArticleCategories
            .AsNoTracking()
            .AnyAsync(x => x.ArticleId == entity.ArticleId && x.CategoryId == entity.CategoryId);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
