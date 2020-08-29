using Frattina.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frattina.Data.Mappings
{
    public class AtendimentoVendidoMapping : IEntityTypeConfiguration<AtendimentoVendido>
    {
        public void Configure(EntityTypeBuilder<AtendimentoVendido> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.VendaCodigo)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(a => a.Data)
                .IsRequired()
                .HasColumnType("datetime");

            builder.HasOne(v => v.Atendimento)
                .WithOne(a => a.Vendido);

            builder.ToTable("AtendimentosVendidos");
        }
    }
}
