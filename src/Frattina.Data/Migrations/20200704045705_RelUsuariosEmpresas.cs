using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Frattina.Data.Migrations
{
    public partial class RelUsuariosEmpresas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SalesEmployeeCode",
                table: "Usuarios",
                newName: "VendedorSapId");

            migrationBuilder.RenameColumn(
                name: "NomeVendedorSap",
                table: "Usuarios",
                newName: "VendedorSapNome");

            migrationBuilder.RenameColumn(
                name: "NomeUsuarioSap",
                table: "Usuarios",
                newName: "UsuarioSapNome");

            migrationBuilder.RenameColumn(
                name: "InternalKey",
                table: "Usuarios",
                newName: "UsuarioSapId");

            migrationBuilder.RenameColumn(
                name: "ProdutoId",
                table: "AtendimentosProdutos",
                newName: "Tipo");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "AtendimentosProdutos",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(500)");

            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "AtendimentosProdutos",
                type: "varchar(300)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Marca",
                table: "AtendimentosProdutos",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Modelo",
                table: "AtendimentosProdutos",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProdutoSapId",
                table: "AtendimentosProdutos",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Referencia",
                table: "AtendimentosProdutos",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RelUsuariosEmpresas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UsuarioId = table.Column<Guid>(nullable: false),
                    EmpresaId = table.Column<int>(nullable: false),
                    EmpresaRazaoSocial = table.Column<string>(type: "varchar(100)", nullable: false),
                    EmpresaNomeFantasia = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelUsuariosEmpresas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelUsuariosEmpresas_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelUsuariosEmpresas_UsuarioId",
                table: "RelUsuariosEmpresas",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelUsuariosEmpresas");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "AtendimentosProdutos");

            migrationBuilder.DropColumn(
                name: "Marca",
                table: "AtendimentosProdutos");

            migrationBuilder.DropColumn(
                name: "Modelo",
                table: "AtendimentosProdutos");

            migrationBuilder.DropColumn(
                name: "ProdutoSapId",
                table: "AtendimentosProdutos");

            migrationBuilder.DropColumn(
                name: "Referencia",
                table: "AtendimentosProdutos");

            migrationBuilder.RenameColumn(
                name: "VendedorSapNome",
                table: "Usuarios",
                newName: "NomeVendedorSap");

            migrationBuilder.RenameColumn(
                name: "VendedorSapId",
                table: "Usuarios",
                newName: "SalesEmployeeCode");

            migrationBuilder.RenameColumn(
                name: "UsuarioSapNome",
                table: "Usuarios",
                newName: "NomeUsuarioSap");

            migrationBuilder.RenameColumn(
                name: "UsuarioSapId",
                table: "Usuarios",
                newName: "InternalKey");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "AtendimentosProdutos",
                newName: "ProdutoId");

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "AtendimentosProdutos",
                type: "varchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");
        }
    }
}
