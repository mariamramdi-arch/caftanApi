using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mkBoutiqueCaftan.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoCINToReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ajouter la colonne photo_cin seulement si elle n'existe pas déjà (MySQL compatible)
            migrationBuilder.Sql(@"
                SET @col_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Reservations' 
                    AND COLUMN_NAME = 'photo_cin'
                );
                
                SET @sql = IF(@col_exists = 0,
                    'ALTER TABLE Reservations ADD COLUMN photo_cin LONGTEXT NULL',
                    'SELECT ''Column already exists'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Supprimer la colonne photo_cin
            migrationBuilder.Sql(@"
                SET @col_exists = (
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND TABLE_NAME = 'Reservations' 
                    AND COLUMN_NAME = 'photo_cin'
                );
                
                SET @sql = IF(@col_exists > 0,
                    'ALTER TABLE Reservations DROP COLUMN photo_cin',
                    'SELECT ''Column does not exist'' AS message'
                );
                
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");
        }
    }
}
