using ArticlesAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesAPI.Controllers;

[ApiController]
[Route("api/users/{personId:int}/articles")]
public class UserArticlesController : ControllerBase
{
    private readonly IArticleService articleService;

    public UserArticlesController(IArticleService articleService)
    {
        this.articleService = articleService;
    }
}
