using Frattina.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frattina.Data.Mappings
{
    public class AuditoriaMapping : IEntityTypeConfiguration<Auditoria>
    {
        public void Configure(EntityTypeBuilder<Auditoria> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Data)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(a => a.Tabela)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(a => a.Evento)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(a => a.Chave)
                .IsRequired();

            builder.Property(a => a.ValorAntigo)
                .HasColumnType("varchar(1000)");

            builder.Property(a => a.ValorAtual)
                .HasColumnType("varchar(1000)");

            builder.HasOne(a => a.Usuario)
                .WithMany(u => u.Auditorias)
                .HasForeignKey(a => a.UsuarioId);

            builder.ToTable("Auditorias");
        }
    }
}
