using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mkBoutiqueCaftan.Migrations
{
    /// <inheritdoc />
    public partial class CreateClientsTableComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Vérifier si la table Clients existe
            migrationBuilder.Sql(@"
                SET @table_exists := (SELECT COUNT(*) FROM information_schema.tables 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients');
                
                -- Si la table n'existe pas, la créer
                SET @sqlstmt := IF(@table_exists = 0,
                    'CREATE TABLE `Clients` (
                        `id_client` int NOT NULL AUTO_INCREMENT,
                        `nom_client` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
                        `prenom_client` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
                        `telephone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
                        `email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
                        `adresse_principale` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL,
                        `total_commandes` int NOT NULL DEFAULT 0,
                        `date_creation_fiche` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        `id_societe` int NOT NULL,
                        `actif` tinyint(1) NOT NULL DEFAULT TRUE,
                        PRIMARY KEY (`id_client`),
                        UNIQUE INDEX `IX_Clients_Telephone_Societe` (`telephone`, `id_societe`)
                    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;',
                    'SELECT ''Table already exists'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Ajouter les colonnes manquantes si la table existe déjà
            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND column_name = 'nom_client');
                SET @sqlstmt := IF(@exist = 0, 
                    'ALTER TABLE `Clients` ADD COLUMN `nom_client` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL;', 
                    'SELECT ''Column already exists'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND column_name = 'prenom_client');
                SET @sqlstmt := IF(@exist = 0, 
                    'ALTER TABLE `Clients` ADD COLUMN `prenom_client` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL;', 
                    'SELECT ''Column already exists'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND column_name = 'telephone');
                SET @sqlstmt := IF(@exist = 0, 
                    'ALTER TABLE `Clients` ADD COLUMN `telephone` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL;', 
                    'SELECT ''Column already exists'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND column_name = 'email');
                SET @sqlstmt := IF(@exist = 0, 
                    'ALTER TABLE `Clients` ADD COLUMN `email` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL;', 
                    'SELECT ''Column already exists'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND column_name = 'adresse_principale');
                SET @sqlstmt := IF(@exist = 0, 
                    'ALTER TABLE `Clients` ADD COLUMN `adresse_principale` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL;', 
                    'SELECT ''Column already exists'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND column_name = 'total_commandes');
                SET @sqlstmt := IF(@exist = 0, 
                    'ALTER TABLE `Clients` ADD COLUMN `total_commandes` int NOT NULL DEFAULT 0;', 
                    'SELECT ''Column already exists'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND column_name = 'date_creation_fiche');
                SET @sqlstmt := IF(@exist = 0, 
                    'ALTER TABLE `Clients` ADD COLUMN `date_creation_fiche` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP;', 
                    'SELECT ''Column already exists'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND column_name = 'id_societe');
                SET @sqlstmt := IF(@exist = 0, 
                    'ALTER TABLE `Clients` ADD COLUMN `id_societe` int NOT NULL;', 
                    'SELECT ''Column already exists'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND column_name = 'actif');
                SET @sqlstmt := IF(@exist = 0, 
                    'ALTER TABLE `Clients` ADD COLUMN `actif` tinyint(1) NOT NULL DEFAULT TRUE;', 
                    'SELECT ''Column already exists'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Créer l'index unique sur telephone et id_societe s'il n'existe pas
            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.statistics 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Clients' 
                    AND index_name = 'IX_Clients_Telephone_Societe');
                SET @sqlstmt := IF(@exist = 0, 
                    'CREATE UNIQUE INDEX `IX_Clients_Telephone_Societe` ON `Clients` (`telephone`, `id_societe`);', 
                    'SELECT ''Index already exists'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS `Clients`;");
        }
    }
}
