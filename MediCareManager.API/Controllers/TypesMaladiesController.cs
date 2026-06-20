using MediCareManager.Core.DTOs;
using MediCareManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCareManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TypesMaladiesController : ControllerBase
{
    private readonly ITypeMaladieService _service;

    public TypesMaladiesController(ITypeMaladieService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _service.GetAllAsync());

    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateTypeMaladieDto dto)
    {
        var id = await _service.CreateAsync(dto);
        return StatusCode(StatusCodes.Status201Created,
            new { id_maladie = id, libelle = dto.Libelle, code_cim = dto.CodeCIM });
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
