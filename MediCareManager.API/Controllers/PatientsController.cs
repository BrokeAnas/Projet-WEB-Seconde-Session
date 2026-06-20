using MediCareManager.Core.DTOs;
using MediCareManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCareManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    // GET /api/patients?search=&page=&pageSize=  (tous rôles)
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        => Ok(await _patientService.GetAllAsync(search, page, pageSize));

    // GET /api/patients/{id}  (tous rôles)
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
        => Ok(await _patientService.GetByIdAsync(id));

    // POST /api/patients  (secrétaire, admin)
    [HttpPost]
    [Authorize(Roles = "secretaire,admin")]
    public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
    {
        var idNat = await _patientService.CreateAsync(dto);
        var patient = await _patientService.GetByIdAsync(idNat);
        return CreatedAtAction(nameof(GetById), new { id = idNat }, patient);
    }

    // PUT /api/patients/{id}  (secrétaire, admin)
    [HttpPut("{id:long}")]
    [Authorize(Roles = "secretaire,admin")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdatePatientDto dto)
    {
        await _patientService.UpdateAsync(id, dto);
        return Ok(await _patientService.GetByIdAsync(id));
    }

    // DELETE /api/patients/{id}  (admin)
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Delete(long id)
    {
        await _patientService.DeleteAsync(id);
        return NoContent();
    }

    // ----- Dossier médical (secret médical : médecin/admin uniquement) -----

    // GET /api/patients/{id}/maladies  (médecin, admin)
    [HttpGet("{id:long}/maladies")]
    [Authorize(Roles = "medecin,admin")]
    public async Task<IActionResult> GetMaladies(long id)
        => Ok(await _patientService.GetMaladiesAsync(id));

    // POST /api/patients/{id}/maladies  (médecin, admin)
    [HttpPost("{id:long}/maladies")]
    [Authorize(Roles = "medecin,admin")]
    public async Task<IActionResult> AddMaladie(long id, [FromBody] AddMaladieDto dto)
    {
        await _patientService.AddMaladieAsync(id, dto);
        return StatusCode(StatusCodes.Status201Created, await _patientService.GetMaladiesAsync(id));
    }

    // ----- Assurances -----

    // GET /api/patients/{id}/assurances  (tous rôles)
    [HttpGet("{id:long}/assurances")]
    public async Task<IActionResult> GetAssurances(long id)
        => Ok(await _patientService.GetAssurancesAsync(id));

    // POST /api/patients/{id}/assurances  (secrétaire, admin)
    [HttpPost("{id:long}/assurances")]
    [Authorize(Roles = "secretaire,admin")]
    public async Task<IActionResult> AddAssurance(long id, [FromBody] AddAssuranceDto dto)
    {
        await _patientService.AddAssuranceAsync(id, dto);
        return StatusCode(StatusCodes.Status201Created, await _patientService.GetAssurancesAsync(id));
    }

    // DELETE /api/patients/{id}/assurances/{idAssurance}  (secrétaire, admin)
    [HttpDelete("{id:long}/assurances/{idAssurance:int}")]
    [Authorize(Roles = "secretaire,admin")]
    public async Task<IActionResult> RemoveAssurance(long id, int idAssurance)
    {
        await _patientService.RemoveAssuranceAsync(id, idAssurance);
        return NoContent();
    }
}
