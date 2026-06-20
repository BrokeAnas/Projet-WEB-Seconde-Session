using MediCareManager.Core.DTOs;
using MediCareManager.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediCareManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class SecretairesController : ControllerBase
{
    private readonly ISecretaireService _secretaireService;

    public SecretairesController(ISecretaireService secretaireService)
    {
        _secretaireService = secretaireService;
    }

    // GET /api/secretaires  (admin)
    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _secretaireService.GetAllAsync());

    // GET /api/secretaires/{id}  (admin)
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
        => Ok(await _secretaireService.GetByIdAsync(id));

    // POST /api/secretaires  (admin)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSecretaireDto dto)
    {
        var idNat = await _secretaireService.CreateAsync(dto);
        var secretaire = await _secretaireService.GetByIdAsync(idNat);
        return CreatedAtAction(nameof(GetById), new { id = idNat }, secretaire);
    }

    // PUT /api/secretaires/{id}  (admin)
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] UpdateSecretaireDto dto)
    {
        await _secretaireService.UpdateAsync(id, dto);
        return Ok(await _secretaireService.GetByIdAsync(id));
    }

    // DELETE /api/secretaires/{id}  (admin)
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _secretaireService.DeleteAsync(id);
        return NoContent();
    }
}
