using Frattina.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Frattina.Data.Context
{
    public class FrattinaDbContext : DbContext
    {
        private readonly IHttpContextAccessor _accessor;

        public FrattinaDbContext(IHttpContextAccessor accessor, DbContextOptions options) : base(options)
        {
            _accessor = accessor;
        }

        public DbSet<Cargo> Cargos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Atendimento> Atendimentos { get; set; }

        public DbSet<AtendimentoProduto> AtendimentosProdutos { get; set; }

        public DbSet<AtendimentoTarefa> AtendimentosTarefas { get; set; }

        public DbSet<AtendimentoEncerrado> AtendimentosEncerrados { get; set; }

        public DbSet<AtendimentoVendido> AtendimentosVendidos { get; set; }

        public DbSet<Auditoria> Auditorias { get; set; }

        public DbSet<RelUsuarioEmpresa> RelUsuariosEmpresas { get; set; }

        public DbSet<UsuarioProdutoConsulta> UsuariosProdutosConsultas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                .Where(p => p.ClrType == typeof(string))))
                property.Relational().ColumnType = "varchar(100)";

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FrattinaDbContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataAlteracao") != null))
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataAlteracao").CurrentValue = DateTime.Now;
                }
            }

            this.LogarAuditoria(_accessor);

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}