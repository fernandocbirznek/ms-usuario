using Microsoft.EntityFrameworkCore;
using ms_usuario.Domains;
using ms_usuario.Tables;
using System.Data.Common;

namespace ms_usuario
{
    public class UsuarioDbContext : DbContext, IDbContext
    {
        public UsuarioDbContext(DbContextOptions<UsuarioDbContext> options) : base(options) { }
        public DbSet<AreaInteresse> AreaInteresse { get; set; }
        public DbSet<Conquistas> Conquistas { get; set; }
        public DbSet<Sociedade> Sociedade { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<UsuarioAreaInteresse> UsuarioAreaInteresse { get; set; }
        public DbSet<UsuarioConquistas> UsuarioConquistas { get; set; }
        public DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }

        public DbConnection Connection => base.Database.GetDbConnection();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetupUsuarioTable();
        }
    }
}
