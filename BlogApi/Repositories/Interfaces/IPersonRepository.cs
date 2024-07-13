using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.Interfaces;
using BlogApi.Models;

namespace ArticlesAPI.Repositories.Interfaces;
public interface IPersonRepository : IRepositoryBase<Person>
{
    Task<IEnumerable<Person>> GetAll(PersonFilter personFilter);
    Task<IEnumerable<Person>> GetAll(PersonFilter personFilter, IQueryable<Person> queryable);
    Task<Person> GetPersonByUserId(string userId);
}
