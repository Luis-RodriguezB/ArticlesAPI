using ArticlesAPI.DTOs.Article;
using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Repositories.Interfaces;
using ArticlesAPI.Services.Interfaces;
using AutoMapper;
using BlogApi;
using BlogApi.DTOs.Blog;
using BlogApi.Models;

namespace ArticlesAPI.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository articleRepository;
    private readonly IArticleCategoryService articleCategoryService;
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public ArticleService(IArticleRepository articleRepository,
        IArticleCategoryService articleCategoryService,
        ApplicationDbContext context,
        IMapper mapper)
    {
        this.articleRepository = articleRepository;
        this.articleCategoryService = articleCategoryService;
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<ArticleDTO>> GetAll()
    {
        var articles = await articleRepository.GetAll();

        return mapper.Map<List<ArticleDTO>>(articles);
    }

    public async Task<List<ArticleDTO>> GetAll(ArticleFilter articleFilter)
    {
        var articles = await articleRepository.GetAll(articleFilter);

        return mapper.Map<List<ArticleDTO>>(articles);
    }

    public async Task<List<ArticleDTO>> Search(ArticleFilter articleFilter)
    {
        var articles = await articleRepository.Search(articleFilter);

        return mapper.Map<List<ArticleDTO>>(articles);
    }

    public async Task<ArticleDTO> GetById(int id)
    {
        var article = await articleRepository.GetById(id) 
            ?? throw new NotFoundException($"The article with the id {id} does not exist");

        return mapper.Map<ArticleDTO>(article);
    }

    public async Task<ArticleDTO> Save(ArticleCreateDTO entity)
    {
        using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                var article = mapper.Map<Article>(entity);
                await articleRepository.Save(article);
                await articleCategoryService.ValidateAndCreate(entity.Categories, article.Id);

                await transaction.CommitAsync();
                return await GetById(article.Id);
            }
            catch (NotFoundException)
            {
                await transaction.RollbackAsync();
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new BadRequestException("An error occurred while creating the article. " + ex.Message);
            }
        }
    }

    public async Task Update(int id, ArticleUpdateDTO entity, string userId)
    {
        var articleDb = await articleRepository.GetById(id) 
            ?? throw new NotFoundException($"The article with the Id {id} not exist");

        if (articleDb.Person.UserId != userId)
        {
            throw new ForbiddenException($"The article with the id {id} doesn´t belong to the user with the id {userId}");
        }

        using (var transaction = await context.Database.BeginTransactionAsync())
        {
            try
            {
                articleDb = mapper.Map(entity, articleDb);
                await articleRepository.Update(articleDb);
                await articleCategoryService.ValidateAndUpdate(entity.Categories, articleDb.Id);

                await transaction.CommitAsync();
            } catch (NotFoundException)
            {
                await transaction.RollbackAsync();
                throw;
            } catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new BadRequestException("An error occurred while updating the article. " + ex.Message);
            }
        }
    }

    public async Task Delete(int id, string userId)
    {
        var articleDb = await articleRepository.GetById(id) 
            ?? throw new NotFoundException($"The article with the Id {id} not exist");

        if (articleDb.Person.UserId != userId)
        {
            throw new ForbiddenException($"The article with the id {id} doesn´t belong to the user with the id {userId}");
        }
        
        await articleRepository.Delete(id);
    }
}
