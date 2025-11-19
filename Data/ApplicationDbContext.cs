using Microsoft.EntityFrameworkCore;
using mkBoutiqueCaftan.Models;

namespace mkBoutiqueCaftan.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Taille> Tailles { get; set; }
    public DbSet<Categorie> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration de l'entité Role
        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Roles");
            entity.HasKey(e => e.IdRole);
            
            entity.Property(e => e.IdRole)
                .HasColumnName("id_role")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.NomRole)
                .HasColumnName("nom_role")
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(255);
            
            entity.Property(e => e.IdSociete)
                .HasColumnName("id_societe")
                .IsRequired();
            
            entity.Property(e => e.Actif)
                .HasColumnName("actif")
                .IsRequired()
                .HasDefaultValue(true);

            // Index unique sur NomRole et IdSociete (même nom de rôle peut exister pour différentes sociétés)
            entity.HasIndex(e => new { e.NomRole, e.IdSociete })
                .IsUnique()
                .HasDatabaseName("IX_Roles_NomRole_Societe");
        });

        // Configuration de l'entité User
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.IdUtilisateur);
            
            entity.Property(e => e.IdUtilisateur)
                .HasColumnName("id_utilisateur")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.NomComplet)
                .HasColumnName("nom_complet")
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Login)
                .HasColumnName("login")
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.MotDePasseHash)
                .HasColumnName("mot_de_passe_hash")
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(e => e.IdRole)
                .HasColumnName("id_role")
                .IsRequired();
            
            entity.Property(e => e.IdSociete)
                .HasColumnName("id_societe")
                .IsRequired();
            
            entity.Property(e => e.Telephone)
                .HasColumnName("telephone")
                .HasMaxLength(20);
            
            entity.Property(e => e.Actif)
                .HasColumnName("actif")
                .IsRequired()
                .HasDefaultValue(true);
            
            entity.Property(e => e.DateCreationCompte)
                .HasColumnName("date_creation_compte")
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Index unique sur Login et IdSociete (même login peut exister pour différentes sociétés)
            entity.HasIndex(e => new { e.Login, e.IdSociete })
                .IsUnique()
                .HasDatabaseName("IX_Users_Login_Societe");
            
            // Index unique sur Email et IdSociete (même email peut exister pour différentes sociétés)
            entity.HasIndex(e => new { e.Email, e.IdSociete })
                .IsUnique()
                .HasDatabaseName("IX_Users_Email_Societe");

            // Relation avec Role
            entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.IdRole)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuration de l'entité Taille
        modelBuilder.Entity<Taille>(entity =>
        {
            entity.ToTable("Tailles");
            entity.HasKey(e => e.IdTaille);
            
            entity.Property(e => e.IdTaille)
                .HasColumnName("id_taille")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.Libelle)
                .HasColumnName("taille")
                .IsRequired()
                .HasMaxLength(20);
            
            entity.Property(e => e.IdSociete)
                .HasColumnName("id_societe")
                .IsRequired();

            // Index unique sur Libelle et IdSociete (même taille peut exister pour différentes sociétés)
            entity.HasIndex(e => new { e.Libelle, e.IdSociete })
                .IsUnique()
                .HasDatabaseName("IX_Tailles_Taille_Societe");
        });

        // Configuration de l'entité Categorie
        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(e => e.IdCategorie);
            
            entity.Property(e => e.IdCategorie)
                .HasColumnName("id_categorie")
                .ValueGeneratedOnAdd();
            
            entity.Property(e => e.NomCategorie)
                .HasColumnName("nom_categorie")
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasColumnType("TEXT");
            
            entity.Property(e => e.OrdreAffichage)
                .HasColumnName("ordre_affichage");
            
            entity.Property(e => e.IdSociete)
                .HasColumnName("id_societe")
                .IsRequired();

            // Index unique sur NomCategorie et IdSociete (même nom peut exister pour différentes sociétés)
            entity.HasIndex(e => new { e.NomCategorie, e.IdSociete })
                .IsUnique()
                .HasDatabaseName("IX_Categories_NomCategorie_Societe");
        });
    }
}

