using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.Entities;
using ArticlesAPI.Interfaces;

namespace ArticlesAPI.Repositories.Interfaces;
public interface ICategoryRepository : IRepositoryBase<Category>
{
    Task<IEnumerable<Category>> GetAll(CategoryFilter categoryFilter);
    Task<bool> ExistCategoryName(string nameNormalized);
}
