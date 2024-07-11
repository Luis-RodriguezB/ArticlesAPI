using ArticlesAPI.DTOs.Category;
using ArticlesAPI.DTOs.Filters;
using ArticlesAPI.HandleErrors;
using ArticlesAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArticlesAPI.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoryDTO>>> Get([FromQuery] CategoryFilter categoryFilter)
    {
        return await categoryService.GetAll(categoryFilter);
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    public async Task<ActionResult<CategoryDTO>> GetById(int id)
    {
        try
        {
            return await categoryService.GetById(id);
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<CategoryDTO>> Post([FromBody] CategoryCreateDTO categoryCreateDTO)
    {
        try
        {
            var category = await categoryService.Save(categoryCreateDTO);
            return CreatedAtRoute("GetCategory", new { category.Id }, category);
        }
        catch(BadRequestException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<CategoryDTO>> Update(int id, [FromBody] CategoryCreateDTO categoryCreateDTO)
    {
        try
        {
            await categoryService.Update(id, categoryCreateDTO);
            return NoContent();
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

    [HttpDelete("{id:int}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<ActionResult<CategoryDTO>> Update(int id)
    {
        try
        {
            await categoryService.Delete(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }
}
