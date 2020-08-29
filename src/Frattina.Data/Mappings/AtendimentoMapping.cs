using Frattina.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frattina.Data.Mappings
{
    public class AtendimentoMapping : IEntityTypeConfiguration<Atendimento>
    {
        public void Configure(EntityTypeBuilder<Atendimento> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.EmpresaId)
                .IsRequired();

            builder.Property(a => a.EmpresaNome)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(a => a.VendedorId)
                .IsRequired();

            builder.Property(a => a.VendedorNome)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(a => a.ClienteId)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(a => a.ClienteNome)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(a => a.TipoPessoa)
                .IsRequired()
                .HasColumnType("varchar(2)");

            builder.Property(a => a.ClienteEmail)
                .HasColumnType("varchar(100)");

            builder.Property(a => a.ClienteTelefone)
                .HasColumnType("varchar(20)");

            builder.Property(a => a.ClienteNiver)
                .HasColumnType("date");

            builder.Property(a => a.Contribuinte)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(a => a.Etapa)
                .IsRequired();

            builder.Property(a => a.Negociacao)
                .HasColumnType("varchar(1000)");

            builder.Property(a => a.Data)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(a => a.ClienteIdVenda)
                .IsRequired()
                .HasColumnType("varchar(10)");

            builder.Property(a => a.ClienteNomeVenda)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.ToTable("Atendimentos");
        }
    }
}
