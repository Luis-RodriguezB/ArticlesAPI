using BlogApi.Models;

namespace ArticlesAPI.Repositories.Interfaces;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAll();
    Task<User> GetById(string id);
}
