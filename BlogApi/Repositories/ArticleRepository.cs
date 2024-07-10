using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.DTOs.Others;
using ArticlesAPI.Helpers;
using ArticlesAPI.Interfaces;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public interface IArticleRepository : IRepositoryBase<Article>
{
    Task<IEnumerable<Article>> GetAll(PaginationDTO paginationDTO);
    Task<IEnumerable<Article>> GetAll(PaginationDTO paginationDTO, IQueryable<Article> queryable);
    Task<IEnumerable<Article>> Search(ArticleFilter articleFilter);
    Task<bool> IsArticleBelongPerson(int id, int personId);
}

public class ArticleRepository(ApplicationDbContext context, IHttpContextAccessor httpContext) : IArticleRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor httpContext = httpContext;

    public async Task<IEnumerable<Article>> GetAll()
    {
        return await _context.Articles
            .Include(a => a.Person)
            .ThenInclude(p => p.User)
            .Include(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Article>> GetAll(PaginationDTO paginationDTO)
    {
        var queryable = _context.Articles
            .Include(a => a.Person)
            .ThenInclude(p => p.User)
            .Include(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .AsQueryable();
        return await GetAll(paginationDTO, queryable);
    }

    public async Task<IEnumerable<Article>> GetAll(PaginationDTO paginationDTO, IQueryable<Article> queryable)
    {
        await httpContext.HttpContext.InserPaginationParams(queryable, paginationDTO.RecordsByPage);
        return await queryable.Paginate(paginationDTO).ToListAsync();
    }

    public async Task<IEnumerable<Article>> Search(ArticleFilter articleFilter)
    {
        var articlesQueryable = _context.Articles.AsQueryable();

        if (!string.IsNullOrEmpty(articleFilter.Title))
        {
            articlesQueryable = articlesQueryable.Where(x => x.Title.Contains(articleFilter.Title));
        }

        if(articleFilter.Date != null)
        {
            articlesQueryable = articlesQueryable.Where(x => x.CreatedAt <= articleFilter.Date);
        }

        return await articlesQueryable
            .Include(a => a.Person)
            .ThenInclude(p => p.User)
            .Include(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .ToListAsync();
    }

    public async Task<Article> GetById(int id)
    {
        return await _context.Articles
            .Include(a => a.Person)
            .ThenInclude(p => p.User)
            .Include(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
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
}
