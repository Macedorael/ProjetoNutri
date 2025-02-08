using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoNutri.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCamposImceClassificacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Classificacao",
                table: "Imcs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ValorImc",
                table: "Imcs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Classificacao",
                table: "Imcs");

            migrationBuilder.DropColumn(
                name: "ValorImc",
                table: "Imcs");
        }
    }
}
