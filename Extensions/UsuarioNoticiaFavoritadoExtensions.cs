using ms_usuario.Domains;
using ms_usuario.Features.UsuarioNoticiaFeature.Commands;

namespace ms_usuario.Extensions
{
    public static class UsuarioNoticiaFavoritadoExtensions
    {
        public static UsuarioNoticiaFavoritado ToDomain(this InserirUsuarioNoticiaFavoritadoCommand request)
        {
            return new()
            {
                NoticiaId = request.NoticiaId,
                UsuarioId = request.UsuarioId,
                DataCadastro = DateTime.Now
            };
        }
    }
}
