using ArticlesAPI.HandleErrors;
using ArticlesAPI.Repositories;
using AutoMapper;
using BlogApi.DTOs.Blog;
using BlogApi.Repositories;

namespace ArticlesAPI.Services;

public interface IUserArticleService
{
    Task<List<ArticleDTO>> GetArticlesByPersonId(int personId);
    Task<ArticleDTO> GetArticleByPersonId(int id, int personId);
}
public class UserArticleService : IUserArticleService
{
    private readonly IArticleRepository articleRepository;
    private readonly IPersonRepository personRepository;
    private readonly IMapper mapper;

    public UserArticleService(IArticleRepository articleRepository,
        IPersonRepository personRepository,
        IMapper mapper)
    {
        this.articleRepository = articleRepository;
        this.personRepository = personRepository;
        this.mapper = mapper;
    }

    public async Task<List<ArticleDTO>> GetArticlesByPersonId(int personId)
    {
        if (!await personRepository.Exist(personId))
        {
            throw new NotFoundException($"Does not exist a person with the id {personId}");
        }

        var articles = await articleRepository.GetAllByPersonId(personId);
        return mapper.Map<List<ArticleDTO>>(articles);
    }

    public async Task<ArticleDTO> GetArticleByPersonId(int id, int personId)
    {
        if (!await personRepository.Exist(personId))
        {
            throw new NotFoundException($"Does not exist a person with the id {personId}");
        }

        var article = articleRepository.GetByIdAndPersonId(id, personId);
        return mapper.Map<ArticleDTO>(article);
    }
}
