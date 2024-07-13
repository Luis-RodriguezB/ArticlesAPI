using ArticlesAPI.DTOs.Auth;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Services.Interfaces;
using BlogApi.DTOs.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDTO>> Register([FromBody] RegisterDTO registerDTO)
    {
        try
        {
            return await authService.Register(registerDTO);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { ex.Message });
        }

    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginDTO loginDTO)
    {
        try
        {
            return await authService.Login(loginDTO);

        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { ex.Message });
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<List<UserDTO>>> Get()
    {
        return await authService.Get();
    }

    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<UserDTO>> Get(string id)
    {
        return await authService.GetById(id);
    }

    [HttpPost("renew-token")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<AuthResponseDTO>> Renew()
    {
        var userEmail = HttpContext.User.Identity.Name;
        try
        {
            return await authService.RenewToken(userEmail);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpPost("toggle-admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult> ToggleAdmin([FromBody] UserEmailDTO userEmailDTO)
    {
        try
        {
            var response = await authService.ToggleAdmin(userEmailDTO.Email);
            return Ok(response);
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult> Delete(string id)
    {
        try
        {
            var response = await authService.Delete(id);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}
