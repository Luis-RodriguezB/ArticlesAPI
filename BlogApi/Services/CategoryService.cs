using ArticlesAPI.DTOs.Category;
using ArticlesAPI.Entities;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Repositories;
using AutoMapper;

namespace ArticlesAPI.Services;

public interface ICategoryService
{
    Task<List<CategoryDTO>> GetAll();
    Task<CategoryDTO> GetById(int id);
    Task<CategoryDTO> Save(CategoryCreateDTO entity);
    Task Update(int id, CategoryCreateDTO entity);
    Task Delete(int id);
}

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository categoryRepository;
    private readonly IMapper mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        this.categoryRepository = categoryRepository;
        this.mapper = mapper;
    }

    public async Task<List<CategoryDTO>> GetAll()
    {
        var categories = await categoryRepository.GetAll();
        return mapper.Map<List<CategoryDTO>>(categories);
    }

    public async Task<CategoryDTO> GetById(int id)
    {
        var category = await categoryRepository.GetById(id);

        if(category  == null)
        {
            throw new NotFoundException($"Does not exist a category with the id {id}"); ;
        }

        return mapper.Map<CategoryDTO>(category);
    }

    public async Task<CategoryDTO> Save(CategoryCreateDTO entity)
    {
        var category = mapper.Map<Category>(entity);
        category.NameNormalized = category.Name.ToUpper().Trim();
        var categorySaved = await categoryRepository.Save(category);

        return mapper.Map<CategoryDTO>(categorySaved);
    }

    public async Task Update(int id, CategoryCreateDTO entity)
    {
        var categoryDb = await categoryRepository.GetById(id);

        if (categoryDb != null)
        {
            categoryDb = mapper.Map(entity, categoryDb);
            categoryDb.NameNormalized = categoryDb.Name.ToUpper().Trim();
            await categoryRepository.Update(categoryDb);
        } else
        {
            throw new NotFoundException($"Does not exist a category with the id {id}");
        }
    }
    public async Task Delete(int id)
    {
        var exist = await categoryRepository.Exist(id);

        if (exist)
        {
            await categoryRepository.Delete(id);
        } else
        {
            throw new NotFoundException($"Does not exist a category with the id {id}");
        }
    }
}
