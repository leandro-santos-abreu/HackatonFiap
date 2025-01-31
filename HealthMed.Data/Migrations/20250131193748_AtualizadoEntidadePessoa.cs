using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Data.Migrations
{
    /// <inheritdoc />
    public partial class AtualizadoEntidadePessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoPessoa",
                table: "Paciente",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TipoPessoa",
                table: "Medico",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoPessoa",
                table: "Paciente");

            migrationBuilder.DropColumn(
                name: "TipoPessoa",
                table: "Medico");
        }
    }
}
