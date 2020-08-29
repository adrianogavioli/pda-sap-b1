using Microsoft.EntityFrameworkCore.Migrations;

namespace Frattina.Data.Migrations
{
    public partial class RelUsuarioEmpresaAuditoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Removido",
                table: "RelUsuariosEmpresas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Removido",
                table: "RelUsuariosEmpresas");
        }
    }
}
