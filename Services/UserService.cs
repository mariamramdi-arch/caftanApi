using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using mkBoutiqueCaftan.Data;
using mkBoutiqueCaftan.Models;

namespace mkBoutiqueCaftan.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> CreateUserAsync(CreateUserRequest request);
    Task<UserDto?> UpdateUserAsync(int id, UpdateUserRequest request);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> ChangePasswordAsync(int id, string newPassword);
    Task<bool> ToggleUserStatusAsync(int id);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;
    private readonly IUserContextService _userContextService;

    public UserService(ApplicationDbContext context, ILogger<UserService> logger, IUserContextService userContextService)
    {
        _context = context;
        _logger = logger;
        _userContextService = userContextService;
    }

    private UserDto MapToDto(User user, Role? role = null)
    {
        return new UserDto
        {
            IdUtilisateur = user.IdUtilisateur,
            NomComplet = user.NomComplet,
            Login = user.Login,
            Email = user.Email,
            IdRole = user.IdRole,
            IdSociete = user.IdSociete,
            Telephone = user.Telephone,
            Actif = user.Actif,
            DateCreationCompte = user.DateCreationCompte,
            Role = role != null ? new RoleDto
            {
                IdRole = role.IdRole,
                NomRole = role.NomRole,
                Description = role.Description,
                Actif = role.Actif
            } : null
        };
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        try
        {
            var idSociete = _userContextService.GetIdSociete();
            if (!idSociete.HasValue)
            {
                _logger.LogWarning("Tentative de récupération des utilisateurs sans IdSociete dans le token");
                return new List<UserDto>();
            }

            var users = await _context.Users
                .Where(u => u.Actif && u.IdSociete == idSociete.Value)
                .OrderBy(u => u.NomComplet)
                .ToListAsync();

            var userIds = users.Select(u => u.IdRole).Distinct().ToList();
            var roles = await _context.Roles
                .Where(r => userIds.Contains(r.IdRole))
                .ToDictionaryAsync(r => r.IdRole);

            return users.Select(user =>
            {
                roles.TryGetValue(user.IdRole, out var role);
                return MapToDto(user, role);
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération de tous les utilisateurs");
            throw;
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        try
        {
            var idSociete = _userContextService.GetIdSociete();
            if (!idSociete.HasValue)
            {
                _logger.LogWarning("Tentative de récupération d'utilisateur sans IdSociete dans le token");
                return null;
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            // Vérifier que l'utilisateur appartient à la même société
            if (user.IdSociete != idSociete.Value)
            {
                _logger.LogWarning("Tentative d'accès à un utilisateur d'une autre société. UserId: {UserId}, UserSociete: {UserSociete}, CurrentSociete: {CurrentSociete}", 
                    id, user.IdSociete, idSociete.Value);
                return null;
            }

            Role? role = null;
            try
            {
                role = await _context.Roles.FindAsync(user.IdRole);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Impossible de charger le rôle pour l'utilisateur {UserId}", user.IdUtilisateur);
            }

            return MapToDto(user, role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la récupération de l'utilisateur {UserId}", id);
            throw;
        }
    }

    public async Task<UserDto?> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            var idSociete = _userContextService.GetIdSociete();
            if (!idSociete.HasValue)
            {
                _logger.LogWarning("Tentative de création d'utilisateur sans IdSociete dans le token");
                return null;
            }

            // Forcer l'IdSociete à celui de la société connectée
            request.IdSociete = idSociete.Value;

            // Vérifier si le login existe déjà pour cette société
            var loginExists = await _context.Users
                .AnyAsync(u => u.Login.ToLower() == request.Login.ToLower() && u.IdSociete == request.IdSociete);

            if (loginExists)
            {
                _logger.LogWarning("Tentative de création d'utilisateur avec un login déjà existant: {Login}", request.Login);
                return null;
            }

            // Vérifier si l'email existe déjà pour cette société
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email.ToLower() == request.Email.ToLower() && u.IdSociete == request.IdSociete);

                if (emailExists)
                {
                    _logger.LogWarning("Tentative de création d'utilisateur avec un email déjà existant: {Email}", request.Email);
                    return null;
                }
            }

            // Vérifier si le rôle existe
            var roleExists = await _context.Roles
                .AnyAsync(r => r.IdRole == request.IdRole && r.Actif);

            if (!roleExists)
            {
                _logger.LogWarning("Tentative de création d'utilisateur avec un rôle inexistant ou inactif: {RoleId}", request.IdRole);
                return null;
            }

            // Vérifier si la société existe
            var societeExists = await _context.Societes
                .AnyAsync(s => s.IdSociete == request.IdSociete && s.Actif);

            if (!societeExists)
            {
                _logger.LogWarning("Tentative de création d'utilisateur avec une société inexistante ou inactive: {SocieteId}", request.IdSociete);
                return null;
            }

            var newUser = new User
            {
                NomComplet = request.NomComplet,
                Login = request.Login,
                Email = request.Email,
                MotDePasseHash = PasswordHelper.HashPassword(request.Password),
                IdRole = request.IdRole,
                IdSociete = request.IdSociete,
                Telephone = request.Telephone,
                Actif = request.Actif,
                DateCreationCompte = DateTime.Now
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Utilisateur créé avec succès: {Login} (ID: {UserId})", request.Login, newUser.IdUtilisateur);

            var role = await _context.Roles.FindAsync(newUser.IdRole);
            return MapToDto(newUser, role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la création de l'utilisateur");
            throw;
        }
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserRequest request)
    {
        try
        {
            var idSociete = _userContextService.GetIdSociete();
            if (!idSociete.HasValue)
            {
                _logger.LogWarning("Tentative de mise à jour d'utilisateur sans IdSociete dans le token");
                return null;
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            // Vérifier que l'utilisateur appartient à la même société
            if (user.IdSociete != idSociete.Value)
            {
                _logger.LogWarning("Tentative de mise à jour d'un utilisateur d'une autre société. UserId: {UserId}, UserSociete: {UserSociete}, CurrentSociete: {CurrentSociete}", 
                    id, user.IdSociete, idSociete.Value);
                return null;
            }

            // Forcer l'IdSociete à celui de la société connectée (ne pas permettre de changer de société)
            request.IdSociete = idSociete.Value;

            // Vérifier si le login existe déjà (sauf pour l'utilisateur actuel)
            if (!string.IsNullOrWhiteSpace(request.Login) && request.Login.ToLower() != user.Login.ToLower())
            {
                var loginExists = await _context.Users
                    .AnyAsync(u => u.Login.ToLower() == request.Login.ToLower() && u.IdUtilisateur != id && u.IdSociete == idSociete.Value);

                if (loginExists)
                {
                    _logger.LogWarning("Tentative de mise à jour avec un login déjà existant: {Login}", request.Login);
                    return null;
                }
            }

            // Vérifier si l'email existe déjà (sauf pour l'utilisateur actuel)
            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email.ToLower() != user.Email.ToLower())
            {
                var emailExists = await _context.Users
                    .AnyAsync(u => u.Email.ToLower() == request.Email.ToLower() && u.IdUtilisateur != id && u.IdSociete == idSociete.Value);

                if (emailExists)
                {
                    _logger.LogWarning("Tentative de mise à jour avec un email déjà existant: {Email}", request.Email);
                    return null;
                }
            }

            // Vérifier si le rôle existe (si changé)
            if (request.IdRole.HasValue && request.IdRole.Value != user.IdRole)
            {
                var roleExists = await _context.Roles
                    .AnyAsync(r => r.IdRole == request.IdRole.Value && r.Actif);

                if (!roleExists)
                {
                    _logger.LogWarning("Tentative de mise à jour avec un rôle inexistant ou inactif: {RoleId}", request.IdRole.Value);
                    return null;
                }
            }

            // Mettre à jour les propriétés
            if (!string.IsNullOrWhiteSpace(request.NomComplet))
                user.NomComplet = request.NomComplet;

            if (!string.IsNullOrWhiteSpace(request.Login))
                user.Login = request.Login;

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.Email = request.Email;

            if (request.IdRole.HasValue)
                user.IdRole = request.IdRole.Value;

            // L'IdSociete est déjà forcé à celui de la société connectée, on ne le change pas
            // user.IdSociete reste inchangé (déjà vérifié plus haut)

            if (request.Telephone != null)
                user.Telephone = request.Telephone;

            if (request.Actif.HasValue)
                user.Actif = request.Actif.Value;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Utilisateur mis à jour avec succès: {UserId}", id);

            var role = await _context.Roles.FindAsync(user.IdRole);
            return MapToDto(user, role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la mise à jour de l'utilisateur {UserId}", id);
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            var idSociete = _userContextService.GetIdSociete();
            if (!idSociete.HasValue)
            {
                _logger.LogWarning("Tentative de suppression d'utilisateur sans IdSociete dans le token");
                return false;
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            // Vérifier que l'utilisateur appartient à la même société
            if (user.IdSociete != idSociete.Value)
            {
                _logger.LogWarning("Tentative de suppression d'un utilisateur d'une autre société. UserId: {UserId}, UserSociete: {UserSociete}, CurrentSociete: {CurrentSociete}", 
                    id, user.IdSociete, idSociete.Value);
                return false;
            }

            // Soft delete : désactiver l'utilisateur au lieu de le supprimer
            user.Actif = false;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Utilisateur désactivé avec succès: {UserId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la suppression de l'utilisateur {UserId}", id);
            throw;
        }
    }

    public async Task<bool> ChangePasswordAsync(int id, string newPassword)
    {
        try
        {
            var idSociete = _userContextService.GetIdSociete();
            if (!idSociete.HasValue)
            {
                _logger.LogWarning("Tentative de changement de mot de passe sans IdSociete dans le token");
                return false;
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            // Vérifier que l'utilisateur appartient à la même société
            if (user.IdSociete != idSociete.Value)
            {
                _logger.LogWarning("Tentative de changement de mot de passe pour un utilisateur d'une autre société. UserId: {UserId}, UserSociete: {UserSociete}, CurrentSociete: {CurrentSociete}", 
                    id, user.IdSociete, idSociete.Value);
                return false;
            }

            user.MotDePasseHash = PasswordHelper.HashPassword(newPassword);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Mot de passe changé avec succès pour l'utilisateur: {UserId}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du changement de mot de passe pour l'utilisateur {UserId}", id);
            throw;
        }
    }

    public async Task<bool> ToggleUserStatusAsync(int id)
    {
        try
        {
            var idSociete = _userContextService.GetIdSociete();
            if (!idSociete.HasValue)
            {
                _logger.LogWarning("Tentative de changement de statut d'utilisateur sans IdSociete dans le token");
                return false;
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            // Vérifier que l'utilisateur appartient à la même société
            if (user.IdSociete != idSociete.Value)
            {
                _logger.LogWarning("Tentative de changement de statut pour un utilisateur d'une autre société. UserId: {UserId}, UserSociete: {UserSociete}, CurrentSociete: {CurrentSociete}", 
                    id, user.IdSociete, idSociete.Value);
                return false;
            }

            user.Actif = !user.Actif;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Statut de l'utilisateur {UserId} changé à: {Actif}", id, user.Actif);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du changement de statut pour l'utilisateur {UserId}", id);
            throw;
        }
    }
}

