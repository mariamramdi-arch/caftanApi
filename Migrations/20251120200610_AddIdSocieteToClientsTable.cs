using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mkBoutiqueCaftan.Migrations
{
    /// <inheritdoc />
    public partial class AddIdSocieteToClientsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ajouter la colonne id_societe seulement si elle n'existe pas déjà (MySQL compatible)
            migrationBuilder.Sql(@"
                SET @col_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Clients' 
                    AND COLUMN_NAME = 'id_societe'
                );
                
                SET @sql = IF(@col_exists = 0,
                    'ALTER TABLE Clients ADD COLUMN id_societe INT NOT NULL DEFAULT 0',
                    'SELECT ''Column already exists'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Mettre à jour les clients existants avec id_societe = 0 vers un id_societe valide
            migrationBuilder.Sql(@"
                SET @first_societe_id = (SELECT id_societe FROM Societes LIMIT 1);
                IF @first_societe_id IS NULL THEN
                    INSERT INTO Societes (nom_societe, description, adresse, telephone, email, site_web, logo, actif, date_creation)
                    VALUES ('Default Societe', 'Description par défaut', NULL, NULL, NULL, NULL, NULL, TRUE, NOW());
                    SET @first_societe_id = LAST_INSERT_ID();
                END IF;
                UPDATE Clients SET id_societe = @first_societe_id WHERE id_societe = 0;
            ");

            // Supprimer l'ancien index unique sur telephone s'il existe
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Clients' 
                    AND INDEX_NAME = 'IX_Clients_Telephone'
                );
                
                SET @sql = IF(@index_exists > 0,
                    'ALTER TABLE Clients DROP INDEX IX_Clients_Telephone',
                    'SELECT ''Index does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Supprimer l'ancien index composite s'il existe
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Clients' 
                    AND INDEX_NAME = 'IX_Clients_Telephone_Societe'
                );
                
                SET @sql = IF(@index_exists > 0,
                    'ALTER TABLE Clients DROP INDEX IX_Clients_Telephone_Societe',
                    'SELECT ''Index does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Créer le nouvel index composite unique sur (telephone, id_societe)
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Clients' 
                    AND INDEX_NAME = 'IX_Clients_Telephone_Societe'
                );
                
                IF @index_exists = 0 THEN
                    CREATE UNIQUE INDEX `IX_Clients_Telephone_Societe` ON `Clients` (`telephone`, `id_societe`);
                END IF;
            ");

            // Créer l'index sur id_societe s'il n'existe pas
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Clients' 
                    AND INDEX_NAME = 'IX_Clients_id_societe'
                );
                
                IF @index_exists = 0 THEN
                    CREATE INDEX `IX_Clients_id_societe` ON `Clients` (`id_societe`);
                END IF;
            ");

            // Ajouter la clé étrangère si elle n'existe pas
            migrationBuilder.Sql(@"
                SET @fk_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Clients' 
                    AND CONSTRAINT_NAME = 'FK_Clients_Societes_id_societe'
                );
                
                SET @sql = IF(@fk_exists = 0,
                    'ALTER TABLE Clients ADD CONSTRAINT FK_Clients_Societes_id_societe FOREIGN KEY (id_societe) REFERENCES Societes(id_societe) ON DELETE RESTRICT',
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
                    AND TABLE_NAME = 'Clients' 
                    AND CONSTRAINT_NAME = 'FK_Clients_Societes_id_societe'
                );
                
                SET @sql = IF(@fk_exists > 0,
                    'ALTER TABLE Clients DROP FOREIGN KEY FK_Clients_Societes_id_societe',
                    'SELECT ''Foreign key does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Supprimer les index
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Clients' 
                    AND INDEX_NAME = 'IX_Clients_id_societe'
                );
                
                SET @sql = IF(@index_exists > 0,
                    'ALTER TABLE Clients DROP INDEX IX_Clients_id_societe',
                    'SELECT ''Index does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Clients' 
                    AND INDEX_NAME = 'IX_Clients_Telephone_Societe'
                );
                
                SET @sql = IF(@index_exists > 0,
                    'ALTER TABLE Clients DROP INDEX IX_Clients_Telephone_Societe',
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
                    AND TABLE_NAME = 'Clients' 
                    AND COLUMN_NAME = 'id_societe'
                );
                
                SET @sql = IF(@col_exists > 0,
                    'ALTER TABLE Clients DROP COLUMN id_societe',
                    'SELECT ''Column does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Recréer l'ancien index unique sur telephone
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Clients' 
                    AND INDEX_NAME = 'IX_Clients_Telephone'
                );
                
                IF @index_exists = 0 THEN
                    CREATE UNIQUE INDEX `IX_Clients_Telephone` ON `Clients` (`telephone`);
                END IF;
            ");
        }
    }
}
