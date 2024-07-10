using ArticlesAPI.Interfaces;
using BlogApi;
using BlogApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticlesAPI.Repositories;

public interface IPersonRepository : IRepositoryBase<Person>
{
    Task<Person> GetPersonByUserId(string userId);
}

public class PersonRepository : IPersonRepository
{
    private readonly ApplicationDbContext _context;

    public PersonRepository(ApplicationDbContext context)
    {
        this._context = context;
    }
    public async Task<IEnumerable<Person>> GetAll()
    {
        return await _context.People
            .Include(p => p.User)
            .Include(p => p.Articles)
            .ThenInclude(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Person> GetById(int id)
    {
        return await _context.People
            .Include(p => p.User)
            .Include (p => p.Articles)
            .ThenInclude(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Person> GetPersonByUserId(string userId)
    {
        return await _context.People
            .Include(p => p.User)
            .Include(p => p.Articles)
            .ThenInclude(a => a.ArticleCategories)
            .ThenInclude(ac => ac.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<Person> Save(Person entity)
    {
        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;

        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task Update(Person person)
    {
        person.UpdatedAt = DateTime.Now;
        _context.Entry(person).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var exist = await Exist(id);
        if (exist)
        {
            _context.Remove(new Person { Id = id });
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.People.AnyAsync(x => x.Id == id);
    }
}
