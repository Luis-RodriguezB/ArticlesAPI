using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Services;
using BlogApi.DTOs.Blog;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesAPI.Controllers;

[ApiController]
[Route("api/users/{personId:int}/articles")]
public class UserArticlesController : ControllerBase
{
    private readonly IUserArticleService userArticleService;

    public UserArticlesController(IUserArticleService userArticleService)
    {
        this.userArticleService = userArticleService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ArticleDTO>>> Get(int personId, [FromQuery] ArticleFilter articleFilter)
    {
        try
        {
            return await userArticleService.GetArticlesByPersonId(personId, articleFilter);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ArticleDTO>> GetById(int personId, int id)
    {
        try
        {
            return await userArticleService.GetArticleByPersonId(id, personId);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }
}
