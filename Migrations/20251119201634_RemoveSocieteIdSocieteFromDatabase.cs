using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mkBoutiqueCaftan.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSocieteIdSocieteFromDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocieteIdSociete",
                table: "Clients",
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
                name: "IX_Reservations_SocieteIdSociete",
                table: "Reservations",
                column: "SocieteIdSociete");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_SocieteIdSociete",
                table: "Clients",
                column: "SocieteIdSociete");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SocieteIdSociete",
                table: "Categories",
                column: "SocieteIdSociete");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_SocieteIdSociete",
                table: "Articles",
                column: "SocieteIdSociete");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Societes_SocieteIdSociete",
                table: "Articles",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Societes_SocieteIdSociete",
                table: "Roles",
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
                name: "FK_Users_Societes_SocieteIdSociete",
                table: "Users",
                column: "SocieteIdSociete",
                principalTable: "Societes",
                principalColumn: "id_societe");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Societes_SocieteIdSociete",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Societes_SocieteIdSociete",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Societes_SocieteIdSociete",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Societes_SocieteIdSociete",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Societes_SocieteIdSociete",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Tailles_Societes_SocieteIdSociete",
                table: "Tailles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Societes_SocieteIdSociete",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_SocieteIdSociete",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tailles_SocieteIdSociete",
                table: "Tailles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_SocieteIdSociete",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_SocieteIdSociete",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Clients_SocieteIdSociete",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Categories_SocieteIdSociete",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Articles_SocieteIdSociete",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "SocieteIdSociete",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SocieteIdSociete",
                table: "Tailles");

            migrationBuilder.DropColumn(
                name: "SocieteIdSociete",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "SocieteIdSociete",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "SocieteIdSociete",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "SocieteIdSociete",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SocieteIdSociete",
                table: "Articles");
        }
    }
}
