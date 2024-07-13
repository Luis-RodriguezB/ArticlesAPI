using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.DTOs.Person;

namespace ArticlesAPI.Services.Interfaces;
public interface IPersonService
{
    Task<List<PersonDTO>> GetAll(PersonFilter personFilter);
    Task<PersonDTO> GetById(int id);
    Task Update(int id, PersonUpdateDTO entity, string userId);
}
