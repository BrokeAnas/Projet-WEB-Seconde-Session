using MediCareManager.Core.DTOs;
using MediCareManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCareManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaiementsController : ControllerBase
{
    private readonly IPaiementService _paiementService;

    public PaiementsController(IPaiementService paiementService)
    {
        _paiementService = paiementService;
    }

    // GET /api/paiements?patientId=&dateDebut=&dateFin=  (secrétaire, admin)
    [HttpGet]
    [Authorize(Roles = "secretaire,admin")]
    public async Task<IActionResult> GetAll(
        [FromQuery] long? patientId, [FromQuery] DateOnly? dateDebut, [FromQuery] DateOnly? dateFin)
        => Ok(await _paiementService.GetAllAsync(patientId, dateDebut, dateFin));

    // GET /api/paiements/audit  (admin)  — placé avant {id} pour éviter la collision de route.
    [HttpGet("audit")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetAuditLog()
        => Ok(await _paiementService.GetAuditLogAsync());

    // GET /api/paiements/{id}  (secrétaire, admin)
    [HttpGet("{id:int}")]
    [Authorize(Roles = "secretaire,admin")]
    public async Task<IActionResult> GetById(int id)
        => Ok(await _paiementService.GetByIdAsync(id));

    // POST /api/paiements  (secrétaire, admin) -> 201
    [HttpPost]
    [Authorize(Roles = "secretaire,admin")]
    public async Task<IActionResult> Create([FromBody] CreatePaiementDto dto)
    {
        var id = await _paiementService.CreateAsync(dto);
        var paiement = await _paiementService.GetByIdAsync(id);
        return CreatedAtAction(nameof(GetById), new { id }, paiement);
    }

    // PUT /api/paiements/{id}  (secrétaire, admin) — déclenche le trigger UPDATE
    [HttpPut("{id:int}")]
    [Authorize(Roles = "secretaire,admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePaiementDto dto)
    {
        await _paiementService.UpdateAsync(id, dto);
        return Ok(await _paiementService.GetByIdAsync(id));
    }

    // DELETE /api/paiements/{id}  (admin) — déclenche le trigger DELETE
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _paiementService.DeleteAsync(id);
        return NoContent();
    }
}
