using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mkBoutiqueCaftan.Migrations
{
    /// <inheritdoc />
    public partial class AddIdSocieteToUsersTable : Migration
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
                    AND TABLE_NAME = 'Users' 
                    AND COLUMN_NAME = 'id_societe'
                );
                
                SET @sql = IF(@col_exists = 0,
                    'ALTER TABLE Users ADD COLUMN id_societe INT NOT NULL DEFAULT 0',
                    'SELECT ''Column already exists'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Supprimer les anciens index s'ils existent
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Users' 
                    AND INDEX_NAME = 'IX_Users_Email'
                );
                
                SET @sql = IF(@index_exists > 0,
                    'ALTER TABLE Users DROP INDEX IX_Users_Email',
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
                    AND TABLE_NAME = 'Users' 
                    AND INDEX_NAME = 'IX_Users_Login'
                );
                
                SET @sql = IF(@index_exists > 0,
                    'ALTER TABLE Users DROP INDEX IX_Users_Login',
                    'SELECT ''Index does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Supprimer les anciens index composites s'ils existent
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Users' 
                    AND INDEX_NAME = 'IX_Users_Email_Societe'
                );
                
                SET @sql = IF(@index_exists > 0,
                    'ALTER TABLE Users DROP INDEX IX_Users_Email_Societe',
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
                    AND TABLE_NAME = 'Users' 
                    AND INDEX_NAME = 'IX_Users_Login_Societe'
                );
                
                SET @sql = IF(@index_exists > 0,
                    'ALTER TABLE Users DROP INDEX IX_Users_Login_Societe',
                    'SELECT ''Index does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Créer les nouveaux index composites seulement s'ils n'existent pas
            migrationBuilder.Sql(@"
                SET @index_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.STATISTICS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Users' 
                    AND INDEX_NAME = 'IX_Users_Email_Societe'
                );
                
                SET @sql = IF(@index_exists = 0,
                    'CREATE UNIQUE INDEX IX_Users_Email_Societe ON Users (email, id_societe)',
                    'SELECT ''Index already exists'' AS message'
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
                    AND TABLE_NAME = 'Users' 
                    AND INDEX_NAME = 'IX_Users_id_societe'
                );
                
                SET @sql = IF(@index_exists = 0,
                    'CREATE INDEX IX_Users_id_societe ON Users (id_societe)',
                    'SELECT ''Index already exists'' AS message'
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
                    AND TABLE_NAME = 'Users' 
                    AND INDEX_NAME = 'IX_Users_Login_Societe'
                );
                
                SET @sql = IF(@index_exists = 0,
                    'CREATE UNIQUE INDEX IX_Users_Login_Societe ON Users (login, id_societe)',
                    'SELECT ''Index already exists'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Mettre à jour les valeurs id_societe invalides (0 ou NULL) avec la première société existante
            migrationBuilder.Sql(@"
                SET @first_societe_id = (
                    SELECT id_societe FROM Societes ORDER BY id_societe LIMIT 1
                );
                
                -- Si aucune société n'existe, créer une société par défaut
                IF @first_societe_id IS NULL THEN
                    INSERT INTO Societes (nom_societe, actif, date_creation) 
                    VALUES ('Société par défaut', 1, CURRENT_TIMESTAMP);
                    SET @first_societe_id = LAST_INSERT_ID();
                END IF;
                
                -- Mettre à jour tous les utilisateurs avec id_societe = 0 ou NULL
                UPDATE Users 
                SET id_societe = @first_societe_id 
                WHERE id_societe = 0 OR id_societe IS NULL;
            ");

            // Ajouter la clé étrangère seulement si elle n'existe pas
            migrationBuilder.Sql(@"
                SET @fk_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Users' 
                    AND CONSTRAINT_NAME = 'FK_Users_Societes_id_societe'
                );
                
                SET @sql = IF(@fk_exists = 0,
                    'ALTER TABLE Users ADD CONSTRAINT FK_Users_Societes_id_societe FOREIGN KEY (id_societe) REFERENCES Societes(id_societe) ON DELETE RESTRICT',
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
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Societes_id_societe",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Email_Societe",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_id_societe",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Login_Societe",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "id_societe",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "login",
                unique: true);
        }
    }
}
