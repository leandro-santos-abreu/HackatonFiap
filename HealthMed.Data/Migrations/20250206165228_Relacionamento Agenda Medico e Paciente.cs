using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthMed.Data.Migrations
{
    /// <inheritdoc />
    public partial class RelacionamentoAgendaMedicoePaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agenda_Medico_MedicoIdMedico",
                table: "Agenda");

            migrationBuilder.DropIndex(
                name: "IX_Agenda_MedicoIdMedico",
                table: "Agenda");

            migrationBuilder.DropColumn(
                name: "MedicoIdMedico",
                table: "Agenda");

            migrationBuilder.AddColumn<int>(
                name: "IdPaciente",
                table: "Agenda",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Agenda_IdMedico",
                table: "Agenda",
                column: "IdMedico");

            migrationBuilder.CreateIndex(
                name: "IX_Agenda_IdPaciente",
                table: "Agenda",
                column: "IdPaciente");

            migrationBuilder.AddForeignKey(
                name: "FK_Agenda_Medico_IdMedico",
                table: "Agenda",
                column: "IdMedico",
                principalTable: "Medico",
                principalColumn: "IdMedico",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agenda_Paciente_IdPaciente",
                table: "Agenda",
                column: "IdPaciente",
                principalTable: "Paciente",
                principalColumn: "IdPaciente",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agenda_Medico_IdMedico",
                table: "Agenda");

            migrationBuilder.DropForeignKey(
                name: "FK_Agenda_Paciente_IdPaciente",
                table: "Agenda");

            migrationBuilder.DropIndex(
                name: "IX_Agenda_IdMedico",
                table: "Agenda");

            migrationBuilder.DropIndex(
                name: "IX_Agenda_IdPaciente",
                table: "Agenda");

            migrationBuilder.DropColumn(
                name: "IdPaciente",
                table: "Agenda");

            migrationBuilder.AddColumn<int>(
                name: "MedicoIdMedico",
                table: "Agenda",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Agenda_MedicoIdMedico",
                table: "Agenda",
                column: "MedicoIdMedico");

            migrationBuilder.AddForeignKey(
                name: "FK_Agenda_Medico_MedicoIdMedico",
                table: "Agenda",
                column: "MedicoIdMedico",
                principalTable: "Medico",
                principalColumn: "IdMedico",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
