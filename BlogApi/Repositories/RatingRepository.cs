using ArticlesAPI.Entities;
using ArticlesAPI.Repositories.Interfaces;
using BlogApi;
using Microsoft.EntityFrameworkCore;

namespace ArticlesAPI.Repositories;
public class RatingRepository : IRatingRepository
{
    private readonly ApplicationDbContext _context;

    public RatingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Rating> GetByIds(int personId, int articleId)
    {
        return await _context.Ratings
            .FirstOrDefaultAsync(x => x.PersonId == personId && x.ArticleId == articleId);
    }

    public async Task Save(Rating rating)
    {
        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Rating rating)
    {
        _context.Entry(rating).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    } 

    public async Task Delete(Rating rating)
    {
        _context.Ratings.Remove(rating);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Exist(int personId, int articleId)
    {
        return await _context.Ratings.AnyAsync(x => x.PersonId == personId && x.ArticleId == articleId);
    }
}
