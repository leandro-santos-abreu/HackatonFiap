using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Data.Migrations
{
    /// <inheritdoc />
    public partial class EspecialidadeMedica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Especialidade",
                table: "Medico",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Especialidade",
                table: "Medico");
        }
    }
}
