using ms_usuario.Domains;
using ms_usuario.Features.ConquistasFeature.Commands;

namespace ms_usuario.Extensions
{
    public static class ConquistasExtensions
    {
        public static Conquistas ToDomain(this InserirConquistasCommand request)
        {
            return new()
            {
                Nome = request.Nome,
                DataCadastro = DateTime.Now
            };
        }
    }
}
