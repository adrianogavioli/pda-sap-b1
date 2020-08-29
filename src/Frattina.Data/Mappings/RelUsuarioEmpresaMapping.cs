using Frattina.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frattina.Data.Mappings
{
    public class RelUsuarioEmpresaMapping : IEntityTypeConfiguration<RelUsuarioEmpresa>
    {
        public void Configure(EntityTypeBuilder<RelUsuarioEmpresa> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.Usuario)
                .WithMany(u => u.Empresas)
                .HasForeignKey(u => u.UsuarioId);

            builder.Property(r => r.EmpresaId)
                .IsRequired();

            builder.Property(r => r.EmpresaRazaoSocial)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(r => r.EmpresaNomeFantasia)
                .HasColumnType("varchar(100)")
                .IsRequired();

            builder.Property(r => r.Removido)
                .HasColumnType("bit")
                .IsRequired();

            builder.ToTable("RelUsuariosEmpresas");
        }
    }
}
