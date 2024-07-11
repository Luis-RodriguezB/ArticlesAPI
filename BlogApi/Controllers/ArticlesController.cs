using ArticlesAPI.DTOs.Article;
using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.DTOs.Others;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Services;
using BlogApi.DTOs.Blog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticlesController : ControllerBase
{
    private readonly IArticleService articleService;

    public ArticlesController(IArticleService articleService)
    {
        this.articleService = articleService;
    }

    [HttpGet(Name = "GetArticles")]
    public async Task<ActionResult<List<ArticleDTO>>> Get([FromQuery] ArticleFilter articleFilter)
    {
        return await articleService.GetAll(articleFilter);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<ArticleDTO>>> Search([FromQuery] ArticleFilter articleFilter)
    {
        return await articleService.Search(articleFilter);
    }

    [HttpGet("{id:int}", Name = "GetArticle")]
    public async Task<ActionResult<ArticleDTO>> GetById(int id)
    {
        try
        {
            return await articleService.GetById(id);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<ArticleDTO>> Post([FromBody] ArticleCreateDTO articleCreateDTO)
    {
        try
        {
            var article = await articleService.Save(articleCreateDTO);
            return CreatedAtRoute("GetArticle", new { article.Id }, article);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Update(int id, [FromBody] ArticleUpdateDTO articleUpdateDTO)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await articleService.Update(id, articleUpdateDTO, userId);

            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (ForbiddenException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await articleService.Delete(id, userId);

            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (ForbiddenException ex)
        {
            return Forbid(ex.Message);
        }
    }
}
