using MediCareManager.Core.DTOs;
using MediCareManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCareManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RendezVousController : ControllerBase
{
    private readonly IRendezVousService _rendezVousService;

    public RendezVousController(IRendezVousService rendezVousService)
    {
        _rendezVousService = rendezVousService;
    }

    // GET /api/rendezvous?medecinId=&patientId=&sucursaleId=&date=
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] long? medecinId,
        [FromQuery] long? patientId,
        [FromQuery] int? sucursaleId,
        [FromQuery] DateOnly? date)
    {
        // RG-04 : une secrétaire ne voit que la succursale à laquelle elle est rattachée.
        if (User.IsInRole("secretaire") && !User.IsInRole("admin"))
        {
            var sucursale = GetSucursaleClaim();
            if (sucursale.HasValue) sucursaleId = sucursale.Value;
        }

        return Ok(await _rendezVousService.GetAllAsync(medecinId, patientId, sucursaleId, date));
    }

    // GET /api/rendezvous/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _rendezVousService.GetByIdAsync(id));

    // POST /api/rendezvous  (secrétaire, admin) -> 201 ou 409
    [HttpPost]
    [Authorize(Roles = "secretaire,admin")]
    public async Task<IActionResult> Create([FromBody] CreateRendezVousDto dto)
    {
        var id = await _rendezVousService.CreateAsync(dto);
        var rdv = await _rendezVousService.GetByIdAsync(id);
        return CreatedAtAction(nameof(GetById), new { id }, rdv);
    }

    // PUT /api/rendezvous/{id}  (secrétaire, admin)
    [HttpPut("{id:int}")]
    [Authorize(Roles = "secretaire,admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRendezVousDto dto)
    {
        await _rendezVousService.UpdateAsync(id, dto);
        return Ok(await _rendezVousService.GetByIdAsync(id));
    }

    // PATCH /api/rendezvous/{id}/statut  (secrétaire, médecin, admin)
    [HttpPatch("{id:int}/statut")]
    [Authorize(Roles = "secretaire,medecin,admin")]
    public async Task<IActionResult> UpdateStatut(int id, [FromBody] UpdateStatutDto dto)
    {
        // Un médecin (qui n'est ni admin ni secrétaire) ne peut modifier que SES propres rendez-vous.
        if (User.IsInRole("medecin") && !User.IsInRole("admin") && !User.IsInRole("secretaire"))
        {
            var rdv = await _rendezVousService.GetByIdAsync(id);
            if (rdv.IdNatMedecin.ToString() != GetSub())
                return Forbid();
        }

        await _rendezVousService.UpdateStatutAsync(id, dto.Statut);
        return Ok(await _rendezVousService.GetByIdAsync(id));
    }

    // DELETE /api/rendezvous/{id}  (secrétaire, admin)
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "secretaire,admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _rendezVousService.DeleteAsync(id);
        return NoContent();
    }

    private string? GetSub() => User.FindFirst("sub")?.Value;

    private int? GetSucursaleClaim()
        => int.TryParse(User.FindFirst("sucursale")?.Value, out var s) ? s : null;
}
