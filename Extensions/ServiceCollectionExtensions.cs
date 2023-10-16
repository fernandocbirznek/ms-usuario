using Microsoft.EntityFrameworkCore;
using ms_usuario.Domains;
using ms_usuario.Interface;
using ms_usuario.Repositories;

namespace ms_usuario.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<AreaInteresse>), typeof(Repository<AreaInteresse>));
            services.AddScoped(typeof(IRepository<Conquistas>), typeof(Repository<Conquistas>));
            services.AddScoped(typeof(IRepository<Sociedade>), typeof(Repository<Sociedade>));
            services.AddScoped(typeof(IRepository<Usuario>), typeof(Repository<Usuario>));
            services.AddScoped(typeof(IRepository<UsuarioAreaInteresse>), typeof(Repository<UsuarioAreaInteresse>));
            services.AddScoped(typeof(IRepository<UsuarioConquistas>), typeof(Repository<UsuarioConquistas>));
            services.AddScoped(typeof(IRepository<UsuarioPerfil>), typeof(Repository<UsuarioPerfil>));
        }

        public static void SetupDbContext(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<UsuarioDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(UsuarioDbContext).Assembly.FullName)),
                ServiceLifetime.Transient, ServiceLifetime.Transient
                );
        }
    }
}
