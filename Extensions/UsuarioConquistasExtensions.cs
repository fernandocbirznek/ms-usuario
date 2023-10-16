using ms_usuario.Domains;
using ms_usuario.Features.UsuarioConquistasFeature.Commands;

namespace ms_usuario.Extensions
{
    public static class UsuarioConquistasExtensions
    {
        public static UsuarioConquistas ToDomain(this InserirUsuarioConquistasCommand request)
        {
            return new()
            {
                UsuarioId = request.UsuarioId,
                ConquistaId = request.ConquistaId,
                DataCadastro = DateTime.Now
            };
        }
    }
}
