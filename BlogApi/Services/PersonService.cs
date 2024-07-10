using ArticlesAPI.DTOs.Person;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Repositories;
using AutoMapper;

namespace ArticlesAPI.Services;

public interface IPersonService
{
    Task<List<PersonDTO>> GetAll();
    Task<PersonDTO> GetById(int id);
    Task Update(int id, PersonUpdateDTO entity, string userId);
}
public class PersonService : IPersonService
{
    private readonly IPersonRepository personRepository;
    private readonly IMapper mapper;

    public PersonService(IPersonRepository personRepository, IMapper mapper)
    {
        this.personRepository = personRepository;
        this.mapper = mapper;
    }

    public async Task<List<PersonDTO>> GetAll()
    {
        var users = await personRepository.GetAll();

        return mapper.Map<List<PersonDTO>>(users);
    }

    public async Task<PersonDTO> GetById(int id)
    {
        var person = await personRepository.GetById(id);

        if (person == null)
        {
            throw new NotFoundException($"The person with the id {id} does not exist");
        }

        return mapper.Map<PersonDTO>(person);
    }

    public async Task Update(int id, PersonUpdateDTO entity, string userId)
    {
        if (userId is null)
        {
            throw new ArgumentNullException(nameof(userId));
        }

        var personDb = await personRepository.GetById(id);

        if (personDb != null)
        {
            if (personDb.UserId == userId)
            {
                personDb = mapper.Map(entity, personDb);
                await personRepository.Update(personDb);
            } else
            {
                throw new ForbiddenException($"Not authorized to perform this action");
            }
        } else
        {
            throw new NotFoundException($"The user with the id {id} not exist");
        }
    }
}
