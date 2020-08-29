using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Frattina.Data.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atendimentos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmpresaId = table.Column<int>(nullable: false),
                    EmpresaNome = table.Column<string>(type: "varchar(100)", nullable: false),
                    VendedorId = table.Column<int>(nullable: false),
                    VendedorNome = table.Column<string>(type: "varchar(100)", nullable: false),
                    ClienteId = table.Column<string>(type: "varchar(10)", nullable: false),
                    ClienteNome = table.Column<string>(type: "varchar(100)", nullable: false),
                    TipoPessoa = table.Column<string>(type: "varchar(2)", nullable: false),
                    ClienteEmail = table.Column<string>(type: "varchar(100)", nullable: true),
                    ClienteTelefone = table.Column<string>(type: "varchar(20)", nullable: true),
                    ClienteNiver = table.Column<DateTime>(type: "date", nullable: true),
                    Etapa = table.Column<int>(nullable: false),
                    Negociacao = table.Column<string>(type: "varchar(1000)", nullable: true),
                    Data = table.Column<DateTime>(type: "datetime", nullable: false),
                    ClienteIdVenda = table.Column<string>(type: "varchar(10)", nullable: false),
                    ClienteNomeVenda = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atendimentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cargos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Descricao = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AtendimentosEncerrados",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Motivo = table.Column<int>(nullable: false),
                    Justificativa = table.Column<string>(type: "varchar(1000)", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime", nullable: false),
                    AtendimentoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtendimentosEncerrados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtendimentosEncerrados_Atendimentos_AtendimentoId",
                        column: x => x.AtendimentoId,
                        principalTable: "Atendimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AtendimentosProdutos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProdutoId = table.Column<string>(type: "varchar(100)", nullable: false),
                    ValorTabela = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ValorNegociado = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Descricao = table.Column<string>(type: "varchar(500)", nullable: false),
                    NivelInteresse = table.Column<int>(type: "int", nullable: true),
                    RemovidoAtendimento = table.Column<bool>(type: "bit", nullable: false),
                    RemovidoVenda = table.Column<bool>(type: "bit", nullable: false),
                    AtendimentoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtendimentosProdutos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtendimentosProdutos_Atendimentos_AtendimentoId",
                        column: x => x.AtendimentoId,
                        principalTable: "Atendimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AtendimentosTarefas",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    Assunto = table.Column<string>(type: "varchar(500)", nullable: false),
                    DataTarefa = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataFinalizacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Removida = table.Column<bool>(type: "bit", nullable: false),
                    AtendimentoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtendimentosTarefas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtendimentosTarefas_Atendimentos_AtendimentoId",
                        column: x => x.AtendimentoId,
                        principalTable: "Atendimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AtendimentosVendidos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VendaCodigo = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime", nullable: false),
                    AtendimentoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtendimentosVendidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AtendimentosVendidos_Atendimentos_AtendimentoId",
                        column: x => x.AtendimentoId,
                        principalTable: "Atendimentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", nullable: false),
                    Tipo = table.Column<int>(nullable: false),
                    CargoId = table.Column<Guid>(nullable: true),
                    InternalKey = table.Column<int>(type: "int", nullable: true),
                    NomeUsuarioSap = table.Column<string>(type: "varchar(100)", nullable: true),
                    SalesEmployeeCode = table.Column<int>(type: "int", nullable: true),
                    NomeVendedorSap = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Cargos_CargoId",
                        column: x => x.CargoId,
                        principalTable: "Cargos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auditorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Data = table.Column<DateTime>(type: "datetime", nullable: false),
                    Tabela = table.Column<string>(type: "varchar(100)", nullable: false),
                    Evento = table.Column<string>(type: "varchar(20)", nullable: false),
                    Chave = table.Column<Guid>(nullable: false),
                    ValorAntigo = table.Column<string>(type: "varchar(1000)", nullable: true),
                    ValorAtual = table.Column<string>(type: "varchar(1000)", nullable: true),
                    UsuarioId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auditorias_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtendimentosEncerrados_AtendimentoId",
                table: "AtendimentosEncerrados",
                column: "AtendimentoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AtendimentosProdutos_AtendimentoId",
                table: "AtendimentosProdutos",
                column: "AtendimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtendimentosTarefas_AtendimentoId",
                table: "AtendimentosTarefas",
                column: "AtendimentoId");

            migrationBuilder.CreateIndex(
                name: "IX_AtendimentosVendidos_AtendimentoId",
                table: "AtendimentosVendidos",
                column: "AtendimentoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auditorias_UsuarioId",
                table: "Auditorias",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CargoId",
                table: "Usuarios",
                column: "CargoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtendimentosEncerrados");

            migrationBuilder.DropTable(
                name: "AtendimentosProdutos");

            migrationBuilder.DropTable(
                name: "AtendimentosTarefas");

            migrationBuilder.DropTable(
                name: "AtendimentosVendidos");

            migrationBuilder.DropTable(
                name: "Auditorias");

            migrationBuilder.DropTable(
                name: "Atendimentos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Cargos");
        }
    }
}
