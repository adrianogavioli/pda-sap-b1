using Microsoft.EntityFrameworkCore.Migrations;

namespace Frattina.Data.Migrations
{
    public partial class atendimentocontribuinte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Contribuinte",
                table: "Atendimentos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Contribuinte",
                table: "Atendimentos");
        }
    }
}
