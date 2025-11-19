using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mkBoutiqueCaftan.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIncorrectSocieteIdSocieteColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Supprimer les index sur SocieteIdSociete (si ils existent)
            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.statistics 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Users' 
                    AND index_name = 'IX_Users_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Users` DROP INDEX `IX_Users_SocieteIdSociete`;', 'SELECT ''Index does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.statistics 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Tailles' 
                    AND index_name = 'IX_Tailles_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Tailles` DROP INDEX `IX_Tailles_SocieteIdSociete`;', 'SELECT ''Index does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.statistics 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Roles' 
                    AND index_name = 'IX_Roles_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Roles` DROP INDEX `IX_Roles_SocieteIdSociete`;', 'SELECT ''Index does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.statistics 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Categories' 
                    AND index_name = 'IX_Categories_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Categories` DROP INDEX `IX_Categories_SocieteIdSociete`;', 'SELECT ''Index does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.statistics 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Articles' 
                    AND index_name = 'IX_Articles_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Articles` DROP INDEX `IX_Articles_SocieteIdSociete`;', 'SELECT ''Index does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

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

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.statistics 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Reservations' 
                    AND index_name = 'IX_Reservations_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Reservations` DROP INDEX `IX_Reservations_SocieteIdSociete`;', 'SELECT ''Index does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Supprimer les clés étrangères (si elles existent)
            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.table_constraints 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Users' 
                    AND constraint_name = 'FK_Users_Societes_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Users` DROP FOREIGN KEY `FK_Users_Societes_SocieteIdSociete`;', 'SELECT ''FK does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.table_constraints 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Tailles' 
                    AND constraint_name = 'FK_Tailles_Societes_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Tailles` DROP FOREIGN KEY `FK_Tailles_Societes_SocieteIdSociete`;', 'SELECT ''FK does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.table_constraints 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Roles' 
                    AND constraint_name = 'FK_Roles_Societes_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Roles` DROP FOREIGN KEY `FK_Roles_Societes_SocieteIdSociete`;', 'SELECT ''FK does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.table_constraints 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Categories' 
                    AND constraint_name = 'FK_Categories_Societes_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Categories` DROP FOREIGN KEY `FK_Categories_Societes_SocieteIdSociete`;', 'SELECT ''FK does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.table_constraints 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Articles' 
                    AND constraint_name = 'FK_Articles_Societes_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Articles` DROP FOREIGN KEY `FK_Articles_Societes_SocieteIdSociete`;', 'SELECT ''FK does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

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

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.table_constraints 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Reservations' 
                    AND constraint_name = 'FK_Reservations_Societes_SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Reservations` DROP FOREIGN KEY `FK_Reservations_Societes_SocieteIdSociete`;', 'SELECT ''FK does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            // Supprimer les colonnes SocieteIdSociete (si elles existent)
            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Users' 
                    AND column_name = 'SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Users` DROP COLUMN `SocieteIdSociete`;', 'SELECT ''Column does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Tailles' 
                    AND column_name = 'SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Tailles` DROP COLUMN `SocieteIdSociete`;', 'SELECT ''Column does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Roles' 
                    AND column_name = 'SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Roles` DROP COLUMN `SocieteIdSociete`;', 'SELECT ''Column does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Categories' 
                    AND column_name = 'SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Categories` DROP COLUMN `SocieteIdSociete`;', 'SELECT ''Column does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Articles' 
                    AND column_name = 'SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Articles` DROP COLUMN `SocieteIdSociete`;', 'SELECT ''Column does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

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

            migrationBuilder.Sql(@"
                SET @exist := (SELECT COUNT(*) FROM information_schema.columns 
                    WHERE table_schema = DATABASE() 
                    AND table_name = 'Reservations' 
                    AND column_name = 'SocieteIdSociete');
                SET @sqlstmt := IF(@exist > 0, 'ALTER TABLE `Reservations` DROP COLUMN `SocieteIdSociete`;', 'SELECT ''Column does not exist'';');
                PREPARE stmt FROM @sqlstmt;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recréer les colonnes (pour rollback)
            migrationBuilder.AddColumn<int>(
                name: "SocieteIdSociete",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocieteIdSociete",
                table: "Tailles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocieteIdSociete",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocieteIdSociete",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocieteIdSociete",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocieteIdSociete",
                table: "Clients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocieteIdSociete",
                table: "Reservations",
                type: "int",
                nullable: true);

            // Recréer les index et clés étrangères (pour rollback)
            migrationBuilder.CreateIndex(
                name: "IX_Users_SocieteIdSociete",
                table: "Users",
                column: "SocieteIdSociete");

            migrationBuilder.CreateIndex(
                name: "IX_Tailles_SocieteIdSociete",
                table: "Tailles",
                column: "SocieteIdSociete");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_SocieteIdSociete",
                table: "Roles",
                column: "SocieteIdSociete");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SocieteIdSociete",
                table: "Categories",
                column: "SocieteIdSociete");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_SocieteIdSociete",
                table: "Articles",
                column: "SocieteIdSociete");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_SocieteIdSociete",
                table: "Clients",
                column: "SocieteIdSociete");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SocieteIdSociete",
                table: "Reservations",
                column: "SocieteIdSociete");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Societes_SocieteIdSociete",
                table: "Users",
                column: "SocieteIdSociete",
                principalTable: "Societes",
                principalColumn: "id_societe");

            migrationBuilder.AddForeignKey(
                name: "FK_Tailles_Societes_SocieteIdSociete",
                table: "Tailles",
                column: "SocieteIdSociete",
                principalTable: "Societes",
                principalColumn: "id_societe");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Societes_SocieteIdSociete",
                table: "Roles",
                column: "SocieteIdSociete",
                principalTable: "Societes",
                principalColumn: "id_societe");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Societes_SocieteIdSociete",
                table: "Categories",
                column: "SocieteIdSociete",
                principalTable: "Societes",
                principalColumn: "id_societe");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Societes_SocieteIdSociete",
                table: "Articles",
                column: "SocieteIdSociete",
                principalTable: "Societes",
                principalColumn: "id_societe");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Societes_SocieteIdSociete",
                table: "Clients",
                column: "SocieteIdSociete",
                principalTable: "Societes",
                principalColumn: "id_societe");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Societes_SocieteIdSociete",
                table: "Reservations",
                column: "SocieteIdSociete",
                principalTable: "Societes",
                principalColumn: "id_societe");
        }
    }
}
