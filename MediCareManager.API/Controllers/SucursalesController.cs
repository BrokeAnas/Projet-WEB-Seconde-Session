using MediCareManager.Core.DTOs;
using MediCareManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCareManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SucursalesController : ControllerBase
{
    private readonly ISucursaleService _sucursaleService;

    public SucursalesController(ISucursaleService sucursaleService)
    {
        _sucursaleService = sucursaleService;
    }

    // GET /api/sucursales  (tous rôles)
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _sucursaleService.GetAllAsync());

    // GET /api/sucursales/{id}  (tous rôles)
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _sucursaleService.GetByIdAsync(id));

    // POST /api/sucursales  (admin)
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateSucursaleDto dto)
    {
        var id = await _sucursaleService.CreateAsync(dto);
        var sucursale = await _sucursaleService.GetByIdAsync(id);
        return CreatedAtAction(nameof(GetById), new { id }, sucursale);
    }

    // PUT /api/sucursales/{id}  (admin)
    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSucursaleDto dto)
    {
        await _sucursaleService.UpdateAsync(id, dto);
        return Ok(await _sucursaleService.GetByIdAsync(id));
    }

    // DELETE /api/sucursales/{id}  (admin) -> 409 si personnel affecté
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _sucursaleService.DeleteAsync(id);
        return NoContent();
    }
}
