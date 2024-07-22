using ArticlesAPI.DTOs.Rating;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesAPI.Controllers;

[ApiController]
[Route("api/rating")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RatingController : ControllerBase
{
    private readonly IRatingService ratingService;

    public RatingController(IRatingService ratingService)
    {
        this.ratingService = ratingService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(RatingCreateDTO ratingCreateDTO)
    {
        try
        {
            await ratingService.Save(ratingCreateDTO);
            return Ok();
        }
        catch (BadRequestException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(RatingCreateDTO ratingCreateDTO)
    {
        try
        {
            await ratingService.Update(ratingCreateDTO);
            return Ok();
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

    [HttpDelete]
    public async Task<IActionResult> Delete(RatingDeleteDTO ratingDeleteDTO)
    {
        try
        {
            await ratingService.Delete(ratingDeleteDTO);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }
}
