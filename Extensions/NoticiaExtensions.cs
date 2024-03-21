using ms_usuario.Domains;
using ms_usuario.Features.NoticiaFeature.Commands;

namespace ms_usuario.Extensions
{
    public static class NoticiaExtensions
    {
        public static Noticia ToDomain(this InserirNoticiaCommand request)
        {
            return new()
            {
                Titulo = request.Titulo,
                Resumo = request.Resumo,
                Conteudo = request.Conteudo,
                UsuarioCadastroId = request.UsuarioCadastroId,
                DataCadastro = DateTime.Now
            };
        }

        public static Noticia ToDomain
        (
            this AtualizarNoticiaCommand request
        )
        {
            return new()
            {
                Titulo = request.Titulo,
                Resumo = request.Resumo,
                Conteudo = request.Conteudo,
                UsuarioCadastroId = request.UsuarioCadastroId,
                DataCadastro = DateTime.Now
            };
        }
    }
}
