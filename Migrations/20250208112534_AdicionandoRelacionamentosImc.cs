using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoNutri.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoRelacionamentosImc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imc_Projetos_ProjetoId",
                table: "Imc");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Imc",
                table: "Imc");

            migrationBuilder.DropIndex(
                name: "IX_Imc_ProjetoId",
                table: "Imc");

            migrationBuilder.DropColumn(
                name: "ProjetoId",
                table: "Imc");

            migrationBuilder.RenameTable(
                name: "Imc",
                newName: "Imcs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Imcs",
                table: "Imcs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Imcs_IdProjeto",
                table: "Imcs",
                column: "IdProjeto");

            migrationBuilder.AddForeignKey(
                name: "FK_Imcs_Projetos_IdProjeto",
                table: "Imcs",
                column: "IdProjeto",
                principalTable: "Projetos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imcs_Projetos_IdProjeto",
                table: "Imcs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Imcs",
                table: "Imcs");

            migrationBuilder.DropIndex(
                name: "IX_Imcs_IdProjeto",
                table: "Imcs");

            migrationBuilder.RenameTable(
                name: "Imcs",
                newName: "Imc");

            migrationBuilder.AddColumn<int>(
                name: "ProjetoId",
                table: "Imc",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Imc",
                table: "Imc",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Imc_ProjetoId",
                table: "Imc",
                column: "ProjetoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imc_Projetos_ProjetoId",
                table: "Imc",
                column: "ProjetoId",
                principalTable: "Projetos",
                principalColumn: "Id");
        }
    }
}
