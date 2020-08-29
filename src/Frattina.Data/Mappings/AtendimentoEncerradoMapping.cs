using Frattina.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frattina.Data.Mappings
{
    public class AtendimentoEncerradoMapping : IEntityTypeConfiguration<AtendimentoEncerrado>
    {
        public void Configure(EntityTypeBuilder<AtendimentoEncerrado> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Motivo)
                .IsRequired();

            builder.Property(a => a.Justificativa)
                .IsRequired()
                .HasColumnType("varchar(1000)");

            builder.Property(a => a.Data)
                .IsRequired()
                .HasColumnType("datetime");

            builder.HasOne(e => e.Atendimento)
                .WithOne(a => a.Encerrado);

            builder.ToTable("AtendimentosEncerrados");
        }
    }
}
