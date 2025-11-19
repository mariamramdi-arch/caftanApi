using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mkBoutiqueCaftan.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSocieteIdSocieteFromSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Supprimer l'index sur SocieteIdSociete pour Clients (s'il existe)
            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.statistics 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND index_name = 'IX_Clients_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Clients` DROP INDEX `IX_Clients_SocieteIdSociete`;', 'SELECT ''Index does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Supprimer la clé étrangère (s'il existe)
            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.table_constraints 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND constraint_name = 'FK_Clients_Societes_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Clients` DROP FOREIGN KEY `FK_Clients_Societes_SocieteIdSociete`;', 'SELECT ''FK does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Supprimer la colonne SocieteIdSociete (s'il existe)
            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND column_name = 'SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Clients` DROP COLUMN `SocieteIdSociete`;', 'SELECT ''Column does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Cette migration est un nettoyage, pas de rollback nécessaire
        }
    }
}
