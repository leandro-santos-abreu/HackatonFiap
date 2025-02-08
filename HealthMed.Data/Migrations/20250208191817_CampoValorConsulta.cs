using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Data.Migrations
{
    /// <inheritdoc />
    public partial class CampoValorConsulta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JustificativaCancelamento",
                table: "Agenda",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorConsulta",
                table: "Agenda",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "isConfirmacaoMedico",
                table: "Agenda",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JustificativaCancelamento",
                table: "Agenda");

            migrationBuilder.DropColumn(
                name: "ValorConsulta",
                table: "Agenda");

            migrationBuilder.DropColumn(
                name: "isConfirmacaoMedico",
                table: "Agenda");
        }
    }
}
