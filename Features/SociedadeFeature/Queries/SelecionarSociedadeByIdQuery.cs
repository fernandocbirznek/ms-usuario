using MediatR;
using ms_usuario.Domains;
using ms_usuario.Helpers;
using ms_usuario.Interface;

namespace ms_usuario.Features.SociedadeFeature.Queries
{
    public class SelecionarSociedadeByIdQuery : IRequest<SelecionarSociedadeByIdQueryResponse>
    {
        public long Id { get; set; }
    }

    public class SelecionarSociedadeByIdQueryResponse : Entity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public long UsuarioLiderId { get; set; }
        public List<NoticiaResponse> Noticias { get; set; }
    }

    public class NoticiaResponse : Entity
    {
        public string Titulo { get; set; }
        public string Resumo { get; set; }
        public string Conteudo { get; set; }
        public long Favoritado { get; set; }
        public long UsuarioCadastroId { get; set; }
        public IEnumerable<NoticiaAreaInteresse> noticiaAreaInteresseMany { get; set; }
    }

    public class SelecionarSociedadeByIdQueryHandler : IRequestHandler<SelecionarSociedadeByIdQuery, SelecionarSociedadeByIdQueryResponse>
    {
        private readonly IRepository<Sociedade> _repository;

        public SelecionarSociedadeByIdQueryHandler
        (
            IRepository<Sociedade> repository
        )
        {
            _repository = repository;
        }

        public async Task<SelecionarSociedadeByIdQueryResponse> Handle
        (
            SelecionarSociedadeByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request is null)
                throw new ArgumentNullException(MessageHelper.NullFor<SelecionarSociedadeByIdQuery>());

            Sociedade sociedade = await GetFirstAsync(request, cancellationToken);

            Validator(sociedade);

            SelecionarSociedadeByIdQueryResponse response = new SelecionarSociedadeByIdQueryResponse();
            response.Nome = sociedade.Nome;
            response.Descricao = sociedade.Descricao;
            response.DataCadastro = sociedade.DataCadastro;
            response.DataAtualizacao = sociedade.DataAtualizacao;
            response.UsuarioLiderId = sociedade.UsuarioLiderId;
            response.Id = sociedade.Id;
            response.Noticias = sociedade.Noticias?
                .Select(item => new NoticiaResponse
                {
                    Id = item.Id,
                    Titulo = item.Titulo,
                    Resumo = item.Resumo,
                    Conteudo = item.Conteudo,
                    Favoritado = item.Favoritado,
                    UsuarioCadastroId = item.UsuarioCadastroId,
                    noticiaAreaInteresseMany = item.NoticiaAreaInteresseMany
                })
                .ToList() ?? new List<NoticiaResponse>();

            return response;
        }

        private async void Validator
        (
            Sociedade sociedade
        )
        {
            if (sociedade is null) throw new ArgumentNullException("Sociedade não encontrado");
        }

        private async Task<Sociedade> GetFirstAsync
        (
            SelecionarSociedadeByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetFirstAsync
                (
                    item => item.Id.Equals(request.Id),
                    cancellationToken,
                    item => item.Noticias,
                    item => item.Noticias.Select(noticia => noticia.NoticiaAreaInteresseMany)
                );
        }
    }
}
