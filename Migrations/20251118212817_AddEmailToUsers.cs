using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mkBoutiqueCaftan.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Ajouter la colonne email comme nullable d'abord pour gérer les utilisateurs existants
            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            // Mettre à jour les utilisateurs existants avec un email basé sur le login
            migrationBuilder.Sql(@"
                UPDATE Users 
                SET email = CONCAT(login, '@example.com')
                WHERE email IS NULL;
            ");

            // Rendre la colonne non nullable maintenant que tous les utilisateurs ont un email
            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            // Créer l'index unique sur email
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Users");
        }
    }
}
