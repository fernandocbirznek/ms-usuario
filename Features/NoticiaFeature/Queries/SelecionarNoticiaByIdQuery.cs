using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.NoticiaFeature.Queries
{
    public class SelecionarNoticiaByIdQuery : IRequest<SelecionarNoticiaByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarNoticiaByIdQueryResponse : Entity
    {
        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public string Conteudo { get; set; }
        public long Favoritado {  get; set; }
        public long UsuarioCadastroId { get; set; }
        public IEnumerable<NoticiaAreaInteresse> noticiaAreaInteresseMany { get; set; }
    }

    public class SelecionarNoticiaByIdQueryHandler : IRequestHandler<SelecionarNoticiaByIdQuery, SelecionarNoticiaByIdQueryResponse>
    {
        private readonly IRepository<Noticia> _repository;

        public SelecionarNoticiaByIdQueryHandler
        (
            IRepository<Noticia> repository
        )
        {
            _repository = repository;
        }

        public async Task<SelecionarNoticiaByIdQueryResponse> Handle
        (
            SelecionarNoticiaByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarNoticiaByIdQuery>());

            Noticia noticia = await GetFirstAsync(request, cancellationToken);

            Validator(noticia);

            SelecionarNoticiaByIdQueryResponse response = new SelecionarNoticiaByIdQueryResponse();
            response.Titulo = noticia.Titulo;
            response.Resumo = noticia.Resumo;
            response.Conteudo = noticia.Conteudo;
            response.Favoritado = noticia.Favoritado;
            response.noticiaAreaInteresseMany = noticia.NoticiaAreaInteresseMany;
            response.DataCadastro = noticia.DataCadastro;
            response.DataAtualizacao = noticia.DataAtualizacao;
            response.Id = noticia.Id;

            return response;
        }

        private void Validator
        (
            Noticia noticia
        )
        {
            if (noticia is null) throw new ArgumentNullException("Noticia não encontrado");
        }

        private async Task<Noticia> GetFirstAsync
        (
            SelecionarNoticiaByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken,
                    item => item.NoticiaAreaInteresseMany
                );
        }
    }
}
