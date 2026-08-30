using Microsoft.EntityFrameworkCore;
using ms_usuario.Interface;
using ms_usuario.Repositories;

namespace ms_usuario.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void SetupRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        public static void SetupDbContext(this IServiceCollection services, string? connectionString)
        {
            services.AddDbContext<UsuarioDbContext>(options =>
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(UsuarioDbContext).Assembly.FullName)),
                ServiceLifetime.Scoped,
                ServiceLifetime.Scoped
                );
        }
    }
}
