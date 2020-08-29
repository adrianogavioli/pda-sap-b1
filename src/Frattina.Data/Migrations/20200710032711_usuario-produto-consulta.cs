using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Frattina.Data.Migrations
{
    public partial class usuarioprodutoconsulta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsuariosProdutosConsultas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsuarioId = table.Column<Guid>(nullable: false),
                    ProdutoId = table.Column<string>(type: "varchar(50)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime", nullable: false),
                    Imagem = table.Column<string>(type: "varchar(300)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosProdutosConsultas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosProdutosConsultas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosProdutosConsultas_UsuarioId",
                table: "UsuariosProdutosConsultas",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsuariosProdutosConsultas");
        }
    }
}
