using ms_usuario.Domains;
using ms_usuario.Features.NoticiaAreaInteresseFeature.Commands;

namespace ms_usuario.Extensions
{
    public static class NoticiaAreaInteresseExtensions
    {
        public static NoticiaAreaInteresse ToDomain(this InserirNoticiaAreaInteresseCommand request)
        {
            return new()
            {
                NoticiaId = request.NoticiaId,
                AreaInteresseId = request.AreaInteresseId,
                DataCadastro = DateTime.Now
            };
        }
    }
}
