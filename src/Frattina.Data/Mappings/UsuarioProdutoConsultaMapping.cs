using Frattina.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frattina.Data.Mappings
{
    public class UsuarioProdutoConsultaMapping : IEntityTypeConfiguration<UsuarioProdutoConsulta>
    {
        public void Configure(EntityTypeBuilder<UsuarioProdutoConsulta> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Usuario)
                .WithMany(u => u.ProdutosConsultas)
                .HasForeignKey(c => c.UsuarioId);

            builder.Property(c => c.ProdutoId)
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(c => c.DataCadastro)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(c => c.Imagem)
                .HasColumnType("varchar(300)")
                .IsRequired();

            builder.ToTable("UsuariosProdutosConsultas");
        }
    }
}
