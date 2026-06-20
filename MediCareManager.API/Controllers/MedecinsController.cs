using MediCareManager.Core.DTOs;
using MediCareManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCareManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MedecinsController : ControllerBase
{
    private readonly IMedecinService _medecinService;

    public MedecinsController(IMedecinService medecinService)
    {
        _medecinService = medecinService;
    }

    // GET /api/medecins  (tous rôles)
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
        => Ok(await _medecinService.GetAllAsync(search));

    // GET /api/medecins/{id}  (tous rôles)
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
        => Ok(await _medecinService.GetByIdAsync(id));

    // POST /api/medecins  (admin)
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create([FromBody] CreateMedecinDto dto)
    {
        var idNat = await _medecinService.CreateAsync(dto);
        var medecin = await _medecinService.GetByIdAsync(idNat);
        return CreatedAtAction(nameof(GetById), new { id = idNat }, medecin);
    }

    // PUT /api/medecins/{id}  (admin)
    [HttpPut("{id:long}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateMedecinDto dto)
    {
        await _medecinService.UpdateAsync(id, dto);
        return Ok(await _medecinService.GetByIdAsync(id));
    }

    // DELETE /api/medecins/{id}  (admin) -> 409 si rendez-vous futurs
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(long id)
    {
        await _medecinService.DeleteAsync(id);
        return NoContent();
    }
}
