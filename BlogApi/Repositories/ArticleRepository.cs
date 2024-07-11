﻿using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.DTOs.Others;
using ArticlesAPI.Helpers;
using ArticlesAPI.Interfaces;
using ArticlesAPI.Utils;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public interface IArticleRepository : IRepositoryBase<Article>
{
    Task<IEnumerable<Article>> GetAll(ArticleFilter articleFilter);
    Task<IEnumerable<Article>> GetAll(ArticleFilter articleFilter, IQueryable<Article> queryable);
    Task<IEnumerable<Article>> Search(ArticleFilter articleFilter);
    Task<IEnumerable<Article>> GetAllByPersonId(int personId, ArticleFilter articleFilter);
    Task<Article> GetByIdAndPersonId(int id, int personId);   
    Task<bool> IsArticleBelongPerson(int id, int personId);
}

public class ArticleRepository(ApplicationDbContext context, IHttpContextAccessor httpContext) : IArticleRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor httpContext = httpContext;

    public async Task<IEnumerable<Article>> GetAll()
    {
        return await GetArticleQueryable()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetAll(ArticleFilter articleFilter)
    {
        IQueryable<Article> queryable = GetArticleQueryableFilter(articleFilter).AsNoTracking();
        return await GetAll(articleFilter, queryable);
    }

    public async Task<IEnumerable<Article>> GetAll(ArticleFilter articleFilter, IQueryable<Article> queryable)
    {
        var paginationDTO = Utilities.GetPaginationDTO(articleFilter);
        await httpContext.HttpContext.InserPaginationParams(queryable, paginationDTO.RecordsByPage);

        return await queryable.Paginate(paginationDTO).ToListAsync();
    }

    public async Task<IEnumerable<Article>> Search(ArticleFilter articleFilter)
    {
        IQueryable<Article> articleQueryable = GetArticleQueryableFilter(articleFilter).AsNoTracking();
        return await GetAll(articleFilter, articleQueryable);
    }

    public async Task<IEnumerable<Article>> GetAllByPersonId(int personId, ArticleFilter articleFilter)
    {
        IQueryable<Article> articleQueryable = GetArticleQueryableFilter(articleFilter);
        articleQueryable =  articleQueryable
            .Where(a => a.PersonId == personId)
            .AsNoTracking();

        return await GetAll(articleFilter, articleQueryable);
    }

    public async Task<Article> GetById(int id)
    {  
        return await GetArticleQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Article> GetByIdAndPersonId(int id, int personId)
    {
        IQueryable<Article> articleQueryable = GetArticleQueryable();

        return await articleQueryable
            .FirstOrDefaultAsync(x => x.Id == id && x.PersonId == personId);
    }

    public async Task<Article> Save(Article article)
    {
        article.CreatedAt = DateTime.Now;
        article.UpdatedAt = DateTime.Now;

        _context.Add(article);
        await _context.SaveChangesAsync();

        return article;
    }

    public async Task Update(Article article)
    {
        article.UpdatedAt = DateTime.Now;
        _context.Entry(article).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        _context.Remove(new Article { Id = id });
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.Articles
            .AsNoTracking()
            .AnyAsync(x => x.Id == id);
    }

    public async Task<bool> IsArticleBelongPerson(int id, int personId)
    {
        return await _context.Articles
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && x.PersonId == personId);
    }

    private IQueryable<Article> GetArticleQueryableFilter(ArticleFilter articleFilter)
    {
        IQueryable<Article> articlesQueryable = GetArticleQueryable();

        if (articleFilter != null)
        {
            if (!string.IsNullOrEmpty(articleFilter.Title))
            {
                articlesQueryable = articlesQueryable.Where(x => x.Title.Contains(articleFilter.Title));
            }

            if (articleFilter.Date != null)
            {
                articlesQueryable = articlesQueryable.Where(x => x.CreatedAt <= articleFilter.Date);
            }

            if (articleFilter.OrderBy != null)
            {
                articlesQueryable = articleFilter.OrderBy == "asc" 
                    ? articlesQueryable.OrderBy(x => x.CreatedAt) 
                    : articlesQueryable.OrderByDescending(x => x.CreatedAt);
            } else
            {
                articlesQueryable = articlesQueryable.OrderByDescending(x => x.CreatedAt);
            }
        }

        return articlesQueryable;
    }

    private IQueryable<Article> GetArticleQueryable()
    {
        return _context.Articles
            .Include(a => a.Person)
            .ThenInclude(p => p.User)
            .Include(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .AsQueryable();
    }
}
