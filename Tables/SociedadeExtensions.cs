using Microsoft.EntityFrameworkCore;
using ms_usuario.Domains;

namespace ms_usuario.Tables
{
    internal static class SociedadeTableExtensions
    {
        internal static void SetupSociedadeTable
        (
            this ModelBuilder modelBuilder
        )
        {
            modelBuilder
                .Entity<Sociedade>(builder =>
                {
                    builder.HasKey(item => item.Id);

                    builder.Property(item => item.Nome)
                           .IsRequired()
                           .HasMaxLength(100);

                    builder.Property(item => item.Descricao)
                           .HasMaxLength(500);

                    builder.HasOne(item => item.UsuarioLider)
                           .WithMany(item => item.SociedadesLideradas)
                           .HasForeignKey(item => item.UsuarioLiderId)
                           .OnDelete(DeleteBehavior.Restrict);
                });
        }
    }
}
