using ArticlesAPI.DTOs.Person;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArticlesAPI.Controllers;

[ApiController]
[Route("api/users")]
public class PeopleController : ControllerBase
{
    private readonly IPersonService personService;

    public PeopleController(IPersonService personService)
    {
        this.personService = personService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonDTO>> Get(int id)
    {
        try
        {
            return await personService.GetById(id);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<PersonDTO>> Update(int id, PersonUpdateDTO personUpdateDTO)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await personService.Update(id, personUpdateDTO, userId);

            return NoContent();
        }
        catch (ForbiddenException ex)
        {
            return Forbid(ex.Message);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }
}
