using Microsoft.EntityFrameworkCore;
using ms_usuario.Domains;

namespace ms_usuario.Tables
{
    internal static class UsuarioTableExtensions
    {
        internal static void SetupUsuarioTable(this ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Usuario>()
                .Property(item => item.Nome)
                .HasMaxLength(100);

            modelBuilder
                .Entity<Usuario>()
                .Property(item => item.Email)
                .HasMaxLength(100);

            modelBuilder
                .Entity<Usuario>()
                .HasOne(item => item.Perfil)
                .WithOne(item => item.Usuario)
                .HasForeignKey<Usuario>(item => item.PerfilId);
        }
    }
}
