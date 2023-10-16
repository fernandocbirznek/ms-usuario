using ms_usuario.Domains;
using ms_usuario.Features.UsuarioPerfilFeature.Commands;

namespace ms_usuario.Extensions
{
    public static class UsuarioPerfilExtensions
    {
        public static UsuarioPerfil ToDomain(this InserirUsuarioPerfilCommand request)
        {
            return new()
            {
                Foto = request.Foto,
                Hobbie = request.Hobbie,
                DataCadastro = DateTime.Now
            };
        }
    }
}
