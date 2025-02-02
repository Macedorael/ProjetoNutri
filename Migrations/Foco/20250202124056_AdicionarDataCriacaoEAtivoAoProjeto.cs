using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoNutri.Migrations.Foco
{
    /// <inheritdoc />
    public partial class AdicionarDataCriacaoEAtivoAoProjeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Projetos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCriacao",
                table: "Projetos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Projetos");

            migrationBuilder.DropColumn(
                name: "DataCriacao",
                table: "Projetos");
        }
    }
}
