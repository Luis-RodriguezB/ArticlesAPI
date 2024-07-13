using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.Entities;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Repositories.Interfaces;
using BlogApi;
using Microsoft.EntityFrameworkCore;

namespace ArticlesAPI.Repositories;

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

    public async Task<IEnumerable<Category>> GetAll(CategoryFilter categoryFilter)
    {
        IQueryable<Category> categoryQueryable = GetCategortQueryableFilter(categoryFilter);
        return await categoryQueryable
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
            .AnyAsync(x => x.NameNormalized.Equals(nameNormalized));
    }

    private IQueryable<Category> GetCategortQueryableFilter(CategoryFilter categoryFilter)
    {
        IQueryable<Category> categoryQueryable = _context.Categories.AsQueryable();

        if (categoryFilter != null)
        {
            if (!string.IsNullOrEmpty(categoryFilter.Name))
            {
                categoryQueryable = categoryQueryable.Where(x => x.Name.Contains(categoryFilter.Name));
            }

            if (!string.IsNullOrEmpty(categoryFilter.OrderBy))
            {
                categoryQueryable = categoryFilter.OrderBy == "asc"
                    ? categoryQueryable.OrderBy(x => x.Name)
                    : categoryQueryable.OrderByDescending(x => x.Name);
            }
        }

        return categoryQueryable;
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
