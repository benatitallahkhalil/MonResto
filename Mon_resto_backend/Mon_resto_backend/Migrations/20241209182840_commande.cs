using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mon_resto_backend.Migrations
{
    /// <inheritdoc />
    public partial class commande : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Statut",
                table: "Commandes");

            migrationBuilder.AddColumn<bool>(
                name: "EstAnnulee",
                table: "Commandes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstAnnulee",
                table: "Commandes");

            migrationBuilder.AddColumn<string>(
                name: "Statut",
                table: "Commandes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
