using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mkBoutiqueCaftan.Models;
using mkBoutiqueCaftan.Services;

namespace mkBoutiqueCaftan.Controllers;

[ApiController]
[Route("api/tailles")]
[Authorize]
public class TailleController : ControllerBase
{
    private readonly ITailleService _tailleService;
    private readonly ILogger<TailleController> _logger;

    public TailleController(ITailleService tailleService, ILogger<TailleController> logger)
    {
        _tailleService = tailleService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère toutes les tailles
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TailleDto>>> GetAllTailles()
    {
        try
        {
            var tailles = await _tailleService.GetAllTaillesAsync();
            return Ok(tailles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération des tailles");
            return StatusCode(500, new { message = "Erreur lors de la récupération des tailles" });
        }
    }

    /// <summary>
    /// Récupère une taille par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TailleDto>> GetTailleById(int id)
    {
        try
        {
            var taille = await _tailleService.GetTailleByIdAsync(id);
            if (taille == null)
            {
                return NotFound(new { message = $"Taille avec l'ID {id} introuvable" });
            }
            return Ok(taille);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération de la taille {TailleId}", id);
            return StatusCode(500, new { message = "Erreur lors de la récupération de la taille" });
        }
    }

    /// <summary>
    /// Crée une nouvelle taille
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TailleDto>> CreateTaille([FromBody] CreateTailleRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(request.Taille))
        {
            return BadRequest(new { message = "Le libellé de la taille est requis" });
        }

        if (request.Taille.Length > 20)
        {
            return BadRequest(new { message = "Le libellé de la taille ne peut pas dépasser 20 caractères" });
        }

        try
        {
            var taille = await _tailleService.CreateTailleAsync(request);
            return CreatedAtAction(nameof(GetTailleById), new { id = taille.IdTaille }, taille);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création de la taille");
            return StatusCode(500, new { message = "Erreur lors de la création de la taille" });
        }
    }

    /// <summary>
    /// Met à jour une taille existante
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TailleDto>> UpdateTaille(int id, [FromBody] UpdateTailleRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (string.IsNullOrWhiteSpace(request.Taille))
        {
            return BadRequest(new { message = "Le libellé de la taille est requis" });
        }

        if (request.Taille.Length > 20)
        {
            return BadRequest(new { message = "Le libellé de la taille ne peut pas dépasser 20 caractères" });
        }

        try
        {
            var taille = await _tailleService.UpdateTailleAsync(id, request);
            if (taille == null)
            {
                return NotFound(new { message = $"Taille avec l'ID {id} introuvable" });
            }
            return Ok(taille);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour de la taille {TailleId}", id);
            return StatusCode(500, new { message = "Erreur lors de la mise à jour de la taille" });
        }
    }

    /// <summary>
    /// Supprime une taille
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTaille(int id)
    {
        try
        {
            var deleted = await _tailleService.DeleteTailleAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Taille avec l'ID {id} introuvable" });
            }
            return Ok(new { message = "Taille supprimée avec succès" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la suppression de la taille {TailleId}", id);
            return StatusCode(500, new { message = "Erreur lors de la suppression de la taille" });
        }
    }
}

