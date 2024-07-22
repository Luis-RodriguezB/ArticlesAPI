using ArticlesAPI.DTOs.Filters;
using BlogApi.Models;

namespace ArticlesAPI.Repositories.Interfaces;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll(UserFilter userFilter);
    Task<User> GetById(string id);
}
