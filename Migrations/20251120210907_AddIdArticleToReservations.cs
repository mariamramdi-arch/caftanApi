using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mkBoutiqueCaftan.Migrations
{
    /// <inheritdoc />
    public partial class AddIdArticleToReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ajouter la colonne id_article seulement si elle n'existe pas déjà (MySQL compatible)
            migrationBuilder.Sql(@"
                SET @col_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Reservations' 
                    AND COLUMN_NAME = 'id_article'
                );
                
                SET @sql = IF(@col_exists = 0,
                    'ALTER TABLE Reservations ADD COLUMN id_article INT NULL',
                    'SELECT ''Column already exists'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Créer l'index sur id_article s'il n'existe pas
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Reservations' 
                    AND INDEX_NAME = 'IX_Reservations_id_article'
                );
                
                IF @index_exists = 0 THEN
                    CREATE INDEX `IX_Reservations_id_article` ON `Reservations` (`id_article`);
                END IF;
            ");

            // Ajouter la clé étrangère si elle n'existe pas
            migrationBuilder.Sql(@"
                SET @fk_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Reservations' 
                    AND CONSTRAINT_NAME = 'FK_Reservations_Articles_id_article'
                );
                
                SET @sql = IF(@fk_exists = 0,
                    'ALTER TABLE Reservations ADD CONSTRAINT FK_Reservations_Articles_id_article FOREIGN KEY (id_article) REFERENCES Articles(id_article) ON DELETE RESTRICT',
                    'SELECT ''Foreign key already exists'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Supprimer la clé étrangère
            migrationBuilder.Sql(@"
                SET @fk_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Reservations' 
                    AND CONSTRAINT_NAME = 'FK_Reservations_Articles_id_article'
                );
                
                SET @sql = IF(@fk_exists > 0,
                    'ALTER TABLE Reservations DROP FOREIGN KEY FK_Reservations_Articles_id_article',
                    'SELECT ''Foreign key does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Supprimer l'index
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Reservations' 
                    AND INDEX_NAME = 'IX_Reservations_id_article'
                );
                
                SET @sql = IF(@index_exists > 0,
                    'ALTER TABLE Reservations DROP INDEX IX_Reservations_id_article',
                    'SELECT ''Index does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Supprimer la colonne
            migrationBuilder.Sql(@"
                SET @col_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Reservations' 
                    AND COLUMN_NAME = 'id_article'
                );
                
                SET @sql = IF(@col_exists > 0,
                    'ALTER TABLE Reservations DROP COLUMN id_article',
                    'SELECT ''Column does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");
        }
    }
}
