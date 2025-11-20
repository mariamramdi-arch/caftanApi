using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mkBoutiqueCaftan.Migrations
{
    /// <inheritdoc />
    public partial class CreateReservationArticlesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Créer la table ReservationArticles seulement si elle n'existe pas déjà (MySQL compatible)
            migrationBuilder.Sql(@"
                SET @table_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'ReservationArticles'
                );
                
                IF @table_exists = 0 THEN
                    CREATE TABLE `ReservationArticles` (
                        `id_reservation` INT NOT NULL,
                        `id_article` INT NOT NULL,
                        `quantite` INT NOT NULL DEFAULT 1,
                        PRIMARY KEY (`id_reservation`, `id_article`),
                        CONSTRAINT `FK_ReservationArticles_Reservations_id_reservation` 
                            FOREIGN KEY (`id_reservation`) REFERENCES `Reservations` (`id_reservation`) ON DELETE CASCADE,
                        CONSTRAINT `FK_ReservationArticles_Articles_id_article` 
                            FOREIGN KEY (`id_article`) REFERENCES `Articles` (`id_article`) ON DELETE RESTRICT
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
                END IF;
            ");

            // Créer l'index sur id_article s'il n'existe pas
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'ReservationArticles' 
                    AND INDEX_NAME = 'IX_ReservationArticles_id_article'
                );
                
                IF @index_exists = 0 THEN
                    CREATE INDEX `IX_ReservationArticles_id_article` ON `ReservationArticles` (`id_article`);
                END IF;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Supprimer l'index
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'ReservationArticles' 
                    AND INDEX_NAME = 'IX_ReservationArticles_id_article'
                );
                
                SET @sql = IF(@index_exists > 0,
                    'ALTER TABLE ReservationArticles DROP INDEX IX_ReservationArticles_id_article',
                    'SELECT ''Index does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Supprimer la table
            migrationBuilder.Sql(@"
                SET @table_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'ReservationArticles'
                );
                
                IF @table_exists > 0 THEN
                    DROP TABLE `ReservationArticles`;
                END IF;
            ");
        }
    }
}
