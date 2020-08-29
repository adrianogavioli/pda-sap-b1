using Frattina.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frattina.Data.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(u => u.Tipo)
                .IsRequired();

            builder.HasOne(u => u.Cargo)
                .WithMany(c => c.Usuarios)
                .HasForeignKey(u => u.CargoId);

            builder.Property(u => u.UsuarioSapId)
                .HasColumnType("int");

            builder.Property(u => u.UsuarioSapNome)
                .HasColumnType("varchar(100)");

            builder.Property(u => u.VendedorSapId)
                .HasColumnType("int");

            builder.Property(u => u.VendedorSapNome)
                .HasColumnType("varchar(200)");

            builder.ToTable("Usuarios");
        }
    }
}
