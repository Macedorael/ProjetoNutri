using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoNutri.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projetos_Pacientes_PacienteId1",
                table: "Projetos");

            migrationBuilder.DropIndex(
                name: "IX_Projetos_PacienteId1",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "PacienteId1",
                table: "Projetos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PacienteId1",
                table: "Projetos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projetos_PacienteId1",
                table: "Projetos",
                column: "PacienteId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Projetos_Pacientes_PacienteId1",
                table: "Projetos",
                column: "PacienteId1",
                principalTable: "Pacientes",
                principalColumn: "Id");
        }
    }
}
