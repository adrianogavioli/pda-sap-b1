using Frattina.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frattina.Data.Mappings
{
    public class AtendimentoProdutoMapping : IEntityTypeConfiguration<AtendimentoProduto>
    {
        public void Configure(EntityTypeBuilder<AtendimentoProduto> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.ProdutoSapId)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(a => a.Tipo)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(a => a.Marca)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(a => a.Modelo)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(a => a.Referencia)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(a => a.Descricao)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.Property(a => a.Imagem)
                .IsRequired()
                .HasColumnType("varchar(300)");

            builder.Property(a => a.ValorTabela)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(a => a.ValorNegociado)
                .IsRequired()
                .HasColumnType("decimal(10,2)");

            builder.Property(a => a.NivelInteresse)
                .HasColumnType("int");

            builder.Property(a => a.RemovidoAtendimento)
                .IsRequired()
                .HasColumnType("bit");

            builder.Property(a => a.RemovidoVenda)
                .IsRequired()
                .HasColumnType("bit");

            builder.HasOne(p => p.Atendimento)
                .WithMany(a => a.Produtos)
                .HasForeignKey(p => p.AtendimentoId);

            builder.ToTable("AtendimentosProdutos");
        }
    }
}
