# Exemples d'utilisation de PasswordHelper

## Exemple 1 : Hashage d'un mot de passe

```csharp
using mkBoutiqueCaftan.Services;

// Hashage d'un mot de passe
string password = "MonMotDePasse123!";
string hashedPassword = PasswordHelper.HashPassword(password);

Console.WriteLine($"Mot de passe original: {password}");
Console.WriteLine($"Mot de passe hashé: {hashedPassword}");
// Résultat: Mot de passe hashé: X7k9mP2qR5sT8vW1xY4zA6bC9dE0fG3hI...
```

## Exemple 2 : Vérification d'un mot de passe

```csharp
// Lors de la connexion, vérifier le mot de passe
string userInput = "MonMotDePasse123!";
string storedHash = "X7k9mP2qR5sT8vW1xY4zA6bC9dE0fG3hI..."; // Hash stocké en base

bool isValid = PasswordHelper.VerifyPassword(userInput, storedHash);
if (isValid)
{
    Console.WriteLine("Mot de passe correct !");
}
else
{
    Console.WriteLine("Mot de passe incorrect !");
}
```

## Exemple 3 : Utilisation dans UserService

```csharp
// Lors de la création d'un utilisateur
var newUser = new User
{
    NomComplet = "John Doe",
    Login = "john.doe",
    MotDePasseHash = PasswordHelper.HashPassword("password123"), // Hash avant stockage
    IdRole = 1,
    Actif = true
};
```

## Exemple 4 : Utilisation dans AuthService

```csharp
// Lors de la connexion
var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);

if (user != null && PasswordHelper.VerifyPassword(password, user.MotDePasseHash))
{
    // Connexion réussie
}
else
{
    // Mot de passe incorrect
}
```

## Résultats attendus

- **Même mot de passe** → **Même hash** (déterministe)
- **Mot de passe différent** → **Hash différent**
- **Vérification correcte** → `true`
- **Vérification incorrecte** → `false`

