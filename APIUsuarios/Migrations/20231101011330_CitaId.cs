using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIUsuarios.Migrations
{
    /// <inheritdoc />
    public partial class CitaId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Citado",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "Medico",
                table: "Citas");

            migrationBuilder.AddColumn<int>(
                name: "IdMedico",
                table: "Citas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Citas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdMedico",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Citas");

            migrationBuilder.AddColumn<string>(
                name: "Citado",
                table: "Citas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Medico",
                table: "Citas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
