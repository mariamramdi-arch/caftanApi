using System.Security.Cryptography;
using System.Text;

namespace mkBoutiqueCaftan.Services;

public static class PasswordHelper
{
    /// <summary>
    /// Hash un mot de passe en utilisant SHA256
    /// TODO: Utiliser BCrypt.Net-Next pour un hashage plus sécurisé en production
    /// </summary>
    /// <param name="password">Le mot de passe en clair à hasher</param>
    /// <returns>Le hash du mot de passe en Base64</returns>
    /// <example>
    /// <code>
    /// string password = "MonMotDePasse123!";
    /// string hashed = PasswordHelper.HashPassword(password);
    /// // Résultat: "X7k9mP2qR5sT8vW1xY4zA6bC9dE0fG3hI..."
    /// </code>
    /// </example>
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    /// <summary>
    /// Vérifie si un mot de passe correspond au hash stocké
    /// </summary>
    /// <param name="password">Le mot de passe en clair à vérifier</param>
    /// <param name="hashedPassword">Le hash stocké en base de données</param>
    /// <returns>True si le mot de passe correspond, False sinon</returns>
    /// <example>
    /// <code>
    /// string userInput = "MonMotDePasse123!";
    /// string storedHash = "X7k9mP2qR5sT8vW1xY4zA6bC9dE0fG3hI...";
    /// bool isValid = PasswordHelper.VerifyPassword(userInput, storedHash);
    /// if (isValid) { /* Connexion réussie */ }
    /// </code>
    /// </example>
    public static bool VerifyPassword(string password, string hashedPassword)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == hashedPassword;
    }
}

