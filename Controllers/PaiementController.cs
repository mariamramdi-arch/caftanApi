using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mkBoutiqueCaftan.Models;
using mkBoutiqueCaftan.Services;

namespace mkBoutiqueCaftan.Controllers;

[ApiController]
[Route("api/paiements")]
[Authorize]
public class PaiementController : ControllerBase
{
    private readonly IPaiementService _paiementService;
    private readonly ILogger<PaiementController> _logger;

    public PaiementController(IPaiementService paiementService, ILogger<PaiementController> logger)
    {
        _paiementService = paiementService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère tous les paiements
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaiementDto>>> GetAllPaiements([FromQuery] int? idReservation = null)
    {
        try
        {
            var paiements = await _paiementService.GetAllPaiementsAsync(idReservation);
            return Ok(paiements);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération des paiements");
            return StatusCode(500, new { message = "Erreur lors de la récupération des paiements" });
        }
    }

    /// <summary>
    /// Récupère un paiement par son ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PaiementDto>> GetPaiementById(int id)
    {
        try
        {
            var paiement = await _paiementService.GetPaiementByIdAsync(id);
            if (paiement == null)
            {
                return NotFound(new { message = $"Paiement avec l'ID {id} introuvable" });
            }
            return Ok(paiement);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération du paiement {PaiementId}", id);
            return StatusCode(500, new { message = "Erreur lors de la récupération du paiement" });
        }
    }

    /// <summary>
    /// Crée un nouveau paiement
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<PaiementDto>> CreatePaiement([FromBody] CreatePaiementRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (request.Montant <= 0)
        {
            return BadRequest(new { message = "Le montant du paiement doit être supérieur à zéro" });
        }

        if (!string.IsNullOrWhiteSpace(request.MethodePaiement) && request.MethodePaiement.Length > 50)
        {
            return BadRequest(new { message = "La méthode de paiement ne peut pas dépasser 50 caractères" });
        }

        if (!string.IsNullOrWhiteSpace(request.Reference) && request.Reference.Length > 100)
        {
            return BadRequest(new { message = "La référence ne peut pas dépasser 100 caractères" });
        }

        try
        {
            var paiement = await _paiementService.CreatePaiementAsync(request);
            return CreatedAtAction(nameof(GetPaiementById), new { id = paiement.IdPaiement }, paiement);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création du paiement");
            return StatusCode(500, new { message = "Erreur lors de la création du paiement" });
        }
    }

    /// <summary>
    /// Met à jour un paiement existant
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<PaiementDto>> UpdatePaiement(int id, [FromBody] UpdatePaiementRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (request.Montant.HasValue && request.Montant.Value <= 0)
        {
            return BadRequest(new { message = "Le montant du paiement doit être supérieur à zéro" });
        }

        if (!string.IsNullOrWhiteSpace(request.MethodePaiement) && request.MethodePaiement.Length > 50)
        {
            return BadRequest(new { message = "La méthode de paiement ne peut pas dépasser 50 caractères" });
        }

        if (!string.IsNullOrWhiteSpace(request.Reference) && request.Reference.Length > 100)
        {
            return BadRequest(new { message = "La référence ne peut pas dépasser 100 caractères" });
        }

        try
        {
            var paiement = await _paiementService.UpdatePaiementAsync(id, request);
            if (paiement == null)
            {
                return NotFound(new { message = $"Paiement avec l'ID {id} introuvable" });
            }
            return Ok(paiement);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour du paiement {PaiementId}", id);
            return StatusCode(500, new { message = "Erreur lors de la mise à jour du paiement" });
        }
    }

    /// <summary>
    /// Supprime un paiement
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePaiement(int id)
    {
        try
        {
            var deleted = await _paiementService.DeletePaiementAsync(id);
            if (!deleted)
            {
                return NotFound(new { message = $"Paiement avec l'ID {id} introuvable" });
            }
            return Ok(new { message = "Paiement supprimé avec succès" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la suppression du paiement {PaiementId}", id);
            return StatusCode(500, new { message = "Erreur lors de la suppression du paiement" });
        }
    }
}

