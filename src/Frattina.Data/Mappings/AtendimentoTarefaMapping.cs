using Frattina.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Frattina.Data.Mappings
{
    public class AtendimentoTarefaMapping : IEntityTypeConfiguration<AtendimentoTarefa>
    {
        public void Configure(EntityTypeBuilder<AtendimentoTarefa> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Tipo)
                .IsRequired();

            builder.Property(a => a.Assunto)
                .IsRequired()
                .HasColumnType("varchar(500)");

            builder.Property(a => a.DataTarefa)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(a => a.DataFinalizacao)
                .HasColumnType("datetime");

            builder.Property(a => a.Removida)
                .IsRequired()
                .HasColumnType("bit");

            builder.HasOne(t => t.Atendimento)
                .WithMany(a => a.Tarefas)
                .HasForeignKey(t => t.AtendimentoId);

            builder.ToTable("AtendimentosTarefas");
        }
    }
}
