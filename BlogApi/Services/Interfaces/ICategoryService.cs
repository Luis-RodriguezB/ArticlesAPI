using ArticlesAPI.DTOs.Category;
using ArticlesAPI.DTOs.Filters;

namespace ArticlesAPI.Services.Interfaces;
public interface ICategoryService
{
    Task<List<CategoryDTO>> GetAll(CategoryFilter categoryFilter);
    Task<CategoryDTO> GetById(int id);
    Task<CategoryDTO> Save(CategoryCreateDTO entity);
    Task Update(int id, CategoryCreateDTO entity);
    Task Delete(int id);
}

