using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ms_usuario.Domains;

namespace ms_usuario.Tables
{
    internal static class UsuarioSociedadeExtensions
    {
        public static void SetupUsuarioSociedadeTable
        (
            this ModelBuilder modelBuilder
        )
        {
            modelBuilder
           .Entity<UsuarioSociedade>
           (
                builder =>
               {
                   builder.HasKey(item => new { item.UsuarioId, item.SociedadeId });

                   builder
                       .HasOne(item => item.Usuario)
                       .WithMany(item => item.UsuarioSociedades)
                       .HasForeignKey(item => item.UsuarioId);

                   builder
                       .HasOne(item => item.Sociedade)
                       .WithMany(item => item.UsuarioSociedades)
                       .HasForeignKey(item => item.SociedadeId);
               }
           );
        }
    }
}
