using ArticlesAPI.Entities;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Interfaces;
using BlogApi;
using Microsoft.EntityFrameworkCore;

namespace ArticlesAPI.Repositories;

public interface ICategoryRepository : IRepositoryBase<Category>
{
    Task<bool> ExistCategoryName(string nameNormalized);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        this._context = context;
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _context.Categories
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Category> GetById(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Category> Save(Category entity)
    {
        if (!await ExistCategoryName(entity.NameNormalized))
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        throw new BadRequestException($"Already exist a category with the name { entity.Name }");
    }

    public async Task Update(Category entity)
    {
        if (!await ExistCategoryName(entity.NameNormalized))
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        } else
        {
            throw new BadRequestException($"Already exist a category with the name {entity.Name}");
        }
    }

    public async Task Delete(int id)
    {
        if (await Exist(id))
        {
            _context.Categories.Remove(new Category {  Id = id });
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> Exist(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .AnyAsync(x => x.Id == id);
    }

    public async Task<bool> ExistCategoryName(string nameNormalized)
    {
        return await _context.Categories
            .AsNoTracking()
            .AnyAsync(x => x.NameNormalized.Contains(nameNormalized));
    }
}
