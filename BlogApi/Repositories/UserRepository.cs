using ArticlesAPI.Repositories.Interfaces;
using BlogApi;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticlesAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext context;

    public UserRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await GetQueryable().ToListAsync();
    }

    public async Task<User> GetById(string id)
    {
        return await GetQueryable().FirstOrDefaultAsync(x => x.Id == id);
    }

    private IQueryable<User> GetQueryable()
    {
        return context.Users
            .Include(u => u.Person)
            .ThenInclude(p => p.Articles)
            .ThenInclude(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .Where(u => !u.Email.Contains("admin@admin.com"))
            .AsNoTracking()
            .AsQueryable();
    }
}
