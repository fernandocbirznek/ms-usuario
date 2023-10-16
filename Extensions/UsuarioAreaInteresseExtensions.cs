using ms_usuario.Domains;
using ms_usuario.Features.UsuarioAreaInteresseFeature.Commands;

namespace ms_usuario.Extensions
{
    public static class UsuarioAreaInteresseExtensions
    {
        public static UsuarioAreaInteresse ToDomain(this InserirUsuarioAreaInteresseCommand request)
        {
            return new()
            {
                UsuarioId = request.UsuarioId,
                AreaInteresseId = request.AreaInteresseId,
                DataCadastro = DateTime.Now
            };
        }
    }
}
